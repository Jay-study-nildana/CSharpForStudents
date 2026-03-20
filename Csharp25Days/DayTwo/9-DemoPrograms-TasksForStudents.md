# Day 2 — Small Practice Problems (10)

Below are 10 short problems focused on primitive types, variables, expressions, type conversion, and basic operators. Each problem is intended to be solved as a small console program. Filenames for solutions follow each problem title (e.g., `SumOfTwoNumbers.cs`).

1. SumOfTwoNumbers
- Description: Read two integers from the user and print their sum and the resulting type.
- Filename: SumOfTwoNumbers.cs

2. SwapValuesWithoutTemp
- Description: Read two integers, swap their values without using a temporary variable, and print the swapped values.
- Filename: SwapValuesWithoutTemp.cs

3. AverageWithFormatting
- Description: Read three floating-point numbers (double), compute their average, and print the average formatted to 2 decimal places.
- Filename: AverageWithFormatting.cs

4. FahrenheitToCelsius
- Description: Read a temperature in Fahrenheit (double), convert it to Celsius using the formula C = (F - 32) * 5/9, and print the result with one decimal place.
- Filename: FahrenheitToCelsius.cs

5. IsEvenAndDivisible
- Description: Read an integer and print whether it is even or odd, and whether it is divisible by 3 and by 5.
- Filename: IsEvenAndDivisible.cs

6. CharToAscii
- Description: Read a single character from input, print its Unicode code point (integer), and print the next character in Unicode.
- Filename: CharToAscii.cs

7. StringToIntSafeParse
- Description: Read a string, attempt to parse it to an integer using `int.TryParse(...)` (with `out`), and print either the parsed value or an informative error message.
- Filename: StringToIntSafeParse.cs

8. ImplicitExplicitTypes
- Description: Demonstrate implicit (`var`) and explicit typing: declare variables with `var` and explicit types, show an implicit widening conversion (int → long) and an explicit narrowing cast (double → int), and print results.
- Filename: ImplicitExplicitTypes.cs

9. ConstantsAndCircumference
- Description: Declare a `const double PI` and read a radius (double). Compute and print the circumference (2 * PI * r) and area (PI * r * r), formatted with 3 decimal places.
- Filename: ConstantsAndCircumference.cs

10. SumNumbersFromString
- Description: Read a line containing space-separated tokens, parse the tokens that are valid numbers (integers or doubles), sum them, and print the sum. Ignore invalid tokens but print a count of ignored tokens.
- Filename: SumNumbersFromString.cs

---
Usage notes
- These problems focus on user input, parsing, type conversion, basic operators, and formatting.
- Encourage students to consider edge cases (empty input, invalid tokens, division by zero where applicable).
- For parsing and formatting, prefer `double.TryParse` / `int.TryParse` and `CultureInfo.InvariantCulture` for stable behavior.