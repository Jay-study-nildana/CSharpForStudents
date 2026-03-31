# Structs Versus Classes (C# / .NET)

Overview
--------
This note compares C# structs and classes with C#/.NET examples, focusing on semantics, memory, practical use, and pitfalls. It assumes C# 9+ (so records are available) and .NET Core/.NET 5+ runtime behavior.

Definitions (C#)
----------------
- struct: A value type (System.ValueType) defined with the `struct` keyword. Instances are copied on assignment unless passed by reference (`ref`, `in`, `out`). Useful for small data containers and performance-sensitive code when used correctly.
- class: A reference type. Variables hold references to heap-allocated objects. Assignment copies the reference; multiple variables may reference the same object.

Core differences in C#/.NET
---------------------------
1. Value vs Reference semantics
   - struct: value semantics — assignment copies fields.
   - class: reference semantics — assignment copies reference.

2. Memory/boxing
   - struct: typically allocated inline (stack or inline in arrays). Passing a struct to a method expecting `object` or an interface can cause boxing (allocation on the heap).
   - class: always heap-allocated (except in some advanced runtime cases like stackalloc for objects via experimental features).

3. Default constructor and initialization
   - struct: always has an implicit parameterless constructor that sets fields to defaults. You cannot define a parameterless constructor (until C# 10+ allows parameterless struct constructors with restrictions).
   - class: can have custom parameterless constructors.

4. Inheritance & polymorphism
   - struct: cannot inherit from another struct or class (only from interfaces), and cannot be `abstract` or `sealed` in the same way classes can.
   - class: supports inheritance, virtual methods, and polymorphism.

5. Equality
   - struct: default `Equals` does a field-wise comparison when using ValueType's implementation, but you should override `IEquatable<T>` for efficient equality.
   - class: default `Equals` is reference equality unless overridden.

Practical examples
------------------

Value copying (struct):
```csharp
public struct PointStruct
{
    public int X;
    public int Y;
}

var a = new PointStruct { X = 1, Y = 2 };
var b = a;     // copy
b.X = 10;
Console.WriteLine(a.X); // 1 — original unaffected
```

Reference sharing (class):
```csharp
public class PointClass
{
    public int X;
    public int Y;
}

var a = new PointClass { X = 1, Y = 2 };
var b = a;     // reference copy
b.X = 10;
Console.WriteLine(a.X); // 10 — both refer to same object
```

Equality and IEquatable<T> for structs (recommended):
```csharp
public readonly struct Point : IEquatable<Point>
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) => (X, Y) = (x, y);
    public bool Equals(Point other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Point p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}
```

Performance notes
-----------------
- Prefer structs when the type is small (commonly recommended: <= 16 bytes, but this depends). Large structs are expensive to copy.
- Mark structs `readonly` when immutable to enable better optimizations and avoid defensive copies.
- Avoid mutable structs used in APIs as they lead to subtle bugs (e.g., modifying a copy from a property getter).

Boxing considerations
--------------------
When a struct is cast to `object` or an interface, it is boxed (heap allocation):
```csharp
Point p = new Point(1,2);
object o = p; // boxing: heap allocation
```
Minimize boxing in hot paths.

When to use struct vs class
---------------------------
Use struct when:
- Small, logically single values (Point, Color, complex keys).
- You want value semantics and immutability (prefer `readonly struct`).
- You need predictable layout (e.g., interop, memory-mapped data).

Use class when:
- You need identity, shared mutable state, inheritance, or polymorphism.
- Object is large, or copying is expensive.
- Lifetime management across asynchronous boundaries / complex lifecycles.

Common pitfalls (C# specifics)
-----------------------------
- Mutable struct stored in collections: `list[0].X = 5;` modifies a copy — won't update the item in the list.
- Structs implementing interfaces can cause boxing on interface calls.
- Large `readonly struct` can still be expensive if copied frequently.

Exercises for students
----------------------
1. Implement a `readonly struct` `Complex` with operations `Add` and `Multiply` and compare performance to a `class` implementation in a tight loop (BenchmarkDotNet).
2. Demonstrate boxing by implementing an interface on a struct and calling the interface method; observe allocations with a memory profiler.

Summary
-------
In C#/.NET, choose structs for small value-like data with immutability and predictable layout. Choose classes for objects with identity, complex behavior, or where copying would be expensive. Understand boxing, `readonly struct`, and `IEquatable<T>` to build efficient, bug-free code in .NET.
