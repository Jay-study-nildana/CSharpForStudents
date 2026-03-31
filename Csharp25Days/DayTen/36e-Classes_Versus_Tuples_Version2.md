# Classes Versus Tuples (C# / .NET)

Overview
--------
This note compares C# classes (reference types) with tuples (System.ValueTuple), showing the scenarios when each is appropriate and providing example code for patterns commonly used in .NET.

Definitions (C#)
----------------
- class: A reference type defined with the `class` keyword. Classes support methods, properties, inheritance, and encapsulation.
- tuple: A value-based ordered collection using `(T1, T2)` syntax and `System.ValueTuple<T1,...>`; often used for returning multiple values.

Key differences (C# specifics)
-----------------------------
1. Behavior & Methods
   - class: encapsulates behavior and state; can have methods, events, and lifecycle behavior.
   - tuple: holds data only; no methods beyond deconstruction and generated members.

2. Identity
   - class: has object identity (`ReferenceEquals`). Two distinct objects can be unequal by reference.
   - tuple: structural value equality — `(1,2) == (1,2)` is true.

3. API design & clarity
   - class: use for domain objects, long-lived instances, and public APIs.
   - tuple: use for local, short-lived multi-value returns or destructuring.

Example: returning multiple values
----------------------------------
Tuple (concise):
```csharp
public (bool Success, string Message, int Count) TryParseLine(string line)
{
    if (string.IsNullOrEmpty(line))
        return (false, "Empty", 0);
    // parse...
    return (true, "Ok", 1);
}
```

Class (clearer if data has behavior or lifecycle):
```csharp
public class ParseResult
{
    public bool Success { get; }
    public string Message { get; }
    public int Count { get; }

    public ParseResult(bool success, string message, int count) =>
        (Success, Message, Count) = (success, message, count);

    public void Log() => Console.WriteLine($"{Message} ({Count})");
}
```

When to prefer class over tuple
-------------------------------
- Use a class (or record) for public APIs, when data has behavior, or requires future extension.
- Use a tuple for simple, local returns, especially in private helper methods, where introducing a type would be overkill.

Deconstruction & pattern matching
--------------------------------
Tuples integrate smoothly with pattern matching:
```csharp
var (a, b) = (1, 2);
if ((a, b) is (1, _)) Console.WriteLine("starts with 1");
```
Classes can provide `Deconstruct` to support deconstruction:
```csharp
public class Person
{
    public string Name { get; }
    public int Age { get; }
    public Person(string name, int age) => (Name, Age) = (name, age);
    public void Deconstruct(out string name, out int age) => (name, age) = (Name, Age);
}
```

Pitfalls & best practices
-------------------------
- Avoid tuples for public surface area; they lack semantic clarity and are brittle to change.
- Avoid using tuples as a poor-man's class when behavior is expected.
- Consider `record` as a concise alternative to class for immutable, data-focused APIs.

Exercise
--------
1. Refactor code that returns `(int, string)` into a `record` and update callers; compare readability and discoverability in IDEs.

Summary
-------
Tuples are convenient for short-lived grouping and quick returns in C#; classes are the right choice for behavior, identity, and rich APIs. Prefer explicit named types for public APIs or when the data needs methods/validation.
