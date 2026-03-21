// 05-IUnitOfWorkInterface.cs
// Purpose: Define IUnitOfWork contract exposing repositories and transactional operations.
// DI/Lifetime: UoW should be Scoped when backed by DbContext; for in-memory tests use Transient per test.
// Testability note: Services depend on IUnitOfWork so tests can inject a fake implementation to observe commit behavior.

using System;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    IRepository<Customer> Customers { get; } // assume Customer is a domain POCO
    /// <summary>Commit persisted changes as a transaction. Throws on failure.</summary>
    void Commit();

    /// <summary>Rollback any uncommitted changes (optional for some implementations).</summary>
    void Rollback();
}

// Simple Customer placeholder
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}