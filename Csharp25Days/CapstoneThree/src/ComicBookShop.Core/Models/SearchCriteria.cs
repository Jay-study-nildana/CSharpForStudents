using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Models;

/// <summary>
/// Criteria record used to search/filter comics declaratively.
/// Demonstrates records, nullable properties, and query-object pattern (Days 10, 13).
/// </summary>
public record SearchCriteria(
    string? TitleContains = null,
    string? AuthorContains = null,
    Genre? Genre = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    ComicCondition? MinCondition = null,
    int? Year = null);
