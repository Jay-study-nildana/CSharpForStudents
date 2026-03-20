using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter first integer (a): ");
        int a = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter second integer (b): ");
        int b = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine($"Before swap: a={a}, b={b}");

        // Swap without temporary using tuple (modern) or arithmetic
        // Using tuple:
        (a, b) = (b, a);

        Console.WriteLine($"After swap: a={a}, b={b}");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}