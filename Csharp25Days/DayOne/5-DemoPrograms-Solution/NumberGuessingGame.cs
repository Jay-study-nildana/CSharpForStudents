using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Number Guessing Game");
        var rnd = new Random();
        int secret = rnd.Next(1, 101); // 1..100
        int attempts = 0;
        Console.WriteLine("I have picked a number between 1 and 100. Try to guess it!");

        while (true)
        {
            Console.Write("Your guess: ");
            string? line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Please enter a number.");
                continue;
            }

            if (!int.TryParse(line.Trim(), out int guess))
            {
                Console.WriteLine("Invalid number.");
                continue;
            }

            attempts++;

            if (guess == secret)
            {
                Console.WriteLine($"Correct! You took {attempts} attempts.");
                break;
            }

            if (guess < secret)
                Console.WriteLine("Higher.");
            else
                Console.WriteLine("Lower.");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}