using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Tests.Models;

/// <summary>
/// Tests for the Loan domain model — lifecycle, due-date calculation, and IsOverdue computed property.
/// Demonstrates testing computed (derived) properties and state transitions (Day 21).
/// </summary>
public class LoanTests
{
    [Fact]
    public void NewLoan_StatusIsActive()
    {
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());
        Assert.Equal(LoanStatus.Active, loan.Status);
    }

    [Fact]
    public void NewLoan_DueDateIs14DaysAhead_ByDefault()
    {
        var before = DateTime.UtcNow;
        var loan   = new Loan(Guid.NewGuid(), Guid.NewGuid());
        var after  = DateTime.UtcNow;

        // DueDate should be BorrowedDate + 14 days, so between before+14 and after+14
        Assert.InRange(loan.DueDate,
            before.AddDays(14).AddSeconds(-1),
            after.AddDays(14).AddSeconds(1));
    }

    [Fact]
    public void NewLoan_WithCustomDuration_HasCorrectDueDate()
    {
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid(), loanDurationDays: 7);
        Assert.Equal(loan.BorrowedDate.AddDays(7).Date, loan.DueDate.Date);
    }

    [Fact]
    public void IsOverdue_WhenActivePastDueDate_ReturnsTrue()
    {
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid())
        {
            DueDate = DateTime.UtcNow.AddDays(-1),
            Status  = LoanStatus.Active
        };

        Assert.True(loan.IsOverdue);
    }

    [Fact]
    public void IsOverdue_WhenNotYetDue_ReturnsFalse()
    {
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid())
        {
            DueDate = DateTime.UtcNow.AddDays(7),
            Status  = LoanStatus.Active
        };

        Assert.False(loan.IsOverdue);
    }

    [Fact]
    public void IsOverdue_WhenReturnedEvenIfPastDueDate_ReturnsFalse()
    {
        // A returned loan is closed, so it cannot be "overdue"
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid())
        {
            DueDate = DateTime.UtcNow.AddDays(-5),
            Status  = LoanStatus.Returned
        };

        Assert.False(loan.IsOverdue);
    }

    [Fact]
    public void NewLoan_HasNonEmptyId()
    {
        var loan = new Loan(Guid.NewGuid(), Guid.NewGuid());
        Assert.NotEqual(Guid.Empty, loan.Id);
    }

    [Fact]
    public void TwoNewLoans_HaveUniqueIds()
    {
        var l1 = new Loan(Guid.NewGuid(), Guid.NewGuid());
        var l2 = new Loan(Guid.NewGuid(), Guid.NewGuid());
        Assert.NotEqual(l1.Id, l2.Id);
    }
}
