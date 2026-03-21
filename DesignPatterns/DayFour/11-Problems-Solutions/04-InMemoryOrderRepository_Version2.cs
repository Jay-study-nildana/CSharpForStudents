// 04-InMemoryOrderRepository.cs
// Purpose: Implement IOrderRepository on top of InMemoryRepository<Order> and add GetByCustomerId and DTO queries.
// DI/Lifetime: Use Transient per test or Scoped in a per-request simulation; in-memory data is ephemeral.
// Testability note: Useful for unit/integration tests that avoid databases.

using System.Collections.Generic;
using System.Linq;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly InMemoryRepository<Order> _inner = new();

    public void Add(Order entity) => _inner.Add(entity);

    public IEnumerable<Order> GetAll() => _inner.GetAll();

    public Order GetById(int id) => _inner.GetById(id);

    public void Remove(Order entity) => _inner.Remove(entity);

    public void Update(Order entity) => _inner.Update(entity);

    public IEnumerable<Order> GetByCustomerId(int customerId)
    {
        return _inner.GetAll().Where(o => o.CustomerId == customerId).ToList();
    }

    public IEnumerable<OrderDto> GetOrderDtosByCustomer(int customerId)
    {
        return GetByCustomerId(customerId).Select(OrderMapper.ToDto).ToList();
    }
}