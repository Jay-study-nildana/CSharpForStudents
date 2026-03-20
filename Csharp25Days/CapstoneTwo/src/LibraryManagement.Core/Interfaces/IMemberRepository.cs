using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Interfaces;

/// <summary>Member-specific query operations.</summary>
public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByEmailAsync(string email);
    Task<IEnumerable<Member>> SearchAsync(string query);
}
