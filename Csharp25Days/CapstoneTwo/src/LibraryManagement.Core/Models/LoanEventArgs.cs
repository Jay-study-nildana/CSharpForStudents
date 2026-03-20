namespace LibraryManagement.Core.Models;

/// <summary>
/// Event data for BookBorrowed / BookReturned events raised by ILibraryService.
/// Bundles loan, book, and member so subscribers have full context.
/// Demonstrates EventArgs pattern (Day 19 — delegates &amp; events).
/// </summary>
public sealed class LoanEventArgs : EventArgs
{
    public Loan Loan { get; }
    public Book Book { get; }
    public Member Member { get; }

    public LoanEventArgs(Loan loan, Book book, Member member)
    {
        Loan = loan;
        Book = book;
        Member = member;
    }
}
