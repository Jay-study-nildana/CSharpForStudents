// 10-UnitOfWork.cs
// Simple in-memory UnitOfWork that returns repositories and simulates commit/rollback.
// Designed to show structure; not a production implementation.

using System;
using System.Collections.Concurrent;

public class InMemoryUnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        var repo = (IRepository<T>)_repositories.GetOrAdd(type, _ => new InMemoryRepository<T>());
        return repo;
    }

    public void Commit()
    {
        // In a real UoW, persist changes to the database here
        Console.WriteLine("Committing transaction (simulated).");
    }

    public void Rollback()
    {
        Console.WriteLine("Rollback (simulated).");
    }

    public void Dispose()
    {
        // cleanup
    }
}