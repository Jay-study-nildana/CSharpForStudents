using System;

/// <summary>
/// Problem: Refactor_Monolithic_Class
/// Shows a refactored design: IDataFetcher, IReportFormatter, IReportWriter and composition.
/// </summary>
class Refactor_Monolithic_Class
{
    public interface IDataFetcher { string Fetch(); }
    public interface IReportFormatter { string Format(string data); }
    public interface IReportWriter { void Write(string report); }

    public class DataFetcher : IDataFetcher { public string Fetch() => "raw-data"; }
    public class ReportFormatter : IReportFormatter { public string Format(string data) => $"Report: {data}"; }
    public class ConsoleReportWriter : IReportWriter { public void Write(string r) => Console.WriteLine(r); }

    // Composition class coordinates the responsibilities (high cohesion)
    public class ReportGenerator
    {
        private readonly IDataFetcher _fetcher;
        private readonly IReportFormatter _formatter;
        private readonly IReportWriter _writer;

        public ReportGenerator(IDataFetcher f, IReportFormatter fmt, IReportWriter w)
        {
            _fetcher = f; _formatter = fmt; _writer = w;
        }

        public void Generate()
        {
            var data = _fetcher.Fetch();
            var report = _formatter.Format(data);
            _writer.Write(report);
        }
    }

    static void Main()
    {
        var gen = new ReportGenerator(new DataFetcher(), new ReportFormatter(), new ConsoleReportWriter());
        gen.Generate();
        Console.WriteLine("Refactor: monolith split into cohesive components (single responsibility each).");
    }
}