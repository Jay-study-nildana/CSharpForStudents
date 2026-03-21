# Day 1 — Design Patterns: What they are, anatomy, UML primer, SOLID, and Dependency Injection

Welcome! This page summarizes the fundamentals you need before we learn individual design patterns. Read through the concepts, examine the short C# examples, and bring questions to the lab.

## 1. What are Design Patterns and why they matter
Design patterns are proven, reusable solutions to common software design problems. They are not finished code — they are templates describing:
- When to apply a solution (intent),
- The problem it solves,
- How components collaborate (structure/interaction),
- Tradeoffs and consequences.

Why patterns matter:
- Improve communication: patterns give a shared vocabulary (“use a Repository here”).
- Capture proven design choices, accelerating decision-making.
- Make code more maintainable, testable, and extensible when used appropriately.

## 2. Pattern anatomy (concise template)
Every pattern description usually contains:
- Intent — what the pattern does.
- Problem — the context and forces (constraints).
- Solution — the structure and participants (classes, interfaces).
- Consequences / Tradeoffs — advantages and costs.
- Example usage — when/how to apply it.

Example pattern header you might use in notes:
```text
Pattern: Strategy
Intent: Encapsulate interchangeable algorithms behind a common interface.
Problem: Multiple algorithms are needed, and they vary independently of clients.
Solution: Define IStrategy and several concrete strategies; clients hold a reference to IStrategy.
Tradeoffs: Adds indirection; improves flexibility and testability.
```

## 3. Short UML primer
UML class diagram basics:
- Class: [ClassName]
- Interface: <<interface>> IName
- Solid arrow with open head: inheritance (A —|> B)
- Dotted arrow: dependency or usage
- Composition / Aggregation: filled/empty diamond

Mermaid-style class example (for visualization tools):
```mermaid
classDiagram
    <<interface>> IReporter
    IReporter <|.. ConsoleReporter
    IReporter <|.. FileReporter
    Consumer --> IReporter : depends on
```

Key idea: Use simple class diagrams to clarify responsibilities and relationships before coding.

## 4. SOLID recap (short)
SOLID principles help structure classes and modules for change and testability.

- Single Responsibility Principle (SRP): A class should have one reason to change.
- Open/Closed Principle (OCP): Modules should be open for extension, closed for modification.
- Liskov Substitution Principle (LSP): Subtypes must be substitutable for their base types.
- Interface Segregation Principle (ISP): Prefer many small, client-specific interfaces over one large interface.
- Dependency Inversion Principle (DIP): High-level modules should not depend on low-level modules; both should depend on abstractions.

DIP is foundational for patterns that promote loose coupling: we program to interfaces/abstractions, not concrete classes.

## 5. Dependency Inversion vs Dependency Injection
- Dependency Inversion Principle (DIP) is a design guideline: depend on abstractions.
- Dependency Injection (DI) is a technique for supplying an object's dependencies from the outside (rather than creating them inside the object). DI is a common way to realize DIP.

Constructor injection (recommended) — small example:

```csharp
public interface IMessageSender
{
    void Send(string message);
}

public class EmailSender : IMessageSender
{
    public void Send(string message) { /* send email */ }
}

public class AlertService
{
    private readonly IMessageSender _sender;

    // Dependency provided via constructor (constructor injection)
    public AlertService(IMessageSender sender)
    {
        _sender = sender;
    }

    public void Alert(string text)
    {
        _sender.Send(text);
    }
}
```

Without DI, `AlertService` might directly instantiate `EmailSender`, which couples it to a concrete implementation and makes testing harder.

## 6. DI in modern .NET (registration & lifetimes)
.NET provides a built-in DI container via `IServiceCollection`. When you register services, you choose lifetimes:

- Transient: A new instance every time it's requested.
- Scoped: One instance per scope (e.g., per web request).
- Singleton: One instance for the application lifetime.

Registration example:

```csharp
// using Microsoft.Extensions.DependencyInjection;
var services = new ServiceCollection();

services.AddTransient<IMessageSender, EmailSender>(); // new instance each time
services.AddScoped<IRepository, EfRepository>();      // one per scope (e.g., web request)
services.AddSingleton<IConfig, AppConfig>();         // single instance app-wide

var provider = services.BuildServiceProvider();
```

Choosing lifetimes — practical guidance:
- Transient: lightweight, stateless services. Good for services that contain no shared mutable state.
- Scoped: typical for data repositories in web apps to share a DbContext per request.
- Singleton: for truly global, thread-safe services (configuration caches, factories). Avoid storing request‑specific or mutable per-user state in singletons.

Singleton caveat example:
```csharp
public class BadCache
{
    private readonly Dictionary<string,string> _store = new(); // mutable shared state

    public void Put(string k, string v) => _store[k] = v; // dangerous in singleton
}
```
Mutable state in a singleton can lead to concurrency bugs and leakage between users; prefer stateless singletons or thread-safe caches.

## 7. Putting it together — small design snapshot
- Apply SOLID first: identify responsibilities and abstractions.
- Use DIP: have high-level modules depend on interfaces.
- Use DI to supply implementations, letting tests inject mocks/fakes.
- Choose pattern(s) as solutions to clearly identified design problems; always state intent and tradeoffs.

Quick checklist before applying a pattern:
- What problem are we solving (tight coupling, duplicated code, hard-to-test logic)?
- Which components change together and which don’t?
- Are we introducing unnecessary complexity or indirection?
- How will this affect testing and performance?

## 8. Quick tips for the lab
- Start with a small UML sketch before any refactor.
- Favor constructor injection; avoid service locators.
- Keep services small and focused (SRP).
- When introducing a pattern, explicitly list the intended benefit and potential cost.

_*
