using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

namespace LibraryManagement.CLI.Menus;

/// <summary>Sub-menu for all book-related operations.</summary>
public sealed class BookMenu
{
    private readonly ILibraryService _service;

    public BookMenu(ILibraryService service) => _service = service;

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("─── BOOKS ───────────────────────────────");
            Console.WriteLine("  1. List all books");
            Console.WriteLine("  2. List available books");
            Console.WriteLine("  3. Search books");
            Console.WriteLine("  4. Add a book");
            Console.WriteLine("  5. Remove a book");
            Console.WriteLine("  0. Back");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Select: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": await ListAllAsync();       break;
                case "2": await ListAvailableAsync(); break;
                case "3": await SearchAsync();        break;
                case "4": await AddAsync();           break;
                case "5": await RemoveAsync();        break;
                case "0": return;
                default:  Console.WriteLine("Invalid option."); break;
            }
        }
    }

    // ── Actions ───────────────────────────────────────────────────────────────

    private async Task ListAllAsync()
    {
        var books = (await _service.GetAllBooksAsync()).ToList();
        if (books.Count == 0) { Console.WriteLine("No books in the catalogue."); return; }

        PrintBookHeader();
        for (int i = 0; i < books.Count; i++)
            PrintBookRow(i + 1, books[i]);
    }

    private async Task ListAvailableAsync()
    {
        var books = (await _service.GetAvailableBooksAsync()).ToList();
        if (books.Count == 0) { Console.WriteLine("No books currently available."); return; }

        PrintBookHeader();
        for (int i = 0; i < books.Count; i++)
            PrintBookRow(i + 1, books[i]);
    }

    private async Task SearchAsync()
    {
        Console.Write("Search (title / author / ISBN / category): ");
        var query = Console.ReadLine() ?? string.Empty;

        var books = (await _service.SearchBooksAsync(query)).ToList();
        if (books.Count == 0) { Console.WriteLine("No books matched."); return; }

        Console.WriteLine($"\n{books.Count} result(s):");
        PrintBookHeader();
        for (int i = 0; i < books.Count; i++)
            PrintBookRow(i + 1, books[i]);
    }

    private async Task AddAsync()
    {
        Console.Write("Title      : "); var title  = Console.ReadLine() ?? string.Empty;
        Console.Write("Author     : "); var author = Console.ReadLine() ?? string.Empty;
        Console.Write("ISBN       : "); var isbn   = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Categories : " + string.Join(", ", Enum.GetNames<BookCategory>()));
        Console.Write("Category   : "); var catStr  = Console.ReadLine() ?? string.Empty;
        Console.Write("Pub. Year  : "); var yearStr = Console.ReadLine() ?? string.Empty;

        if (!Enum.TryParse<BookCategory>(catStr, ignoreCase: true, out var category))
        {
            Console.WriteLine("Unknown category — defaulting to 'Other'.");
            category = BookCategory.Other;
        }

        if (!int.TryParse(yearStr, out var year))
        {
            Console.WriteLine("Invalid year. Aborting.");
            return;
        }

        try
        {
            var book = await _service.AddBookAsync(title, author, isbn, category, year);
            Console.WriteLine($"  Book added. ID: {book.Id}");
        }
        catch (Exception ex) { PrintError(ex.Message); }
    }

    private async Task RemoveAsync()
    {
        Console.Write("Book ID to remove: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid GUID format.");
            return;
        }

        try
        {
            await _service.RemoveBookAsync(id);
            Console.WriteLine("  Book removed.");
        }
        catch (BookNotFoundException) { Console.WriteLine("  Book not found."); }
        catch (Exception ex)          { PrintError(ex.Message); }
    }

    // ── Formatting helpers ────────────────────────────────────────────────────

    private static void PrintBookHeader()
    {
        Console.WriteLine();
        Console.WriteLine($"{"#",-4} {"Title",-34} {"Author",-24} {"ISBN",-16} {"Cat.",-12} {"Avail.",-6}");
        Console.WriteLine(new string('─', 100));
    }

    private static void PrintBookRow(int num, Book b)
    {
        var avail = b.IsAvailable ? "Yes" : "No";
        Console.ForegroundColor = b.IsAvailable ? ConsoleColor.Green : ConsoleColor.DarkYellow;
        Console.WriteLine(
            $"{num,-4} {Truncate(b.Title,34),-34} {Truncate(b.Author,24),-24} " +
            $"{b.ISBN,-16} {b.Category,-12} {avail,-6}");
        Console.ResetColor();
    }

    private static string Truncate(string s, int max) =>
        s.Length <= max ? s : s[..(max - 1)] + "…";

    private static void PrintError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Error: {msg}");
        Console.ResetColor();
    }
}
