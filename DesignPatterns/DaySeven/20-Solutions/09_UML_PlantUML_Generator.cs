// 09_UML_PlantUML_Generator.cs
// Utility to print PlantUML descriptions for Composite and Bridge patterns used in the exercises.

using System;
using System.Text;

namespace Day07.UmlGenerator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== PlantUML for Composite ===\n");
            Console.WriteLine(GenerateCompositePlantUml());

            Console.WriteLine("\n=== PlantUML for Bridge ===\n");
            Console.WriteLine(GenerateBridgePlantUml());
        }

        static string GenerateCompositePlantUml()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("title Composite Pattern - Menu Example");
            sb.AppendLine("interface MenuComponent");
            sb.AppendLine("class MenuItem");
            sb.AppendLine("class MenuGroup");
            sb.AppendLine("MenuGroup --|> MenuComponent");
            sb.AppendLine("MenuItem --|> MenuComponent");
            sb.AppendLine("MenuGroup o-- \"*\" MenuComponent : children");
            sb.AppendLine("@enduml");
            return sb.ToString();
        }

        static string GenerateBridgePlantUml()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("title Bridge Pattern - Renderer Example");
            sb.AppendLine("interface IRenderer");
            sb.AppendLine("class ConsoleRenderer");
            sb.AppendLine("class HtmlRenderer");
            sb.AppendLine("abstract class Widget");
            sb.AppendLine("class Button");
            sb.AppendLine("Widget --> IRenderer : uses");
            sb.AppendLine("Button --|> Widget");
            sb.AppendLine("ConsoleRenderer --|> IRenderer");
            sb.AppendLine("HtmlRenderer --|> IRenderer");
            sb.AppendLine("@enduml");
            return sb.ToString();
        }
    }
}