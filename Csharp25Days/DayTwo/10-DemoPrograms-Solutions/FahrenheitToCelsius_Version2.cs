using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.Write("Enter temperature in Fahrenheit: ");
        double f = double.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);

        double c = (f - 32.0) * 5.0 / 9.0;
        Console.WriteLine($"Celsius: {c.ToString("F1", CultureInfo.InvariantCulture)} °C");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}