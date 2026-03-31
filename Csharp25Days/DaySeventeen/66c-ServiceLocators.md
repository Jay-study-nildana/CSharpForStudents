# Service Locators — concept, anti-patterns, and alternatives

This document explains the Service Locator pattern, why it is often considered an anti-pattern in modern application design, the problems it introduces, and practical ways to refactor away from it. It includes short code examples, refactoring steps, teaching tips, and guidance you can share with students.

---

## Quick summary

- Service Locator: a component (often global/static) that returns service instances on demand (e.g., ServiceLocator.GetService(typeof(IFoo))).
- Why it's problematic: hides dependencies, makes code harder to test, introduces runtime resolution errors, and obscures object creation responsibility — all symptoms that reduce maintainability.
- Preferred alternatives: constructor injection (DI), factory or abstract-factory patterns, explicit composition root, and small, focused factory delegates.

---

## What is a Service Locator?

A Service Locator exposes a single point to retrieve dependencies. Typical usage:

```csharp
// Anti-pattern example
public static class ServiceLocator
{
    public static IServiceProvider Provider { get; set; } // globally accessible
    public static T Get<T>() => (T)Provider.GetService(typeof(T));
}

public class OrderProcessor
{
    public void Process(Order o)
    {
        var repo = ServiceLocator.Get<IOrderRepository>(); // hidden dependency
        repo.Save(o);
    }
}
```

The caller asks a global registry for dependencies rather than having them supplied explicitly.

---

## Why Service Locator is often considered an anti-pattern

1. Hidden dependencies
   - Constructor signatures no longer reveal what a class needs. Maintaining and understanding dependencies requires inspecting method bodies.

2. Harder to unit test
   - Tests must configure the global locator or rely on side effects, which is error-prone and creates test coupling. You can't simply instantiate the class with test doubles.

3. Violates single responsibility and separation of concerns
   - Classes now perform two jobs: their domain responsibility and resolving dependencies.

4. Runtime errors instead of compile-time errors
   - Missing registrations cause failures at runtime (GetService returns null or throws), while constructor injection surfaces missing dependencies when the DI container or compiler wiring is incorrect.

5. Encourages service-location spread
   - Developers may use the locator everywhere, turning the codebase into a tightly coupled, hard-to-evolve system.

6. Lifecycle and scoping bugs
   - Retrieving services from wrong scopes (e.g., requesting scoped services from a long-lived static Provider) causes captured-scope issues or incorrect lifetimes.

---

## When Service Locator is acceptable (rare)

There are limited cases where a locator-like approach can be pragmatic, but with care:
- Bootstrapping: temporary short-lived locator used only during app startup before the composition root is established.
- Framework-level code where the host must offer a generic resolution mechanism (but avoid exposing this to application business logic).
- Plugin loaders where types are discovered at runtime and need to be instantiated; prefer controlled factories or plugin registration APIs over a global static locator.

Even in these cases, restrict scope and avoid leaking locator use into business code.

---

## Refactoring: from Service Locator to Constructor Injection

Step-by-step approach:
1. Identify uses of the locator (grep for ServiceLocator, Provider.GetService, or IServiceProvider calls inside business code).
2. For each class that calls the locator, add constructor parameters for the dependencies it requires.
3. Register concrete implementations in the composition root (Program.cs/Startup).
4. Update call sites or DI container configuration to build the object graph.

Bad (service locator):
```csharp
public class PaymentService
{
    public void Pay() {
        var gateway = ServiceLocator.Get<IPaymentGateway>();
        gateway.Charge();
    }
}
```

Good (constructor injection):
```csharp
public class PaymentService
{
    private readonly IPaymentGateway _gateway;
    public PaymentService(IPaymentGateway gateway) => _gateway = gateway;

    public void Pay() => _gateway.Charge();
}
```

Composition root:
```csharp
services.AddTransient<IPaymentGateway, StripeGateway>();
services.AddTransient<PaymentService>();
```

Benefits: dependencies are explicit, testable (pass fakes), and lifecycle rules are enforced by the DI container.

---

## Alternatives when runtime resolution is needed

1. Factory delegates (Func<...> or custom factory)
   - Use factory delegates when creation requires runtime parameters:
   ```csharp
   services.AddTransient<Func<string, IReportGenerator>>(sp => template => new ReportGenerator(template));
   ```

2. Abstract Factory pattern
   - Define a factory interface and register implementations through DI:
   ```csharp
   public interface IReportFactory { IReportGenerator Create(string template); }
   ```

3. Provider objects scoped to the composition root
   - If many modules need late binding, expose a typed provider with explicit API rather than an omnipotent service locator.

4. Scoped IServiceProvider usage (with care)
   - When you must create scopes dynamically (background workers), inject IServiceScopeFactory and create a scope; avoid storing or using a global provider.

---

## Teaching tips and demo ideas

- Show the smell: a small project where classes call ServiceLocator.Get<T> and demonstrate the difficulty of writing unit tests.
- Live refactor: pick one class, add constructor parameters, adjust the composition root, and update tests.
- Test demonstration: show how constructor injection lets you pass a fake or mock without global state.
- Scope exercise: demonstrate captured-scope bug where a singleton uses a scoped service obtained from a global provider; then show the correct fix (make dependent service scoped or use IServiceScopeFactory).

---

## Common patterns of problems to look for

- Static Provider or globally accessible IServiceProvider
- Objects calling GetService/GetRequiredService inside business logic
- Factories that implicitly call service locator to resolve dependencies of created types
- Code that "new"s concrete implementations in many places and then hides them via locator

---

## Checklist for migrating a codebase

- Audit: find locator usages.
- Decide a plan: replace with constructor injection where possible.
- Use factory delegates or typed factories where runtime parameters are required.
- Centralize registrations in one composition root.
- Add unit tests as you refactor to lock behavior.
- Remove the global locator and compile — fix remaining failures by making dependencies explicit.

---

## Short reference matrix

- Need explicit, testable dependencies → Constructor injection
- Need runtime parameterized creation → Factory delegate or abstract factory (registered with DI)
- Need occasional dynamic scope creation in long-lived services → IServiceScopeFactory (not a global static provider)
- Bootstrapping or framework internals → very limited, well-documented locator usage only

---

## Conclusion

Service Locators hide dependencies, hinder testability, and encourage fragile code. Favor constructor injection, factories, and explicit composition in a single composition root. If you must use a locator-like mechanism, restrict its scope to bootstrap/framework code and avoid exposing it to application-level business logic. Teach students to prefer explicitness — visible constructor parameters make code easier to read, test, and maintain.
