// ReportBuilderDemo.cs
// Console demo of the Builder pattern for creating reports.

using System;
using System.Collections.Generic;

// Top-level statements
var reports = new List<Report>
{
    new ReportBuilder()
        .WithTitle("SOLID Principles")
        .WithAuthor("Alice")
        .AddSection("Single Responsibility Principle")
        .AddSection("Open/Closed Principle")
        .AddSection("Liskov Substitution Principle")
        .Build(),
    new ReportBuilder()
        .WithTitle("Design Patterns")
        .WithAuthor("Bob")
        .AddSection("Factory Pattern")
        .AddSection("Strategy Pattern")
        .Build(),
    new ReportBuilder()
        .WithTitle(".NET Best Practices")
        .WithAuthor("Carol")
        .AddSection("Dependency Injection")
        .AddSection("Logging")
        .AddSection("Configuration")
        .Build()
};

foreach (var report in reports)
{
    Console.WriteLine($"Report: {report.Title}\nAuthor: {report.Author}");
    foreach (var section in report.Sections)
        Console.WriteLine($"  - {section}");
    Console.WriteLine();
}

public class Report
{
    public string Title { get; }
    public string Author { get; }
    public IReadOnlyList<string> Sections { get; }

    internal Report(string title, string author, List<string> sections)
    {
        Title = title; Author = author; Sections = sections.AsReadOnly();
    }
}

public class ReportBuilder
{
    private string _title = "Untitled";
    private string _author = "Unknown";
    private readonly List<string> _sections = new();

    public ReportBuilder WithTitle(string title) { _title = title; return this; }
    public ReportBuilder WithAuthor(string author) { _author = author; return this; }
    public ReportBuilder AddSection(string section) { _sections.Add(section); return this; }

    public Report Build() => new Report(_title, _author, new List<string>(_sections));
}


