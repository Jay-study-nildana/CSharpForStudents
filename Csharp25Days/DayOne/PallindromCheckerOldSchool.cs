using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Palindrome Checker (old-school)");
        Console.Write("Enter text: ");
        string input = Console.ReadLine() ?? string.Empty;

        // Normalize: keep letters/digits, ignore case
        var sb = new StringBuilder();
        foreach (char ch in input)
        {
            if (char.IsLetterOrDigit(ch))
                sb.Append(char.ToLowerInvariant(ch));
        }

        // Old-school two-pointer palindrome check (no Reverse, no LINQ)
        bool isPalindrome = true;
        int left = 0;
        int right = sb.Length - 1;

        while (left < right)
        {
            if (sb[left] != sb[right])
            {
                isPalindrome = false;
                break;
            }
            left++;
            right--;
        }

        Console.WriteLine(isPalindrome ? "Palindrome" : "Not a palindrome");
        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}