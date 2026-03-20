using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        // Implicit typing with var (compiler infers types)
        var a = 10;           // int
        var b = 3.14;         // double
        var text = "hello";   // string

        Console.WriteLine($"a (var) = {a} (type: {a.GetType()})");
        Console.WriteLine($"b (var) = {b} (type: {b.GetType()})");
        Console.WriteLine($"text (var) = {text} (type: {text.GetType()})");

        // Implicit widening conversion (int -> long)
        int small = 100;
        long big = small; // implicit
        Console.WriteLine($"Implicit convert int->long: big = {big} (type: {big.GetType()})");

        // Explicit narrowing cast (double -> int)
        double dbl = 9.99;
        int truncated = (int)dbl; // explicit cast, fractional part lost
        Console.WriteLine($"Explicit cast double->int: from {dbl.ToString(CultureInfo.InvariantCulture)} to {truncated}");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}