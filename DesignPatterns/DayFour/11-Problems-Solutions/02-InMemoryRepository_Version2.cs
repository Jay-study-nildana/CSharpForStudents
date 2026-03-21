// 02-InMemoryRepository.cs
// Purpose: Provide a simple thread-unsafe InMemoryRepository<T> implementing IRepository<T>.
// DI/Lifetime recommendation: For tests, create new instances per test (Transient). For concurrent access, wrap operations with locks or use concurrent collections.
// Testability note: Simple in-memory repo is useful for fast unit tests and for teaching repo contract semantics.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Simple in-memory repository. Assumes entity has public int Id { get; set; } property.
/// Not thread-safe by default. For production-concurrent scenarios, add locking or use ConcurrentDictionary.
/// </summary>
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _store = new();
    private int _nextId = 1;
    private readonly PropertyInfo _idProp;

    public InMemoryRepository()
    {
        // Find Id property for simple id assignment; throws if not found.
        _idProp = typeof(T).GetProperty("Id") ?? throw new InvalidOperationException("Entity must have an int Id property.");
    }

    public T GetById(int id)
    {
        return _store.FirstOrDefault(e => (int)_idProp.GetValue(e) == id);
    }

    public IEnumerable<T> GetAll() => _store.ToList();

    public void Add(T entity)
    {
        // assign next id if zero
        var id = (int)_idProp.GetValue(entity);
        if (id == 0)
        {
            _idProp.SetValue(entity, _nextId++);
        }
        _store.Add(entity);
    }

    public void Update(T entity)
    {
        var id = (int)_idProp.GetValue(entity);
        var existing = GetById(id);
        if (existing == null) throw new InvalidOperationException($"Entity with Id {id} not found.");
        // naive replace: remove and add (for class reference semantics)
        _store.Remove(existing);
        _store.Add(entity);
    }

    public void Remove(T entity)
    {
        _store.Remove(entity);
    }
}