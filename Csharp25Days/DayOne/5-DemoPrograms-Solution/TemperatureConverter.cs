using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Temperature Converter");
        Console.WriteLine("1) Celsius -> Fahrenheit");
        Console.WriteLine("2) Fahrenheit -> Celsius");
        Console.Write("Choose (1 or 2): ");
        string choice = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Enter temperature: ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double t))
        {
            Console.WriteLine("Invalid temperature.");
            return;
        }

        if (choice == "1")
        {
            double f = t * 9 / 5 + 32;
            Console.WriteLine($"{t}°C = {f:F2}°F");
        }
        else if (choice == "2")
        {
            double c = (t - 32) * 5 / 9;
            Console.WriteLine($"{t}°F = {c:F2}°C");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}