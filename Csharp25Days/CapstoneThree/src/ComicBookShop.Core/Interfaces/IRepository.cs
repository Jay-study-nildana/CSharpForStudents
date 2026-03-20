using ComicBookShop.Core.Entities;

namespace ComicBookShop.Core.Interfaces;

/// <summary>
/// Generic repository contract for CRUD + query operations.
/// Demonstrates generics and type constraints (Day 12).
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyList<T>> FindAsync(Func<T, bool> predicate);
    Task<int> CountAsync();
}
