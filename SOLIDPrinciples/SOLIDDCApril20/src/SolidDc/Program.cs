using SolidDc.Helpers;
using SolidDc.Principles;

// DC Comics SOLID Tutor - interactive console app
// This file wires up a small menu so students can choose which
// SOLID principle to explore. Each principle file contains
// explanations and small runnable examples.

ConsoleHelper.WriteBanner();

while (true)
{
    ConsoleHelper.WriteLine("Select a SOLID principle to explore (DC Comics theme):");
    ConsoleHelper.WriteLine("1) Single Responsibility Principle (SRP)");
    ConsoleHelper.WriteLine("2) Open/Closed Principle (OCP)");
    ConsoleHelper.WriteLine("3) Liskov Substitution Principle (LSP)");
    ConsoleHelper.WriteLine("4) Interface Segregation Principle (ISP)");
    ConsoleHelper.WriteLine("5) Dependency Inversion Principle (DIP)");
    ConsoleHelper.WriteLine("0) Exit");

    var choice = ConsoleHelper.Prompt("Enter choice");

    switch (choice)
    {
        case "1": SingleResponsibilityExample.Run(); break;
        case "2": OpenClosedExample.Run(); break;
        case "3": LiskovSubstitutionExample.Run(); break;
        case "4": InterfaceSegregationExample.Run(); break;
        case "5": DependencyInversionExample.Run(); break;
        case "0": ConsoleHelper.WriteLine("Goodbye — keep learning!"); return;
        default: ConsoleHelper.WriteLine("Unknown choice, try again."); break;
    }

    ConsoleHelper.PressEnterToContinue();
}
