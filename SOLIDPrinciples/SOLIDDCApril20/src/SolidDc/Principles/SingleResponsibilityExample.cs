using SolidDc.Models;
using SolidDc.Services;

namespace SolidDc.Principles
{
    // Single Responsibility Principle (SRP)
    // Example: A naive Character class that prints itself mixes responsibilities.
    // We show a bad version (commented) and then the refactored version
    // where printing is moved to a separate printer class.
    public static class SingleResponsibilityExample
    {
        public static void Run()
        {
            System.Console.WriteLine("--- Single Responsibility Principle (SRP) — DC Example ---\n");

            System.Console.WriteLine("Scenario: Batman's character should only hold data, not handle presentation.");

            // Good design: Character holds data; ConsolePrinter handles presentation.
            var batman = new Character("Batman", 90);
            IPrinter<Character> printer = new SolidDc.Services.ConsolePrinter<Character>();

            System.Console.WriteLine("Refactored (SRP) result:");
            // Print the Character object via the printer abstraction. Character overrides ToString().
            printer.Print(batman);

            System.Console.WriteLine("\nExplanation: The `Character` is responsible for state only.");
            System.Console.WriteLine("Presentation is delegated to `IPrinter<T>` implementations.");
        }
    }
}
