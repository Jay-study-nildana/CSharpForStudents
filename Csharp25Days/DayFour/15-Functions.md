# Methods, Parameters, Return Values, and Scope (C# / .NET)

Objectives: learn how to break problems into methods, understand parameter passing and return values, and reason about variable scope and lifetime. Emphasize single responsibility and readable signatures.

---

## Why methods?
Methods let you:
- Decompose complex tasks into named, testable steps (single responsibility).
- Reuse behavior and reduce duplication.
- Separate "what" (high-level algorithm) from "how" (implementation details).

A good method has:
- A clear name describing what it does.
- A small number of parameters (preferably immutable inputs).
- A single responsibility and a clear return value or side-effect.

---

## Basic syntax and example

```csharp
// A simple method that returns the sum of two integers
int Add(int a, int b)
{
    return a + b;
}

// Usage
int result = Add(3, 5); // 8
```

Notice: parameters `a` and `b` are local to `Add` — they are in scope only inside the method.

---

## Parameter passing: value, ref, out, params, and defaults

- By default, C# passes value types (e.g., int, struct) by value: a copy is made.
- Reference types (objects) are passed by value of the reference: the method receives a copy of the reference, so it can mutate the object's state but not reassign the caller's variable unless `ref` is used.
- Use `ref` to pass a variable by reference (both read and write). Use `out` when the method will definitely assign the parameter before returning.
- `params` allows a variable number of arguments.
- Default parameter values let callers omit common arguments.

Examples:

```csharp
void IncrementByValue(int x) { x++; }          // caller not affected
void IncrementByRef(ref int x) { x++; }        // caller variable updated

void GetCoordinates(out int x, out int y)      // out parameters must be assigned
{
    x = 10;
    y = 20;
}

int Sum(params int[] values)                    // accepts 0..n ints
{
    int s = 0;
    foreach (var v in values) s += v;
    return s;
}

void Log(string message, bool verbose = false)  // default parameter
{
    if (verbose) Console.WriteLine("[VERBOSE] " + message);
    else Console.WriteLine(message);
}
```

Use `ref` and `out` sparingly: prefer returning values (including tuples) for clarity.

---

## Returning values: simple, tuples, and side-effects

Return styles:
- Single value (primitive or object).
- Tuple for multiple return values (C# tuples are lightweight).
- `void` for actions (methods that produce side-effects).
- Prefer returning values over mutating parameters when possible — it's easier to reason about and test.

Example returning multiple results with tuple:

```csharp
(bool success, int quotient, int remainder) Divide(int a, int b)
{
    if (b == 0) return (false, 0, 0);
    return (true, a / b, a % b);
}

var result = Divide(7, 3);
if (result.success) Console.WriteLine($"{result.quotient} rem {result.remainder}");
```

---

## Scope and lifetime: locals, parameters, fields, and static

- Parameter scope: method parameters are local to the method.
- Local variables: exist only inside their block; after the method returns they are eligible for garbage collection (or popped from the stack for value types).
- Block scope: variables declared inside `{ }` are invisible outside that block.
- Instance fields: belong to an object and live as long as the object is reachable.
- Static fields: belong to the type and live for the lifetime of the AppDomain.
- Heap vs Stack (brief): reference types and their data live on the heap; local value types are often on the stack; the GC reclaims heap objects when no references remain.

Example illustrating scopes:

```csharp
class Counter
{
    private int _count;           // instance field: lives with the object

    public void Increment()
    {
        int local = 0;            // local variable: scope limited to Increment
        local++;
        _count++;
    }
}
```

Be careful with captured variables in lambdas — closures extend the lifetime of captured variables to match the delegate's lifetime.

---

## Decomposition example: solve by composing small methods

Problem: Read text lines, parse ints, compute average ignoring invalid lines.

High-level decomposition:
- ReadLines(string path) : IEnumerable<string>
- TryParseInt(string s, out int value) : bool
- ComputeStatistics(IEnumerable<int> values) : (int count, double mean)

Sketch:

```csharp
IEnumerable<string> ReadLines(string path) { /* file IO */ }
bool TryParseInt(string s, out int value) { return int.TryParse(s, out value); }
(double mean, int count) ComputeStatistics(IEnumerable<int> values) { /* accumulate */ }
```

Each method is small, testable, and focused.

---

## Best practices and style

- Keep methods short (ideally < 20–30 lines). If it's longer, consider extracting helpers.
- Name methods for their action: GetUser, CalculateTax, IsPrime — using verbs for behavior, predicates for bools (Is/Has/Can).
- Prefer pure functions (no side effects) where possible. If side-effects are necessary (I/O, updating state), keep them at the edges of your program.
- Validate arguments early (guard clauses) and throw meaningful exceptions for invalid inputs.
- Use `async`/`await` for I/O-bound methods that should not block threads (signature: Task or Task<T>).

---

## Classroom tasks / Homework (method-level pseudo-implementations)

1. Write method signatures and pseudocode for:
   - ParseCsvRow(string row) : string[] — split and trim fields; handle quoted commas.
   - AggregateScores(IEnumerable<int> scores) : (int count, int sum, double mean)

2. Decompose a medium problem (e.g., password strength check) into: 
   - bool HasRequiredLength(string p), 
   - bool HasUpperLowerDigits(string p),
   - bool HasSpecialChar(string p),
   - int PasswordStrength(string p) // uses the helpers above

For each method provide:
- Signature
- Short pseudocode / one-line description
- Expected time/space complexity (Big-O)

---

## Quick summary
- Use methods to modularize and clarify code.
- Prefer returning values (including tuples) over mutating inputs; use `ref`/`out` only when necessary.
- Understand scope: parameters and locals are local to a method or block; fields persist with objects or types.
- Keep method responsibilities small and explicit; use clear names and guard clauses.

Bring two medium-sized problems to the next class; we will sketch method signatures and pseudocode together and discuss testability and scope/lifetime trade-offs.