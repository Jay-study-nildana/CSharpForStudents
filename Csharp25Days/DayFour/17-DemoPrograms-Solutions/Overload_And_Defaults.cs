using System;

class Overload_And_Defaults
{
    // Demonstrates default parameters with overloads
    // Best practice: avoid ambiguous overloads; keep signatures distinct
    // Time: O(1), Space: O(1)

    static void PrintReport(string title, bool detailed = false)
    {
        Console.WriteLine($"Report: {title} (detailed={detailed})");
    }

    static void PrintReport(string title, int copies)
    {
        Console.WriteLine($"Report: {title} (copies={copies})");
    }

    static void Main()
    {
        // Calls PrintReport(string, bool) using default
        PrintReport("Monthly");         // resolves to (string, bool)
        PrintReport("Monthly", true);   // (string, bool)
        PrintReport("Monthly", 3);      // (string, int)
        // Avoid overloading where default values cause ambiguity
    }
}