using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Interfaces;

/// <summary>Book-specific query operations on top of the generic CRUD contract.</summary>
public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> SearchAsync(string query);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task<Book?> GetByIsbnAsync(string isbn);
}
