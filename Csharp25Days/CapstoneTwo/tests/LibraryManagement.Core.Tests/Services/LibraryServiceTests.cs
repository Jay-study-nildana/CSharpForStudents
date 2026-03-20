using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Services;
using Moq;

namespace LibraryManagement.Core.Tests.Services;

/// <summary>
/// Unit tests for LibraryService — the core business logic class.
///
/// Pattern: Arrange-Act-Assert (Day 21).
/// Dependencies are replaced with Moq test doubles so each test is isolated (Day 21).
/// Tests cover: happy paths, guard clauses, state changes, and event firing.
/// </summary>
public class LibraryServiceTests
{
    // ── Shared test doubles (Day 21 — mocking / test doubles) ────────────────
    private readonly Mock<IBookRepository>   _bookRepo   = new();
    private readonly Mock<IMemberRepository> _memberRepo = new();
    private readonly Mock<ILoanRepository>   _loanRepo   = new();
    private readonly Mock<IAppLogger>        _logger     = new();
    private readonly LibraryService          _sut;          // System Under Test

    public LibraryServiceTests()
    {
        _sut = new LibraryService(
            _bookRepo.Object, _memberRepo.Object, _loanRepo.Object, _logger.Object);
    }

    // ════════════════════════════════════════════════════════════════════════
    // AddBook
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task AddBook_ValidData_ReturnsBookAndPersists()
    {
        // Arrange
        _bookRepo.Setup(r => r.GetByIsbnAsync("978-0-000")).ReturnsAsync((Book?)null);
        _bookRepo.Setup(r => r.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

        // Act
        var book = await _sut.AddBookAsync("Clean Code", "Robert Martin", "978-0-000",
            BookCategory.Technology, 2008);

        // Assert
        Assert.Equal("Clean Code", book.Title);
        Assert.Equal("Robert Martin", book.Author);
        Assert.True(book.IsAvailable);
        _bookRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public async Task AddBook_DuplicateIsbn_ThrowsInvalidOperationException()
    {
        // Arrange — ISBN already exists
        _bookRepo.Setup(r => r.GetByIsbnAsync("978-dup"))
                 .ReturnsAsync(new Book("Existing", "Author", "978-dup", BookCategory.Fiction, 2000));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.AddBookAsync("New Book", "Author", "978-dup", BookCategory.Fiction, 2024));
    }

    [Fact]
    public async Task AddBook_EmptyTitle_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.AddBookAsync("", "Author", "978-0-001", BookCategory.Fiction, 2024));
    }

    [Fact]
    public async Task AddBook_EmptyIsbn_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.AddBookAsync("Title", "Author", "", BookCategory.Fiction, 2024));
    }

    // ════════════════════════════════════════════════════════════════════════
    // RemoveBook
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task RemoveBook_BookNotFound_ThrowsBookNotFoundException()
    {
        // Arrange
        var missingId = Guid.NewGuid();
        _bookRepo.Setup(r => r.GetByIdAsync(missingId)).ReturnsAsync((Book?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BookNotFoundException>(() => _sut.RemoveBookAsync(missingId));
    }

    [Fact]
    public async Task RemoveBook_BookOnActiveLoan_ThrowsInvalidOperationException()
    {
        // Arrange
        var book = new Book("Title", "Author", "isbn", BookCategory.Fiction, 2020);
        var loan = new Loan(book.Id, Guid.NewGuid());

        _bookRepo.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _loanRepo.Setup(r => r.GetActiveLoanForBookAsync(book.Id)).ReturnsAsync(loan);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.RemoveBookAsync(book.Id));
    }

    // ════════════════════════════════════════════════════════════════════════
    // RegisterMember
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task RegisterMember_ValidData_AddsMemberAndReturnsIt()
    {
        // Arrange
        _memberRepo.Setup(r => r.GetByEmailAsync("john@test.com")).ReturnsAsync((Member?)null);
        _memberRepo.Setup(r => r.AddAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);

        // Act
        var member = await _sut.RegisterMemberAsync("John Doe", "john@test.com");

        // Assert
        Assert.Equal("John Doe", member.Name);
        _memberRepo.Verify(r => r.AddAsync(It.IsAny<Member>()), Times.Once);
    }

    [Fact]
    public async Task RegisterMember_DuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange — email already registered
        _memberRepo.Setup(r => r.GetByEmailAsync("dupe@test.com"))
                   .ReturnsAsync(new Member("Existing", "dupe@test.com"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.RegisterMemberAsync("Other User", "dupe@test.com"));
    }

    // ════════════════════════════════════════════════════════════════════════
    // BorrowBook
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task BorrowBook_HappyPath_CreatesLoanAndMarksBookUnavailable()
    {
        // Arrange
        var book   = new Book("Book", "Author", "isbn", BookCategory.Fiction, 2020) { IsAvailable = true };
        var member = new Member("Alice", "alice@test.com");

        SetupBorrow(book, member, existingLoanCount: 0);

        // Act
        var loan = await _sut.BorrowBookAsync(book.Id, member.Id);

        // Assert
        Assert.Equal(book.Id,   loan.BookId);
        Assert.Equal(member.Id, loan.MemberId);
        Assert.Equal(LoanStatus.Active, loan.Status);
        Assert.False(book.IsAvailable);          // side-effect: book marked unavailable
        _loanRepo.Verify(r => r.AddAsync(It.IsAny<Loan>()), Times.Once);
    }

    [Fact]
    public async Task BorrowBook_BookNotAvailable_ThrowsBookNotAvailableException()
    {
        // Arrange
        var book   = new Book("Book", "Author", "isbn", BookCategory.Fiction, 2020) { IsAvailable = false };
        var member = new Member("Bob", "bob@test.com");

        _bookRepo.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _memberRepo.Setup(r => r.GetByIdAsync(member.Id)).ReturnsAsync(member);

        // Act & Assert
        await Assert.ThrowsAsync<BookNotAvailableException>(() =>
            _sut.BorrowBookAsync(book.Id, member.Id));
    }

    [Fact]
    public async Task BorrowBook_MemberAtLoanLimit_ThrowsInvalidOperationException()
    {
        // Arrange — member already holds their maximum of 2 loans
        var book   = new Book("Book", "Author", "isbn", BookCategory.Fiction, 2020) { IsAvailable = true };
        var member = new Member("Charlie", "charlie@test.com") { MaxLoansAllowed = 2 };

        SetupBorrow(book, member, existingLoanCount: 2);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.BorrowBookAsync(book.Id, member.Id));
    }

    // ════════════════════════════════════════════════════════════════════════
    // ReturnBook
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task ReturnBook_HappyPath_ClosesLoanAndRestoresAvailability()
    {
        // Arrange
        var book   = new Book("Book", "Author", "isbn", BookCategory.Fiction, 2020) { IsAvailable = false };
        var member = new Member("Dave", "dave@test.com");
        var loan   = new Loan(book.Id, member.Id) { Status = LoanStatus.Active };

        SetupReturn(loan, book, member);

        // Act
        var result = await _sut.ReturnBookAsync(loan.Id);

        // Assert
        Assert.Equal(LoanStatus.Returned, result.Status);
        Assert.NotNull(result.ReturnedDate);
        Assert.True(book.IsAvailable);
        _loanRepo.Verify(r => r.UpdateAsync(It.IsAny<Loan>()), Times.Once);
        _bookRepo.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public async Task ReturnBook_AlreadyReturned_ThrowsInvalidOperationException()
    {
        // Arrange
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid()) { Status = LoanStatus.Returned };
        _loanRepo.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ReturnBookAsync(loan.Id));
    }

    // ════════════════════════════════════════════════════════════════════════
    // Events (Day 19 — delegate-based events)
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task BorrowBook_FiresBookBorrowedEvent_WithCorrectArgs()
    {
        // Arrange
        var book   = new Book("Book", "Author", "isbn", BookCategory.Fiction, 2020) { IsAvailable = true };
        var member = new Member("Eve", "eve@test.com");

        SetupBorrow(book, member, existingLoanCount: 0);

        LoanEventArgs? captured = null;
        _sut.BookBorrowed += (_, args) => captured = args;

        // Act
        await _sut.BorrowBookAsync(book.Id, member.Id);

        // Assert — event fired and carried the right book and member
        Assert.NotNull(captured);
        Assert.Equal(book.Id,   captured.Book.Id);
        Assert.Equal(member.Id, captured.Member.Id);
    }

    [Fact]
    public async Task ReturnBook_FiresBookReturnedEvent_WithCorrectArgs()
    {
        // Arrange
        var book   = new Book("Book", "Author", "isbn", BookCategory.Fiction, 2020) { IsAvailable = false };
        var member = new Member("Frank", "frank@test.com");
        var loan   = new Loan(book.Id, member.Id) { Status = LoanStatus.Active };

        SetupReturn(loan, book, member);

        LoanEventArgs? captured = null;
        _sut.BookReturned += (_, args) => captured = args;

        // Act
        await _sut.ReturnBookAsync(loan.Id);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(book.Id, captured.Book.Id);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Private helpers — reduce test setup duplication
    // ════════════════════════════════════════════════════════════════════════

    private void SetupBorrow(Book book, Member member, int existingLoanCount)
    {
        var existingLoans = Enumerable
            .Range(0, existingLoanCount)
            .Select(_ => new Loan(Guid.NewGuid(), member.Id))
            .ToList();

        _bookRepo.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _memberRepo.Setup(r => r.GetByIdAsync(member.Id)).ReturnsAsync(member);
        _loanRepo.Setup(r => r.GetActiveLoansByMemberAsync(member.Id))
                 .ReturnsAsync(existingLoans);
        _loanRepo.Setup(r => r.AddAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask);
        _bookRepo.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
    }

    private void SetupReturn(Loan loan, Book book, Member member)
    {
        _loanRepo.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);
        _bookRepo.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _memberRepo.Setup(r => r.GetByIdAsync(member.Id)).ReturnsAsync(member);
        _loanRepo.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask);
        _bookRepo.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
    }
}
