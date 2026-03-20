using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Greeting App");
        Console.Write("Enter your name: ");

        // ?? is C#’s null-coalescing operator. It returns the left-hand operand if that is not null; otherwise it returns the right-hand operand
        // In this case, if Console.ReadLine() returns null (which can happen if the input stream is closed), we will use an empty string instead to avoid a NullReferenceException.

        string name = Console.ReadLine() ?? string.Empty;

        Console.WriteLine($"Hello, {name}! Welcome.");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}