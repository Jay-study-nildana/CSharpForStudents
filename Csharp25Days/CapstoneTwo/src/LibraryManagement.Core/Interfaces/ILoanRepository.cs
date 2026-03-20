using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Interfaces;

/// <summary>Loan-specific query operations.</summary>
public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<Loan>> GetActiveLoansByMemberAsync(Guid memberId);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<Loan?> GetActiveLoanForBookAsync(Guid bookId);
}
