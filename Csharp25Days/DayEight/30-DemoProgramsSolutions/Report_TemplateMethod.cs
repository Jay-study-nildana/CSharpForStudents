using System;

class Report_TemplateMethod
{
    // Template method pattern: base defines algorithm skeleton, subclasses implement steps
    public abstract class ReportGenerator
    {
        // Template
        public void Generate()
        {
            var data = FetchData();
            var content = Format(data);
            Save(content);
        }

        protected abstract string FetchData();
        protected abstract string Format(string data);
        protected virtual void Save(string content) => Console.WriteLine($"Saving report:\n{content}\n---");
    }

    public class SalesReport : ReportGenerator
    {
        protected override string FetchData() => "sales: 100, 200, 150";
        protected override string Format(string data) => $"SalesReport Data: {data}";
    }

    public class InventoryReport : ReportGenerator
    {
        protected override string FetchData() => "inventory: 5 items low";
        protected override string Format(string data) => $"InventoryReport Data: {data}";
    }

    static void Main()
    {
        ReportGenerator r1 = new SalesReport();
        ReportGenerator r2 = new InventoryReport();
        r1.Generate();
        r2.Generate();

        // Polymorphism + template method gives consistent algorithm structure with variant steps.
    }
}