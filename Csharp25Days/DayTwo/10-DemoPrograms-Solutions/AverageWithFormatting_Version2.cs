using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.Write("Enter first number: ");
        double x = double.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);
        Console.Write("Enter second number: ");
        double y = double.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);
        Console.Write("Enter third number: ");
        double z = double.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);

        double avg = (x + y + z) / 3.0;
        Console.WriteLine($"Average: {avg.ToString("F2", CultureInfo.InvariantCulture)}");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}