# Classes Versus Records (C# / .NET)

Overview
--------
This document compares C# classes (reference types) and records (C# 9+), demonstrating how records change the idiomatic way you model data in .NET and when a class still makes sense.

Definitions (C#)
----------------
- class: a reference type with identity, mutable state (unless designed otherwise), methods, and inheritance.
- record: syntactic sugar and generator-driven type focused on data: `record class` (reference) and `record struct` (value). `record` provides value-based equality, built-in `Deconstruct`, and `with` expressions for convenient copying.

Default equality and semantics
------------------------------
- class: default equality is reference equality (`ReferenceEquals`). Override `Equals`/`GetHashCode` for value semantics.
- record class: default equality is value-based (by primary constructor properties). Example:
```csharp
public record Person(string Name, int Age);
var p1 = new Person("Bob", 25);
var p2 = new Person("Bob", 25);
Console.WriteLine(p1 == p2); // True
```

Immutability & with-expressions
------------------------------
Records are commonly used immutably:
```csharp
var p3 = p1 with { Age = 26 }; // new Person("Bob",26)
```
Classes typically require manual copy logic for similar behavior.

Inheritance
-----------
Records can participate in inheritance:
```csharp
public record Employee(string Name, int Age, int Id) : Person(Name, Age);
```
Classes remain superior if complex OO hierarchies with behavior and virtual/override patterns are required.

When to choose a class
----------------------
- Your object has identity that matters (e.g., database entity with mutable identity).
- Object has complex behavior, lifecycle, or needs to be mutable.
- You require specific memory or lifecycle guarantees; classes can hold large state without copying cost.

When to choose a record class
-----------------------------
- You want concise data containers with value equality, `ToString` and `Deconstruct` generated for you.
- Immutable DTOs, messages, and configuration objects are ideal record candidates.
- You want `with` to create modified copies non-destructively.

Record struct nuance
--------------------
- `record struct` combines record convenience with value semantics — good for small, immutable data where value equality is desirable and heap allocations should be avoided.

Examples and migration
----------------------
Convert boilerplate class to record:
```csharp
// original
public class Address
{
    public string Street { get; }
    public string City { get; }
    public Address(string street, string city) => (Street, City) = (street, city);
    public override bool Equals(object? obj) => // lots of boilerplate
}

// new
public record Address(string Street, string City);
```

Considerations
--------------
- Serialization: records serialize like classes; ensure framework support for positional records if relying on constructor mapping.
- Mutability: records can still have mutable properties — be explicit.
- APIs: switching a public class to a record changes equality semantics; evaluate carefully.

Exercises
---------
1. Replace a DTO class with a `record` and write unit tests confirming equality semantics and `with`-based updates.
2. Compare memory allocations when creating many instances of `record class` vs `record struct` vs `class`.

Summary
-------
Records simplify and clarify data-centric types in C#/.NET by providing value-based equality and succinct syntax. Use classes for behavior, identity, and complex lifecycles. Use `record` (class or struct) for DTOs, messages, and immutable domain values to reduce boilerplate and improve correctness.
