namespace SmartStore.Patterns.Behavioral;

// ================================================================
// OBSERVER PATTERN
// ================================================================
// OrderEventManager is the Subject (publisher).
// IOrderObserver instances are Observers (subscribers).
// When an order event fires, all registered observers are notified.
//
// Intent   : Define a one-to-many dependency so that when one object
//            changes state, all its dependents are notified automatically.
// Problem  : When an order is confirmed, several independent subsystems
//            (email, inventory, audit) must react — but the order service
//            should not be coupled to any of them.
// Solution : Observers subscribe to the event manager. The order service
//            calls Notify() and is unaware of how many or which observers
//            handle the event.
// ================================================================
public interface IOrderObserver
{
    string ObserverName { get; }
    void OnOrderEvent(string eventName, Order order);
}

public class OrderEventManager
{
    private readonly List<IOrderObserver> _observers = new();

    public void Subscribe(IOrderObserver observer)
    {
        _observers.Add(observer);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"  [Observer] '{observer.ObserverName}' subscribed.");
        Console.ResetColor();
    }

    public void Unsubscribe(IOrderObserver observer)
    {
        _observers.Remove(observer);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"  [Observer] '{observer.ObserverName}' unsubscribed.");
        Console.ResetColor();
    }

    public void Notify(Order order, string eventName = "OrderConfirmed")
    {
        Console.WriteLine($"  [Observer] Broadcasting '{eventName}' to {_observers.Count} subscriber(s)...");
        foreach (var obs in _observers)
            obs.OnOrderEvent(eventName, order);
    }
}

// ---- Concrete Observers ----

public class EmailObserver : IOrderObserver
{
    public string ObserverName => "EmailObserver";

    public void OnOrderEvent(string eventName, Order order)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  [EmailObserver]     → Sending '{eventName}' email to {order.Customer.Name}");
        Console.ResetColor();
    }
}

public class InventoryObserver : IOrderObserver
{
    public string ObserverName => "InventoryObserver";

    public void OnOrderEvent(string eventName, Order order)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [InventoryObserver] → Updating warehouse records for Order #{order.Id}");
        Console.ResetColor();
    }
}

public class AuditLogObserver : IOrderObserver
{
    public string ObserverName => "AuditLogObserver";
    private readonly List<string> _log = new();

    public void OnOrderEvent(string eventName, Order order)
    {
        var entry = $"{DateTime.Now:HH:mm:ss}  |  {eventName,-20}  |  Order #{order.Id}  |  " +
                    $"{order.Customer.Name,-15}  |  ${order.Total:F2}";
        _log.Add(entry);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"  [AuditLog]          → {entry}");
        Console.ResetColor();
    }

    public IReadOnlyList<string> GetLog() => _log.AsReadOnly();
}
