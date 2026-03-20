using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Infrastructure.Repositories;

/// <summary>JSON-backed implementation of ILoanRepository.</summary>
public sealed class JsonLoanRepository : JsonRepositoryBase<Loan>, ILoanRepository
{
    public JsonLoanRepository(string dataDirectory)
        : base(Path.Combine(dataDirectory, "loans.json")) { }

    protected override Guid GetId(Loan entity) => entity.Id;

    public async Task<IEnumerable<Loan>> GetActiveLoansByMemberAsync(Guid memberId)
    {
        var all = await GetAllAsync();
        return all.Where(l => l.MemberId == memberId && l.Status == LoanStatus.Active);
    }

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
    {
        var all = await GetAllAsync();
        // A loan is overdue when it is still Active and past its DueDate
        return all.Where(l => l.Status == LoanStatus.Active && DateTime.UtcNow > l.DueDate);
    }

    public async Task<Loan?> GetActiveLoanForBookAsync(Guid bookId)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(l => l.BookId == bookId && l.Status == LoanStatus.Active);
    }
}
