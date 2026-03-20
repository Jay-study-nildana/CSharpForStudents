# Structs vs Classes (C# / .NET)

This guide compares structs and classes in C# and helps you decide which to use. It covers value vs reference semantics, performance considerations, mutability, equality, and common pitfalls — with short code examples.

---

## Value semantics (struct) vs reference semantics (class)

- Struct: a value type. Variables hold the *data* directly. Assigning or passing a struct copies its fields.
- Class: a reference type. Variables hold a *reference* (pointer) to an object on the heap. Assigning or passing a class copies the reference, not the object.

Example — copy behavior:
```csharp
public struct PointStruct { public int X, Y; }
public class PointClass { public int X, Y; }

var a = new PointStruct { X = 1, Y = 2 };
var b = a; // copy of values
b.X = 9;
// a.X is still 1

var c = new PointClass { X = 1, Y = 2 };
var d = c; // copy of reference
d.X = 9;
// c.X is now 9
```

Key takeaway: struct assignments are independent copies; class assignments share the same instance.

---

## When to prefer a struct

Use a struct when:
- The type represents a single value (e.g., point, complex number, color, small DTO).
- The type is small and immutable (guideline: typically up to 16 bytes; avoid large structs).
- Value semantics are natural and expected (mathematical values).
- Instances are short-lived or allocated frequently (avoids many small heap allocations).

Prefer `readonly struct` for immutable value types:
```csharp
public readonly struct Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
}
```

C# also supports `record struct` (value-like records) starting in newer language versions for concise, immutable structs.

---

## When to prefer a class

Use a class when:
- The object has identity (two instances with same data are not necessarily the same).
- The object is large, mutable, or part of a richer domain model with behavior and lifecycle.
- The type will be stored in collections that expect reference semantics, or will be shared between components.
- You need inheritance (structs cannot inherit from other structs or classes; they only implement interfaces).

---

## Mutability and surprising behavior

Mutable structs are a common source of bugs. Because structs are copied, mutations can be applied to a copy without affecting the original — especially when used via properties, collections, or interfaces.

Pitfall example:
```csharp
public struct Counter { public int Value; public void Increment() => Value++; }

var list = new List<Counter> { new Counter() };
list[0].Increment(); // increments a copy, not the element in the list! No effect.
```

Avoid mutable structs. If you need mutability, prefer a class or use methods that replace the whole struct.

---

## Equality and identity

- Structs: by default, value equality (field-wise) via `ValueType.Equals`. For better performance and correctness, implement `IEquatable<T>` and override `GetHashCode`.
- Classes: default is reference equality (two references equal only if they point to the same instance). Override `Equals`/`GetHashCode` to provide value-based equality.

Example implementing equality on a struct:
```csharp
public readonly struct Point : IEquatable<Point>
{
    public int X { get; } public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
    public bool Equals(Point other) => X == other.X && Y == other.Y;
    public override bool Equals(object obj) => obj is Point p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}
```

---

## Boxing and performance considerations

- Boxing: when a struct is used where an `object` or interface is expected, the runtime creates a boxed copy on the heap — this is an allocation and can hurt performance if frequent.
- Arrays of structs store the data inline (no per-item heap object), which can be cache-friendly.
- But be careful: large structs copy a lot of memory on assignment/passing; that cost can outweigh the benefit of avoiding heap allocation.

Guidelines:
- Keep structs small (preferably under 16 bytes).
- Avoid boxing in tight loops — use generic constraints or specialized APIs to prevent boxing.
- Measure with a profiler before optimizing; sometimes classes are simpler and faster.

---

## Default values and nullable

- Structs have a default value: all fields zeroed. `default(Point)` yields `(0,0)`. Be mindful when a default state is invalid for your domain.
- Use `Nullable<T>` (`Point?`) when you need a null-like state for a value type.

---

## Interoperability and records

- Records: `record class` gives concise immutable reference types with value-like equality semantics (by content). `record struct` provides similar benefits for value types where appropriate.
- Choose records when you want concise, intention-revealing immutable DTOs.

Example record struct:
```csharp
public readonly record struct Size(int Width, int Height);
```

---

## Summary checklist

- Use struct when: small, immutable, represents a value, frequent short-lived instances, no inheritance needed.
- Use class when: identity matters, type is large or mutable, you need inheritance, or you will store references/shared state.
- Prefer `readonly struct` and `IEquatable<T>` for well-behaved value types.
- Avoid mutable structs, be careful with boxing, and measure performance when in doubt.

---

Practical exercise (in class/homework)
- For each of these types decide struct vs class and justify:
  - 2D integer Point used in tight math loops.
  - Customer record with orders and lifecycle.
  - Color (RGB bytes) used in graphics buffers.
  - Database entity with many fields and relationships.

Write one sentence justification per decision.