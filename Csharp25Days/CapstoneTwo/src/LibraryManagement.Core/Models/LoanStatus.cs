namespace LibraryManagement.Core.Models;

/// <summary>Lifecycle state of a library loan.</summary>
public enum LoanStatus
{
    Active,
    Returned,
    Overdue
}
