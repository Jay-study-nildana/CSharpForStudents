# C#: struct, record (class) and record struct — explanation & examples

A concise guide to C# value and reference types focused on `struct`, `record` (class), and `record struct`. Suitable for sharing with students as a reference.

---

## Short recommendation (TL;DR)

- `struct` — a traditional value type (copy-by-value). Use for small, short-lived data or interop; prefer `readonly struct` for immutability.
- `record` — a reference type (class) with compiler-generated value-based equality, `ToString()`, `Deconstruct`, and `with` expressions. Use for immutable data models and DTOs when reference semantics are acceptable.
- `record struct` — a value-type record: value-based equality plus record conveniences but as a value type. Use for small immutable value objects to avoid heap allocation.

---

## Quick comparison

| Feature | struct | record (class) | record struct |
|---|---:|---|---:|
| Type category | Value type | Reference type | Value type |
| Equality | By-value (default ValueType.Equals/reflection) unless custom | Compiler-generated value equality (properties) | Compiler-generated value equality; implements IEquatable<T> |
| Copy semantics | Copied on assignment/passed by value | Reference assignment; `with` creates clone | Copied on assignment/passed by value; `with` produces modified copy |
| Compiler helpers | None (you implement) | Synthesizes properties, Equals, GetHashCode, ToString, Deconstruct, with support | Similar to record class but for a value type |
| Inheritance | No | Yes (records support inheritance) | No |
| Mutability | Can be mutable or readonly | Typically immutable (init-only props) but can be mutable | Can be readonly record struct for immutability |
| Introduced | Long-standing | C# 9 (.NET 5) | C# 10 (.NET 6) |

---

## What the compiler generates for records (high level)

For a positional record like:

```csharp
public record Person(string Name, int Age);
```

the compiler synthesizes:
- Public init-only properties for `Name` and `Age`
- `Deconstruct(out string name, out int age)`
- Value-based `Equals(object)`, `Equals(Person)`, and `GetHashCode()`
- `ToString()` that prints property names and values
- Support for `with` expressions (shallow copy then apply changes)

For `record struct` you get analogous synthesized methods appropriate for value types (including an efficient `IEquatable<T>` implementation).

---

## Detailed sections and examples

All examples assume modern C# (9/10+) and target frameworks that support records (C# 9 for `record` and C# 10 for `record struct`).

### 1) struct — value type

Use for small, efficient value objects. Be careful with mutability.

```csharp
public struct Point
{
    public int X { get; set; }    // mutable (be careful)
    public int Y { get; set; }
}
```

Example showing copy semantics:

```csharp
var a = new Point { X = 1, Y = 2 };
var b = a;          // copy of a
b.X = 10;
Console.WriteLine(a.X); // prints 1 — a is unchanged
```

Pitfall: mutable struct returned from a property can be surprising:

```csharp
public struct Counter { public int Value; public void Increment() => Value++; }
public class Holder { public Counter C { get; set; } } 

var h = new Holder { C = new Counter { Value = 0 } };
h.C.Increment(); // increments a copy — original not updated
```

To avoid problems:
- Prefer `readonly struct` for immutable value types.
- Keep structs small (guideline: ideally small like up to ~16 bytes; large structs cost to copy).
- Implement `IEquatable<T>` if equality is important and you want good performance.

### 2) record (class) — reference type record

Records are reference types by default and provide value-based equality and conveniences.

```csharp
public record Person(string Name, int Age);

// usage
var p1 = new Person("Alice", 30);
var p2 = new Person("Alice", 30);
Console.WriteLine(p1 == p2); // True — value-based equality

var p3 = p1 with { Age = 31 }; // shallow clone with changed Age
Console.WriteLine(p1);  // Person { Name = Alice, Age = 30 }
Console.WriteLine(p3);  // Person { Name = Alice, Age = 31 }
```

Notes:
- `==` is overridden to use value equality.
- Records support inheritance; equality members are generated as virtual to enable correct behavior across derived records.
- If you need identity/reference checks, use `ReferenceEquals` or compare by reference explicitly.

### 3) record struct — value type record

Combines record conveniences with value semantics.

```csharp
public readonly record struct Money(decimal Amount, string Currency);

// usage
var m1 = new Money(10m, "USD");
var m2 = m1 with { Amount = 20m }; // Creates a modified copy
Console.WriteLine(m1 == m2); // False
Console.WriteLine(m1); // Money { Amount = 10, Currency = USD }
```

Important:
- `record struct` still copies on assignment.
- Boxing rules apply if cast to `object`/interfaces (boxing allocates).
- Use `readonly record struct` for immutable semantics and to help the compiler optimize.

---

## Equality and identity — more detail

- class without overrides: equals by reference (two different instances with same state are not equal).
- `record` class: value-based equality comparing the declared properties.
- `struct`: value-based in concept, but the default `ValueType.Equals` uses reflection — thus often you should implement `IEquatable<T>` for speed and clarity.
- `record struct`: the compiler synthesizes an efficient `IEquatable<T>` implementation.

Example comparisons:

```csharp
// class without override
public class C { public int X { get; set; } }
var c1 = new C { X = 1 };
var c2 = new C { X = 1 };
Console.WriteLine(c1 == c2); // False (reference equality)

// record class
public record R(int X);
Console.WriteLine(new R(1) == new R(1)); // True

// regular struct (no custom equality)
public struct S { public int X; }
Console.WriteLine(new S { X = 1 }.Equals(new S { X = 1 })); // True (ValueType.Equals)
```

---

## Memory and performance considerations

- Value types (`struct`, `record struct`) live on the stack or inline in other objects; copying occurs on assignment and when passed by value.
- Large structs are expensive to copy — prefer classes if the payload is big or mutated frequently.
- Boxing: casting a struct/record struct to `object` or an interface boxes it (heap allocation). Avoid repeated boxing in tight loops.
- `readonly struct`/`readonly record struct`:
  - Prevent accidental mutation.
  - Allow more optimizations because the compiler knows the instance won't change.

General guideline: for small (<~16 bytes) immutable values, a `readonly struct` or `readonly record struct` is appropriate. For larger data or shared mutable state, use reference types.

---

## Common pitfalls and gotchas

- Mutable structs can behave unexpectedly due to copying semantics (especially when returned from properties or used as auto-properties).
- Expect `record struct` to behave like a struct — it does not give reference semantics.
- `with` on a `record` class is a shallow copy. If properties are reference types, the references are copied (not deep-cloned).
- Equality for records is value-based; if you rely on object identity, use `ReferenceEquals`.
- When using inheritance with records, equality generation is careful to avoid mixing types incorrectly, but it adds complexity — test expected behavior.

---

## When to choose which

- Use `struct` when:
  - Representing a small value-like data (Point, Color, small numeric tuple).
  - You need low-level value semantics or interop with native code.
  - Prefer `readonly struct` to signal immutability.
- Use `record` (class) when:
  - You want concise immutable types with value-based equality, built-in `ToString()` and `with`.
  - You might need inheritance among record types.
- Use `record struct` when:
  - You want compact value semantics + record conveniences (value-based equality, `with`, `ToString`, `Deconstruct`).
  - The type is small and efficiently copied.

---

## Version notes

- `record` (reference records) were introduced in **C# 9** (.NET 5).
- `record struct` was introduced in **C# 10** (.NET 6).
- `init`-only setters and `with` expressions came alongside record improvements in these C# versions.

---

## Example: runnable program showing behaviors

```csharp
using System;

public record Person(string Name, int Age);
public readonly record struct Money(decimal Amount, string Currency);
public struct Point { public int X; public int Y; }

class Program
{
    static void Main()
    {
        var p1 = new Person("Alice", 30);
        var p2 = p1 with { Age = 31 };
        Console.WriteLine($"Person equality: {p1 == p2}"); // False
        Console.WriteLine(p1); // Person { Name = Alice, Age = 30 }

        var m1 = new Money(10m, "USD");
        var m2 = m1 with { Amount = 20m };
        Console.WriteLine($"Money equality: {m1 == m2}"); // False
        Console.WriteLine(m1); // Money { Amount = 10, Currency = USD }

        var a = new Point { X = 1, Y = 2 };
        var b = a;
        b.X = 10;
        Console.WriteLine($"Point a.X = {a.X}, b.X = {b.X}"); // 1 and 10
    }
}
```

Expected console output:
```
Person equality: False
Person { Name = Alice, Age = 30 }
Money equality: False
Money { Amount = 10, Currency = USD }
Point a.X = 1, b.X = 10
```

---

## Short checklist for students

- Is it small and used like a number? Consider `readonly struct` or `readonly record struct`.
- Do you want auto-generated value equality, `ToString`, and `with`? Use `record` or `record struct`.
- Do you need inheritance? Use `record` (class).
- Is copying cost a concern (large payload)? Prefer `class` (maybe `record`) to avoid expensive copies.
- Avoid mutable structs unless you know copying semantics well.

---

## Further reading / references

- C# language reference: Records — see Microsoft docs for `record` and `record struct`.
- Best practices for value types: guidelines on struct size, immutability, and implementing `IEquatable<T>`.

