using Xunit;
using Moq;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Models;
using ComicBookShop.Core.Services;

namespace ComicBookShop.Tests.Unit;

/// <summary>
/// Unit tests for ComicBookService.
/// Demonstrates Arrange-Act-Assert, mocking, and testing validation (Day 21).
/// </summary>
public class ComicBookServiceTests
{
    private readonly Mock<IRepository<ComicBook>> _mockRepo;
    private readonly Mock<IAppLogger> _mockLogger;
    private readonly ComicBookService _service;

    public ComicBookServiceTests()
    {
        _mockRepo = new Mock<IRepository<ComicBook>>();
        _mockLogger = new Mock<IAppLogger>();
        _service = new ComicBookService(_mockRepo.Object, _mockLogger.Object);
    }

    // ── AddComicAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task AddComicAsync_ValidComic_ReturnsComic()
    {
        // Arrange
        var comic = CreateSampleComic();
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddComicAsync(comic);

        // Assert
        Assert.Equal("Spider-Man", result.Title);
        _mockRepo.Verify(r => r.AddAsync(comic), Times.Once);
    }

    [Fact]
    public async Task AddComicAsync_EmptyTitle_ThrowsValidationException()
    {
        var comic = CreateSampleComic();
        comic.Title = "";

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddComicAsync(comic));
    }

    [Fact]
    public async Task AddComicAsync_EmptyAuthor_ThrowsValidationException()
    {
        var comic = CreateSampleComic();
        comic.Author = "";

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddComicAsync(comic));
    }

    [Fact]
    public async Task AddComicAsync_ZeroPrice_ThrowsValidationException()
    {
        var comic = CreateSampleComic();
        comic.Price = 0;

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddComicAsync(comic));
    }

    // ── UpdateComicAsync ────────────────────────────────────────────────

    [Fact]
    public async Task UpdateComicAsync_ExistingComic_UpdatesAndReturns()
    {
        var existing = CreateSampleComic();
        _mockRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);

        var updated = CreateSampleComic();
        updated.Id = existing.Id;
        updated.Title = "The Amazing Spider-Man";

        var result = await _service.UpdateComicAsync(updated);

        Assert.Equal("The Amazing Spider-Man", result.Title);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<ComicBook>()), Times.Once);
    }

    [Fact]
    public async Task UpdateComicAsync_NotFound_ThrowsEntityNotFoundException()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ComicBook?)null);

        var comic = CreateSampleComic();
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateComicAsync(comic));
    }

    // ── RemoveComicAsync ────────────────────────────────────────────────

    [Fact]
    public async Task RemoveComicAsync_NotFound_ThrowsEntityNotFoundException()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ComicBook?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.RemoveComicAsync(Guid.NewGuid()));
    }

    // ── SearchAsync (LINQ) ──────────────────────────────────────────────

    [Fact]
    public async Task SearchAsync_ByTitle_ReturnsMatching()
    {
        var comics = SampleComicList();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(comics.AsReadOnly());

        var criteria = new SearchCriteria(TitleContains: "Spider");
        var results = await _service.SearchAsync(criteria);

        Assert.Single(results);
        Assert.Contains("Spider", results[0].Title);
    }

    [Fact]
    public async Task SearchAsync_ByGenre_ReturnsMatching()
    {
        var comics = SampleComicList();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(comics.AsReadOnly());

        var criteria = new SearchCriteria(Genre: Genre.Horror);
        var results = await _service.SearchAsync(criteria);

        Assert.All(results, c => Assert.Equal(Genre.Horror, c.Genre));
    }

    [Fact]
    public async Task SearchAsync_ByPriceRange_ReturnsMatching()
    {
        var comics = SampleComicList();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(comics.AsReadOnly());

        var criteria = new SearchCriteria(MinPrice: 10m, MaxPrice: 15m);
        var results = await _service.SearchAsync(criteria);

        Assert.All(results, c =>
        {
            Assert.True(c.Price >= 10m);
            Assert.True(c.Price <= 15m);
        });
    }

    // ── GetSummariesAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetSummariesAsync_ReturnsRecordsOrderedByTitle()
    {
        var comics = SampleComicList();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(comics.AsReadOnly());

        var summaries = await _service.GetSummariesAsync();

        Assert.Equal(comics.Count, summaries.Count);
        Assert.True(summaries.SequenceEqual(summaries.OrderBy(s => s.Title)));
    }

    // ── GetTotalInventoryValueAsync ─────────────────────────────────────

    [Fact]
    public async Task GetTotalInventoryValueAsync_CalculatesCorrectly()
    {
        var comics = SampleComicList();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(comics.AsReadOnly());

        var total = await _service.GetTotalInventoryValueAsync();

        var expected = comics.Sum(c => c.Price * c.StockQuantity);
        Assert.Equal(expected, total);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static ComicBook CreateSampleComic() => new()
    {
        Title = "Spider-Man", Author = "Stan Lee", Artist = "Steve Ditko",
        Publisher = "Marvel", Genre = Genre.Superhero, Price = 12.99m,
        StockQuantity = 10, ISBN = "978-0-1234-5678-9", Year = 1963,
        IssueNumber = 1, Condition = ComicCondition.NearMint
    };

    private static List<ComicBook> SampleComicList() => new()
    {
        new() { Title = "Spider-Man", Author = "Stan Lee", Genre = Genre.Superhero, Price = 12.99m, StockQuantity = 10 },
        new() { Title = "Batman", Author = "Frank Miller", Genre = Genre.Action, Price = 14.99m, StockQuantity = 5 },
        new() { Title = "Walking Dead", Author = "Robert Kirkman", Genre = Genre.Horror, Price = 10.99m, StockQuantity = 8 },
        new() { Title = "Saga", Author = "Brian K. Vaughan", Genre = Genre.Fantasy, Price = 9.99m, StockQuantity = 15 },
    };
}
