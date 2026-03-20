using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a character: ");
        string? s = Console.ReadLine();
        if (string.IsNullOrEmpty(s))
        {
            Console.WriteLine("No input.");
        }
        else
        {
            char ch = s[0];
            int code = ch;
            char next = (char)(code + 1);
            Console.WriteLine($"Character: '{ch}'");
            Console.WriteLine($"Unicode code point: {code}");
            Console.WriteLine($"Next character: '{next}' (code {(int)next})");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}