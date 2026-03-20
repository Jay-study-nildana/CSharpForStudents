using Xunit;
using Moq;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Events;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Services;

namespace ComicBookShop.Tests.Unit;

/// <summary>Unit tests for InventoryManager — events, thread safety, exceptions (Days 19, 21, 25).</summary>
public class InventoryManagerTests
{
    private readonly Mock<IRepository<ComicBook>> _mockRepo;
    private readonly Mock<IAppLogger> _mockLogger;
    private readonly InventoryManager _manager;

    public InventoryManagerTests()
    {
        _mockRepo = new Mock<IRepository<ComicBook>>();
        _mockLogger = new Mock<IAppLogger>();
        _manager = new InventoryManager(_mockRepo.Object, _mockLogger.Object, lowStockThreshold: 5);
    }

    // ── DeductStockAsync ────────────────────────────────────────────────

    [Fact]
    public async Task DeductStockAsync_SufficientStock_Deducts()
    {
        var comic = SampleComic(stock: 10);
        _mockRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        await _manager.DeductStockAsync(comic.Id, 3);

        Assert.Equal(7, comic.StockQuantity);
    }

    [Fact]
    public async Task DeductStockAsync_InsufficientStock_Throws()
    {
        var comic = SampleComic(stock: 2);
        _mockRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);

        await Assert.ThrowsAsync<InsufficientStockException>(
            () => _manager.DeductStockAsync(comic.Id, 5));
    }

    [Fact]
    public async Task DeductStockAsync_NotFound_Throws()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ComicBook?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _manager.DeductStockAsync(Guid.NewGuid(), 1));
    }

    [Fact]
    public async Task DeductStockAsync_FiresStockChangedEvent()
    {
        var comic = SampleComic(stock: 10);
        _mockRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        StockChangedEventArgs? args = null;
        _manager.StockChanged += (_, e) => args = e;

        await _manager.DeductStockAsync(comic.Id, 2);

        Assert.NotNull(args);
        Assert.Equal(10, args!.PreviousQuantity);
        Assert.Equal(8, args.NewQuantity);
    }

    [Fact]
    public async Task DeductStockAsync_BelowThreshold_FiresLowStockAlert()
    {
        var comic = SampleComic(stock: 6);
        _mockRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        bool alertFired = false;
        _manager.LowStockAlert += (_, _) => alertFired = true;

        await _manager.DeductStockAsync(comic.Id, 2); // 6 → 4, threshold is 5

        Assert.True(alertFired);
    }

    // ── RestockAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task RestockAsync_ValidQuantity_Increases()
    {
        var comic = SampleComic(stock: 3);
        _mockRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        await _manager.RestockAsync(comic.Id, 10);

        Assert.Equal(13, comic.StockQuantity);
    }

    [Fact]
    public async Task RestockAsync_ZeroQuantity_ThrowsValidation()
    {
        await Assert.ThrowsAsync<ValidationException>(
            () => _manager.RestockAsync(Guid.NewGuid(), 0));
    }

    // ── Concurrency safety (Day 25) ─────────────────────────────────────

    [Fact]
    public async Task DeductStockAsync_ConcurrentCalls_MaintainsConsistency()
    {
        var comic = SampleComic(stock: 100);
        _mockRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        // Launch 10 concurrent deductions of 5 each — total 50
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _manager.DeductStockAsync(comic.Id, 5))
            .ToArray();

        await Task.WhenAll(tasks);

        Assert.Equal(50, comic.StockQuantity);
    }

    // ── Helper ──────────────────────────────────────────────────────────

    private static ComicBook SampleComic(int stock) => new()
    {
        Title = "Test Comic", Author = "Author",
        Genre = Genre.Superhero, Price = 9.99m,
        StockQuantity = stock
    };
}
