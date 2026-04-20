using System;

namespace SolidDc.Helpers
{
    // Small console helper to keep Program.cs tidy and to centralize
    // I/O behavior and formatting for students.
    public static class ConsoleHelper
    {
        public static void WriteBanner()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine(" DC Comics SOLID Principles — Interactive");
            Console.WriteLine("========================================\n");
        }

        public static void WriteLine(string text = "") => Console.WriteLine(text);

        public static string Prompt(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public static void PressEnterToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
            WriteBanner();
        }
    }
}
