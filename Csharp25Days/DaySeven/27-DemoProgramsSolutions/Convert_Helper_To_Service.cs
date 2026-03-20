using System;

/// <summary>
/// Problem: Convert_Helper_To_Service
/// Refactors a static helper into an instance service with interface for DI and testing.
/// </summary>
class Convert_Helper_To_Service
{
    public interface IStringFormatter
    {
        string FormatName(string first, string last);
    }

    // Instance implementation (injectable)
    public class SimpleStringFormatter : IStringFormatter
    {
        public string FormatName(string first, string last) => $"{last}, {first}";
    }

    // Component that depends on IStringFormatter
    public class PersonPrinter
    {
        private readonly IStringFormatter _formatter;
        public PersonPrinter(IStringFormatter formatter) => _formatter = formatter;
        public void Print(string first, string last) =>
            Console.WriteLine(_formatter.FormatName(first, last));
    }

    static void Main()
    {
        // Production wiring
        IStringFormatter formatter = new SimpleStringFormatter();
        var printer = new PersonPrinter(formatter);
        printer.Print("Ada", "Lovelace");

        // Testing would use a fake:
        IStringFormatter fake = new FakeFormatter();
        var testPrinter = new PersonPrinter(fake);
        testPrinter.Print("X", "Y");

        Console.WriteLine("Refactor: static helper -> IStringFormatter allows easy testing and DI.");
    }

    // Fake used to show how tests can provide a deterministic formatter
    class FakeFormatter : IStringFormatter
    {
        public string FormatName(string first, string last) => $"FAKE:{first}-{last}";
    }
}