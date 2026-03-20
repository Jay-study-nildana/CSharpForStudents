// 10-DetectAndFixCodeSmells_ReviewSnippet.cs
// Original had these smells: poor naming, primitive obsession (no Money type), mixing calculation and output, no null checks.
// Refactored: clear names, ExtractCalculateTotal, use decimal for money, separate rendering.
using System;
using System.Collections.Generic;
using System.Linq;

public class Order
{
    public List<OrderLine> Lines = new();
    public decimal Total;
}

public class OrderLine { public int Quantity; public decimal UnitPrice; }

public class OrderProcessor
{
    public void DoWork(Order order, IOrderRenderer renderer)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        var total = CalculateTotal(order.Lines);
        order.Total = total;
        renderer.RenderTotal(order.Total);
    }

    private static decimal CalculateTotal(IEnumerable<OrderLine> lines)
    {
        return lines?.Where(l => l.Quantity > 0).Sum(l => l.Quantity * l.UnitPrice) ?? 0m;
    }
}

public interface IOrderRenderer { void RenderTotal(decimal total); }

public class ConsoleOrderRenderer : IOrderRenderer
{
    public void RenderTotal(decimal total) => Console.WriteLine($"Total: {total:C}");
}