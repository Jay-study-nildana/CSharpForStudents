using ComicBookShop.CLI.Helpers;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.CLI.Menus;

/// <summary>Console sub-menu for placing and managing orders.</summary>
public class OrderMenu
{
    private readonly IOrderService _orderService;
    private readonly IComicBookService _comicService;
    private readonly ICustomerService _customerService;

    public OrderMenu(
        IOrderService orderService,
        IComicBookService comicService,
        ICustomerService customerService)
    {
        _orderService = orderService;
        _comicService = comicService;
        _customerService = customerService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Orders");
            Console.WriteLine("  1. Place New Order");
            Console.WriteLine("  2. View All Orders");
            Console.WriteLine("  3. View Order Details");
            Console.WriteLine("  4. Update Order Status");
            Console.WriteLine("  5. View Orders by Customer");
            Console.WriteLine("  6. Back to Main Menu");

            switch (ConsoleHelper.GetMenuChoice("Select option", 1, 6))
            {
                case 1: await PlaceOrderAsync(); break;
                case 2: await ViewAllAsync(); break;
                case 3: await ViewDetailsAsync(); break;
                case 4: await UpdateStatusAsync(); break;
                case 5: await ViewByCustomerAsync(); break;
                case 6: return;
            }
        }
    }

    // ── Place order (multi-step workflow) ────────────────────────────────

    private async Task PlaceOrderAsync()
    {
        // Select customer
        var customers = await _customerService.GetAllAsync();
        if (customers.Count == 0) { ConsoleHelper.PrintInfo("No customers — add one first."); return; }

        ConsoleHelper.PrintSubHeader("Select Customer");
        DisplayCustomerBrief(customers);
        int custIdx = ConsoleHelper.GetMenuChoice("Customer #", 1, customers.Count) - 1;
        var customer = customers[custIdx];

        // Select comics
        var comics = await _comicService.GetAllAsync();
        if (comics.Count == 0) { ConsoleHelper.PrintInfo("No comics in inventory."); return; }

        var items = new List<(Guid ComicId, int Quantity)>();

        while (true)
        {
            ConsoleHelper.PrintSubHeader("Available Comics");
            DisplayComicBrief(comics);

            int comicIdx = ConsoleHelper.GetMenuChoice("Comic #", 1, comics.Count) - 1;
            var comic = comics[comicIdx];

            int qty = ConsoleHelper.GetIntInput($"Quantity (available: {comic.StockQuantity})", 1, comic.StockQuantity);
            items.Add((comic.Id, qty));

            ConsoleHelper.PrintSuccess($"Added {qty}x \"{comic.Title}\"");

            if (!ConsoleHelper.Confirm("Add another item?"))
                break;
        }

        // Confirm
        Console.WriteLine($"\n  Customer : {customer.FullName} ({customer.Membership} — {customer.GetDiscountPercentage() * 100}% discount)");
        Console.WriteLine($"  Items    : {items.Count}");

        if (!ConsoleHelper.Confirm("Place this order?"))
        {
            ConsoleHelper.PrintInfo("Order cancelled.");
            return;
        }

        try
        {
            var receipt = await _orderService.PlaceOrderAsync(customer.Id, items);

            ConsoleHelper.PrintHeader("Order Receipt");
            Console.WriteLine($"  Order ID  : {receipt.OrderId.ToString()[..8]}");
            Console.WriteLine($"  Customer  : {receipt.CustomerName}");
            Console.WriteLine($"  Date      : {receipt.OrderDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine();

            var headers = new[] { "Comic", "Qty", "Unit Price", "Line Total" };
            var rows = receipt.Lines.Select(l => new[]
            {
                l.ComicTitle, l.Quantity.ToString(),
                $"${l.UnitPrice:F2}", $"${l.LineTotal:F2}"
            }).ToList();
            ConsoleHelper.PrintTable(headers, rows);

            Console.WriteLine($"\n  Subtotal  : ${receipt.Subtotal:F2}");
            Console.WriteLine($"  Discount  : -${receipt.Discount:F2}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  TOTAL     : ${receipt.Total:F2}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.WaitForKey();
    }

    // ── View all ─────────────────────────────────────────────────────────

    private async Task ViewAllAsync()
    {
        var orders = await _orderService.GetAllAsync();
        DisplayOrderList(orders);
        ConsoleHelper.WaitForKey();
    }

    // ── View details ─────────────────────────────────────────────────────

    private async Task ViewDetailsAsync()
    {
        var orders = await _orderService.GetAllAsync();
        if (orders.Count == 0) { ConsoleHelper.PrintInfo("No orders yet."); return; }

        DisplayOrderList(orders);
        int idx = ConsoleHelper.GetMenuChoice("Select order #", 1, orders.Count) - 1;
        var order = orders[idx];

        ConsoleHelper.PrintSubHeader($"Order {order.Id.ToString()[..8]}");
        Console.WriteLine($"  Customer  : {order.CustomerName}");
        Console.WriteLine($"  Status    : {order.Status}");
        Console.WriteLine($"  Created   : {order.CreatedAt:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Subtotal  : ${order.Subtotal:F2}");
        Console.WriteLine($"  Discount  : -${order.DiscountAmount:F2}");
        Console.WriteLine($"  Total     : ${order.Total:F2}");

        var headers = new[] { "Comic", "Genre", "Qty", "Unit Price", "Line Total" };
        var rows = order.Items.Select(i => new[]
        {
            i.ComicTitle, i.Genre.ToString(), i.Quantity.ToString(),
            $"${i.UnitPrice:F2}", $"${i.LineTotal:F2}"
        }).ToList();
        ConsoleHelper.PrintTable(headers, rows);
        ConsoleHelper.WaitForKey();
    }

    // ── Update status ────────────────────────────────────────────────────

    private async Task UpdateStatusAsync()
    {
        var orders = await _orderService.GetAllAsync();
        if (orders.Count == 0) { ConsoleHelper.PrintInfo("No orders."); return; }

        DisplayOrderList(orders);
        int idx = ConsoleHelper.GetMenuChoice("Select order #", 1, orders.Count) - 1;
        var order = orders[idx];

        ConsoleHelper.PrintInfo($"Current status: {order.Status}");
        var newStatus = ConsoleHelper.GetEnumChoice<OrderStatus>("New status");

        try
        {
            await _orderService.UpdateStatusAsync(order.Id, newStatus);
            ConsoleHelper.PrintSuccess($"Order status updated to {newStatus}.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }
        ConsoleHelper.WaitForKey();
    }

    // ── View by customer ─────────────────────────────────────────────────

    private async Task ViewByCustomerAsync()
    {
        var customers = await _customerService.GetAllAsync();
        if (customers.Count == 0) { ConsoleHelper.PrintInfo("No customers."); return; }

        DisplayCustomerBrief(customers);
        int idx = ConsoleHelper.GetMenuChoice("Customer #", 1, customers.Count) - 1;
        var orders = await _orderService.GetByCustomerAsync(customers[idx].Id);
        DisplayOrderList(orders);
        ConsoleHelper.WaitForKey();
    }

    // ── Display helpers ──────────────────────────────────────────────────

    private static void DisplayOrderList(IReadOnlyList<Order> orders)
    {
        var headers = new[] { "#", "Order ID", "Customer", "Items", "Total", "Status", "Date" };
        var rows = orders.Select((o, i) => new[]
        {
            (i + 1).ToString(),
            o.Id.ToString()[..8],
            o.CustomerName,
            o.Items.Count.ToString(),
            $"${o.Total:F2}",
            o.Status.ToString(),
            o.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        ConsoleHelper.PrintTable(headers, rows);
    }

    private static void DisplayCustomerBrief(IReadOnlyList<Customer> customers)
    {
        for (int i = 0; i < customers.Count; i++)
            Console.WriteLine($"  {i + 1}. {customers[i].FullName} ({customers[i].Membership})");
    }

    private static void DisplayComicBrief(IReadOnlyList<ComicBook> comics)
    {
        for (int i = 0; i < comics.Count; i++)
            Console.WriteLine($"  {i + 1}. {comics[i].Title} #{comics[i].IssueNumber} — ${comics[i].Price:F2} (Stock: {comics[i].StockQuantity})");
    }
}
