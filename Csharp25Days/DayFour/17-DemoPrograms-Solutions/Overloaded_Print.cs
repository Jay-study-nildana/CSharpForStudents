using System;

class Overloaded_Print
{
    // Demonstrates method overloading
    // Time: O(n) where n = times parameter, Space: O(1)

    static void Print(string s)
    {
        Console.WriteLine(s);
    }

    static void Print(string s, int times)
    {
        for (int i = 0; i < times; i++) Console.WriteLine(s);
    }

    static void Print(string s, ConsoleColor color)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(s);
        Console.ForegroundColor = prev;
    }

    static void Main()
    {
        Print("Hello");
        Print("Repeat 3 times", 3);
        Print("In green", ConsoleColor.Green);
    }
}