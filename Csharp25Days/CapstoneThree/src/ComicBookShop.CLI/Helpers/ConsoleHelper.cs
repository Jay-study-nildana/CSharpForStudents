namespace ComicBookShop.CLI.Helpers;

/// <summary>
/// Reusable console I/O helpers for coloured output, menus, prompts, and tables.
/// Demonstrates static methods, method overloading, and control flow (Days 4, 7, 3).
/// </summary>
public static class ConsoleHelper
{
    // ── Decorated output ────────────────────────────────────────────────

    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 60));
        Console.ResetColor();
    }

    public static void PrintSubHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"\n  --- {title} ---");
        Console.ResetColor();
    }

    public static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [OK] {message}");
        Console.ResetColor();
    }

    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  [ERROR] {message}");
        Console.ResetColor();
    }

    public static void PrintWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  [WARN] {message}");
        Console.ResetColor();
    }

    public static void PrintInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"  {message}");
        Console.ResetColor();
    }

    // ── Input helpers ───────────────────────────────────────────────────

    public static int GetMenuChoice(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write($"\n  {prompt} [{min}-{max}]: ");
            if (int.TryParse(Console.ReadLine()?.Trim(), out int choice) && choice >= min && choice <= max)
                return choice;
            PrintError($"Please enter a number between {min} and {max}.");
        }
    }

    public static string GetRequiredInput(string prompt)
    {
        while (true)
        {
            Console.Write($"  {prompt}: ");
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
                return input;
            PrintError("This field is required.");
        }
    }

    public static string GetOptionalInput(string prompt, string currentValue)
    {
        Console.Write($"  {prompt} [{currentValue}]: ");
        var input = Console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(input) ? currentValue : input;
    }

    public static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            Console.Write($"  {prompt}: ");
            if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal value) && value > 0)
                return value;
            PrintError("Please enter a valid positive number.");
        }
    }

    public static int GetIntInput(string prompt, int min = 1, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write($"  {prompt}: ");
            if (int.TryParse(Console.ReadLine()?.Trim(), out int value) && value >= min && value <= max)
                return value;
            PrintError($"Please enter a whole number between {min} and {max}.");
        }
    }

    public static bool Confirm(string prompt)
    {
        Console.Write($"  {prompt} (y/n): ");
        var input = Console.ReadLine()?.Trim().ToLowerInvariant();
        return input is "y" or "yes";
    }

    public static TEnum GetEnumChoice<TEnum>(string prompt) where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();
        Console.WriteLine($"  {prompt}:");
        for (int i = 0; i < values.Length; i++)
            Console.WriteLine($"    {i + 1}. {values[i]}");

        int choice = GetMenuChoice("Select", 1, values.Length);
        return values[choice - 1];
    }

    // ── Table display ───────────────────────────────────────────────────

    public static void PrintTable(string[] headers, List<string[]> rows)
    {
        if (rows.Count == 0)
        {
            PrintInfo("(no data)");
            return;
        }

        // Calculate column widths
        var widths = new int[headers.Length];
        for (int c = 0; c < headers.Length; c++)
            widths[c] = headers[c].Length;

        foreach (var row in rows)
        {
            for (int c = 0; c < Math.Min(row.Length, headers.Length); c++)
                widths[c] = Math.Max(widths[c], (row[c] ?? "").Length);
        }

        // Print header
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("  ");
        for (int c = 0; c < headers.Length; c++)
            Console.Write(headers[c].PadRight(widths[c] + 2));
        Console.WriteLine();

        Console.Write("  ");
        for (int c = 0; c < headers.Length; c++)
            Console.Write(new string('-', widths[c]) + "  ");
        Console.WriteLine();
        Console.ResetColor();

        // Print rows
        foreach (var row in rows)
        {
            Console.Write("  ");
            for (int c = 0; c < headers.Length; c++)
            {
                var val = c < row.Length ? row[c] ?? "" : "";
                Console.Write(val.PadRight(widths[c] + 2));
            }
            Console.WriteLine();
        }
    }

    public static void WaitForKey()
    {
        Console.WriteLine();
        Console.Write("  Press any key to continue...");
        Console.ReadKey(true);
        Console.WriteLine();
    }
}
