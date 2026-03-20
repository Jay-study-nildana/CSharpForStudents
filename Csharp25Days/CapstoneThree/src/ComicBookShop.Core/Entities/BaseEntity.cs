namespace ComicBookShop.Core.Entities;

/// <summary>
/// Abstract base class for all domain entities.
/// Provides common identity and audit fields (Day 8 — Inheritance).
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Stamps the UpdatedAt timestamp to the current UTC time.</summary>
    public void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
