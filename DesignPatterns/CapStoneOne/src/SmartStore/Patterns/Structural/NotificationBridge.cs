namespace SmartStore.Patterns.Structural;

// ================================================================
// BRIDGE PATTERN
// ================================================================
// Decouples the notification abstraction (WHAT event) from
// the delivery implementation (HOW it is delivered).
// Both hierarchies can evolve independently.
//
// Intent   : Decouple an abstraction from its implementation so that
//            the two can vary independently.
// Problem  : We need OrderConfirmed / OrderCancelled notifications
//            deliverable via Console, Email, or SMS. That's 2×3 = 6
//            combinations — a class explosion if we use inheritance.
// Solution : Abstraction (OrderNotification) holds a reference to the
//            implementor (INotificationChannel). We compose at runtime.
//
//  Abstraction hierarchy     |  Implementor hierarchy
//  ──────────────────────    |  ──────────────────────
//  OrderNotification         |  INotificationChannel
//    OrderConfirmedNotification|    ConsoleNotificationChannel
//    OrderCancelledNotification|    EmailNotificationChannel
// ================================================================

// ---- Implementor side (INotificationChannel is in Core.Interfaces) ----

public class ConsoleNotificationChannel : INotificationChannel
{
    public string ChannelName => "Console";

    public void Send(string recipient, string subject, string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  [Console Notification]");
        Console.WriteLine($"    To      : {recipient}");
        Console.WriteLine($"    Subject : {subject}");
        Console.WriteLine($"    Message : {message}");
        Console.ResetColor();
    }
}

public class EmailNotificationChannel : INotificationChannel
{
    public string ChannelName => "Email";

    public void Send(string recipient, string subject, string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  [Email Notification]  → {recipient}@smartstore.com");
        Console.WriteLine($"    Subject : {subject}");
        Console.WriteLine($"    Body    : {message}");
        Console.ResetColor();
    }
}

// ---- Abstraction side ----

public abstract class OrderNotification
{
    protected readonly INotificationChannel _channel;

    protected OrderNotification(INotificationChannel channel) =>
        _channel = channel;

    public abstract void Send(Order order);
}

public class OrderConfirmedNotification : OrderNotification
{
    public OrderConfirmedNotification(INotificationChannel channel) : base(channel) { }

    public override void Send(Order order) =>
        _channel.Send(
            order.Customer.Name,
            $"Order #{order.Id} Confirmed",
            $"Your order of ${order.Total:F2} has been confirmed. Thank you, {order.Customer.Name}!"
        );
}

public class OrderCancelledNotification : OrderNotification
{
    public OrderCancelledNotification(INotificationChannel channel) : base(channel) { }

    public override void Send(Order order) =>
        _channel.Send(
            order.Customer.Name,
            $"Order #{order.Id} Cancelled",
            $"Your order #{order.Id} has been cancelled. Contact support for details."
        );
}
