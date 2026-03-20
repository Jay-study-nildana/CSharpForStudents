# Day 8 — Inheritance & Polymorphism (C# / .NET)

Objectives: learn base and derived classes, `virtual` methods and `override`, and the benefits of polymorphism for clearer, more extensible designs. Includes short C# examples that show how polymorphism replaces conditional logic.

---

## Key concepts

- Class inheritance: a derived class (subclass) extends a base class (superclass), inheriting fields, properties, and behavior.
- virtual method: a method in the base class marked `virtual` that derived classes may override.
- override: a derived class replaces the base implementation using the `override` keyword.
- abstract: an `abstract` method has no implementation in the base class and must be implemented by non-abstract derived classes.
- polymorphism: the ability to treat derived objects as instances of their base type and call the appropriate overridden method at runtime (dynamic dispatch).
- Liskov Substitution Principle (LSP): derived classes should be usable wherever the base type is expected, without surprising behavior.

---

## Simple example — Animal and Speak()

```csharp
public class Animal
{
    public virtual string Speak() => "(silence)";
}

public class Dog : Animal
{
    public override string Speak() => "Woof!";
}

public class Cat : Animal
{
    public override string Speak() => "Meow!";
}

// Usage
var animals = new List<Animal> { new Dog(), new Cat(), new Animal() };
foreach (var a in animals)
{
    Console.WriteLine(a.Speak()); // Prints "Woof!", "Meow!", "(silence)"
}
```

Why this matters: code that works with `Animal` does not need to know concrete types. Adding a new `Bird` that overrides `Speak()` requires no changes in the loop above.

---

## virtual vs abstract vs sealed

- `virtual`: provides a default implementation; derived classes may override.
- `abstract`: declares a method without implementation; base class must be `abstract`; derived concrete classes must implement.
- `sealed`: prevents further derivation (applies to classes or overridden methods). Use `sealed` when you want to lock down behavior.

Examples:
```csharp
public abstract class Shape
{
    public abstract double Area(); // no implementation here
}

public sealed class Square : Shape
{
    public double Side { get; }
    public Square(double s) { Side = s; }
    public override double Area() => Side * Side; // required
}
```

---

## Replacing conditional logic with polymorphism

Conditional-heavy code:
```csharp
decimal CalculateDiscount(Customer customer)
{
    if (customer.Type == CustomerType.Regular) return 0m;
    if (customer.Type == CustomerType.Gold) return 0.10m;
    if (customer.Type == CustomerType.Platinum) return 0.20m;
    return 0m;
}
```

Polymorphic design:
```csharp
public abstract class Customer
{
    public abstract decimal GetDiscount();
}

public class RegularCustomer : Customer { public override decimal GetDiscount() => 0m; }
public class GoldCustomer : Customer { public override decimal GetDiscount() => 0.10m; }
public class PlatinumCustomer : Customer { public override decimal GetDiscount() => 0.20m; }

// Usage:
decimal discount = customer.GetDiscount(); // no if/else or switch
```

Benefits:
- Open/Closed: adding a new customer type means creating a new subclass — no edits to existing switch/if chains.
- Encapsulation: discount logic lives with the `Customer` type that owns it.
- Testability: each subclass can be tested in isolation.

---

## Constructor chaining and protected members

Derived classes can call base constructors and access `protected` members (visible to derived classes but not to external callers).

```csharp
public class Account
{
    protected decimal Balance;
    public Account(decimal initial) { Balance = initial; }
}

public class SavingsAccount : Account
{
    public SavingsAccount(decimal initial) : base(initial) { }
    public void AddInterest(decimal rate) { Balance += Balance * rate; } // uses protected Balance
}
```

Prefer `private` for strict hiding; use `protected` when derived types legitimately need access.

---

## Polymorphism & real-world design trade-offs

- Use inheritance when there is a clear "is-a" relationship and derived classes truly specialize base behavior.
- Prefer composition when behavior can be described as "has-a" or when you want runtime swapping of behavior (strategy pattern).
- Overuse of inheritance can lead to fragile hierarchies. Keep hierarchies shallow and cohesive.

Common patterns:
- Strategy pattern: favor composition to vary behavior by injecting strategy objects rather than subclassing for every variant.
- Template Method: base class provides a template algorithm with `virtual`/`abstract` extension points for subclasses.

Template method example:
```csharp
public abstract class ReportGenerator
{
    public void Generate()
    {
        var data = FetchData();
        var report = Format(data);
        Save(report);
    }
    protected abstract string FetchData();
    protected abstract string Format(string data);
    protected virtual void Save(string report) => File.WriteAllText("report.txt", report);
}
```
Subclasses implement `FetchData` and `Format`, while sharing the overall algorithm.

---

## Liskov Substitution Principle (LSP) — a caution

Ensure overriding methods respect the base contract:
- Pre-conditions should not be strengthened in derived methods.
- Post-conditions should not be weakened.
- Exceptions and return types should remain compatible.

Bad example:
If `Base.Process(x)` accepts any non-null `x`, a derived `Process` that throws for some inputs will violate LSP and break callers that expect the base behavior.

---

## Practical tips

- Keep hierarchies focused: group behavior that logically belongs together.
- Avoid using inheritance to share implementation; prefer composition for code reuse when sharing utility behavior.
- Use interfaces to define contracts when multiple unrelated classes should share capabilities (e.g., `IDrawable`, `ISerializable`).
- Use unit tests to ensure subclass behavior matches base expectations and that polymorphism works across collections of the base type.
- Document expected invariants and side-effects in base classes so implementers of derived classes know the contract.

---

## Short summary / classroom task

- Practice: Take a piece of code with `if/else` or `switch` that branches by type/value and sketch a class hierarchy that encapsulates each branch as a subclass (or a strategy).
- Homework hint: In your write-up compare when inheritance helps (shared contract + specialization) versus when composition is safer (flexibility, testability, fewer coupling issues).

What I prepared and next:
- This markdown summarizes base/derived classes, `virtual`/`override`, `abstract` members, and demonstrates how polymorphism replaces conditionals. Next, we can convert one of your in-class conditional examples into a class hierarchy and produce a short UML-like outline and tests for it.