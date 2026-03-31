# C#: The params keyword — guide for students

Date: 2026-03-31

This document explains the C# `params` keyword: what it does, how to use it, common patterns, pitfalls, and alternatives. It is intended as a classroom handout and includes concise examples.

---

## TL;DR

- `params` lets a method accept a variable number of arguments of a specified element type by using a single parameter that is a one-dimensional array.
- Callers can pass zero or more individual arguments, or an array. The compiler assembles the arguments into an array at the call site.
- The `params` parameter must be the last parameter in the method signature, and only one `params` parameter is allowed.
- Use `params` for convenience APIs (e.g., `Console.WriteLine`, logging, simple aggregation). For performance-sensitive code, watch out for array allocations.

---

## Syntax

```csharp
// basic form
void Foo(params int[] values) { ... }

// example with other parameters (params must be last)
void Log(string level, params string[] messages) { ... }
```

Key rules:
- The `params` parameter type must be a single-dimensional array (e.g., `T[]`).
- There can be at most one `params` parameter and it must come last.
- The parameter can be any element type: `int[]`, `string[]`, `object[]`, user-defined types, etc.

---

## How calls work

Given `void Sum(params int[] xs)`, these calls are valid:

```csharp
Sum();               // xs is an empty array
Sum(1);              // xs = new int[] { 1 }
Sum(1, 2, 3);        // xs = new int[] { 1, 2, 3 }
Sum(new int[] { 4, 5 }); // caller passes an array directly
Sum(null);           // xs == null (caller explicitly passed null)
```

Important: when the caller passes no arguments, the compiler typically passes an empty array (not `null`). If the caller explicitly passes `null`, the parameter receives `null`.

---

## Common examples

1) Simple aggregator

```csharp
static int Sum(params int[] values)
{
    if (values == null) return 0; // handle explicit null
    int s = 0;
    foreach (var v in values) s += v;
    return s;
}
```

2) Print-like API (heterogeneous arguments)

```csharp
static void PrintAll(params object[] items)
{
    foreach (var item in items)
        Console.WriteLine(item);
}

// Usage:
PrintAll(1, "two", 3.0);
PrintAll(new object[] { 1, 2, 3 });
```

3) Extension method with params

```csharp
public static class Extensions
{
    public static void AddRange<T>(this List<T> list, params T[] items)
    {
        if (items == null) return;
        list.AddRange(items);
    }
}
```

---

## Overload resolution and ambiguity

Because `params` accepts a variable number of arguments, you can run into ambiguous calls if overloads exist. The compiler prefers the best overload with exact matches. Examples:

```csharp
static void M(params int[] x) { Console.WriteLine("params"); }
static void M(int x)         { Console.WriteLine("single"); }

M(1); // calls M(int) because it's an exact match
M(1, 2); // calls M(params int[]) because no single-parameter overload matches two ints
```

Ambiguities can appear with `null` or with overloads that could accept the same argument list. Prefer distinct method names or explicit array creation when ambiguity is problematic.

---

## Performance considerations

- The compiler creates an array at the callsite when a caller provides arguments individually. That allocation can be significant in hot paths or tight loops.
- When the caller already has an array and passes it directly, no extra allocation occurs (the same array is passed).
- If you want to avoid allocations for performance-sensitive APIs, consider:
  - Overloads that accept `ReadOnlySpan<T>` / `Span<T>` (but `params` cannot be `Span<T>` — `params` requires an array).
  - Overloads that accept `IEnumerable<T>` or accept a `ReadOnlySpan<T>` in addition to the `params` overload.
  - Providing both `void Foo(params T[] items)` and `void Foo(ReadOnlySpan<T> items)` overloads; callers with arrays or spans can pick the optimal overload.

Example pattern:

```csharp
void Process(ReadOnlySpan<int> items) { ... }   // no allocation for spans
void Process(params int[] items) => Process(items); // convenience overload, may allocate
```

---

## Pitfalls & gotchas

- `params` must be last. Trying to declare `void F(params int[] x, int y)` is a compile-time error.
- Only one `params` parameter is allowed.
- `params` parameter is syntactic sugar — it is an array at runtime. Beware boxing if element type is `object`.
- Passing `null` explicitly results in a `null` inside the method — check for `null` if you accept it.
- Overuse of `params` can hide APIs that should accept a sequence/collection or a span for performance reasons.
- `params` does not support `ref` or `out` for the elements — the array elements themselves are not passed by reference.

---

## When to use params (best practices)

- Use `params` for convenience overloads and small collections of arguments (e.g., logging, simple combinators, test helpers).
- Prefer `params object[]` for variadic APIs that accept heterogeneous types (e.g., a simple formatting or diagnostic API).
- Avoid `params` in performance-sensitive code that is called frequently. Provide an alternative method that accepts `ReadOnlySpan<T>` or `IEnumerable<T>` and document the allocation behavior.
- If you need to allow "no arguments" and prefer to avoid allocations, offer an overload that takes no arguments and call the core implementation.

---

## Practical tips for students

- Remember the call-site allocation cost: write micro-benchmarks if performance is a concern.
- When you see a method signature with `params`, you can call it in either of the two forms: separate arguments, or an array.
- Be explicit when passing `null` if you want the callee to receive `null`; otherwise pass nothing to get an empty array.
- For APIs that accept many items where order or streaming matters, prefer `IEnumerable<T>` or `ReadOnlySpan<T>`.

---

## Summary

`params` is a helpful feature for making APIs friendlier to call by allowing a variable number of arguments. It is best used for convenience methods and small collections of inputs. Keep in mind the array allocation behavior and consider alternatives (spans, enumerables) for performance-sensitive code. Use clear overloads to avoid ambiguity and validate `null` if you need to handle it.