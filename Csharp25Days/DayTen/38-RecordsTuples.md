# Day 10 — Simple Record-like Data Shapes & Tuples (C# / .NET)

This guide introduces two lightweight ways to represent grouped data in C#: record-like data shapes (records / record structs) and tuples (ValueTuple / named tuples). You’ll learn syntax, semantics, when to choose each, and practical examples for everyday use.

---

## Records (record / record struct)

Records are designed for immutable, named data shapes with value-like equality and concise syntax. Use records when the type represents a meaningful domain concept, DTO, or value object.

Example — immutable reference record (C# 9+):
```csharp
public record Person(string FirstName, string LastName, int Age);

// create
var p1 = new Person("Ada", "Lovelace", 36);

// with-expression (create modified copy)
var p2 = p1 with { Age = 37 };

// structural equality
Console.WriteLine(p1 == p2); // false

// deconstruct
var (first, last, age) = p1;
```

Key points:
- Records provide value-based equality (contents compared), helpful for tests and caching.
- `record` is a reference type; `record struct` is a value type (C# 10+). Use `record struct` for small, immutable value-like types.
- Records support deconstruction and `with` expressions to create modified copies.
- Records can include methods, validation, and computed properties — they are full types, not anonymous.

Example — readonly record struct:
```csharp
public readonly record struct Size(int Width, int Height);
var s = new Size(100, 200);
```

Use records when:
- You need a named type in public APIs.
- You want immutability by default and clear equality semantics.
- You want to attach behavior (methods) or add XML docs.

---

## Tuples (ValueTuple / named tuples)

Tuples are anonymous, lightweight groupings of values, ideal for returning multiple values from a local method or quick destructuring.

Example — named ValueTuple return:
```csharp
public static (int Sum, double Average) Summarize(int[] values)
{
    int sum = 0;
    foreach (var v in values) sum += v;
    double avg = values.Length == 0 ? 0 : (double)sum / values.Length;
    return (sum, avg);
}

var result = Summarize(new[] { 1, 2, 3 });
Console.WriteLine($"Sum={result.Sum}, Avg={result.Average}");
```

Key points:
- ValueTuple supports structural equality and deconstruction: `(a, b) = (1, 2);`
- Named tuples give field-like names but are still anonymous types — they have no explicit type name you can reference elsewhere.
- `System.Tuple` (the older reference tuple) exists but ValueTuple is preferred for performance and syntax.

Use tuples when:
- You need to return small sets of values from a method (often internal/local).
- You want concise deconstruction without defining a separate type.
- The data is temporary and doesn’t need documentation or attached behavior.

Avoid using tuples for public or long-lived API contracts — prefer named types (records or classes) for clarity, versioning, and documentation.

---

## Comparison: Records vs Tuples

- Named/Documented vs Anonymous:
  - Record: named type, documented, extensible, can add methods and validation.
  - Tuple: anonymous, concise, good for local returns.
- Equality:
  - Record: structural equality implemented for you (and can be customized).
  - ValueTuple: structural equality of elements.
- Immutability:
  - Record: commonly immutable (init-only or positional constructor); supports `with`.
  - Tuple: element mutability depends on types used; ValueTuple elements are mutable if declared mutable.
- Performance:
  - record struct / ValueTuple are value types — stack-allocated and copy semantics (be mindful of size).
  - record (reference) allocates on heap.
- Versioning:
  - Records are safer for public APIs (you can add members in controlled ways).
  - Tuples are brittle for long-term API evolution (adding fields breaks callers).

---

## Practical tips & best practices

- Use records for DTOs, domain value objects (Money, Point), and public API data shapes.
- Use tuples for short-lived, internal returns (e.g., parsing results, small aggregations).
- Prefer `record struct` for small immutable value types that you want value semantics for (e.g., `Size`, `Point`).
- Avoid returning tuples from public library methods; prefer a record type for clarity and future changes.
- Use deconstruction when convenient, but keep code readable — named properties on records are clearer than destructured tuples in many cases.
- When serializing, records are friendly with serializers (JSON) and keep property names explicit; named tuples serialize less clearly and may produce odd field names.

---

## Quick examples: converting between the two
```csharp
// Tuple to Record
var t = (Sum: 10, Avg: 5.0);
var summary = new SummaryRecord(t.Sum, t.Avg);

// Record to Tuple by deconstruction
var (sum, avg) = summary;
```

---

## Classroom exercise
- Convert a method that returns a tuple `(bool success, string message, Data dto)` into returning a record `OperationResult` and discuss advantages for tests and documentation.
- Design a small immutable `Money` record or `record struct` and show equality and `with` usage.

---

What I did and next
- I prepared a one‑page Markdown reference covering record-like data shapes and tuples with examples and guidance. If you want, I can: (a) make a short slide or printable cheat-sheet, (b) produce starter exercises converting tuples to records, or (c) generate runnable sample projects demonstrating usage.