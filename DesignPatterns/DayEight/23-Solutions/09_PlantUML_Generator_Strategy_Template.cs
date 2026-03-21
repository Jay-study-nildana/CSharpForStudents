// 09_PlantUML_Generator_Strategy_Template.cs
// Generates PlantUML text for Strategy and Template Method participants.

using System;
using System.Text;

namespace Day08.Uml09
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Strategy Pattern PlantUML ===");
            Console.WriteLine(GenerateStrategyUml());
            Console.WriteLine("\n=== Template Method PlantUML ===");
            Console.WriteLine(GenerateTemplateUml());
        }

        static string GenerateStrategyUml()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("title Strategy Pattern");
            sb.AppendLine("interface IPriceStrategy");
            sb.AppendLine("class NoDiscount");
            sb.AppendLine("class PercentageDiscount");
            sb.AppendLine("class PricingService");
            sb.AppendLine("IPriceStrategy <|-- NoDiscount");
            sb.AppendLine("IPriceStrategy <|-- PercentageDiscount");
            sb.AppendLine("PricingService --> IPriceStrategy : uses");
            sb.AppendLine("@enduml");
            return sb.ToString();
        }

        static string GenerateTemplateUml()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("title Template Method");
            sb.AppendLine("abstract class OrderProcessor");
            sb.AppendLine("class DomesticOrderProcessor");
            sb.AppendLine("class InternationalOrderProcessor");
            sb.AppendLine("OrderProcessor <|-- DomesticOrderProcessor");
            sb.AppendLine("OrderProcessor <|-- InternationalOrderProcessor");
            sb.AppendLine("@enduml");
            return sb.ToString();
        }
    }
}