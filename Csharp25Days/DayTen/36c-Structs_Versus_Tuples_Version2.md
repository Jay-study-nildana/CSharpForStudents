# Structs Versus Tuples (C# / .NET)

Overview
--------
This guide compares C# structs and tuples (ValueTuples and tuple syntax) with examples: when to use each, equality semantics, deconstruction, and practical API guidance.

Definitions (C#)
----------------
- struct: a named value type declared with `struct`. Can have fields, properties, methods, and implement interfaces.
- tuple (ValueTuple): a lightweight ordered collection of values. In modern C#, tuples are represented by System.ValueTuple<T1,...> and have language syntax `(T1, T2)` and optional element names.

Tuple creation and named elements
--------------------------------
```csharp
// unnamed elements (positional)
var t1 = (1, "Alice", true);

// named elements
var t2 = (Id: 1, Name: "Alice", IsActive: true);

// deconstruction
var (id, name, active) = t2;
Console.WriteLine(name); // "Alice"
```

Struct example
--------------
```csharp
public readonly struct PersonStruct
{
    public int Id { get; }
    public string Name { get; }
    public bool IsActive { get; }

    public PersonStruct(int id, string name, bool isActive) =>
        (Id, Name, IsActive) = (id, name, isActive);

    public void Deconstruct(out int id, out string name, out bool isActive)
        => (id, name, isActive) = (Id, Name, IsActive);
}
```

Key differences in C#/.NET
-------------------------
1. Naming & readability
   - struct: named fields/properties make intent explicit.
   - tuple: element order matters; you can assign names, but those are syntactic conveniences and compile-time aliases.

2. Type identity and reuse
   - struct: defines a named type you can reuse in signatures.
   - tuple: often used for local returns; ValueTuple types appear in method signatures but are less self-documenting.

3. Methods & behavior
   - struct: can have methods, validation, custom equality (IEquatable<T>).
   - tuple: no methods beyond generated deconstruction and equality; not the place for behavior.

4. Equality semantics
   - ValueTuple equality is structural — element-wise equality is used.
   - struct can implement optimized equality or immutability patterns for robust semantics.

Examples comparing uses
-----------------------
Return multiple values from a function (tuple is succinct):
```csharp
// Tuple return
public static (int min, int max) MinMax(int[] arr)
{
    var min = arr.Min();
    var max = arr.Max();
    return (min, max);
}

// Usage
var result = MinMax(new[] {3, 1, 4});
Console.WriteLine(result.min); // 1
```

Domain modeling (struct is better):
```csharp
public readonly struct Range
{
    public int Min { get; }
    public int Max { get; }
    public Range(int min, int max) => (Min, Max) = (min, max);
}
```

Performance & interoperability
-------------------------------
- ValueTuple is a value type (System.ValueTuple) and avoids heap allocation for the tuple itself.
- Both structs and ValueTuple avoid GC for their storage when used locally, but large tuples/structs can be expensive to copy.
- Use `ref`/`in` parameters to avoid copying large structs.

API design guidance
-------------------
- Use tuples for quick local returns, small helper methods, or internal code where positional grouping is natural.
- Use structs for domain types where meaning, reusability, and methods are needed.
- For public APIs, prefer named types (struct/class/record) for clarity unless the tuple is well documented (named elements help).

Pitfalls with tuples in C#
--------------------------
- Misuse in public APIs makes the API brittle and less readable.
- Relying on positional elements when names exist can cause confusion if code changes.
- Deeply nested tuples are hard to read and maintain.

Exercises
---------
1. Replace a method that returns `(int, int, int)` with a `readonly struct` and compare readability and caller code.
2. Measure the cost (BenchmarkDotNet) of copying a `ValueTuple` of four ints vs a `readonly struct` of four ints in a tight loop.

Summary
-------
Use ValueTuple for simple, local multi-value returns and destructuring. Use structs when you need named types, methods, or to represent domain data. Favor readability and API stability when choosing between them in C#/.NET.
