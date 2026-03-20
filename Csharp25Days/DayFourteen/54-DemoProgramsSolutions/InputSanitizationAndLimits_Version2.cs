// InputSanitizationAndLimits.cs
// Problem: InputSanitizationAndLimits
// Implement SanitizeAndValidate protecting against huge inputs and normalizing whitespace.

using System;
using System.Text.RegularExpressions;

class InputSanitizationAndLimits
{
    public static string SanitizeAndValidate(string? input, int maxLength)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Length > maxLength * 10) // early guard to avoid heavy processing on extremely large inputs
            throw new ArgumentException("Input too large", nameof(input));
        // Normalize whitespace: trim and collapse internal whitespace
        var trimmed = input.Trim();
        var normalized = Regex.Replace(trimmed, @"\s+", " ");
        if (normalized.Length > maxLength) throw new ArgumentException($"Input exceeds max length of {maxLength}");
        return normalized;
    }

    static void Main()
    {
        var good = "  hello   world   ";
        Console.WriteLine($"Sanitized: '{SanitizeAndValidate(good, 50)}'");

        var huge = new string('x', 1000000);
        try
        {
            SanitizeAndValidate(huge, 1000);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Rejected large input: {ex.Message}");
        }
    }
}