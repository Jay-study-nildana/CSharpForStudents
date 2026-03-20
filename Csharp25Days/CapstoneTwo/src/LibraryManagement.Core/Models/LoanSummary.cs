namespace LibraryManagement.Core.Models;

/// <summary>
/// Immutable read-only DTO that combines a Loan with the book title and member name.
/// Demonstrates record types and the DTO vs domain-model separation (Day 10 &amp; Day 26).
/// </summary>
public record LoanSummary(
    Guid LoanId,
    Guid BookId,
    string BookTitle,
    Guid MemberId,
    string MemberName,
    DateTime BorrowedDate,
    DateTime DueDate,
    LoanStatus Status,
    bool IsOverdue);
