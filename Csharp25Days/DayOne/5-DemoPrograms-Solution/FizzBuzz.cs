using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("FizzBuzz");
        Console.Write("Enter N (positive integer): ");

        // In that statement out int n declares a new variable n and passes it by reference to int.TryParse. 
        // TryParse will assign n if parsing succeeds. Because of the boolean logic and short‑circuiting, 
        // n is only read on the right side of the || when TryParse returned true (so n was assigned), which makes the whole expression safe and legal.
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
        {
            Console.WriteLine("Invalid N.");
            return;
        }

        for (int i = 1; i <= n; i++)
        {
            if (i % 15 == 0)
                Console.WriteLine("FizzBuzz");
            else if (i % 3 == 0)
                Console.WriteLine("Fizz");
            else if (i % 5 == 0)
                Console.WriteLine("Buzz");
            else
                Console.WriteLine(i);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}