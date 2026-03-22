# Idiomatic in C# — explanation, trade-offs, and examples

Summary
- "Idiomatic" means using the conventions, patterns, and style that are natural and familiar to the community of a language.
- This note explains what "idiomatic" means in practice for C#, contrasts the Try/out pattern with returning result objects, describes trade-offs, and provides example implementations you can copy into your notes or codebase.

Table of contents
1. What "idiomatic" means
2. Why the Try(out) pattern exists
3. Why returning an object/tuple can be better
4. Trade-offs summary
5. Examples
   - 5.1 Try/out pattern (idiomatic Try-style)
   - 5.2 Return a result class
   - 5.3 Return a ValueTuple
   - 5.4 Return a record struct
   - 5.5 Cleaned-up Program with input validation
6. Other considerations
7. Recommendation
8. How to learn idiomatic C#

---

## 1. What "idiomatic" means
In programming, "idiomatic" refers to writing code that follows the conventions, patterns, and style natural to a language and its community — the way experienced practitioners expect problems to be solved. Idiomatic code is easier for other developers of the same language to read, maintain, and integrate with existing libraries and tooling.

Examples of idiomatic choices in C#:
- Prefer `int.TryParse` over catching exceptions for normal parse failures.
- Use `using` or `using var` for `IDisposable` cleanup.
- Use `await` for asynchronous calls instead of blocking with `.Result`.
- Use `foreach` over manual index-based loops for enumerable traversal.

---

## 2. Why the Try(out) pattern exists
The Try-style (`bool TryX(out T value)`) is established in .NET (e.g., `int.TryParse`, `Dictionary<TKey, TValue>.TryGetValue`). Reasons:
- Expresses intent clearly: method attempts work and returns success/failure via `bool`.
- Avoids allocations: results can be returned via `out` parameters without creating heap objects.
- Simple, lightweight API for common single-value success/case scenarios.
- Familiar and expected by many .NET developers.

---

## 3. Why returning an object/tuple can be better
Returning a result (class, struct, record, or tuple) has advantages:
- Readability: a single return value often reads better and composes with other APIs.
- Extensibility: you can add more fields (error codes, diagnostics) later without changing the signature drastically.
- Immutability and explicit naming: records or named tuples make intent clearer.
- Functional/composable code fits return-value patterns better than `out` parameters.

---

## 4. Trade-offs summary
- Performance: `out` and value-type returns (ValueTuple, record struct) can avoid heap allocations. Returning a class allocates on the heap.
- API surface & extensibility: classes/records are easier to extend; `bool + out` is rigid.
- Familiarity: `Try`-pattern is idiomatic for certain operations (parse/get).
- Composition: returning values fits better with expressions, LINQ, and functional styles.
- Breaking changes: changing return shape is breaking for public APIs, so choose carefully.

---

## 5. Examples

### 5.1 Try/out pattern (idiomatic Try-style)
```csharp
using System;

class Program
{
    static bool TryNumberIsTwenty(int somenumber, out string result)
    {
        if (somenumber == 20)
        {
            result = "The number you entered is definitely twenty.";
            return true;
        }

        result = "The number you entered is not twenty.";
        return false;
    }

    static void Main()
    {
        Console.Write("Please enter a number: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int number))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        if (TryNumberIsTwenty(number, out string message))
            Console.WriteLine(message);
        else
            Console.WriteLine(message);
    }
}
```

When to prefer:
- You want minimal allocations in performance-sensitive code.
- You want to follow .NET `Try` conventions (parsing, lookups).

---

### 5.2 Return a result class (extensible, readable)
```csharp
using System;

public class CheckResult
{
    public bool IsTwenty { get; }
    public string Message { get; }

    public CheckResult(bool isTwenty, string message)
    {
        IsTwenty = isTwenty;
        Message = message;
    }
}

class Program
{
    static CheckResult CheckNumber(int n)
    {
        if (n == 20) return new CheckResult(true, "The number you entered is definitely twenty.");
        return new CheckResult(false, "The number you entered is not twenty.");
    }

    static void Main()
    {
        Console.Write("Please enter a number: ");
        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int number))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        CheckResult r = CheckNumber(number);
        Console.WriteLine(r.Message);
    }
}
```

When to prefer:
- You want an extensible API; you may add properties later (e.g., `ErrorCode`, `Details`).
- Heap allocation is acceptable.

---

### 5.3 Return a ValueTuple (lightweight, no class allocation)
```csharp
using System;

class Program
{
    static (bool IsTwenty, string Message) CheckNumber(int n)
    {
        return n == 20
            ? (true, "The number you entered is definitely twenty.")
            : (false, "The number you entered is not twenty.");
    }

    static void Main()
    {
        Console.Write("Please enter a number: ");
        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int number))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        var result = CheckNumber(number);
        Console.WriteLine(result.Message);
    }
}
```

When to prefer:
- You want named return values, minimal ceremony, and avoid heap allocation.
- Suitable for small result sets.

---

### 5.4 Return a record struct (named, immutable value type)
```csharp
using System;

public readonly record struct CheckResult(bool IsTwenty, string Message);

class Program
{
    static CheckResult CheckNumber(int n)
    {
        return n == 20
            ? new CheckResult(true, "The number you entered is definitely twenty.")
            : new CheckResult(false, "The number you entered is not twenty.");
    }

    static void Main()
    {
        Console.Write("Please enter a number: ");
        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int number))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        var r = CheckNumber(number);
        Console.WriteLine(r.Message);
    }
}
```

When to prefer:
- You want the clarity of a named result type with the performance of a value type.

---

### 5.5 Cleaned-up Program with input validation
```csharp
using System;

class Program
{
    static bool TryNumberIsTwenty(int someNumber, out string result)
    {
        if (someNumber == 20)
        {
            result = "The number you entered is definitely twenty.";
            return true;
        }

        result = "The number you entered is not twenty.";
        return false;
    }

    static void Main()
    {
        Console.Write("Please enter a number: ");
        string? userInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine("No input provided.");
            return;
        }

        if (!int.TryParse(userInput.Trim(), out int number))
        {
            Console.WriteLine("Invalid number entered.");
            return;
        }

        bool isTwenty = TryNumberIsTwenty(number, out string message);
        Console.WriteLine(message);
    }
}
```

---

## 6. Other considerations
- Public API stability: changing method signatures is a breaking change; design return shapes carefully.
- Async: return values are better suited in `async` code; `out` parameters are less common for asynchronous result flows.
- Error handling: use return/result patterns for expected failures; reserve exceptions for exceptional conditions.
- Performance: measure in real scenarios. For many apps the allocation cost of a small class is negligible; in hot paths, prefer value types or `out` patterns.

---

## 7. Recommendation
- Use `Try`/`out` for small, common, performance-sensitive operations that match .NET conventions (parsing, lookups).
- Use `ValueTuple` or `record struct` for small, named value returns with no heap allocation.
- Use a class or record when you expect to extend the result with more fields or metadata, and allocations are not a concern.
- For public library APIs that may evolve, favor a dedicated result type (class/record) for future extensibility.

---

## 8. How to learn idiomatic C#
- Read the Microsoft C# coding conventions and .NET runtime/aspnet core source.
- Use Roslyn analyzers, StyleCop, and EditorConfig in your projects — they suggest idiomatic fixes.
- Read high-quality open-source C# projects to see patterns in real code.
- Participate in code reviews and pay attention to reviewer guidance on style and design.
- Practice rewriting small snippets using different patterns and benchmark when performance matters.

---

Notes
- "Idiomatic" is about shared expectations and readability within a community. It's not an absolute rule — sometimes non-idiomatic choices are necessary for performance or domain-specific reasons.
