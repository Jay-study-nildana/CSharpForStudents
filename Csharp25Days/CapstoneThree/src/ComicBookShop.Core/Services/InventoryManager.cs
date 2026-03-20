using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Events;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.Core.Services;

/// <summary>
/// Thread-safe inventory manager that raises stock-change events.
/// Demonstrates SemaphoreSlim (Day 25), events / delegates (Day 19), and async (Day 20).
/// </summary>
public class InventoryManager
{
    private readonly IRepository<ComicBook> _comicRepository;
    private readonly IAppLogger _logger;
    private readonly int _lowStockThreshold;
    private readonly SemaphoreSlim _stockLock = new(1, 1);

    // ── Events (Day 19) ─────────────────────────────────────────────────
    public event EventHandler<StockChangedEventArgs>? StockChanged;
    public event EventHandler<StockChangedEventArgs>? LowStockAlert;

    public InventoryManager(
        IRepository<ComicBook> comicRepository,
        IAppLogger logger,
        int lowStockThreshold = 5)
    {
        _comicRepository = comicRepository;
        _logger = logger;
        _lowStockThreshold = lowStockThreshold;
    }

    /// <summary>Atomically deducts stock and fires change events.</summary>
    public async Task DeductStockAsync(Guid comicId, int quantity)
    {
        await _stockLock.WaitAsync();
        try
        {
            var comic = await _comicRepository.GetByIdAsync(comicId)
                ?? throw new EntityNotFoundException(nameof(ComicBook), comicId);

            if (comic.StockQuantity < quantity)
                throw new InsufficientStockException(comic.Title, comic.StockQuantity, quantity);

            int oldQty = comic.StockQuantity;
            comic.StockQuantity -= quantity;
            comic.MarkUpdated();
            await _comicRepository.UpdateAsync(comic);

            bool isLow = comic.StockQuantity <= _lowStockThreshold;
            var args = new StockChangedEventArgs(comic.Id, comic.Title, oldQty, comic.StockQuantity, isLow);

            OnStockChanged(args);
            if (isLow) OnLowStockAlert(args);
        }
        finally
        {
            _stockLock.Release();
        }
    }

    /// <summary>Atomically restocks a comic and fires change events.</summary>
    public async Task RestockAsync(Guid comicId, int quantity)
    {
        if (quantity <= 0)
            throw new ValidationException("Restock quantity must be positive.");

        await _stockLock.WaitAsync();
        try
        {
            var comic = await _comicRepository.GetByIdAsync(comicId)
                ?? throw new EntityNotFoundException(nameof(ComicBook), comicId);

            int oldQty = comic.StockQuantity;
            comic.StockQuantity += quantity;
            comic.MarkUpdated();
            await _comicRepository.UpdateAsync(comic);

            var args = new StockChangedEventArgs(comic.Id, comic.Title, oldQty, comic.StockQuantity, false);
            OnStockChanged(args);

            _logger.LogInformation("Restocked '{Title}': {Old} -> {New}", comic.Title, oldQty, comic.StockQuantity);
        }
        finally
        {
            _stockLock.Release();
        }
    }

    public async Task<IReadOnlyList<ComicBook>> GetLowStockAsync()
    {
        var all = await _comicRepository.GetAllAsync();
        return all.Where(c => c.StockQuantity <= _lowStockThreshold)
                  .OrderBy(c => c.StockQuantity)
                  .ToList()
                  .AsReadOnly();
    }

    // ── Protected virtual event raisers ─────────────────────────────────
    protected virtual void OnStockChanged(StockChangedEventArgs e) =>
        StockChanged?.Invoke(this, e);

    protected virtual void OnLowStockAlert(StockChangedEventArgs e) =>
        LowStockAlert?.Invoke(this, e);
}
