using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Models;

/// <summary>
/// Lightweight read-only projection of a ComicBook for display/reports.
/// Demonstrates C# records and immutable DTOs (Day 10).
/// </summary>
public record ComicBookSummary(
    Guid Id,
    string Title,
    string Author,
    Genre Genre,
    decimal Price,
    int StockQuantity,
    ComicCondition Condition);
