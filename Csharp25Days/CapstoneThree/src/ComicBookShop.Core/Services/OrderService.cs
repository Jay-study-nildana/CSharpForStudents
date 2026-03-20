using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Events;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Models;

namespace ComicBookShop.Core.Services;

/// <summary>
/// Orchestrates order placement, stock deduction, and reporting.
/// Demonstrates service composition, LINQ aggregation, events, and async (Days 13, 17, 19, 20).
/// </summary>
public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<ComicBook> _comicRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly InventoryManager _inventoryManager;
    private readonly IAppLogger _logger;

    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<ComicBook> comicRepository,
        IRepository<Customer> customerRepository,
        InventoryManager inventoryManager,
        IAppLogger logger)
    {
        _orderRepository = orderRepository;
        _comicRepository = comicRepository;
        _customerRepository = customerRepository;
        _inventoryManager = inventoryManager;
        _logger = logger;
    }

    public async Task<OrderReceipt> PlaceOrderAsync(
        Guid customerId,
        List<(Guid ComicId, int Quantity)> items)
    {
        if (items.Count == 0)
            throw new ValidationException("Order must contain at least one item.");

        // ── Validate customer ───────────────────────────────────────────
        var customer = await _customerRepository.GetByIdAsync(customerId)
            ?? throw new EntityNotFoundException(nameof(Customer), customerId);

        // ── Validate and collect line items ──────────────────────────────
        var orderItems = new List<OrderItem>();
        foreach (var (comicId, qty) in items)
        {
            var comic = await _comicRepository.GetByIdAsync(comicId)
                ?? throw new EntityNotFoundException(nameof(ComicBook), comicId);

            if (comic.StockQuantity < qty)
                throw new InsufficientStockException(comic.Title, comic.StockQuantity, qty);

            orderItems.Add(new OrderItem
            {
                ComicBookId = comic.Id,
                ComicTitle = comic.Title,
                Genre = comic.Genre,
                Quantity = qty,
                UnitPrice = comic.Price
            });
        }

        // ── Calculate totals with membership discount ───────────────────
        decimal subtotal = orderItems.Sum(i => i.LineTotal);
        decimal discountPct = customer.GetDiscountPercentage();
        decimal discountAmount = Math.Round(subtotal * discountPct, 2);
        decimal total = subtotal - discountAmount;

        // ── Create order entity ─────────────────────────────────────────
        var order = new Order
        {
            CustomerId = customer.Id,
            CustomerName = customer.FullName,
            Items = orderItems,
            Subtotal = subtotal,
            DiscountAmount = discountAmount,
            Total = total,
            Status = OrderStatus.Confirmed
        };

        // ── Deduct stock (thread-safe via InventoryManager) ─────────────
        foreach (var item in orderItems)
        {
            await _inventoryManager.DeductStockAsync(item.ComicBookId, item.Quantity);
        }

        // ── Update customer spend total ─────────────────────────────────
        customer.TotalSpent += total;
        customer.MarkUpdated();
        await _customerRepository.UpdateAsync(customer);

        // ── Persist order ───────────────────────────────────────────────
        await _orderRepository.AddAsync(order);

        _logger.LogInformation("Order {OrderId} placed by {Customer} — ${Total}",
            order.Id, customer.FullName, total);

        // ── Fire event (Day 19) ─────────────────────────────────────────
        OnOrderPlaced(new OrderPlacedEventArgs(order.Id, customer.FullName, total, orderItems.Count));

        // ── Return immutable receipt (record, Day 10) ───────────────────
        var receiptLines = orderItems
            .Select(i => new OrderLineItem(i.ComicTitle, i.Quantity, i.UnitPrice, i.LineTotal))
            .ToList();

        return new OrderReceipt(order.Id, customer.FullName, order.CreatedAt,
            receiptLines, subtotal, discountAmount, total);
    }

    public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new EntityNotFoundException(nameof(Order), orderId);

        var oldStatus = order.Status;
        order.Status = newStatus;
        order.MarkUpdated();
        await _orderRepository.UpdateAsync(order);

        _logger.LogInformation("Order {Id} status: {Old} -> {New}", orderId, oldStatus, newStatus);
    }

    public async Task<Order?> GetByIdAsync(Guid id) =>
        await _orderRepository.GetByIdAsync(id);

    public async Task<IReadOnlyList<Order>> GetAllAsync() =>
        await _orderRepository.GetAllAsync();

    public async Task<IReadOnlyList<Order>> GetByCustomerAsync(Guid customerId)
    {
        var results = await _orderRepository.FindAsync(o => o.CustomerId == customerId);
        return results.OrderByDescending(o => o.CreatedAt).ToList().AsReadOnly();
    }

    // ── LINQ-heavy reporting (Day 13) ───────────────────────────────────

    public async Task<decimal> GetTotalRevenueAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .Sum(o => o.Total);
    }

    public async Task<IReadOnlyList<(string Title, int TotalSold)>> GetTopSellingComicsAsync(int count)
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ComicTitle)
            .Select(g => (Title: g.Key, TotalSold: g.Sum(i => i.Quantity)))
            .OrderByDescending(x => x.TotalSold)
            .Take(count)
            .ToList();
    }

    public async Task<Dictionary<Genre, decimal>> GetRevenueByGenreAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.Genre)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.LineTotal));
    }

    protected virtual void OnOrderPlaced(OrderPlacedEventArgs e) =>
        OrderPlaced?.Invoke(this, e);
}
