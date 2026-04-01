using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;

// --- Program (top-level statements) ---
Console.WriteLine("Report formatter factory demo");

// Sample data
var sampleReport = new List<ReportEntry>
{
    new ReportEntry { Id = 1, Name = "Alice", Value = 123.45m },
    new ReportEntry { Id = 2, Name = "Bob, Jr.", Value = 67.89m }, // contains comma to show CSV escaping
    new ReportEntry { Id = 3, Name = "Carol \"The Great\"", Value = 1000m } // contains quotes to show CSV escaping
};

// Show both formats programmatically
var csvFormatter = ReportFormatterFactory.Create("csv");
Console.WriteLine("CSV output:");
Console.WriteLine(csvFormatter.Format(sampleReport));

var jsonFormatter = ReportFormatterFactory.Create("json");
Console.WriteLine("JSON output:");
Console.WriteLine(jsonFormatter.Format(sampleReport));

// Interactive example: let the user pick format
Console.WriteLine();
Console.Write("Enter format (csv/json) or leave empty for json: ");
var kind = Console.ReadLine();
var formatter = ReportFormatterFactory.Create(kind);
Console.WriteLine();
Console.WriteLine($"Formatted ({kind ?? "json"}):");
Console.WriteLine(formatter.Format(sampleReport));

Console.WriteLine();
Console.WriteLine("Demo finished. Press Enter to exit.");
Console.ReadLine();

// Report model
public class ReportEntry
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public decimal Value { get; init; }
}

// Formatter contract
public interface IReportFormatter
{
    string Format(IEnumerable<ReportEntry> entries);
}

// CSV formatter
public class CsvFormatter : IReportFormatter
{
    public string Format(IEnumerable<ReportEntry> entries)
    {
        var sb = new StringBuilder();
        // Header
        sb.AppendLine("Id,Name,Value");
        foreach (var e in entries)
        {
            sb.AppendLine($"{Escape(e.Id.ToString())},{Escape(e.Name)},{Escape(e.Value.ToString())}");
        }
        return sb.ToString();
    }

    // Basic CSV escaping for commas and quotes
    private static string Escape(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
        {
            var escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        return field;
    }
}

// JSON formatter
public class JsonFormatter : IReportFormatter
{
    public string Format(IEnumerable<ReportEntry> entries)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(entries, options);
    }
}

// Factory / Provider (matches your snippet)
public static class ReportFormatterFactory
{
    public static IReportFormatter Create(string kind) =>
        kind?.ToLowerInvariant() == "csv" ? new CsvFormatter() : new JsonFormatter();
}

