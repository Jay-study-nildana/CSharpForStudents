// 08-MakeMethodsTestable_ConsoleOutput.cs
// Separates data retrieval and calculation from rendering by introducing IReportRenderer.
using System;
using System.Collections.Generic;
using System.Linq;

public class User { public int Id; }
public class Row { public decimal Amount; public string Description; }
public interface ITransactionRepository { IEnumerable<Row> Get(int userId, DateTime start, DateTime end); }
public interface IReportRenderer { void RenderDetail(Row row); void RenderSummary(decimal total); }

public class ReportResult
{
    public IEnumerable<Row> Rows { get; init; } = Enumerable.Empty<Row>();
    public decimal Total { get; init; }
}

public class ReportService
{
    private readonly ITransactionRepository _repo;
    public ReportService(ITransactionRepository repo) => _repo = repo;

    public ReportResult RunReport(User u, DateTime start, DateTime end)
    {
        var rows = _repo.Get(u.Id, start, end).ToList();
        var total = rows.Sum(r => r.Amount);
        return new ReportResult { Rows = rows, Total = total };
    }
}

public class ConsoleReportRenderer : IReportRenderer
{
    public void RenderDetail(Row row) => Console.WriteLine($"{row.Description} {row.Amount:C}");
    public void RenderSummary(decimal total) => Console.WriteLine($"Total: {total:C}");
}

// Usage:
// var result = reportService.RunReport(user, start, end);
// if (detailed) renderer.RenderDetail(...) etc.
// Tests can assert result.Total without needing console output.