using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Even/Odd and Divisibility Checker");
        Console.Write("Enter an integer: ");

        // In that statement out int n declares a new variable n and passes it by reference to int.TryParse. 
        // TryParse will assign n if parsing succeeds. Because of the boolean logic and short‑circuiting, 
        // n is only read on the right side of the || when TryParse returned true (so n was assigned), which makes the whole expression safe and legal.

        if (!int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine("Invalid integer.");
            return;
        }

        Console.WriteLine(n % 2 == 0 ? "Even" : "Odd");

        if (n % 3 == 0)
            Console.WriteLine("Divisible by 3");
        else
            Console.WriteLine("Not divisible by 3");

        if (n % 5 == 0)
            Console.WriteLine("Divisible by 5");
        else
            Console.WriteLine("Not divisible by 5");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}