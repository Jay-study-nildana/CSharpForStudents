using System;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Palindrome Checker");
        Console.Write("Enter text: ");
        string input = Console.ReadLine() ?? string.Empty;

        // Normalize: keep letters/digits, ignore case
        var sb = new StringBuilder();
        foreach (char ch in input)
        {
            if (char.IsLetterOrDigit(ch))
                sb.Append(char.ToLowerInvariant(ch));
        }

        string normalized = sb.ToString();
        bool isPalindrome = normalized.SequenceEqual(normalized.Reverse());

        Console.WriteLine(isPalindrome ? "Palindrome" : "Not a palindrome");

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}