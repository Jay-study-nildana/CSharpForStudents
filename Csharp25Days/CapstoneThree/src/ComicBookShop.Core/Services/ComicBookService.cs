using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Models;

namespace ComicBookShop.Core.Services;

/// <summary>
/// Business logic for comic book CRUD and queries.
/// Demonstrates constructor injection (Day 17), LINQ (Day 13), and validation (Day 14).
/// </summary>
public class ComicBookService : IComicBookService
{
    private readonly IRepository<ComicBook> _repository;
    private readonly IAppLogger _logger;

    public ComicBookService(IRepository<ComicBook> repository, IAppLogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ComicBook> AddComicAsync(ComicBook comic)
    {
        if (string.IsNullOrWhiteSpace(comic.Title))
            throw new ValidationException("Comic title is required.");
        if (string.IsNullOrWhiteSpace(comic.Author))
            throw new ValidationException("Author is required.");
        if (comic.Price <= 0)
            throw new ValidationException("Price must be greater than zero.");

        await _repository.AddAsync(comic);
        _logger.LogInformation("Added comic: {Title} #{Issue}", comic.Title, comic.IssueNumber);
        return comic;
    }

    public async Task<ComicBook> UpdateComicAsync(ComicBook comic)
    {
        var existing = await _repository.GetByIdAsync(comic.Id)
            ?? throw new EntityNotFoundException(nameof(ComicBook), comic.Id);

        existing.Title = comic.Title;
        existing.Author = comic.Author;
        existing.Artist = comic.Artist;
        existing.Publisher = comic.Publisher;
        existing.Genre = comic.Genre;
        existing.Price = comic.Price;
        existing.ISBN = comic.ISBN;
        existing.Year = comic.Year;
        existing.Condition = comic.Condition;
        existing.IssueNumber = comic.IssueNumber;
        existing.Description = comic.Description;
        existing.MarkUpdated();

        await _repository.UpdateAsync(existing);
        _logger.LogInformation("Updated comic: {Title}", existing.Title);
        return existing;
    }

    public async Task RemoveComicAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException(nameof(ComicBook), id);

        await _repository.DeleteAsync(id);
        _logger.LogInformation("Removed comic: {Title}", existing.Title);
    }

    public async Task<ComicBook?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<IReadOnlyList<ComicBook>> GetAllAsync() =>
        await _repository.GetAllAsync();

    // ── LINQ-heavy querying (Day 13) ────────────────────────────────────

    public async Task<IReadOnlyList<ComicBook>> SearchAsync(SearchCriteria criteria)
    {
        var all = await _repository.GetAllAsync();

        IEnumerable<ComicBook> query = all;

        if (!string.IsNullOrWhiteSpace(criteria.TitleContains))
            query = query.Where(c => c.Title.Contains(criteria.TitleContains, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(criteria.AuthorContains))
            query = query.Where(c => c.Author.Contains(criteria.AuthorContains, StringComparison.OrdinalIgnoreCase));

        if (criteria.Genre.HasValue)
            query = query.Where(c => c.Genre == criteria.Genre.Value);

        if (criteria.MinPrice.HasValue)
            query = query.Where(c => c.Price >= criteria.MinPrice.Value);

        if (criteria.MaxPrice.HasValue)
            query = query.Where(c => c.Price <= criteria.MaxPrice.Value);

        if (criteria.Year.HasValue)
            query = query.Where(c => c.Year == criteria.Year.Value);

        if (criteria.MinCondition.HasValue)
            query = query.Where(c => c.Condition <= criteria.MinCondition.Value);

        return query.OrderBy(c => c.Title).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<ComicBookSummary>> GetSummariesAsync()
    {
        var all = await _repository.GetAllAsync();

        return all
            .Select(c => new ComicBookSummary(
                c.Id, c.Title, c.Author, c.Genre,
                c.Price, c.StockQuantity, c.Condition))
            .OrderBy(s => s.Title)
            .ToList()
            .AsReadOnly();
    }

    public async Task<IReadOnlyList<ComicBook>> GetByGenreAsync(Genre genre)
    {
        var results = await _repository.FindAsync(c => c.Genre == genre);
        return results.OrderBy(c => c.Title).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<ComicBook>> GetByPriceRangeAsync(PriceRange range)
    {
        var results = await _repository.FindAsync(c => range.Contains(c.Price));
        return results.OrderBy(c => c.Price).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<ComicBook>> GetLowStockAsync(int threshold)
    {
        var results = await _repository.FindAsync(c => c.StockQuantity <= threshold);
        return results.OrderBy(c => c.StockQuantity).ToList().AsReadOnly();
    }

    public async Task<Dictionary<Genre, int>> GetStockByGenreAsync()
    {
        var all = await _repository.GetAllAsync();

        return all
            .GroupBy(c => c.Genre)
            .ToDictionary(g => g.Key, g => g.Sum(c => c.StockQuantity));
    }

    public async Task<decimal> GetTotalInventoryValueAsync()
    {
        var all = await _repository.GetAllAsync();
        return all.Sum(c => c.Price * c.StockQuantity);
    }
}
