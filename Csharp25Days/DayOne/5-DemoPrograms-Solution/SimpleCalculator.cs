using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Simple Calculator");
        Console.Write("Enter first number: ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double a))
        {
            Console.WriteLine("Invalid number. Exiting.");
            return;
        }

        Console.Write("Enter second number: ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double b))
        {
            Console.WriteLine("Invalid number. Exiting.");
            return;
        }

        Console.Write("Enter operation (+, -, *, /): ");
        string op = Console.ReadLine()?.Trim() ?? "";

        double result;
        bool ok = true;
        switch (op)
        {
            case "+":
                result = a + b;
                break;
            case "-":
                result = a - b;
                break;
            case "*":
                result = a * b;
                break;
            case "/":
                if (b == 0)
                {
                    Console.WriteLine("Cannot divide by zero.");
                    ok = false;
                    result = 0;
                }
                else
                {
                    result = a / b;
                }
                break;
            default:
                Console.WriteLine("Unknown operation.");
                ok = false;
                result = 0;
                break;
        }

        if (ok)
            Console.WriteLine($"Result: {result}");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}