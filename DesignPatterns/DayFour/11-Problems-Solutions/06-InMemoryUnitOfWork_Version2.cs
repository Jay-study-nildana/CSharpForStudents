// 06-InMemoryUnitOfWork.cs
// Purpose: Provide an InMemoryUnitOfWork for tests that returns in-memory repositories and simulates commit/rollback.
// DI/Lifetime: Use Transient for isolated tests; Registry/inspection helpers can be used to assert pending changes.
// Testability note: Allows asserting whether Commit was called and what changes were staged.

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// InMemoryUnitOfWork collects repository instances and simulates commit/rollback.
/// For simplicity, repositories modify their backing store immediately; we track commits for assertions.
/// </summary>
public class InMemoryUnitOfWork : IUnitOfWork
{
    public IOrderRepository Orders { get; }
    public IRepository<Customer> Customers { get; }

    // test inspection flags
    public bool Committed { get; private set; }
    public bool RolledBack { get; private set; }

    public InMemoryUnitOfWork()
    {
        Orders = new InMemoryOrderRepository();
        Customers = new InMemoryRepository<Customer>();
        Committed = false;
        RolledBack = false;
    }

    // In real UoW this would persist changes atomically; here we just mark the checkpoint.
    public void Commit()
    {
        Committed = true;
        RolledBack = false;
        // Could snapshot state or signal to repositories if needed.
    }

    public void Rollback()
    {
        RolledBack = true;
        Committed = false;
        // For full rollback simulation, we'd need transactional staging; omitted for brevity.
    }

    public void Dispose()
    {
        // cleanup if required
    }
}