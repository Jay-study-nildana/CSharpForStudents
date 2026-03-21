// 08-EfRepository.cs
// Simplified repository implementation (no EF dependency). Shows how a concrete low-level module depends on an abstraction.

using System;
using System.Collections.Generic;
using System.Linq;

// A simple in-memory repository that implements IRepository<T> for demos.
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _store = new();

    public void Add(T entity) => _store.Add(entity);

    public IEnumerable<T> GetAll() => _store.ToList();

    public T GetById(int id)
    {
        // Placeholder: real implementation would use an ID property or EF key.
        throw new NotImplementedException("Demo placeholder; adapt to real domain.");
    }

    public void Remove(T entity) => _store.Remove(entity);
}