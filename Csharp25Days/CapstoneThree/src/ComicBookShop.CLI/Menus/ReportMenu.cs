using ComicBookShop.CLI.Helpers;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.CLI.Menus;

/// <summary>
/// Console sub-menu for inventory and sales reports.
/// Demonstrates LINQ aggregation, grouping, and projection (Day 13).
/// </summary>
public class ReportMenu
{
    private readonly IComicBookService _comicService;
    private readonly IOrderService _orderService;

    public ReportMenu(IComicBookService comicService, IOrderService orderService)
    {
        _comicService = comicService;
        _orderService = orderService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Reports");
            Console.WriteLine("  1. Inventory Summary");
            Console.WriteLine("  2. Sales Report");
            Console.WriteLine("  3. Top Selling Comics");
            Console.WriteLine("  4. Revenue by Genre");
            Console.WriteLine("  5. Low Stock Alert");
            Console.WriteLine("  6. Stock by Genre");
            Console.WriteLine("  7. Back to Main Menu");

            switch (ConsoleHelper.GetMenuChoice("Select report", 1, 7))
            {
                case 1: await InventorySummaryAsync(); break;
                case 2: await SalesReportAsync(); break;
                case 3: await TopSellingAsync(); break;
                case 4: await RevenueByGenreAsync(); break;
                case 5: await LowStockAsync(); break;
                case 6: await StockByGenreAsync(); break;
                case 7: return;
            }
        }
    }

    private async Task InventorySummaryAsync()
    {
        ConsoleHelper.PrintSubHeader("Inventory Summary");

        var summaries = await _comicService.GetSummariesAsync();
        var totalValue = await _comicService.GetTotalInventoryValueAsync();

        var headers = new[] { "#", "Title", "Author", "Genre", "Price", "Stock", "Condition" };
        var rows = summaries.Select((s, i) => new[]
        {
            (i + 1).ToString(), s.Title, s.Author, s.Genre.ToString(),
            $"${s.Price:F2}", s.StockQuantity.ToString(), s.Condition.ToString()
        }).ToList();

        ConsoleHelper.PrintTable(headers, rows);
        Console.WriteLine($"\n  Total titles : {summaries.Count}");
        Console.WriteLine($"  Total value  : ${totalValue:F2}");
        ConsoleHelper.WaitForKey();
    }

    private async Task SalesReportAsync()
    {
        ConsoleHelper.PrintSubHeader("Sales Report");

        var totalRevenue = await _orderService.GetTotalRevenueAsync();
        var orders = await _orderService.GetAllAsync();
        int totalOrders = orders.Count;
        int completedOrders = orders.Count(o => o.Status == OrderStatus.Delivered);
        int cancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled);

        Console.WriteLine($"  Total orders      : {totalOrders}");
        Console.WriteLine($"  Delivered         : {completedOrders}");
        Console.WriteLine($"  Cancelled         : {cancelledOrders}");
        Console.WriteLine($"  Total revenue     : ${totalRevenue:F2}");
        if (totalOrders > 0)
            Console.WriteLine($"  Avg. order value  : ${totalRevenue / totalOrders:F2}");
        ConsoleHelper.WaitForKey();
    }

    private async Task TopSellingAsync()
    {
        int count = ConsoleHelper.GetIntInput("How many top comics?", 1, 50);
        var top = await _orderService.GetTopSellingComicsAsync(count);

        if (top.Count == 0) { ConsoleHelper.PrintInfo("No sales data yet."); ConsoleHelper.WaitForKey(); return; }

        ConsoleHelper.PrintSubHeader($"Top {count} Selling Comics");
        var headers = new[] { "#", "Title", "Total Sold" };
        var rows = top.Select((t, i) => new[]
        {
            (i + 1).ToString(), t.Title, t.TotalSold.ToString()
        }).ToList();

        ConsoleHelper.PrintTable(headers, rows);
        ConsoleHelper.WaitForKey();
    }

    private async Task RevenueByGenreAsync()
    {
        var data = await _orderService.GetRevenueByGenreAsync();

        if (data.Count == 0) { ConsoleHelper.PrintInfo("No revenue data yet."); ConsoleHelper.WaitForKey(); return; }

        ConsoleHelper.PrintSubHeader("Revenue by Genre");
        var headers = new[] { "Genre", "Revenue" };
        var rows = data.OrderByDescending(kv => kv.Value)
            .Select(kv => new[] { kv.Key.ToString(), $"${kv.Value:F2}" })
            .ToList();

        ConsoleHelper.PrintTable(headers, rows);
        ConsoleHelper.WaitForKey();
    }

    private async Task LowStockAsync()
    {
        int threshold = ConsoleHelper.GetIntInput("Low-stock threshold", 1, 100);
        var lowStock = await _comicService.GetLowStockAsync(threshold);

        ConsoleHelper.PrintSubHeader($"Comics with stock <= {threshold}");

        if (lowStock.Count == 0)
        {
            ConsoleHelper.PrintSuccess("All comics are well-stocked!");
        }
        else
        {
            var headers = new[] { "#", "Title", "Stock", "Price" };
            var rows = lowStock.Select((c, i) => new[]
            {
                (i + 1).ToString(), $"{c.Title} #{c.IssueNumber}",
                c.StockQuantity.ToString(), $"${c.Price:F2}"
            }).ToList();
            ConsoleHelper.PrintTable(headers, rows);
        }

        ConsoleHelper.WaitForKey();
    }

    private async Task StockByGenreAsync()
    {
        var data = await _comicService.GetStockByGenreAsync();

        ConsoleHelper.PrintSubHeader("Stock by Genre");
        var headers = new[] { "Genre", "Total Stock" };
        var rows = data.OrderByDescending(kv => kv.Value)
            .Select(kv => new[] { kv.Key.ToString(), kv.Value.ToString() })
            .ToList();

        ConsoleHelper.PrintTable(headers, rows);
        ConsoleHelper.WaitForKey();
    }
}
