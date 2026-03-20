using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Interfaces;

/// <summary>
/// Application service contract for all library operations.
/// Events support the publisher/subscriber pattern (Day 19).
/// Async signatures throughout reflect non-blocking I/O design (Day 20).
/// </summary>
public interface ILibraryService
{
    // ── Book operations ─────────────────────────────────────────────────────
    Task<Book> AddBookAsync(string title, string author, string isbn,
        BookCategory category, int publishedYear);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<IEnumerable<Book>> SearchBooksAsync(string query);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task RemoveBookAsync(Guid bookId);

    // ── Member operations ────────────────────────────────────────────────────
    Task<Member> RegisterMemberAsync(string name, string email);
    Task<IEnumerable<Member>> GetAllMembersAsync();
    Task<IEnumerable<Member>> SearchMembersAsync(string query);

    // ── Loan operations ──────────────────────────────────────────────────────
    Task<Loan> BorrowBookAsync(Guid bookId, Guid memberId);
    Task<Loan> ReturnBookAsync(Guid loanId);
    Task<IEnumerable<Loan>> GetActiveLoansAsync();
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<IEnumerable<Loan>> GetLoansByMemberAsync(Guid memberId);

    /// <summary>Enriched loan view combining Loan, Book, and Member data (Day 26 – DTO).</summary>
    Task<IEnumerable<LoanSummary>> GetActiveLoanSummariesAsync();
    Task<IEnumerable<LoanSummary>> GetOverdueLoanSummariesAsync();

    // ── Events (publisher/subscriber, Day 19) ───────────────────────────────
    event EventHandler<LoanEventArgs> BookBorrowed;
    event EventHandler<LoanEventArgs> BookReturned;
}
