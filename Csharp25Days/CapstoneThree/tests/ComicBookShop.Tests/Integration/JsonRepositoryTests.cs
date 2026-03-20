using Xunit;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Infrastructure.Persistence;

namespace ComicBookShop.Tests.Integration;

/// <summary>
/// Integration tests for JsonRepository — real file I/O against a temp directory.
/// Demonstrates integration testing and test data management (Day 22).
/// </summary>
public class JsonRepositoryTests : IDisposable
{
    private readonly string _tempDir;
    private readonly JsonRepository<ComicBook> _repo;

    public JsonRepositoryTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"cbshop_test_{Guid.NewGuid():N}");
        _repo = new JsonRepository<ComicBook>(_tempDir);
    }

    // ── Cleanup (Day 22 — teardown) ─────────────────────────────────────

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    // ── Tests ────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddAndRetrieve_PersistsToFile()
    {
        var comic = SampleComic("Persist Test");
        await _repo.AddAsync(comic);

        var retrieved = await _repo.GetByIdAsync(comic.Id);

        Assert.NotNull(retrieved);
        Assert.Equal("Persist Test", retrieved!.Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllAdded()
    {
        await _repo.AddAsync(SampleComic("A"));
        await _repo.AddAsync(SampleComic("B"));
        await _repo.AddAsync(SampleComic("C"));

        var all = await _repo.GetAllAsync();

        Assert.Equal(3, all.Count);
    }

    [Fact]
    public async Task UpdateAsync_ModifiesExisting()
    {
        var comic = SampleComic("Original");
        await _repo.AddAsync(comic);

        comic.Title = "Updated";
        await _repo.UpdateAsync(comic);

        var fetched = await _repo.GetByIdAsync(comic.Id);
        Assert.Equal("Updated", fetched!.Title);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntity()
    {
        var comic = SampleComic("To Delete");
        await _repo.AddAsync(comic);

        await _repo.DeleteAsync(comic.Id);

        var count = await _repo.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task FindAsync_FiltersCorrectly()
    {
        await _repo.AddAsync(SampleComic("Superhero1", Genre.Superhero));
        await _repo.AddAsync(SampleComic("Horror1", Genre.Horror));
        await _repo.AddAsync(SampleComic("Superhero2", Genre.Superhero));

        var heroes = await _repo.FindAsync(c => c.Genre == Genre.Superhero);

        Assert.Equal(2, heroes.Count);
    }

    [Fact]
    public async Task Persistence_SurvivesNewRepoInstance()
    {
        var comic = SampleComic("Durable");
        await _repo.AddAsync(comic);

        // Create a brand-new repository pointing at the same directory
        var repo2 = new JsonRepository<ComicBook>(_tempDir);
        var retrieved = await repo2.GetByIdAsync(comic.Id);

        Assert.NotNull(retrieved);
        Assert.Equal("Durable", retrieved!.Title);
    }

    [Fact]
    public async Task CountAsync_ReflectsChanges()
    {
        Assert.Equal(0, await _repo.CountAsync());

        await _repo.AddAsync(SampleComic("One"));
        Assert.Equal(1, await _repo.CountAsync());

        await _repo.AddAsync(SampleComic("Two"));
        Assert.Equal(2, await _repo.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_NonExistent_Throws()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repo.DeleteAsync(Guid.NewGuid()));
    }

    // ── Helper ──────────────────────────────────────────────────────────

    private static ComicBook SampleComic(string title, Genre genre = Genre.Superhero) => new()
    {
        Title = title, Author = "Test Author", Genre = genre,
        Price = 9.99m, StockQuantity = 10
    };
}
