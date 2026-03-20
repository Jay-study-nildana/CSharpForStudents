# Immutability Benefits (C# / .NET)

Immutability means an object’s state cannot change after it is created. In C# this is expressed with get-only properties, readonly fields, `readonly struct`, `record` (or `record struct`), and immutable collections. This one‑page guide explains why immutability is useful, shows brief C# examples, lists trade‑offs, and gives quick guidelines for when to use it.

---

## Why prefer immutable types?

1. Predictable reasoning
   - An immutable object is simple to reason about — its state is fixed. You don’t need to track who changed it and when.
   - Fewer bugs from unexpected mutation and fewer invariants to maintain.

2. Thread-safety by default
   - Immutable objects are inherently safe to share across threads without locks. No race conditions on the object’s state.

3. Easier testing and caching
   - Deterministic behavior makes unit tests simpler.
   - Immutable values are safe to cache and reuse (memoization) because they won’t change under the cache.

4. Safer as keys and values
   - Use immutable objects as dictionary keys or in sets without worrying that their hash code or equality will change.

5. Functional composition
   - Immutable data fits functional patterns: transform by producing new values (`with` expressions, pure functions) instead of in-place mutation.

6. Simpler debugging
   - You can inspect an immutable value and be confident it represents the system at the moment it was created.

---

## Small C# examples

Immutable class (hand-written):
```csharp
public class Person
{
    public string Name { get; }
    public int Age { get; }

    public Person(string name, int age)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Age = age;
    }
}
```

C# 9+ record (concise, value-like equals, with-expression):
```csharp
public record PersonRecord(string Name, int Age);

// create modified copy
var p1 = new PersonRecord("Ana", 30);
var p2 = p1 with { Age = 31 }; // new instance
```

Readonly struct / record struct for small value types:
```csharp
public readonly struct Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
}
```

Immutable collections (System.Collections.Immutable):
```csharp
using System.Collections.Immutable;
var list = ImmutableList<int>.Empty.Add(1).Add(2);
// list remains unchanged by operations that return a new list
```

---

## Patterns for working with immutable data

- Builder / Factory: use builders to construct complex immutable objects.
- With-expressions (records): concise copying with changed properties.
- Structural sharing: immutable collections and persistent data structures share memory under the hood to reduce copying cost.
- Defensive copying on boundary: convert mutable inputs into immutable fields on construction.

---

## Trade-offs and pitfalls

1. Allocation / performance
   - Creating new instances (instead of mutating) can allocate more objects. For small types this is cheap; for large objects or hot paths, measure and consider alternatives (e.g., pooling, in-place mutation where safe).

2. Large immutable objects
   - Avoid copying very large structures frequently. Use `readonly struct` for small value types and immutable collections that use structural sharing for medium collections.

3. Interop and frameworks
   - ORMs (e.g., EF Core) and some serializers expect parameterless constructors and mutable setters. For persistence or object-mapping, you may need DTOs or special configuration.

4. Mutable collections inside otherwise immutable objects
   - If an immutable object wraps mutable collections, it’s not truly immutable. Use `IReadOnlyList<T>` or `ImmutableList<T>` for collection properties.

---

## Practical guidelines

- Use immutability for:
  - Value objects (Point, Money, DateRange).
  - DTOs passed between layers or threads.
  - Public API data shapes where consumers should not mutate internals.
  - Concurrent/shared data and cache keys.

- Use mutability for:
  - Entities with identity and lifecycle (e.g., Active domain aggregates where state changes over time).
  - Performance-critical structures where copying cost is prohibitive (after measurement).

- Prefer small immutable types (keep structs small — typically <= 16 bytes).
- When designing APIs, offer immutable types by default, and provide builders or factory methods for complex construction.

---

## Classroom exercise (quick)
Take an existing mutable DTO (e.g., Student with List<Book> Borrowed). Refactor it to:
- Make the DTO immutable (get-only properties or record).
- Use an immutable collection type (ImmutableList<Book> or IReadOnlyList<Book> with defensive copy).
- Show how to create a modified copy when a student borrows a new book (record `with` or builder).

---

## Final note
Immutability reduces accidental complexity: fewer race conditions, clearer invariants, easier testing, and safer reuse. Use it as a default for small values and public data shapes; be pragmatic where performance or framework constraints require controlled mutability.