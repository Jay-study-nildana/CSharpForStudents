# Globalization and Localization in .NET (for students)

This note explains the fundamentals of globalization and localization (a.k.a. region-aware coding), focusing on numeric and date/time formatting and parsing in .NET/C#. It includes practical examples showing CultureInfo, NumberStyles, NumberFormatInfo, and recommended best practices you can use in apps and demonstrate in class.

## Key concepts (short)
- Globalization: designing software so it can work with many cultures (language, numeric formats, dates, sorting, etc.).
- Localization: adapting the product to a specific culture (translations, formats, resources).
- CultureInfo: represents culture-specific information (decimal separator, date formats, currency symbol).
- NumberStyles: controls which syntactic numeric constructs are allowed when parsing.
- InvariantCulture: culture-independent format (useful for machine-readable data).
- CurrentCulture: the culture used for user-facing operations by default (comes from user settings / OS).

---

## Why this matters
Users in different locales expect different formats:
- 1,234.56 (en-US / invariant) vs 1.234,56 (de-DE).
- Date "03/04/2026" may mean March 4 in en-US but 3 April in en-GB.
If your parsing or formatting ignores CultureInfo and NumberStyles, you can accept wrong user input or break machine-readable formats.

---

## Parsing numbers: the important overload

C# provides:
```csharp
double.TryParse(string s, NumberStyles style, IFormatProvider provider, out double result)
```

- NumberStyles: which parts are permitted (decimal point, thousands separator, currency symbol, exponent, parentheses, leading/trailing whitespace, etc.).
- provider (CultureInfo or NumberFormatInfo): the culture-specific symbols (decimal separator, group separator, currency symbol, negative sign, NaN/Infinity text).

Example: parsing with invariant culture vs German culture
```csharp
using System;
using System.Globalization;

string s1 = "1,234.56";   // invariant / en-US style
string s2 = "1.234,56";   // German (de-DE) style

if (double.TryParse(s1, NumberStyles.Any, CultureInfo.InvariantCulture, out var v1))
    Console.WriteLine($"Invariant parsed {s1} -> {v1}"); // True -> 1234.56

if (double.TryParse(s2, NumberStyles.Any, CultureInfo.GetCultureInfo("de-DE"), out var v2))
    Console.WriteLine($"de-DE parsed {s2} -> {v2}"); // True -> 1234.56
```

Note: `NumberStyles.Any` is permissive (allows currency symbols, parentheses, etc.). Use a narrower style when appropriate.

Recommended NumberStyles for common cases:
- User input (allow grouping, sign, decimal, exponent): `NumberStyles.Float | NumberStyles.AllowThousands` or `NumberStyles.Number`.
- Machine-readable or config files: use a specific style — avoid `NumberStyles.Any`.

---

## Formatting numbers: ToString and culture

When formatting numbers, pass a CultureInfo to `ToString` or format them explicitly:

```csharp
double amount = 1234.56;
Console.WriteLine(amount.ToString("N", CultureInfo.InvariantCulture)); // "1,234.56"
Console.WriteLine(amount.ToString("N", CultureInfo.GetCultureInfo("de-DE"))); // "1.234,56"
Console.WriteLine(amount.ToString("C", CultureInfo.GetCultureInfo("en-US"))); // "$1,234.56"
```

Format specifiers:
- "N" = number with group separators.
- "F" = fixed-point.
- "C" = currency (uses culture currency symbol).
- "G" = general.
- Provide explicit culture for consistent behavior (especially for logs, storage, or protocols).

---

## Example: small demo program showing multiple cultures

```csharp
using System;
using System.Globalization;

class CultureDemo
{
    static void Main()
    {
        double value = 1234.56;
        var cultures = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE"), CultureInfo.GetCultureInfo("fr-FR") };

        foreach (var ci in cultures)
        {
            Console.WriteLine($"{ci.Name} -> Number: {value.ToString("N", ci)}, Currency: {value.ToString("C", ci)}, DecimalSep: {ci.NumberFormat.NumberDecimalSeparator}");
        }

        // Parsing demonstration
        string input = "1.234,56";
        if (double.TryParse(input, NumberStyles.Number, CultureInfo.GetCultureInfo("de-DE"), out var parsed))
            Console.WriteLine($"Parsed '{input}' in de-DE as {parsed}");
        else
            Console.WriteLine($"Failed to parse '{input}' with de-DE");
    }
}
```

---

## Customizing numeric formats: NumberFormatInfo

You can create or clone a NumberFormatInfo if you need non-standard rules:

```csharp
var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
nfi.NumberDecimalSeparator = ",";
nfi.NumberGroupSeparator = ".";
double.TryParse("1.234,56", NumberStyles.Number, nfi, out var x); // True -> 1234.56
```

This is useful for parsing legacy formats or fixed external formats that do not map to a single built-in culture.

---

## Dates and times: Culture-aware parsing/formatting

Dates also depend on culture and formats:

```csharp
DateTime dt;
string d1 = "03/04/2026"; // ambiguous
// Prefer explicit formats when parsing strings from files:
if (DateTime.TryParseExact(d1, "dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out dt))
    Console.WriteLine($"Parsed as {dt:O}"); // ISO round-trip for unambiguous logging
```

Use `TryParseExact` when dates are produced by machines or logs. Use `TryParse` with `CurrentCulture` for user-entered dates, if you want to accept the user's normal format.

---

## Currency and monetary best practice
- Use `decimal` (System.Decimal) for money to avoid binary floating-point rounding.
- Keep currency formatting local (use user culture for display).
- For storage/transmission, use a canonical format (e.g., decimal value and an ISO currency code like "USD" or "EUR"), or use invariant culture with predictable formatting.

Example:
```csharp
decimal price = 19.95m;
Console.WriteLine(price.ToString("C", CultureInfo.GetCultureInfo("en-US"))); // "$19.95"
```

---

## Choosing between CurrentCulture and InvariantCulture
- Use `CultureInfo.CurrentCulture` (or omit provider) for user-facing text: respects user preferences.
- Use `CultureInfo.InvariantCulture` for machine-facing text (files, logs, protocols, IDs).
- Be explicit: don’t rely on default behavior if your data is shared across cultures or persisted.

---

## Globalization vs Localization — quick teaching points
- Globalization = take culture differences into account and design your app so it can be localized easily.
- Localization = adapt UI, messages, images, formats, translations to a specific culture.
- Use resource files (`.resx`) or localization frameworks to provide translated strings.
- Keep culture-specific logic out of core business logic where possible — accept and produce culture-agnostic representations for storage/communication.

---

## Common pitfalls and tips
- Pitfall: parsing user input using `InvariantCulture` in a localized app → rejects valid input for the user.
- Pitfall: storing user-typed numbers as strings using CurrentCulture → not stable across servers with different culture settings.
- Tip: For logs and storage use `ToString(CultureInfo.InvariantCulture)` and parse with `InvariantCulture`.
- Tip: For user input use `TryParse` with `CurrentCulture` or the explicit user-chosen culture and a safe NumberStyles (e.g., `Float | AllowThousands`).
- Tip: Avoid NumberStyles.Any for untrusted input — it accepts currency symbols, parentheses, etc., which you might not want.

---

## Short classroom exercise
1. Write a small program that tries to parse the same numeric strings under `en-US`, `de-DE`, and `fr-FR`. Print results.
2. Modify a `NumberFormatInfo` instance so that `,` is decimal separator and parse the same strings.
3. Format a `decimal` money value into currency for different cultures, and then format for invariant culture for file output.

---

## Closing summary
Globalization and localization are about understanding and handling cultural differences (numbers, dates, text). Use CultureInfo and NumberStyles deliberately:
- CurrentCulture for user interactions,
- InvariantCulture for machine interactions,
- Narrow NumberStyles for safe parsing,
- decimal for money,
- resource-based localization for strings.

If you teach these rules and show the code examples above, students will get both the conceptual idea and practical patterns they can use immediately.
