# Day 3 — Creational Patterns: Abstract Factory & Builder

This lesson covers two complementary creational patterns:
- Abstract Factory — to create families of related objects without coupling clients to concrete classes.  
- Builder — to construct complex objects step-by-step, often producing immutable results and readable fluent APIs.

Read the intent, examine the short C# examples, and use the lab prompt to practice designing both patterns.

---

## Why these patterns matter
- Abstract Factory helps you support multiple product families (e.g., platform-specific UI components, database providers) without scattering conditional logic throughout the codebase.  
- Builder helps you create complex objects reliably and readably, especially when objects require many optional parameters or must be immutable.

Both patterns improve separation of concerns and enable easier testing and extension.

---

## Abstract Factory — intent & structure

Intent
- Provide an interface for creating families of related or dependent objects without specifying their concrete classes.

When to use
- You have sets of related objects that must be used together (e.g., Windows vs Linux UI controls).
- You want to switch entire product families at runtime or via configuration without changing client code.

Key participants
- AbstractFactory: declares creation methods for each kind of product in the family.
- ConcreteFactory: implements creation for a specific family.
- AbstractProduct(s): interfaces for product types.
- ConcreteProduct(s): concrete implementations for a family.
- Client: uses AbstractFactory and AbstractProduct interfaces only.

Simple C# example (platform-specific UI components)
```csharp
public interface IButton { void Render(); }
public interface IWindow { void Open(); }

public interface IUiFactory
{
    IButton CreateButton();
    IWindow CreateWindow();
}

// Windows family
public class WindowsButton : IButton { public void Render() => Console.WriteLine("Render Windows Button"); }
public class WindowsWindow : IWindow { public void Open() => Console.WriteLine("Open Windows Window"); }
public class WindowsFactory : IUiFactory
{
    public IButton CreateButton() => new WindowsButton();
    public IWindow CreateWindow() => new WindowsWindow();
}

// Mac family
public class MacButton : IButton { public void Render() => Console.WriteLine("Render Mac Button"); }
public class MacWindow : IWindow { public void Open() => Console.WriteLine("Open Mac Window"); }
public class MacFactory : IUiFactory
{
    public IButton CreateButton() => new MacButton();
    public IWindow CreateWindow() => new MacWindow();
}

// Client uses the abstract factory
public class Application
{
    private readonly IUiFactory _factory;
    public Application(IUiFactory factory) => _factory = factory;
    public void Start()
    {
        var win = _factory.CreateWindow();
        var btn = _factory.CreateButton();
        win.Open(); btn.Render();
    }
}
```

UML hint (conceptual)
- AbstractFactory -> ConcreteFactory
- AbstractFactory creates AbstractProductA, AbstractProductB
- ConcreteFactory creates ConcreteProductA1, ConcreteProductB1

Tradeoffs and notes
- Pros: cleanly encapsulates families, simplifies client code, good for plugin/driver scenarios.
- Cons: adds an abstraction layer and many small factory classes; can be overkill if families are small or rarely change.
- DI integration: register a ConcreteFactory with the DI container and inject IUiFactory. To switch families at runtime, register the factory chosen by configuration.

---

## Builder — intent & structure

Intent
- Separate construction of a complex object from its representation so the same construction process can create different representations. Also used to provide readable fluent APIs for complex instantiation.

When to use
- The target object has many optional properties or combinations of parameters.
- You want to create immutable objects with readable construction syntax.
- You have multi-step construction that benefits from separating the algorithm (Director optionally) from representation.

Participants
- Builder (interface): describes construction steps.
- ConcreteBuilder: implements the steps and returns the product.
- Product: the complex object being built.
- Director (optional): orchestrates the build sequence; useful for predefined build processes.

C# example — building an immutable `Report` using a fluent builder
```csharp
public class Report
{
    public string Title { get; }
    public string Author { get; }
    public IReadOnlyList<string> Sections { get; }

    internal Report(string title, string author, List<string> sections)
    {
        Title = title; Author = author; Sections = sections.AsReadOnly();
    }
}

public class ReportBuilder
{
    private string _title = "Untitled";
    private string _author = "Unknown";
    private readonly List<string> _sections = new();

    public ReportBuilder WithTitle(string title) { _title = title; return this; }
    public ReportBuilder WithAuthor(string author) { _author = author; return this; }
    public ReportBuilder AddSection(string section) { _sections.Add(section); return this; }

    public Report Build() => new Report(_title, _author, new List<string>(_sections));
}
```

Usage example
```csharp
var report = new ReportBuilder()
    .WithTitle("Monthly")
    .WithAuthor("Alice")
    .AddSection("Summary")
    .AddSection("Metrics")
    .Build();
```

Tradeoffs and notes
- Pros: readable construction, encourages immutability, easier testing of construction steps, flexible extension of options.
- Cons: more classes and boilerplate; may be unnecessary for simple objects.
- Fluent builders are excellent for DTOs, complex domain objects, or configuring services in tests.
- Director is optional — useful when you need standard build sequences (e.g., `ReportTemplates.Standard()`).

---

## Combining patterns & DI
- Abstract Factory + DI: register concrete factories in IServiceCollection and inject the abstract factory. This keeps family selection configurable (per environment or plugin).
- Builder + DI: builders can be transient services or provided as factory delegates when composition uses scoped resources. For immutable results, builders often return plain objects that are safe to store.

---

## Lab prompt (short)
1. Design an Abstract Factory for a “Data Access Provider” family that yields `IConnection` and `ICommand` for `SqlProvider` and `InMemoryProvider`. Sketch interfaces and one concrete factory.
2. Create a `UserBuilder` that constructs an immutable `User` object with optional fields (display name, roles, metadata). Use a fluent API. Show usage for two different user creation scenarios.
3. For each design, state: DI registration/lifetime, expected test approach, and one tradeoff.

---

## Homework
Write a short (1‑2 page) scenario where:
- Abstract Factory simplifies swapping implementations (describe the families).
- Builder improves code readability and prevents invalid object states (explain invariant enforcement).
Include UML sketches and justify chosen lifetimes for DI registration.

Further reading
- GoF: Abstract Factory & Builder chapters  
- Articles on fluent builders and immutability in C#
