# Day 6 — Structural Patterns: Decorator & Proxy

This lesson covers two structural patterns that let you modify or control object behavior without changing the original class: Decorator (add responsibilities dynamically) and Proxy (control access or lazily initialize expensive resources). Both favor composition over inheritance and are essential for building flexible, testable systems.

---

## Quick intents

- Decorator: Attach additional responsibilities to an object dynamically, often by wrapping the object with another object that implements the same interface. Useful for stacking cross‑cutting behaviors (logging, caching, validation) without modifying the core implementation.

- Proxy: Provide a surrogate or placeholder for another object to control access, defer costly initialization (virtual proxy), or add access control (protection proxy). The Proxy usually implements the same interface as the real subject.

---

## Pattern anatomy (short)

Decorator:
- Component (interface): the common interface clients depend on.
- ConcreteComponent: the original object providing core behavior.
- Decorator (base): implements Component and holds a reference to a Component.
- ConcreteDecorator(s): extend behavior before/after delegating to the inner Component.

Proxy:
- Subject (interface): the shared interface.
- RealSubject: the real object.
- Proxy: implements Subject and controls access/delegation to RealSubject (may create it lazily or check permissions).

---

## Decorator — C# example (stackable)

```csharp
// Component interface
public interface IService
{
    string GetData(int id);
}

// Concrete component
public class RealService : IService
{
    public string GetData(int id) => $"Data for {id}";
}

// Base decorator
public abstract class ServiceDecorator : IService
{
    protected readonly IService _inner;
    protected ServiceDecorator(IService inner) => _inner = inner;
    public abstract string GetData(int id);
}

// Logging decorator
public class LoggingDecorator : ServiceDecorator
{
    public LoggingDecorator(IService inner) : base(inner) { }
    public override string GetData(int id)
    {
        Console.WriteLine($"[Log] Calling GetData({id})");
        var result = _inner.GetData(id);
        Console.WriteLine($"[Log] Result: {result}");
        return result;
    }
}

// Caching decorator
public class CachingDecorator : ServiceDecorator
{
    private readonly Dictionary<int, string> _cache = new();
    public CachingDecorator(IService inner) : base(inner) { }
    public override string GetData(int id)
    {
        if (_cache.TryGetValue(id, out var cached)) return cached;
        var result = _inner.GetData(id);
        _cache[id] = result;
        return result;
    }
}
```

Stacking works by wrapping: `new LoggingDecorator(new CachingDecorator(new RealService()))` — order matters. A Logging outermost decorator will log calls including cache hits; a caching outermost decorator prevents inner decorators from running on hits.

---

## Proxy — C# example (lazy initialization and protection)

```csharp
public interface IHeavyResource
{
    string Compute();
}

// Real expensive resource
public class HeavyResource : IHeavyResource
{
    public HeavyResource()
    {
        // Simulate expensive startup
        Thread.Sleep(2000);
    }
    public string Compute() => "Expensive result";
}

// Proxy with lazy init and simple auth check
public class HeavyResourceProxy : IHeavyResource
{
    private readonly Func<bool> _isAuthorized;
    private HeavyResource _real;

    public HeavyResourceProxy(Func<bool> isAuthorized) => _isAuthorized = isAuthorized;

    public string Compute()
    {
        if (!_isAuthorized()) throw new UnauthorizedAccessException("Not allowed");
        // Lazy initialization (virtual proxy)
        return (_real ??= new HeavyResource()).Compute();
    }
}
```

Usage advantage: clients use `IHeavyResource` and don't need to know about initialization cost or authorization. Tests can pass a stubbed `isAuthorized` delegate and avoid instantiating `HeavyResource`.

---

## Decorator vs Proxy — when to pick which

- Use Decorator when:
  - You want to add or combine responsibilities dynamically and transparently.
  - You need multiple, composable behaviors (e.g., caching + logging + retry).
  - You want to avoid inheritance explosion for combinations.

- Use Proxy when:
  - You want to control access, lazy-load, manage remote calls, or provide a local placeholder for a remote object.
  - Behavior is about the lifecycle or access rather than adding orthogonal responsibilities.

They can be used together: a Proxy may be wrapped by Decorators (or vice versa) depending on desired ordering.

---

## DI & lifetimes (practical notes)

- Register the core service and decorators in DI. The ordering of registration matters if you rely on decorator injection helpers. Common approaches:
  - Manual factory registration that composes decorators.
  - Use decorator libraries (e.g., Scrutor) that support `Decorate<>`.
- Lifetimes:
  - Decorators that hold per-request caches or use scoped services should be Scoped.
  - Stateless decorators can be Transient or Singleton (be careful with mutable state).
  - Proxies that create heavy resources should be Transient/Scoped to avoid holding onto heavy singletons unintentionally.

Example manual composition in Startup (conceptual):
```csharp
// services.AddTransient<RealService>();
// services.AddTransient<IService>(sp => new LoggingDecorator(
//                                 new CachingDecorator(sp.GetRequiredService<RealService>())));
```

Prefer constructor injection for inner dependencies rather than service locator patterns.

---

## Testing guidance

- Decorators and proxies are easy to test in isolation by injecting fakes:
  - Test LoggingDecorator by using a fake inner IService that returns a known value and assert logs were written.
  - Test CachingDecorator by a fake inner service that counts calls; ensure inner is only called once per key.
  - For Proxy, provide an `isAuthorized` delegate that returns true/false to test protection behavior without constructing heavy resources.

- For integration tests, create specific compositions and assert combined behavior.

---

## Lab (in-class)

1. Design a decorator chain for a web API service that adds: ValidationDecorator -> CachingDecorator -> LoggingDecorator -> RealService. Sketch UML for classes and show how request flows through decorators for both valid and invalid inputs.

2. Implement a proxy for a remote file store that lazily opens a connection and enforces user permissions. Create unit tests that assert:
   - Unauthorized access throws without creating the real connection.
   - Authorized access initializes and performs operations.

Deliverable: UML sketches, sample compositions, and 3 unit tests per pattern.

---

## Homework — Decorator chains vs AOP frameworks

Write a one-page pros/cons comparison. Use these starter points:

Pros of decorator chains:
- Explicit composition, clear runtime structure.
- Type-safe, testable, and easy to debug.
- No magic or tooling required.

Cons of decorator chains:
- Boilerplate to wire many decorators and compose combinations.
- Ordering complexity and potential for mistakes.
- Per-call wrapper overhead.

Pros of AOP frameworks:
- Less boilerplate; cross-cutting concerns applied declaratively.
- Can be applied automatically across many methods.

Cons of AOP frameworks:
- Hidden behavior — harder for newcomers to see runtime flow.
- Tooling/runtime complexity; debugging interceptors can be tricky.
- May rely on dynamic proxies and break some static analysis/refactoring.

Conclude with your recommendation for a small teaching app: which approach you’d pick and why.

---

Use these patterns thoughtfully: decorators for composable behaviors, proxies for lifecycle and access control. In both cases prefer small, focused responsibilities and keep state considerations explicit when deciding lifetimes.