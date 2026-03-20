namespace DCSuperHeroes.Cli.Support;

public static class InputHelpers
{
    public static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid integer.");
        }
    }

    public static bool ReadBoolean(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (input is "y" or "yes")
            {
                return true;
            }

            if (input is "n" or "no")
            {
                return false;
            }

            Console.WriteLine("Enter y or n.");
        }
    }

    public static Guid ReadGuid(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (Guid.TryParse(Console.ReadLine(), out var value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid GUID.");
        }
    }

    public static DateTime ReadDateTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (DateTime.TryParse(Console.ReadLine(), out var value))
            {
                return DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }

            Console.WriteLine("Enter a valid UTC date and time.");
        }
    }

    public static TEnum ReadEnum<TEnum>(string label) where TEnum : struct, Enum
    {
        while (true)
        {
            Console.WriteLine($"{label}: {string.Join(", ", Enum.GetNames<TEnum>())}");
            Console.Write($"> ");
            if (Enum.TryParse<TEnum>(Console.ReadLine(), true, out var value))
            {
                return value;
            }

            Console.WriteLine("Enter one of the listed values.");
        }
    }
}