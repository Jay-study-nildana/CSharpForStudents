using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Tests.Models;

/// <summary>
/// Tests for the Book domain model — verifies constructor behaviour and default values.
/// Demonstrates arrange-act-assert with simple model tests (Day 21).
/// </summary>
public class BookTests
{
    [Fact]
    public void NewBook_IsAvailableByDefault()
    {
        var book = new Book("Title", "Author", "isbn-001", BookCategory.Fiction, 2020);
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void NewBook_HasNonEmptyId()
    {
        var book = new Book("Title", "Author", "isbn-002", BookCategory.Science, 2020);
        Assert.NotEqual(Guid.Empty, book.Id);
    }

    [Fact]
    public void TwoNewBooks_HaveUniqueIds()
    {
        var b1 = new Book("A", "AuthorA", "isbn-a", BookCategory.Fiction, 2020);
        var b2 = new Book("B", "AuthorB", "isbn-b", BookCategory.History, 2021);
        Assert.NotEqual(b1.Id, b2.Id);
    }

    [Fact]
    public void NewBook_AddedDateIsSetToNow()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        var book   = new Book("Title", "Author", "isbn-003", BookCategory.Other, 2024);
        var after  = DateTime.UtcNow.AddSeconds(1);

        Assert.InRange(book.AddedDate, before, after);
    }

    [Fact]
    public void NewBook_StoresAllProperties()
    {
        var book = new Book("Clean Code", "Robert Martin", "978-0-13", BookCategory.Technology, 2008);

        Assert.Equal("Clean Code",       book.Title);
        Assert.Equal("Robert Martin",    book.Author);
        Assert.Equal("978-0-13",         book.ISBN);
        Assert.Equal(BookCategory.Technology, book.Category);
        Assert.Equal(2008,               book.PublishedYear);
    }
}
