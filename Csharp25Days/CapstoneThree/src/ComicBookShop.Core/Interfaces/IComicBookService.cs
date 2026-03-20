using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Models;

namespace ComicBookShop.Core.Interfaces;

/// <summary>
/// Service contract for comic-book management and querying.
/// Demonstrates interface-first design (Day 9).
/// </summary>
public interface IComicBookService
{
    Task<ComicBook> AddComicAsync(ComicBook comic);
    Task<ComicBook> UpdateComicAsync(ComicBook comic);
    Task RemoveComicAsync(Guid id);
    Task<ComicBook?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<ComicBook>> GetAllAsync();
    Task<IReadOnlyList<ComicBook>> SearchAsync(SearchCriteria criteria);
    Task<IReadOnlyList<ComicBookSummary>> GetSummariesAsync();
    Task<IReadOnlyList<ComicBook>> GetByGenreAsync(Genre genre);
    Task<IReadOnlyList<ComicBook>> GetByPriceRangeAsync(PriceRange range);
    Task<IReadOnlyList<ComicBook>> GetLowStockAsync(int threshold);
    Task<Dictionary<Genre, int>> GetStockByGenreAsync();
    Task<decimal> GetTotalInventoryValueAsync();
}
