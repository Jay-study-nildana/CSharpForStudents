namespace LibraryManagement.Core.Interfaces;

/// <summary>
/// Generic CRUD repository contract.
/// Using generics (Day 12) so each entity type gets a type-safe repository
/// without duplicating method signatures.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
