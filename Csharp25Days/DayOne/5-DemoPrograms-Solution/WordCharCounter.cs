using System;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Word and Character Counter");
        Console.WriteLine("Enter a line (or a paragraph). Press Enter when done:");
        string input = Console.ReadLine() ?? string.Empty;

        int totalChars = input.Length;
        int charsNoSpaces = input.Count(c => !char.IsWhiteSpace(c));
        string[] words = input
            .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        int wordCount = words.Length;

        Console.WriteLine($"Total characters (including spaces): {totalChars}");
        Console.WriteLine($"Total characters (excluding spaces): {charsNoSpaces}");
        Console.WriteLine($"Word count: {wordCount}");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}