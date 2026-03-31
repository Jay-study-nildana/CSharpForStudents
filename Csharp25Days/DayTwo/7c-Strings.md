# The string Type — One‑Page Primer

Strings are one of the most used types in everyday programs. In C#, `string` is a reference type that represents an immutable sequence of UTF‑16 characters. This note explains the key concepts you need to teach and use: creation, immutability, common operations, performance considerations, and safe comparison.

## Basics and creation
- Literal:
```csharp
string s1 = "Hello, world!";
```
- Empty vs null:
```csharp
string empty = string.Empty; // preferred over ""
string? maybe = null;        // nullable reference type (C# 8+)
```
- Verbatim strings (ignore escape sequences, useful for file paths and multiline text):
```csharp
string path = @"C:\Users\Public\Documents";
string multi = @"Line1
Line2";
```
- Escape sequences:
```csharp
string escaped = "First line\nSecond line\tTabbed";
```

## Immutability
Strings are immutable: every operation that changes a string returns a new string object. This makes strings safe to share between variables, but repeated concatenation in loops can be inefficient.

Example (creates new objects):
```csharp
string a = "Hi";
string b = a + " there";   // b is a new string; a unchanged
```

## Concatenation and interpolation
- Concatenation:
```csharp
string full = firstName + " " + lastName;
```
- Interpolation (recommended for readability):
```csharp
string name = $"User: {firstName} {lastName}";
```
- `string.Format` (older style) and `string.Concat` / `string.Join` for arrays:
```csharp
string joined = string.Join(", ", new[] { "a", "b", "c" }); // "a, b, c"
```

## Useful instance methods
- `Length`, `IndexOf`, `Substring`, `Contains`, `Replace`, `Split`, `Trim`, `ToUpper` / `ToLower`:
```csharp
string s = "  Hello  ";
s = s.Trim();              // "Hello"
bool has = s.Contains("ell");
int idx = s.IndexOf('l');  // first index
string sub = s.Substring(1, 3);
```

## Equality and comparison
- `==` and `.Equals()` perform content comparisons for `string`. However, for culture- or case-sensitive control use `StringComparison`.
```csharp
if (s1.Equals(s2, StringComparison.OrdinalIgnoreCase)) { /* case-insensitive */ }
if (s1 == s2) { /* ordinal comparison for strings */ }
```
Use `StringComparison.Ordinal` for performance and deterministic behavior in most program logic; use culture-aware comparisons for user-facing comparisons.

## Null, empty, and whitespace
- Test safely:
```csharp
if (string.IsNullOrEmpty(s)) { /* null or empty */ }
if (string.IsNullOrWhiteSpace(s)) { /* null, empty or only whitespace */ }
```
- Use null-coalescing to provide defaults:
```csharp
string safe = input ?? string.Empty;
string trimmed = (input ?? "").Trim();
```

## Performance considerations
- Avoid repeated `+` concatenation in loops — it allocates many intermediate strings.
- Use `StringBuilder` for heavy or incremental construction:
```csharp
var sb = new System.Text.StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i).Append(", ");
}
string result = sb.ToString();
```
- For slicing or temporary views avoid allocating by using `ReadOnlySpan<char>` / `Span<char>` (advanced):
```csharp
ReadOnlySpan<char> span = s.AsSpan(2, 5); // no allocation; available in modern .NET
```

## Interning and memory notes
- The runtime interns string literals: identical literals may reference the same instance. Dynamically created strings are not interned by default.
- Because strings are immutable, sharing references is safe.

## Formatting and culture
- For numbers/dates in strings use `ToString` with format specifiers and culture where applicable:
```csharp
double pi = Math.PI;
string formatted = $"Pi = {pi:F2}"; // "Pi = 3.14"
string currency = price.ToString("C", CultureInfo.CurrentCulture);
```
- Be explicit about culture for logs and stable formats (`CultureInfo.InvariantCulture`).

## Examples: small utilities
- Split & join:
```csharp
string csv = "apples,oranges,pears";
string[] parts = csv.Split(',');
string back = string.Join(" | ", parts);
```
- Replace and indexing:
```csharp
string phone = "(123) 456-7890";
string digitsOnly = new string(phone.Where(char.IsDigit).ToArray());
```

## Teaching tips
- Show immutability by demonstrating that modifying a string produces a new reference.
- Compare `+` in a loop vs `StringBuilder` and profile memory/allocations to make performance visible.
- Emphasize correct comparisons (case/culture) and null safety (`IsNullOrEmpty` / `IsNullOrWhiteSpace`).

Summary: `string` is a powerful, immutable reference type representing text. Learn its common operations, prefer interpolation for readability, use `StringBuilder` for heavy concatenation, and be explicit about comparison semantics and culture when correctness matters.