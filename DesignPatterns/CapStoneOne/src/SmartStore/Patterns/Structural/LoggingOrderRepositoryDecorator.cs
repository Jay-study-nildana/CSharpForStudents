namespace SmartStore.Patterns.Structural;

// ================================================================
// DECORATOR PATTERN
// ================================================================
// LoggingOrderRepositoryDecorator wraps any IOrderRepository
// and adds structured logging before forwarding every call.
//
// Intent   : Attach additional responsibilities to an object dynamically,
//            as an alternative to subclassing for extending functionality.
// Problem  : We want logging on the repository without changing
//            InMemoryOrderRepository or every future implementation.
// Solution : The decorator implements IOrderRepository, wraps an inner
//            instance, logs, then delegates to the inner object.
//            Multiple decorators can be stacked (e.g. logging + caching).
// ================================================================
public class LoggingOrderRepositoryDecorator : IOrderRepository
{
    private readonly IOrderRepository _inner;

    public LoggingOrderRepositoryDecorator(IOrderRepository inner) =>
        _inner = inner;

    public Order? GetById(int id)
    {
        Log($"GetById({id})");
        return _inner.GetById(id);
    }

    public IEnumerable<Order> GetAll()
    {
        Log("GetAll()");
        return _inner.GetAll();
    }

    public void Add(Order order)
    {
        Log($"Add(Order #{order.Id}, Customer={order.Customer.Name})");
        _inner.Add(order);
    }

    public void Update(Order order)
    {
        Log($"Update(Order #{order.Id}, Status={order.Status})");
        _inner.Update(order);
    }

    public void Remove(int id)
    {
        Log($"Remove(id={id})");
        _inner.Remove(id);
    }

    private static void Log(string operation)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [Decorator:Log] OrderRepository.{operation}");
        Console.ResetColor();
    }
}
