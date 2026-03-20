# Day 9 — Interfaces as Contracts & Abstract Classes (C# / .NET)

This brief guide explains what interfaces and abstract classes are, how they differ, and when to use each to design decoupled, testable systems.

## Interfaces — contracts without implementation
An interface defines a contract: a set of members (methods, properties, events, indexers) that a type must implement. Interfaces describe "what" a type can do, not "how" it does it.

Key points:
- Interfaces are implementation-agnostic — many unrelated types can implement the same interface.
- Useful for dependency inversion and testability: depend on interfaces, not concrete types.
- C# supports default interface implementations (C# 8+), but prefer minimal use to avoid unexpected coupling.

Example:
```csharp
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public class FileLogger : ILogger
{
    public void Log(string message) => File.AppendAllText("log.txt", message + "\n");
}
```
Because both loggers implement ILogger, consumers can accept ILogger and remain agnostic to the concrete logger:
```csharp
public class OrderService
{
    private readonly ILogger _logger;
    public OrderService(ILogger logger) => _logger = logger;
    public void PlaceOrder(Order o) { /* ... */ _logger.Log("order placed"); }
}
```

Benefits of interfaces:
- Swappable implementations (production vs test doubles).
- Clear behavioral contracts.
- Multiple inheritance of behavior (a class can implement many interfaces).

## Abstract classes — partial implementation and a common base
Abstract classes can contain both abstract members (no body) and concrete implementations. Use them when derived classes share state or implementation details.

Key points:
- Abstract classes are for "is-a" specialization with shared code.
- They can hold fields and protected helpers for derived classes.
- Derived classes override abstract/virtual members to supply specific behavior.

Example:
```csharp
public abstract class Repository<T>
{
    protected readonly string _connectionString;
    public Repository(string conn) => _connectionString = conn;

    public abstract T GetById(int id);

    // Shared helper
    protected void OpenConnection() { /* common logic */ }
}

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(string conn) : base(conn) { }
    public override Customer GetById(int id) { /* concrete DB access */ return new Customer(); }
}
```

When to choose an abstract class:
- You want to provide reusable implementation or hold shared state.
- You want to force a common constructor pattern or helper methods.
- You expect closely related types that form a clear hierarchy.

## Interfaces vs Abstract Classes — comparison
- Purpose: Interface = contract (capability). Abstract class = partial implementation + contract.
- Multiple inheritance: A class can implement many interfaces but inherit only one class (abstract or not).
- State: Abstract classes can have fields; interfaces cannot (except static members and default implementations in recent C#).
- Versioning: Adding members to interfaces without breaking implementers is harder (default implementations mitigate this but use carefully).

Rule-of-thumb:
- Prefer interfaces to express capabilities (e.g., `ILogger`, `IRepository<T>`, `IPaymentMethod`).
- Use abstract classes when there is shared code/state that derived types should reuse.

## Polymorphism & decoupling
Both interfaces and abstract classes enable polymorphism: code that depends on an abstraction can work with any concrete implementation. This decouples modules and makes code easier to test.

Example (dependency injection & testability):
```csharp
// In production composition root:
ILogger logger = new ConsoleLogger();
var service = new OrderService(logger);

// In unit tests:
ILogger fake = new FakeLogger(); // records calls for assertions
var serviceUnderTest = new OrderService(fake);
```

By depending on an interface rather than a concrete type, OrderService can be tested with lightweight fakes.

## Design tips and anti-patterns
- Favor small focused interfaces (Interface Segregation Principle). Avoid fat interfaces that force implementers to provide unused members.
- Avoid exposing concrete types in APIs; prefer returning interfaces (e.g., `IEnumerable<T>` instead of `List<T>`).
- Don't use abstract classes solely to avoid writing an interface; if there's no shared implementation, an interface is often clearer.
- Beware default interface methods for complex logic — they can hide behavior and complicate testing and versioning.

## Small classroom exercise
1. Define an interface `IPaymentMethod` with `bool Process(decimal amount)`.
2. Implement `CreditCardPayment` and `PaypalPayment`.
3. Refactor an `OrderProcessor` that previously used `if (method == "card") ... else if ...` to accept an `IPaymentMethod` and demonstrate swapping implementations.
4. Write a short note: why is this design easier to extend and test?

---

What I did and next
- I created this one‑page Markdown guide summarizing interfaces and abstract classes with C# examples and design guidance. If you’d like, I can:
  - produce a short exercise handout (with starter code) for classwork,
  - convert the classroom exercise into a ready-to-run project with unit tests,
  - or produce a one-page comparison table as a printable sheet.
```