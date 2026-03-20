# If/Else and Switch — Decision Making in C# (.NET)

Objectives: understand branching with `if`/`else` and `switch`; know when to prefer one over the other; write clear, correct branches; comment on complexity.

## Quick summary
- `if` / `else` — use for general boolean conditions, range checks, and when you need flexible, conditional logic.
- `switch` / `switch expression` — use when selecting behavior based on a single value (or pattern), especially for many discrete cases; modern C# supports expressive pattern matching.

---

## if / else — syntax & examples

Basic if/else:
```csharp
int n = GetNumber();
if (n > 0)
{
    Console.WriteLine("Positive");
}
else if (n < 0)
{
    Console.WriteLine("Negative");
}
else
{
    Console.WriteLine("Zero");
}
// Time: O(1) — checks a small fixed set of conditions
```

Use `if`/`else` when you need boolean logic, ranges, or short-circuit checks:
```csharp
string s = input?.Trim();
if (string.IsNullOrEmpty(s))
{
    Console.WriteLine("No input provided.");
}
else if (int.TryParse(s, out int value))
{
    Console.WriteLine($"Parsed {value}");
}
else
{
    Console.WriteLine("Input is not a number.");
}
```

Tips:
- Evaluate expensive conditions last (short-circuiting matters).
- Keep conditions simple and readable; extract helpers when needed.
- For many range checks (grade boundaries, thresholds), an ordered `if`/`else if` ladder is clear.

---

## switch (statement) and switch expression — examples

Classic `switch` statement:
```csharp
int day = (int)DateTime.Today.DayOfWeek;
switch (day)
{
    case 0:
        Console.WriteLine("Sunday"); break;
    case 1:
        Console.WriteLine("Monday"); break;
    // ...
    default:
        Console.WriteLine("Unknown day"); break;
}
```

Switch expression (concise, returns a value):
```csharp
string dayName = DateTime.Today.DayOfWeek switch
{
    DayOfWeek.Sunday => "Sunday",
    DayOfWeek.Monday => "Monday",
    DayOfWeek.Tuesday => "Tuesday",
    _ => "Other"
};
```

Pattern matching with switch:
```csharp
object obj = GetValue();
string description = obj switch
{
    int i when i < 0 => "Negative int",
    int i => $"Integer {i}",
    string s => $"String of length {s.Length}",
    null => "Null value",
    _ => "Other"
};
```

When to use `switch`:
- Many distinct cases based on one value (enums, exact values).
- When returning different values per case (switch expression reads clearly).
- Pattern matching simplifies type-based branching.

C# notes:
- Switch statements do not allow implicit fall-through (each case must `break`, `return`, or `goto`).
- Switch expressions are exhaustive or require a default (`_`) arm.
- Use `when` guards in patterns for more precise matching.

---

## Common pitfalls and best practices
- Off-by-one / wrong condition order in `if` ladders; check boundaries carefully.
- Modifying the value you’re switching on inside branches can be confusing—avoid.
- Rely on `switch` for clarity when the logic is simple per-case; prefer `if` for complex boolean logic.
- Avoid duplication across branches: extract shared code or return early.
- Comment intention for unusual guards (e.g., `when` clauses).

---

## Homework (4 tasks — implement or pseudocode; include complexity)

1. Classify an integer as Positive / Negative / Zero (use `if`/`else`) — Time: O(1), Space: O(1).  
   Example pseudocode:
   ```
   if n > 0 return "Positive"
   else if n < 0 return "Negative"
   else return "Zero"
   ```

2. Map day-of-week number (0–6) to name (use `switch` or `switch` expression) — Time: O(1).  
   Ask: why is `switch` clearer here than many `if` checks?

3. Grade calculator: given score 0–100, return A/B/C/D/F (use ordered `if`/`else if` for ranges) — Time: O(1).  
   Note: test boundary values (e.g., 89/90) and document correctness.

4. Parse a mixed input (int, string, null) and return a description (use pattern-matching `switch`) — Time: O(1) relative to input size; complexity grows with number of patterns m: O(m).  
   Example:
   ```csharp
   return obj switch
   {
       int i when i < 0 => "Negative int",
       int i => "Non-negative int",
       string s => $"Text({s.Length})",
       null => "Missing",
       _ => "Other"
   };
   ```

---

Final notes
- Explain why you chose `if` vs `switch` in each homework solution.
- Add complexity comments (Big-O) and test boundary/edge cases.
- Bring one example of each branch style to class so we can diagram the flowcharts and discuss trade-offs.