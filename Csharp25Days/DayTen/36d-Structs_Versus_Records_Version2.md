# Structs Versus Records (C# / .NET)

Overview
--------
This document contrasts C# `struct` (value type) with `record` (data-centric type introduced in C# 9), including `record class` and `record struct`. It shows use-cases, equality, immutability, and sample code.

Definitions (C#)
----------------
- struct: A C# value type (`struct`). Can be mutable or readonly. Good for small data, performance, and interop.
- record: A C# type introduced for concise data carriers. There are two primary forms:
  - record class (reference type): `record Person(string Name, int Age);`
  - record struct (value type): `record struct Point(int X, int Y);`

Key differences in .NET
----------------------
1. Default equality
   - record class: provides value-based equality (by contents) and `ToString()` by default.
   - record struct: provides value-based equality, but is a value type (no boxing for the record struct itself).
   - struct: default `ValueType.Equals` may be field-wise, but records explicitly generate equality code; `record struct` also supports `with`-expressions in C# 10+.

2. Mutability and `with` expressions
   - record class: typically immutable when properties are init-only, supports `with` expressions to create modified copies.
   - record struct: can be used for immutable small value records, also supports `with`-style creation (C# versions permitting).
   - struct: can be mutable or immutable; no automatic `with` without custom code.

3. Syntax concision
   - record: concise positional syntax reduces boilerplate for constructors, equality, and `Deconstruct`.

Code examples
-------------

Record class (reference type):
```csharp
public record Person(string Name, int Age);

// usage
var p1 = new Person("Alice", 30);
var p2 = new Person("Alice", 30);
Console.WriteLine(p1 == p2); // True (value equality)
var p3 = p1 with { Age = 31 }; // copy with modification
```

Record struct (value type):
```csharp
public readonly record struct Point(int X, int Y);

// usage
Point a = new Point(1, 2);
Point b = a; // copy
Console.WriteLine(a == b); // True (value equality)
```

Struct with manual equality (if you need control):
```csharp
public readonly struct Vector3 : IEquatable<Vector3>
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public Vector3(float x, float y, float z) => (X, Y, Z) = (x, y, z);
    public bool Equals(Vector3 other) => X==other.X && Y==other.Y && Z==other.Z;
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}
```

When to choose record vs struct
-------------------------------
- Use `record class` when:
  - You want value-based equality for reference types, immutability by default, and `with` semantics.
  - The object may be large or you need reference semantics with content equality.

- Use `record struct` when:
  - You want value semantics plus the convenience of records (generated equality, deconstruct) and small size.
  - You want compact representations with content equality but no heap allocation.

- Use `struct` when:
  - You need low-level control over layout, performance-sensitive value types, or interop scenarios. Prefer readonly and implement IEquatable<T>.

Pitfalls and hazards
--------------------
- `record class` semantics with reference identity: while equality is structural, object identity (ReferenceEquals) remains meaningful.
- Large `record struct` or struct types still suffer copy costs.
- Mutable `record struct` can lead to surprising behavior if treated like immutable records.

Teaching exercises
------------------
1. Convert a `class` with boilerplate `Equals`/`GetHashCode` into a `record` and show how `with` simplifies copy-on-modify patterns.
2. Implement a `record struct` and measure allocation differences vs a `record class` when creating many instances.

Summary
-------
Records are higher-level, data-centric types; `record class` provides reference-type value equality, `record struct` gives value-type convenience. Structs remain appropriate for low-level, small, performance-critical values. Choose based on required semantics (identity vs value), size, and API clarity.
