# Implicit vs Explicit Typing, Constants, and Basic Operators — One‑Page Primer

This note explains when to use implicit (`var`) vs explicit types, how to declare constants and readonly values, and reviews the basic operators you’ll use in C#. Includes short examples and common pitfalls.

## Implicit (`var`) vs explicit typing

C# supports both explicit type declarations and implicit local typing with `var`.

- Explicit:
```csharp
int count = 10;
string name = "Alice";
```
- Implicit (compiler infers the type from the right-hand side):
```csharp
var count = 10;        // compiler infers int
var name = "Alice";    // compiler infers string
```

Guidelines:
- Use `var` when the type is obvious from the initializer or when the actual type is long/noisy (e.g., LINQ results).
- Prefer explicit types when it improves readability, or the initializer hides the actual type.
- `var` requires an initializer; you cannot write `var x;`.
- `var` does not mean “dynamic” — the variable still has a static compile-time type.

Pitfalls:
- `var obj = null;` is illegal because the compiler cannot infer a type.
- Using `var` with APIs that return general types (like `object`) can hide important details:
```csharp
var x = GetSomething(); // What is x? Use explicit if unclear.
```

## Constants and readonly values

- `const` (compile-time constant):
  - Must be initialized with a compile-time constant expression.
  - Implicitly static and cannot change.
```csharp
const double Pi = 3.14159;
```
- `readonly` (runtime constant for instance fields):
  - Can be assigned in declaration or constructor; cannot change after object construction.
  - Use for values that are constant per instance but determined at runtime.
```csharp
class C
{
    public readonly DateTime Created = DateTime.UtcNow;
    public readonly int Id;
    public C(int id) { Id = id; } // allowed
}
```
- `static readonly` is useful for values computed once at runtime:
```csharp
static readonly string Config = LoadConfig();
```

When to use which:
- `const` for true constants (numbers, fixed strings) that won't change across assemblies.
- `readonly`/`static readonly` for values that require computation at runtime or may change between runs (but not during object lifetime).