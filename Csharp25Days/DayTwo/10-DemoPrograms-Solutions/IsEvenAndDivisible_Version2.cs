using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter an integer: ");
        if (!int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine("Invalid integer.");
        }
        else
        {
            Console.WriteLine(n % 2 == 0 ? "Even" : "Odd");
            Console.WriteLine(n % 3 == 0 ? "Divisible by 3" : "Not divisible by 3");
            Console.WriteLine(n % 5 == 0 ? "Divisible by 5" : "Not divisible by 5");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}