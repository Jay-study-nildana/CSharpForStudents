namespace LibraryManagement.Core.Models;

/// <summary>
/// Represents a book in the library collection.
/// Uses init-only properties for identity fields to enforce immutability after creation.
/// </summary>
public class Book
{
    // --- Identity (set once at creation) ---
    public Guid Id { get; init; }
    public DateTime AddedDate { get; init; }

    // --- Mutable catalogue fields ---
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public BookCategory Category { get; set; }
    public int PublishedYear { get; set; }

    // --- Availability flag toggled by loans ---
    public bool IsAvailable { get; set; }

    /// <summary>Full constructor used when adding a new book.</summary>
    public Book(string title, string author, string isbn, BookCategory category, int publishedYear)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        ISBN = isbn;
        Category = category;
        PublishedYear = publishedYear;
        IsAvailable = true;
        AddedDate = DateTime.UtcNow;
    }

    /// <summary>Parameterless constructor required for JSON deserialization.</summary>
    public Book() { }

    public override string ToString() =>
        $"[{Id}] \"{Title}\" by {Author} | ISBN: {ISBN} | {Category} | {PublishedYear} | {(IsAvailable ? "Available" : "On Loan")}";
}
