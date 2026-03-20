// 09-ShortenMethod_SplitLoops.cs
// Example refactor: split a long analysis method into clear steps: Transform, Aggregate, Report.
using System;
using System.Collections.Generic;
using System.Linq;

public class DataPoint { public decimal Value; public bool Flag; public string Name; }

public class DataSet { public IEnumerable<DataPoint> Points = Enumerable.Empty<DataPoint>(); }

public class Analyzer
{
    public void Analyze(DataSet ds)
    {
        var items = Transform(ds);
        var total = Aggregate(items);
        Report(total, items);
    }

    private IEnumerable<DataPoint> Transform(DataSet ds)
    {
        // Filter and normalize data
        return ds.Points.Where(p => p.Flag).Select(p => new DataPoint { Name = p.Name, Value = p.Value });
    }

    private decimal Aggregate(IEnumerable<DataPoint> items)
    {
        return items.Sum(p => p.Value);
    }

    private void Report(decimal total, IEnumerable<DataPoint> items)
    {
        foreach (var it in items)
        {
            Console.WriteLine($"{it.Name}: {it.Value:C}");
        }
        Console.WriteLine($"Total: {total:C}");
    }
}