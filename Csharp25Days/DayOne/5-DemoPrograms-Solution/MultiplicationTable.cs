using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Multiplication Table");
        Console.Write("Enter a number (or leave blank for full 10x10 table): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            // 10x10 table
            for (int r = 1; r <= 10; r++)
            {
                for (int c = 1; c <= 10; c++)
                {
                    Console.Write($"{r * c,4}");
                }
                Console.WriteLine();
            }
        }
        else if (int.TryParse(input.Trim(), out int n))
        {
            for (int i = 1; i <= 12; i++)
            {
                Console.WriteLine($"{n} x {i} = {n * i}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}