# Multiple‑Implementation Patterns (C# / .NET)

When you design against an abstraction (interface or abstract class), you often provide multiple implementations that fulfill the same contract but behave differently. This document summarizes common patterns that rely on multiple implementations, when to use them, and short C# examples.

Why multiple implementations?
- Swap behavior at runtime (strategy).
- Add features without changing existing code (decorator, plugin).
- Provide test doubles and null behavior for easier testing.
- Combine implementations (composite) or adapt one API to another (adapter).

Patterns and examples

1. Strategy (replace conditional logic)
- Use when an algorithm or policy varies independently from callers.
- Define an interface and provide different implementations selected at runtime (DI, factory, or configuration).

```csharp
public interface IDiscountPolicy { decimal Apply(decimal total); }

public class NoDiscount : IDiscountPolicy { public decimal Apply(decimal total) => total; }
public class PercentageDiscount : IDiscountPolicy { decimal _p; public PercentageDiscount(decimal p) => _p = p; public decimal Apply(decimal t) => t * (1 - _p); }

// Usage via DI
public class Checkout { private readonly IDiscountPolicy _policy; public Checkout(IDiscountPolicy p) => _policy = p; public decimal Final(decimal total) => _policy.Apply(total); }
```

2. Decorator (compose behavior)
- Wrap one implementation with another to extend behavior (logging, caching, validation) without changing original code.
- Works well when you want orthogonal concerns applied to many implementations.

```csharp
public class LoggingPayment : IPayment
{
    private readonly IPayment _inner;
    private readonly ILogger _log;
    public LoggingPayment(IPayment inner, ILogger log) { _inner = inner; _log = log; }
    public void Pay(decimal a) { _log.Log($"Paying {a}"); _inner.Pay(a); _log.Log("Paid"); }
}
```

3. Null Object (avoid null checks)
- Provide a do-nothing implementation to avoid repeated `if (obj != null)` checks.
- Makes callers simpler and safer.

```csharp
public class NullLogger : ILogger { public void Log(string _) { /* no-op */ } }
```

4. Composite (treat group like single implementation)
- Combine multiple implementations under one composite that delegates to children.
- Useful for broadcasting, aggregating, or applying policy across many components.

```csharp
public class CompositeNotifier : INotifier
{
    private readonly IEnumerable<INotifier> _children;
    public CompositeNotifier(IEnumerable<INotifier> children) => _children = children;
    public void Notify(string m) { foreach (var c in _children) c.Notify(m); }
}
```

5. Adapter (bridge incompatible interfaces)
- Wrap an existing class to implement the desired interface.
- Useful when integrating legacy or third‑party code.

```csharp
// Third-party LegacySender with SendMsg(string)
public class LegacyAdapter : IEmailSender
{
    private readonly LegacySender _legacy;
    public LegacyAdapter(LegacySender legacy) { _legacy = legacy; }
    public void Send(string to, string body) => _legacy.SendMsg($"{to}:{body}");
}
```

6. Factory / Provider / Plugin patterns (choose implementation)
- Use a factory or DI container to instantiate the right implementation based on config, environment, or runtime data.
- Plugins implement a common interface; loader discovers and runs them.

```csharp
public static IReportFormatter Create(string kind) =>
    kind == "csv" ? (IReportFormatter)new CsvFormatter() : new JsonFormatter();
```

7. Proxy (control access)
- Provide an implementation that controls access to the real one (lazy initialization, caching, security checks).

Design & testing tips
- Prefer small focused interfaces (Interface Segregation). Large interfaces make implementing many variants cumbersome.
- Use dependency injection to supply implementations; tests can inject fakes or stubs easily.
- Keep implementations single-responsibility: a `LoggingPayment` decorator should only log, not modify payment logic.
- Avoid static implementations for behaviors you may need to swap in tests; prefer interfaces and instance-based DI.
- Document behavioral contracts (null handling, thread-safety, exceptions) so all implementations are substitutable (Liskov Substitution Principle).

When to prefer composition over inheritance
- If you need to combine behaviors dynamically (e.g., logging + caching), prefer composing decorators over deep inheritance trees.
- Use inheritance when implementations share substantial protected behavior/state; use interfaces + composition to vary behavior at runtime.

Common pitfalls
- Overusing many tiny implementations can complicate discovery/configuration. Balance clarity with manageability.
- Mutable shared state in static implementations breaks test isolation—ensure thread-safety or avoid static mutable singletons.
- Violating behavioral contracts across implementations (e.g., one throws on empty input while another returns empty) creates surprises — keep contracts consistent.

Quick classroom exercise
- Start with a function that applies discounts with a switch on customer type. Refactor to an IDiscountPolicy hierarchy and a factory that chooses the policy. Add a `LoggingDiscount` decorator that logs when discounts are applied. Write one test using a fake logger to assert logging occurred.

Summary
Multiple‑implementation patterns give you flexibility, extensibility, and testability when used with clear interfaces and composition. Use Strategy for interchangeable algorithms, Decorator for orthogonal features, Composite for aggregation, Adapter for legacy integration, and Factory/Plugin for discovery and selection. Keep interfaces small, document contracts, and prefer DI to make implementations swappable in tests and runtime.