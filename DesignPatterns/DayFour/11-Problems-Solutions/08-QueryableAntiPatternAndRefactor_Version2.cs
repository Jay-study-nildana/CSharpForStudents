// 08-QueryableAntiPatternAndRefactor.cs
// Purpose: Show anti-pattern of exposing IQueryable<T> and refactor to safe query methods returning DTOs/paged results.
// DI/Lifetime: Repository methods should not leak ORM-specific queryables to domain; prefer explicit query APIs.
// Testability note: Returning IEnumerable/DTOs keeps repositories easy to fake for tests.

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Anti-pattern: exposing IQueryable directly from repository (example, pretend repository returns IQueryable).
/// This couples caller to ORM and can cause behavior differences in tests vs production.
/// </summary>
public interface IBadOrderRepository
{
    IQueryable<Order> Query(); // anti-pattern
}

/// <summary>
/// Refactor: Provide explicit, safe query methods. Example: GetOrderDtosByCustomer and paged query.
/// </summary>
public interface IGoodOrderRepository : IOrderRepository
{
    // Paged query returns a slice of DTOs and total count
    (IEnumerable<OrderDto> Items, int TotalCount) GetPagedDtosByCustomer(int customerId, int page, int pageSize);
}

/// <summary>
/// Example implementation on top of IOrderRepository (in-memory) showing safe paging.
/// </summary>
public class GoodOrderRepositoryAdapter : IGoodOrderRepository
{
    private readonly IOrderRepository _inner;
    public GoodOrderRepositoryAdapter(IOrderRepository inner) => _inner = inner;

    public void Add(Order entity) => _inner.Add(entity);
    public IEnumerable<Order> GetAll() => _inner.GetAll();
    public Order GetById(int id) => _inner.GetById(id);
    public void Remove(Order entity) => _inner.Remove(entity);
    public void Update(Order entity) => _inner.Update(entity);

    public IEnumerable<Order> GetByCustomerId(int customerId) => _inner.GetByCustomerId(customerId);

    public IEnumerable<OrderDto> GetOrderDtosByCustomer(int customerId) => _inner.GetOrderDtosByCustomer(customerId);

    public (IEnumerable<OrderDto> Items, int TotalCount) GetPagedDtosByCustomer(int customerId, int page, int pageSize)
    {
        var dtos = _inner.GetOrderDtosByCustomer(customerId).ToList();
        var total = dtos.Count;
        var items = dtos.Skip((page - 1) * pageSize).Take(pageSize);
        return (items, total);
    }
}

/*
Tradeoffs:
- IQueryable allows caller to build queries but couples to provider (EF, LINQ-to-SQL). Tests using in-memory collections may behave differently.
- Explicit query APIs increase method count but keep boundaries clear and portable.
*/