namespace LibraryManagement.Core.Models;

/// <summary>
/// Records the borrowing of a book by a member, including its lifecycle (Active → Returned).
/// </summary>
public class Loan
{
    // --- Identity (immutable) ---
    public Guid Id { get; init; }
    public DateTime BorrowedDate { get; init; }

    // --- Loan data ---
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public LoanStatus Status { get; set; }

    /// <summary>Computed: a loan is overdue when it is still Active past its DueDate.</summary>
    public bool IsOverdue => Status == LoanStatus.Active && DateTime.UtcNow > DueDate;

    public Loan(Guid bookId, Guid memberId, int loanDurationDays = 14)
    {
        Id = Guid.NewGuid();
        BookId = bookId;
        MemberId = memberId;
        BorrowedDate = DateTime.UtcNow;
        DueDate = BorrowedDate.AddDays(loanDurationDays);
        Status = LoanStatus.Active;
    }

    /// <summary>Parameterless constructor required for JSON deserialization.</summary>
    public Loan() { }

    public override string ToString() =>
        $"[{Id}] Book: {BookId} | Member: {MemberId} | Borrowed: {BorrowedDate:yyyy-MM-dd} | Due: {DueDate:yyyy-MM-dd} | {Status}";
}
