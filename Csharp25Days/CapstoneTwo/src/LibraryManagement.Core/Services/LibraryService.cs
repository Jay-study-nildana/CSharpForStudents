using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Services;

/// <summary>
/// Central application service that orchestrates all library operations.
///
/// Curriculum topics demonstrated:
///   Day 8/9  – polymorphism: operates on IRepository&lt;T&gt; abstractions
///   Day 13   – LINQ for querying in-memory collections
///   Day 14   – custom exceptions and fail-fast validation
///   Day 17   – constructor injection (manual DI)
///   Day 19   – delegate-based events (BookBorrowed, BookReturned)
///   Day 20   – fully async using async/await with non-blocking I/O
/// </summary>
public class LibraryService : ILibraryService
{
    private readonly IBookRepository   _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository   _loanRepository;
    private readonly IAppLogger        _logger;

    // ── Events (publisher/subscriber pattern) ────────────────────────────────
    public event EventHandler<LoanEventArgs>? BookBorrowed;
    public event EventHandler<LoanEventArgs>? BookReturned;

    event EventHandler<LoanEventArgs> ILibraryService.BookBorrowed
    {
        add    => BookBorrowed += value;
        remove => BookBorrowed -= value;
    }

    event EventHandler<LoanEventArgs> ILibraryService.BookReturned
    {
        add    => BookReturned += value;
        remove => BookReturned -= value;
    }

    public LibraryService(
        IBookRepository   bookRepository,
        IMemberRepository memberRepository,
        ILoanRepository   loanRepository,
        IAppLogger        logger)
    {
        _bookRepository   = bookRepository;
        _memberRepository = memberRepository;
        _loanRepository   = loanRepository;
        _logger           = logger;
    }

    // ── Book operations ──────────────────────────────────────────────────────

    public async Task<Book> AddBookAsync(string title, string author, string isbn,
        BookCategory category, int publishedYear)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty.", nameof(author));
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty.", nameof(isbn));

        var existing = await _bookRepository.GetByIsbnAsync(isbn);
        if (existing is not null)
            throw new InvalidOperationException($"A book with ISBN '{isbn}' already exists.");

        var book = new Book(title, author, isbn, category, publishedYear);
        await _bookRepository.AddAsync(book);
        _logger.LogInfo($"Book added: \"{title}\" by {author} (ISBN: {isbn})");
        return book;
    }

    public Task<IEnumerable<Book>> GetAllBooksAsync() =>
        _bookRepository.GetAllAsync();

    public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await _bookRepository.GetAllAsync();
        return await _bookRepository.SearchAsync(query);
    }

    public Task<IEnumerable<Book>> GetAvailableBooksAsync() =>
        _bookRepository.GetAvailableBooksAsync();

    public async Task RemoveBookAsync(Guid bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new BookNotFoundException(bookId);

        var activeLoan = await _loanRepository.GetActiveLoanForBookAsync(bookId);
        if (activeLoan is not null)
            throw new InvalidOperationException(
                $"Cannot remove \"{book.Title}\" because it is currently on loan.");

        await _bookRepository.DeleteAsync(bookId);
        _logger.LogInfo($"Book removed: \"{book.Title}\" (ID: {bookId})");
    }

    // ── Member operations ────────────────────────────────────────────────────

    public async Task<Member> RegisterMemberAsync(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        var existing = await _memberRepository.GetByEmailAsync(email);
        if (existing is not null)
            throw new InvalidOperationException($"A member with email '{email}' already exists.");

        var member = new Member(name, email);
        await _memberRepository.AddAsync(member);
        _logger.LogInfo($"Member registered: {name} ({email})");
        return member;
    }

    public Task<IEnumerable<Member>> GetAllMembersAsync() =>
        _memberRepository.GetAllAsync();

    public async Task<IEnumerable<Member>> SearchMembersAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await _memberRepository.GetAllAsync();
        return await _memberRepository.SearchAsync(query);
    }

    // ── Loan operations ──────────────────────────────────────────────────────

    public async Task<Loan> BorrowBookAsync(Guid bookId, Guid memberId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new BookNotFoundException(bookId);

        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new MemberNotFoundException(memberId);

        if (!book.IsAvailable)
            throw new BookNotAvailableException(bookId, book.Title);

        var activeLoans = (await _loanRepository.GetActiveLoansByMemberAsync(memberId)).ToList();
        if (activeLoans.Count >= member.MaxLoansAllowed)
            throw new InvalidOperationException(
                $"Member \"{member.Name}\" has reached the maximum of {member.MaxLoansAllowed} active loan(s).");

        var loan = new Loan(bookId, memberId);
        book.IsAvailable = false;

        await _loanRepository.AddAsync(loan);
        await _bookRepository.UpdateAsync(book);

        _logger.LogInfo(
            $"Book \"{book.Title}\" borrowed by \"{member.Name}\" — due {loan.DueDate:yyyy-MM-dd}");

        BookBorrowed?.Invoke(this, new LoanEventArgs(loan, book, member));
        return loan;
    }

    public async Task<Loan> ReturnBookAsync(Guid loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId)
            ?? throw new InvalidOperationException($"Loan '{loanId}' not found.");

        if (loan.Status != LoanStatus.Active)
            throw new InvalidOperationException("This loan has already been closed.");

        var book = await _bookRepository.GetByIdAsync(loan.BookId)
            ?? throw new BookNotFoundException(loan.BookId);

        var member = await _memberRepository.GetByIdAsync(loan.MemberId)
            ?? throw new MemberNotFoundException(loan.MemberId);

        loan.ReturnedDate = DateTime.UtcNow;
        loan.Status       = LoanStatus.Returned;
        book.IsAvailable  = true;

        await _loanRepository.UpdateAsync(loan);
        await _bookRepository.UpdateAsync(book);

        _logger.LogInfo($"Book \"{book.Title}\" returned by \"{member.Name}\"");
        BookReturned?.Invoke(this, new LoanEventArgs(loan, book, member));
        return loan;
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
    {
        var all = await _loanRepository.GetAllAsync();
        // LINQ: filter by status (Day 13)
        return all.Where(l => l.Status == LoanStatus.Active);
    }

    public Task<IEnumerable<Loan>> GetOverdueLoansAsync() =>
        _loanRepository.GetOverdueLoansAsync();

    public async Task<IEnumerable<Loan>> GetLoansByMemberAsync(Guid memberId)
    {
        _ = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new MemberNotFoundException(memberId);
        return await _loanRepository.GetActiveLoansByMemberAsync(memberId);
    }

    // ── Enriched summary projections (LINQ + records, Days 13 & 26) ─────────

    public async Task<IEnumerable<LoanSummary>> GetActiveLoanSummariesAsync()
    {
        var loans   = (await GetActiveLoansAsync()).ToList();
        var books   = (await _bookRepository.GetAllAsync()).ToDictionary(b => b.Id);
        var members = (await _memberRepository.GetAllAsync()).ToDictionary(m => m.Id);

        return loans.Select(l => new LoanSummary(
            LoanId:      l.Id,
            BookId:      l.BookId,
            BookTitle:   books.TryGetValue(l.BookId,   out var b) ? b.Title   : "Unknown",
            MemberId:    l.MemberId,
            MemberName:  members.TryGetValue(l.MemberId, out var m) ? m.Name  : "Unknown",
            BorrowedDate: l.BorrowedDate,
            DueDate:     l.DueDate,
            Status:      l.Status,
            IsOverdue:   l.IsOverdue));
    }

    public async Task<IEnumerable<LoanSummary>> GetOverdueLoanSummariesAsync()
    {
        var summaries = await GetActiveLoanSummariesAsync();
        return summaries.Where(s => s.IsOverdue);
    }
}
