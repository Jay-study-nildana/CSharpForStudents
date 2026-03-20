using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Events;
using ComicBookShop.Core.Models;

namespace ComicBookShop.Core.Interfaces;

/// <summary>
/// Service contract for order processing.
/// Includes an event so subscribers can react to new orders (Day 19).
/// </summary>
public interface IOrderService
{
    event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

    Task<OrderReceipt> PlaceOrderAsync(Guid customerId, List<(Guid ComicId, int Quantity)> items);
    Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus);
    Task<Order?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Order>> GetAllAsync();
    Task<IReadOnlyList<Order>> GetByCustomerAsync(Guid customerId);
    Task<decimal> GetTotalRevenueAsync();
    Task<IReadOnlyList<(string Title, int TotalSold)>> GetTopSellingComicsAsync(int count);
    Task<Dictionary<Genre, decimal>> GetRevenueByGenreAsync();
}
