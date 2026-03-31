# Records Versus Tuples (C# / .NET)

Overview
--------
This document compares C# records (both `record class` and `record struct`) with tuples (`ValueTuple`) to show when to pick each, with code examples, equality/serialization notes, and guidance for API design.

Definitions (C#)
----------------
- record: a concise type with generated equality, `ToString`, `Deconstruct`, and support for `with`. Variants:
  - `record class` (reference type)
  - `record struct` (value type)
- ValueTuple: `System.ValueTuple<T1,...>` used with short syntax `(T1, T2)`; supports named elements and deconstruction.

Key contrasts
-------------
1. Intent & readability
   - record: clearly named type, intended as a domain concept with semantic meaning.
   - tuple: positional grouping; OK for quick, local uses but less descriptive.

2. Equality
   - both ValueTuple and record provide structural equality. Records generally have clearer semantics, especially when fields have names and the type is reused.

3. Extensibility and behavior
   - record: can have methods, validation, attributes, and be serialized as a proper type.
   - tuple: cannot easily carry methods or attributes; not ideal for public serialization/persistence.

Code examples
-------------
Record example:
```csharp
public record Customer(int Id, string Name);

// usage:
var c1 = new Customer(1, "Acme");
var c2 = new Customer(1, "Acme");
Console.WriteLine(c1 == c2); // True
```

Tuple example:
```csharp
// quick local return
public (int Id, string Name) GetCustomerTuple() => (1, "Acme");
var t = GetCustomerTuple();
Console.WriteLine(t.Id); // 1
```

Serialization & interop
-----------------------
- Records: serialize nicely with JSON serializers (System.Text.Json or Newtonsoft) to named fields — good for APIs.
- Tuples: serializers will generally serialize ValueTuples too, but the output can be less stable or clear for public contracts; prefer named records for stable API contracts.

API design guidance
------------------
- Use tuples for internal helper methods and small local groupings where introducing a new type would add noise.
- Use records for types crossing boundaries (APIs, persistence, messaging) where names, stability, and extensibility matter.

Pattern matching & deconstruction
--------------------------------
Both support deconstruction:
```csharp
var (id, name) = new Customer(2, "Bob");
var (tid, tname) = GetCustomerTuple();
```
Records allow richer constructs (methods, attributes) alongside deconstruction.

Pitfalls
--------
- Using tuples in public APIs: changes to tuple element order may break callers; names may not be obvious in IDEs.
- Overusing records for ephemeral data: introducing a type for a tiny, one-off grouping may be overkill — tuples are fine there.

Exercises
---------
1. Replace a method returning `(int, string, bool)` with a `record` and update unit tests and serialization checks.
2. Compare the JSON output of serializing a tuple vs a record using System.Text.Json.

Summary
-------
Tuples are great for quick, local grouping and destructuring; records are the right choice for named, durable data structures with clear semantics, serialization-friendly shape, and extendability. In C#/.NET, prefer records for public contracts and tuples for short-lived local needs.
