using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a string to parse as integer: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int value))
        {
            Console.WriteLine($"Parsed integer: {value}");
        }
        else
        {
            Console.WriteLine("Input is not a valid integer. Please enter digits only (optional leading sign).");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}