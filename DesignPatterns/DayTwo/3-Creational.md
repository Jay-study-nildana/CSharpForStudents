# Day 2 — Creational Patterns: Singleton (with caveats) & Factory Method

This page summarizes two creational patterns you’ll use today: Singleton and Factory Method. Read the intent, examine the short C# examples, and consider the tradeoffs. In the lab you’ll design a factory method for interchangeable services and evaluate whether a singleton is appropriate.

---

## What you will learn
- The intent and anatomy of Singleton and Factory Method patterns.  
- How to implement them in C# (patterns + modern DI interactions).  
- When to use or avoid each pattern and typical pitfalls.  
- A brief checklist to guide design decisions.

---

## Singleton — intent & examples

Intent
- Ensure a class has only one instance and provide a global point of access.

When to consider it
- Truly global, stateless services (configuration, process-wide read-only caches, logger adapters) where exactly one instance makes sense and is safe.

Why be careful
- Singletons often become “global mutable state”, which causes hidden coupling, testability problems, and concurrency bugs. Prefer DI-managed singletons that are stateless or thread-safe.

Bad (tight-coupling + not thread-safe)
```csharp
public class BadLogger
{
    private static BadLogger _instance; // not thread-safe
    private BadLogger() { }
    public static BadLogger Instance => _instance ??= new BadLogger();
    public void Log(string msg) { /* write to file */ }
}
```

Thread-safe lazy using .NET Lazy<T>
```csharp
public class Logger
{
    private static readonly Lazy<Logger> _lazy = new Lazy<Logger>(() => new Logger());
    public static Logger Instance => _lazy.Value;
    private Logger() { }
    public void Log(string msg) { /* thread-safe logging */ }
}
```

DI-managed singleton (recommended when using DI container)
```csharp
// registration (conceptual)
services.AddSingleton<ILogger, AppLogger>();

// consumption via DI constructor injection
public class OrderService
{
    private readonly ILogger _logger;
    public OrderService(ILogger logger) => _logger = logger;
}
```

Common pitfalls & tradeoffs
- Global mutable state: singletons that hold mutable data can leak state between users/requests and introduce hard-to-find bugs.  
- Testing: singletons can be hard to replace or reset in unit tests unless designed for testability.  
- Lifetime mismatches: registering a long-lived singleton that depends on short-lived (scoped) services can cause runtime errors or captured-disposed resources.  
- Concurrency: ensure internal state is thread-safe or avoid maintaining shared mutable state.

Alternatives
- Use DI with appropriate lifetime (Transient/Scoped/Singleton) and prefer stateless singletons.  
- Use Factory or Provider objects for on-demand creation.  
- For per-request operations in web apps, prefer Scoped services.

---

## Factory Method — intent & examples

Intent
- Define an interface for creating an object, but let subclasses decide which class to instantiate. Factory Method lets a class defer instantiation to subclasses.

When to use it
- When a class wants to delegate responsibility for creating objects to subclasses.  
- When a class needs to create different products depending on subclass behavior or runtime configuration, and you want to avoid exposing concrete product classes to the client.

Structure (UML hint)
- Creator (abstract or base class) declares FactoryMethod(): Product  
- ConcreteCreator overrides FactoryMethod to return ConcreteProduct  
- Client uses Creator and the Product interface

Classic Factory Method example (notifications)
```csharp
public interface INotifier { void Notify(string message); }

public class EmailNotifier : INotifier { public void Notify(string m) { /* email */ } }
public class SmsNotifier : INotifier { public void Notify(string m) { /* sms */ } }

public abstract class NotifierCreator
{
    // Factory method
    protected abstract INotifier CreateNotifier();
    
    // Business method uses the product
    public void Send(string message)
    {
        var notifier = CreateNotifier();
        notifier.Notify(message);
    }
}

public class EmailNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new EmailNotifier();
}

public class SmsNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new SmsNotifier();
}
```

Factory Method vs other approaches
- Direct constructor call: simple but couples client to concrete types.  
- Factory Method: defers responsibility to subclasses and encapsulates product creation. Good when subclass controls product type.  
- Simple factory (static method): centralizes creation but can be harder to extend without modification.  
- DI container: externalizes construction completely; preferred when you want to compose and configure dependencies outside the class.

Using DI together with factories
- You can register factory delegates with the DI container (e.g., Func<IService> or factory interfaces) to combine DI benefits with runtime choice.
```csharp
// register
services.AddTransient<EmailNotifier>();
services.AddTransient<SmsNotifier>();
services.AddTransient<Func<string, INotifier>>(sp => key => key == "sms"
    ? sp.GetRequiredService<SmsNotifier>()
    : sp.GetRequiredService<EmailNotifier>());
```
- Factory methods are still useful when creation logic is embedded in class hierarchies or when different subclasses should create different products.

Tradeoffs
- Factory Method introduces indirection that improves flexibility but adds complexity: more small classes to understand.  
- It’s most valuable when class families and varied product creation are expected to evolve independently.

---

## Lab & Homework (brief)
Lab
- Design a Factory Method that returns interchangeable messaging service implementations (Email or SMS) based on a configuration value.
- For each creation option explain whether the created service should be: Transient, Scoped, or Singleton. Justify your choice.

Homework
- Write a short (1‑2 page) comparison: "Factory Method vs DI for dynamic runtime selection". Include examples where each approach is preferable.

---

## Quick design checklist
- Do you need exactly one instance across the app? If yes, prefer DI singletons that are stateless or thread-safe. Avoid singletons that hold mutable per-request data.  
- Do clients need to vary the concrete product without changing client code? Consider Factory Method.  
- Can the DI container handle construction and lifetime cleanly? If so, prefer DI (with factories/delegates) for composition.  
- Before adding a pattern, answer: what problem does it solve, what complexity does it add, and how will this affect testability?

---

Further reading
- GoF: Factory Method chapter  
- Microsoft docs: Dependency injection in .NET (service lifetimes)  
- Articles on anti-patterns: Global state and Singleton traps

Bring questions and your factory designs to the lab — we’ll review them together.