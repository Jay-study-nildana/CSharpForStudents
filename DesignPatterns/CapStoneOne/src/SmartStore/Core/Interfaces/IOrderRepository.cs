namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// REPOSITORY PATTERN
// -------------------------------------------------------
// Abstracts data access for Order aggregates.
// Domain logic depends on this interface, not on any DB/ORM.
// -------------------------------------------------------
public interface IOrderRepository
{
    Order? GetById(int id);
    IEnumerable<Order> GetAll();
    void Add(Order order);
    void Update(Order order);
    void Remove(int id);
}
