using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Infrastructure.Repositories;

/// <summary>JSON-backed implementation of IMemberRepository.</summary>
public sealed class JsonMemberRepository : JsonRepositoryBase<Member>, IMemberRepository
{
    public JsonMemberRepository(string dataDirectory)
        : base(Path.Combine(dataDirectory, "members.json")) { }

    protected override Guid GetId(Member entity) => entity.Id;

    public async Task<Member?> GetByEmailAsync(string email)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(m =>
            m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Member>> SearchAsync(string query)
    {
        var all = await GetAllAsync();
        return all.Where(m =>
            m.Name.Contains(query,  StringComparison.OrdinalIgnoreCase) ||
            m.Email.Contains(query, StringComparison.OrdinalIgnoreCase));
    }
}
