using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Entities;

/// <summary>
/// Represents a comic book in the shop inventory.
/// Demonstrates properties, enums, and ToString override (Days 2, 6, 7).
/// </summary>
public class ComicBook : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public Genre Genre { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public int Year { get; set; }
    public ComicCondition Condition { get; set; } = ComicCondition.NearMint;
    public int IssueNumber { get; set; } = 1;
    public string Description { get; set; } = string.Empty;

    public override string ToString() =>
        $"{Title} #{IssueNumber} by {Author} ({Year}) - {Genre} - ${Price:F2} [{Condition}] Stock: {StockQuantity}";
}
