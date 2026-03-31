# Value and Reference Types; Numeric, Boolean, and Char — Day 2 Primer

This note explains core C# type categories you’ll use every day: value vs reference types, the common numeric types, booleans, and chars. Read it as a conceptual guide with small code examples you can run later.

## Value types vs Reference types (conceptual)

- Value types store their data directly. When you assign a value type variable to another, a copy of the data is made.
- Reference types store a reference (pointer) to data on the heap. Assigning a reference type variable copies the reference, so two variables can refer to the same object.

Common value types: `int`, `double`, `bool`, `char`, structs (user-defined `struct`).  
Common reference types: `class`, `string`, arrays, delegates.

Why this matters:
- Mutating a value type copy does not affect the original.
- Mutating the object behind a reference type is visible through all references.

Example — copy behavior:
```csharp
struct PointValue { public int X, Y; }    // value type
class PointRef { public int X, Y; }       // reference type

var a = new PointValue { X = 1, Y = 2 };
var b = a;         // b is a copy
b.X = 10;
// a.X stays 1

var c = new PointRef { X = 1, Y = 2 };
var d = c;         // d references the same object
d.X = 10;
// c.X is now 10
```

Important notes:
- Value types are typically stored on the stack (or inlined in objects/arrays); reference-type objects live on the heap.
- Boxing: when you treat a value type as an object (e.g., store an `int` in `object`), the runtime creates a boxed copy on the heap; unboxing extracts a value back.
```csharp
int i = 42;
object o = i;    // boxing
int j = (int)o;  // unboxing
```

## Numeric types
C# offers signed and unsigned integers, floating point, and `decimal` for high-precision financial calculations.

Integers (common choices):
- `byte` (8-bit unsigned) 0..255
- `short` / `Int16` (16-bit signed)
- `int` / `Int32` (32-bit signed) — the default integer type
- `long` / `Int64` (64-bit signed)

Floating-point:
- `float` (32-bit), suffix with `f` or `F`: `3.14f`
- `double` (64-bit) — default for floating literals: `3.14`
- `decimal` (128-bit) — use for money/precision, suffix `m` or `M`: `3.14m`

Example:
```csharp
int count = 100;
long big = 1_000_000_000_000L;
float ratio = 0.75f;
double avg = 2.5;      // double by default
decimal price = 19.95m;
```

Be aware of:
- Range and overflow. Use `checked` to detect overflow:
```csharp
checked
{
    int x = int.MaxValue;
    // this will throw OverflowException in checked context
    x = x + 1;
}
```
- Implicit conversions exist from smaller to larger types (e.g., `int` → `long`) but not necessarily the other way without a cast.

## Boolean (`bool`)
- `bool` stores `true` or `false`.
- Common in control flow and logical expressions.
- Short-circuit operators:
  - `&&` (AND) and `||` (OR) short-circuit — right-hand side is evaluated only when needed.

Example:
```csharp
bool isValid = (count > 0) && (avg > 0);
if (!isValid) Console.WriteLine("Invalid");
```

## Character (`char`)
- `char` holds a single UTF-16 code unit (useful for characters and simple text operations).
- Literals use single quotes: `'A'`, `'\n'`, `'\u03A9'` (Unicode escapes).
- To build strings, you combine `char`s or use `string` (reference type and immutable).

Example:
```csharp
char letter = 'A';
char newline = '\n';
char omega = '\u03A9';   // Ω

string s = "hello";
char first = s[0];       // indexing yields a char
```

## Nullable value types
Value types can be made nullable with `?` (e.g., `int?`). This is useful when a value might be absent.
```csharp
int? maybe = null;
if (maybe.HasValue) Console.WriteLine(maybe.Value);
int safe = maybe ?? 0; // null-coalescing operator provides a default
```

## Quick practical tips for students
- Default integer type: use `int` unless you need a larger/smaller range.
- For money use `decimal`, not `double`.
- Remember `string` is a reference type but compares by value with `==` because the operator is overloaded for `string`.
- Use `var` for local variable declarations when the type is obvious from the right-hand side; prefer explicit types when clarity matters.
- Prefer immutable patterns for safer code — avoid unintentionally sharing mutable reference types.

Summary: Understanding how values are stored and copied (value vs reference), what numeric types are available, and how `bool` and `char` behave will prevent many beginner mistakes and make it easier to reason about program state and performance. Practice small examples to see copying, boxing, overflow, and nullability in action.

--

Code Example - Value Types and Reference Types

```

using System;

struct PointStruct
{
    public int X;
    public int Y;
}

class PointClass
{
    public int X;
    public int Y;
}

class Program
{
    static void Main()
    {
        // 1) Primitive value type: assignment copies the value
        int a = 5;
        int b = a; // copy
        b = 10;
        Console.WriteLine($"Value types (int): a={a}, b={b}"); // a still 5

        // 2) User-defined value type (struct): assignment copies the whole struct
        PointStruct ps1 = new PointStruct { X = 1, Y = 2 };
        PointStruct ps2 = ps1; // copy of data
        ps2.X = 99;
        Console.WriteLine($"Struct copy: ps1.X={ps1.X}, ps2.X={ps2.X}"); // ps1.X still 1

        // 3) Reference type (class): assignment copies the reference (both refer to same object)
        PointClass pc1 = new PointClass { X = 1, Y = 2 };
        PointClass pc2 = pc1; // same object referenced by both variables
        pc2.X = 99;
        Console.WriteLine($"Class reference: pc1.X={pc1.X}, pc2.X={pc2.X}"); // both show 99

        // 4) Passing value vs reference to methods
        MutateStruct(ps1); // receives a copy -> no change
        Console.WriteLine($"After MutateStruct(ps1): ps1.X={ps1.X}");

        MutateClass(pc1); // receives reference -> mutates same object
        Console.WriteLine($"After MutateClass(pc1): pc1.X={pc1.X}");

        // 5) Use ref to modify a value type in-place
        Console.WriteLine($"Before MutateStructByRef: ps1.X={ps1.X}");
        MutateStructByRef(ref ps1);
        Console.WriteLine($"After MutateStructByRef: ps1.X={ps1.X}");

        // 6) Arrays are reference types
        int[] arr1 = { 1, 2, 3 };
        int[] arr2 = arr1; // both variables reference same array
        arr2[0] = 42;
        Console.WriteLine($"Array reference: arr1[0]={arr1[0]}, arr2[0]={arr2[0]}"); // both 42

        // 7) Boxing: value type boxed into object creates a copy on the heap
        int i = 100;
        object boxed = i; // boxing copies the value
        i = 200;
        Console.WriteLine($"Boxing: i={i}, boxed={(int)boxed}"); // boxed still 100

        // 8) Strings: reference type but immutable
        string s1 = "hello";
        string s2 = s1; // both reference same interned string initially
        s2 = s2.ToUpper(); // creates a new string; s1 unchanged
        Console.WriteLine($"Strings (immutable): s1={s1}, s2={s2}");

        // Pause
        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }

    static void MutateStruct(PointStruct p)
    {
        // This method receives a copy — changes here do not affect the caller
        p.X = 555;
    }

    static void MutateClass(PointClass p)
    {
        // This method receives a reference — changes affect the same object
        p.X = 555;
    }

    static void MutateStructByRef(ref PointStruct p)
    {
        // ref parameter modifies the caller's struct in-place
        p.X = 777;
    }
}

```
