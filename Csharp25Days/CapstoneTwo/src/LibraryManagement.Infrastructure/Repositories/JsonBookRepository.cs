using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Infrastructure.Repositories;

/// <summary>JSON-backed implementation of IBookRepository.</summary>
public sealed class JsonBookRepository : JsonRepositoryBase<Book>, IBookRepository
{
    public JsonBookRepository(string dataDirectory)
        : base(Path.Combine(dataDirectory, "books.json")) { }

    protected override Guid GetId(Book entity) => entity.Id;

    public async Task<IEnumerable<Book>> SearchAsync(string query)
    {
        var all = await GetAllAsync();
        // LINQ Where + multi-field OR predicate (Day 13)
        return all.Where(b =>
            b.Title.Contains(query,    StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query,   StringComparison.OrdinalIgnoreCase) ||
            b.ISBN.Contains(query,     StringComparison.OrdinalIgnoreCase) ||
            b.Category.ToString().Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        var all = await GetAllAsync();
        return all.Where(b => b.IsAvailable);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(b =>
            b.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase));
    }
}
