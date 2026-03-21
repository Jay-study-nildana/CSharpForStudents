// 01-OrderRepositoryInterfaces.cs
// Purpose: Define IRepository<T> and IOrderRepository for the Orders domain.
// DI/Lifetime recommendation: Register repositories as Scoped when backed by DbContext; in-memory repos can be Transient in tests.
// Testability note: Code depends on abstractions (IRepository/IOrderRepository) so tests can inject fakes or mocks.

using System.Collections.Generic;

/// <summary>
/// Generic repository abstraction for basic CRUD operations.
/// Responsibilities: simple persistence and retrieval of aggregate roots. Avoid exposing ORM specifics here.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>Get entity by integer Id.</summary>
    T GetById(int id);

    /// <summary>Return all entities.</summary>
    IEnumerable<T> GetAll();

    /// <summary>Add a new entity (assign Id if required).</summary>
    void Add(T entity);

    /// <summary>Update an existing entity.</summary>
    void Update(T entity);

    /// <summary>Remove entity.</summary>
    void Remove(T entity);
}

/// <summary>
/// Order-specific repository with query methods tailored to the Order aggregate.
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>Return orders for a specific customer.</summary>
    IEnumerable<Order> GetByCustomerId(int customerId);

    /// <summary>Return DTOs optimized for read operations (example of read-model).</summary>
    IEnumerable<OrderDto> GetOrderDtosByCustomer(int customerId);
}