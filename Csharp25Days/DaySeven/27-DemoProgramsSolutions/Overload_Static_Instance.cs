using System;

/// <summary>
/// Problem: Overload_Static_Instance
/// Demonstrates overloads as static helpers vs instance methods with state-dependent overload behavior.
/// </summary>
class Overload_Static_Instance
{
    public static class FormatterStatic
    {
        public static string Format(string s) => s.Trim();
        public static string Format(string s, int maxLen) => s.Trim().Substring(0, Math.Min(s.Trim().Length, maxLen));
    }

    public class FormatterInstance
    {
        private readonly string _prefix;
        public FormatterInstance(string prefix) { _prefix = prefix; }
        public string Format(string s) => _prefix + s.Trim();
        public string Format(string s, bool upper) => _prefix + (upper ? s.Trim().ToUpperInvariant() : s.Trim());
    }

    static void Main()
    {
        Console.WriteLine(FormatterStatic.Format(" hello "));
        Console.WriteLine(FormatterStatic.Format(" hello world ", 5));

        var fmt = new FormatterInstance("[P] ");
        Console.WriteLine(fmt.Format(" hi "));
        Console.WriteLine(fmt.Format(" hi ", true));
        Console.WriteLine("Note: instance overloads can use instance state (_prefix) to alter behavior.");
    }
}