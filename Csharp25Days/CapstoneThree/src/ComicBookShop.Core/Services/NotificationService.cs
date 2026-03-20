using ComicBookShop.Core.Events;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.Core.Services;

/// <summary>
/// Subscribes to domain events and logs notifications.
/// Demonstrates event subscriber / observer pattern (Day 19).
/// </summary>
public class NotificationService
{
    private readonly IAppLogger _logger;

    public NotificationService(IAppLogger logger)
    {
        _logger = logger;
    }

    /// <summary>Handles stock-change events.</summary>
    public void OnStockChanged(object? sender, StockChangedEventArgs e)
    {
        _logger.LogInformation(
            "[Notification] Stock changed for '{Title}': {Old} -> {New}",
            e.Title, e.PreviousQuantity, e.NewQuantity);
    }

    /// <summary>Handles low-stock alert events.</summary>
    public void OnLowStockAlert(object? sender, StockChangedEventArgs e)
    {
        _logger.LogWarning(
            "[ALERT] LOW STOCK: '{Title}' has only {Qty} remaining!",
            e.Title, e.NewQuantity);
    }

    /// <summary>Handles order-placed events.</summary>
    public void OnOrderPlaced(object? sender, OrderPlacedEventArgs e)
    {
        _logger.LogInformation(
            "[Notification] New order {OrderId} by {Customer} — ${Total} ({Count} item(s))",
            e.OrderId, e.CustomerName, e.Total, e.ItemCount);
    }
}
