using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.Write("Enter first integer: ");
        int a = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter second integer: ");
        int b = int.Parse(Console.ReadLine() ?? "0");

        int sum = a + b;
        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Type of result: {sum.GetType()}");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}