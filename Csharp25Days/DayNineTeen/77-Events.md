# Events, Event Patterns and Publisher/Subscriber Basics (C# / .NET)

Purpose
- Explain .NET events and common event patterns.
- Show publisher/subscriber basics, safe invocation, async/event aggregates, and practical guidance for use in your capstone.
- Provide concrete C# examples students can reuse.

Why use events?
- Events decouple the code that produces an occurrence (publisher) from the code that reacts to it (subscriber).
- This supports extensibility (multiple independent subscribers), separation of concerns, and simple notification flows (e.g., "OrderPlaced" -> email, audit, analytics).

Core concepts
- Delegate: a typed reference to a method signature.
- Event: a language-level wrapper around a delegate that restricts who can invoke it; typically declared with EventHandler or EventHandler<TEventArgs>.
- Publisher: owns and raises the event.
- Subscriber: registers a handler to react to the event.

Simple event pattern
```csharp
public class OrderPlacedEventArgs : EventArgs
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public DateTime PlacedAt { get; init; }
}

public class OrderService
{
    // public event using EventHandler<T>
    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

    // Protected virtual OnX method is the recommended pattern
    protected virtual void OnOrderPlaced(OrderPlacedEventArgs e)
    {
        // Take a snapshot for thread-safety
        var handler = OrderPlaced;
        handler?.Invoke(this, e);
    }

    public void PlaceOrder(Guid orderId, Guid userId)
    {
        // ... perform placement logic ...
        OnOrderPlaced(new OrderPlacedEventArgs { OrderId = orderId, UserId = userId, PlacedAt = DateTime.UtcNow });
    }
}
```

Subscriber example
```csharp
public class EmailNotifier
{
    public void HandleOrderPlaced(object? sender, OrderPlacedEventArgs e)
    {
        // send email
        Console.WriteLine($"Email: Order {e.OrderId} for user {e.UserId}");
    }
}

// Registration (composition root or wiring code)
var orderService = new OrderService();
var email = new EmailNotifier();
orderService.OrderPlaced += email.HandleOrderPlaced;

// Unsubscribe when no longer interested or on dispose
orderService.OrderPlaced -= email.HandleOrderPlaced;
```
- Always unsubscribe if the subscriber has shorter lifetime than the publisher to avoid memory leaks.

Thread-safety and invocation best practices
- Use a snapshot of the delegate reference before invoking to avoid race conditions:
  var handler = OrderPlaced; handler?.Invoke(this, e);
- Avoid throwing exceptions from one handler that stops other subscribers. Consider catching and aggregating exceptions:
```csharp
protected virtual void OnOrderPlaced(OrderPlacedEventArgs e)
{
    var handler = OrderPlaced;
    if (handler == null) return;

    var exceptions = new List<Exception>();
    foreach (Delegate d in handler.GetInvocationList())
    {
        try
        {
            d.DynamicInvoke(this, e);
        }
        catch (Exception ex)
        {
            exceptions.Add(ex.InnerException ?? ex);
        }
    }
    if (exceptions.Any()) throw new AggregateException(exceptions);
}
```
- Prefer logging and continuing for non-fatal handler errors to make event processing robust.

Async event pattern
- Traditional events are synchronous. For async handlers use a Task-based pattern:
```csharp
public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e);

public class AsyncOrderService
{
    public event AsyncEventHandler<OrderPlacedEventArgs>? OrderPlacedAsync;

    protected virtual async Task OnOrderPlacedAsync(OrderPlacedEventArgs e)
    {
        var handler = OrderPlacedAsync;
        if (handler == null) return;

        // Invoke handlers in parallel or sequentially depending on semantics
        var tasks = handler.GetInvocationList()
                           .Cast<AsyncEventHandler<OrderPlacedEventArgs>>()
                           .Select(h => h(this, e));
        await Task.WhenAll(tasks); // or await sequentially if ordering matters
    }
}
```
- Use async events when subscribers perform I/O (email, DB writes) and you want to await completion or handle failures.

Publisher/Subscriber at scale: Event Aggregator / Bus
- For many components consider an EventAggregator (in-process) or a message bus (out-of-process).
- Simple EventAggregator:
```csharp
public interface IEventAggregator
{
    void Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
    void Publish<T>(T @event);
}
```
- For distributed systems use messaging (RabbitMQ, Kafka) or a mediator library (MediatR) for reliable delivery, retries, and decoupling across processes.

Common pitfalls and guidance
- Memory leaks: subscribers must unsubscribe, especially GUI or short-lived objects subscribing to long-lived publishers.
- Hidden dependencies: events can hide the fact that actions will run; keep side-effects explicit in design documents.
- Ordering and atomicity: multiple subscribers run independently; if you need ordering, orchestration or a pipeline is clearer.
- Exception handling: don’t let one subscriber crash the publisher—log and continue where appropriate.

When to use events vs alternatives
- Use events for in-process, low-latency, push-style notifications (UI updates, simple notifications).
- Use an EventAggregator when many components listen to many event types within a process.
- Use message queues or a streaming platform for cross-process, durable, or replayable events.
- Use MediatR or similar for in-process request/response and pipeline behaviors (CQRS-style).

Capstone example: notification flow
- Event: OrderPlaced(OrderId, UserId)
- Subscribers:
  - EmailService (send confirmation)
  - AuditService (write audit record)
  - InventoryService (reserve stock)
  - AnalyticsService (track metrics)
- Wiring (composition root):
```csharp
orderService.OrderPlaced += (s,e) => emailService.Send(e.OrderId);
orderService.OrderPlaced += (s,e) => auditService.Write(e.OrderId, e.UserId);
orderService.OrderPlaced += (s,e) => inventoryService.Reserve(e.OrderId);
```
- Consider async event or an outbox pattern if any subscriber can fail or be slow; this keeps the placing operation responsive and resilient.

Advanced patterns & variations
- Event arguments best practice: favor immutable or readonly properties in EventArgs to avoid accidental changes by subscribers.
- Weak event pattern: useful for UI/long-lived publishers to avoid forcing subscribers to unsubscribe; .NET provides WeakEventManager in some frameworks.
- Outbox pattern: for durable delivery when publishing to external systems, persist the event and let a background process reliably publish to the bus.
- Event versioning: when events are stored or sent across services, add a version field and design consumers to gracefully ignore unknown fields.

Homework (short)
1. Describe two event handlers your capstone needs (name, who subscribes, what they must do, and unsubscribe strategy).
2. Implement a small publisher/subscriber mini-example and show safe invocation and unsubscription.

Summary
- Events provide light-weight, in-process decoupling for notifications.
- Use EventHandler<T> and the protected OnX pattern, snapshot delegates for thread-safety, and prefer Task-based async events for I/O-bound subscribers.
- For complex scenarios scale to an EventAggregator, mediator, or message bus; always plan for error handling and unsubscription to avoid leaks.

Further reading
- Microsoft docs: Events and delegates
- Patterns: Event Aggregator, Outbox, Pub/Sub (messaging systems), MediatR for in-process mediation