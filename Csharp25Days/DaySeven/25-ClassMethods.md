# Day 7 — Instance vs Static Members, Responsibilities in Methods, and Cohesion (C# / .NET)

Objectives: understand instance vs static members, decide where behavior belongs, and design methods with clear responsibilities. Emphasize cohesion and single responsibility when organizing classes and methods.

---

## Quick summary

- Instance members (fields, properties, methods) belong to an object. They operate on that object's state and require an instance to be used.
- Static members belong to the type itself. Use static for stateless helpers, shared caches, or type-wide configuration.
- Keep each method focused on a single responsibility. Classes should group related responsibilities (high cohesion) and avoid mixing unrelated concerns.

---

## Instance members — when and how to use them

Use instance members when behavior depends on object state or when different objects represent different identities.

Typical uses:
- Model domain state: `Account.Balance`, `Order.Items`
- Behavior that mutates or queries instance state: `account.Deposit(amount)`, `order.AddItem(item)`
- Instance lifetime concerns: disposable resources per object

Example:
```csharp
public class BankAccount
{
    private decimal _balance;
    public string Owner { get; }

    public BankAccount(string owner, decimal initialBalance = 0)
    {
        Owner = owner;
        _balance = initialBalance;
    }

    public decimal Balance => _balance;

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
    }
}
```
Rationale: each `BankAccount` has its own balance; Deposit operates on the instance.

Testing note: instance methods are easy to test by creating instances with controlled state.

---

## Static members — appropriate uses and cautions

Use static members for:
- Pure helper/util functions (stateless): formatting, simple math utilities.
- Singletons or application-wide caches/configuration (careful with testability and concurrency).
- Factory methods that don't need instance data to create new instances.

Example of a static helper:
```csharp
public static class MathUtils
{
    public static int Clamp(int value, int min, int max) =>
        Math.Max(min, Math.Min(max, value));
}
```

Example of a static factory:
```csharp
public class IdGenerator
{
    private static long _next = 0;
    public static long NextId() => Interlocked.Increment(ref _next);
}
```

Cautions:
- Static mutable state creates hidden shared state → harder to reason about and test.
- Static members live for the AppDomain lifetime and can cause memory/GC issues if holding resources.
- Avoid using static for behavior that should vary between instances (violates encapsulation).

Testing & design tip: prefer injecting collaborators (through constructors) over using static singletons, especially for code that requires mocking in tests.

---

## Responsibilities in class methods — keep them focused

Single Responsibility Principle (SRP): a method should do one thing and do it well. Keep methods short and descriptive.

Good design practices:
- Method name describes intent (verb phrase): `CalculateTax`, `SendInvoice`, `IsValid`.
- Avoid mixing I/O, business logic, and formatting in the same method. Instead, separate concerns:
  - Parsing/validation
  - Business rule computation
  - Persistence or side effects (I/O)

Example split:
```csharp
public class OrderProcessor
{
    public ValidationResult Validate(Order order) { /* pure checks */ }
    public Invoice ComputeInvoice(Order order) { /* business logic, pure-ish */ }
    public void PersistInvoice(Invoice invoice) { /* I/O */ }
}
```
Benefits:
- Easier to unit test pure logic (no I/O).
- Easier to reuse `ComputeInvoice` in different contexts (UI, batch, tests).
- Encourages clearer error handling and retry strategies.

---

## Cohesion — grouping related behavior

Cohesion is about keeping related responsibilities together in one class or module. High cohesion means a class has a focused role; low cohesion (God classes) try to do too much.

Questions to decide placement:
- Does the behavior require instance state? Put it on the class that owns that state.
- Is the behavior a general utility unrelated to instance data? Consider a static helper or separate utility class.
- Will the method need dependencies (logging, repositories)? If so, instance members with dependency injection are preferred.

Example: Should `CalculateLateFee` be on `Loan` or `LoanService`?
- If it uses internal loan fields and simple calculation, make it an instance method `loan.CalculateLateFee()`.
- If it requires external policy lookup (config, calendars, holidays), put it in a service: `loanService.CalculateLateFee(loan)` and inject the policy provider.

---

## Patterns and guidelines

- Use static for:
  - Pure functions: no side effects, deterministic.
  - Constants and immutable configuration.
  - Factory methods that produce instances without external dependencies.

- Use instance methods for:
  - Behavior that mutates or depends on instance data.
  - Methods that require injected dependencies (repositories, loggers).

- Prefer composition over static singletons:
  - Inject `IIdGenerator` into constructors and provide a test double for unit tests.
  - Avoid `static` where it hampers testability.

- Keep methods small and factored:
  - If a method has conditionals or multiple responsibilities, extract private helper methods with clear names.

---

## Example: refactor from static to instance for testability

Static approach (hard to test):
```csharp
public static class Logger
{
    public static void Log(string msg) { /* writes to file */ }
}
```

Better: inject an instance:
```csharp
public interface ILogger { void Log(string msg); }
public class FileLogger : ILogger { /* writes to file */ }

public class OrderService
{
    private readonly ILogger _logger;
    public OrderService(ILogger logger) { _logger = logger; }
    public void Process(Order o) { _logger.Log("processing"); }
}
```
Benefits: unit tests can provide a fake `ILogger`.

---

## Concurrency & static members

If you must use static mutable state, ensure thread-safety:
- Use `Interlocked` or locks for counters.
- Use immutable collections for read-mostly caches.
- Consider `ConcurrentDictionary` for shared maps.

Example:
```csharp
private static readonly ConcurrentDictionary<string, CacheEntry> _cache =
    new ConcurrentDictionary<string, CacheEntry>();
```

---

## Classroom exercises / Homework ideas

1. Convert a set of static helper methods into an instance-based service that accepts dependencies via constructor. Explain trade-offs for testability.
2. Given a `ReportGenerator` that formats, fetches data, and writes to disk, break it into at least three methods/classes with single responsibilities. Draw a small UML-like outline (text).
3. Identify four methods in your project and classify them as instance/static and explain why.

---

## Final notes — what I prepared and what’s next

I created a one-page Markdown guide that compares instance and static members, gives rules for placing behavior, shows simple C# examples, and explains cohesion and SRP. Use it as a reference when deciding where a method should live and how to structure classes for testability and clarity.

Next: if you want, I can
- convert this into a printable one-sheet,
- produce three in-class refactoring exercises with starter code,
- or generate small UML text outlines for the homework tasks.

Bring one example from your own code (or a toy class) and we will practice converting responsibilities and choosing instance vs static in class.