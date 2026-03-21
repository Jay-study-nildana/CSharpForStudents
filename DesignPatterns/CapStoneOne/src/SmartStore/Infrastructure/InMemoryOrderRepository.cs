namespace SmartStore.Infrastructure;

// -------------------------------------------------------
// REPOSITORY PATTERN — In-Memory implementation
// -------------------------------------------------------
// Stores orders in a dictionary keyed by Order.Id.
// In a real app this would be replaced by an EF Core implementation
// without any changes to the domain or application layers.
// -------------------------------------------------------
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _store = new();

    public Order? GetById(int id) =>
        _store.TryGetValue(id, out var order) ? order : null;

    public IEnumerable<Order> GetAll() =>
        _store.Values.OrderBy(o => o.Id).ToList();

    public void Add(Order order)    => _store[order.Id] = order;
    public void Update(Order order) => _store[order.Id] = order;
    public void Remove(int id)      => _store.Remove(id);
}
