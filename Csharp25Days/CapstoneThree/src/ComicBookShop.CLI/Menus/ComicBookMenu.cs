using ComicBookShop.CLI.Helpers;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Models;

namespace ComicBookShop.CLI.Menus;

/// <summary>
/// Console sub-menu for comic-book CRUD and search.
/// Demonstrates async menus, LINQ-driven search, and user input validation (Days 3, 13, 14, 20).
/// </summary>
public class ComicBookMenu
{
    private readonly IComicBookService _service;

    public ComicBookMenu(IComicBookService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Comics Management");
            Console.WriteLine("  1. List All Comics");
            Console.WriteLine("  2. Search Comics");
            Console.WriteLine("  3. Add New Comic");
            Console.WriteLine("  4. Update Comic");
            Console.WriteLine("  5. Remove Comic");
            Console.WriteLine("  6. Back to Main Menu");

            switch (ConsoleHelper.GetMenuChoice("Select option", 1, 6))
            {
                case 1: await ListAllAsync(); break;
                case 2: await SearchAsync(); break;
                case 3: await AddComicAsync(); break;
                case 4: await UpdateComicAsync(); break;
                case 5: await RemoveComicAsync(); break;
                case 6: return;
            }
        }
    }

    // ── List ─────────────────────────────────────────────────────────────

    private async Task ListAllAsync()
    {
        var comics = await _service.GetAllAsync();
        DisplayComicList(comics);
        ConsoleHelper.WaitForKey();
    }

    // ── Search (demonstrates LINQ criteria object) ──────────────────────

    private async Task SearchAsync()
    {
        ConsoleHelper.PrintSubHeader("Search Comics (leave blank to skip a filter)");

        Console.Write("  Title contains: ");
        var title = Console.ReadLine()?.Trim();

        Console.Write("  Author contains: ");
        var author = Console.ReadLine()?.Trim();

        Console.Write("  Genre (leave blank for any): ");
        var genreInput = Console.ReadLine()?.Trim();
        Genre? genre = Enum.TryParse<Genre>(genreInput, true, out var g) ? g : null;

        Console.Write("  Min price: ");
        decimal? minPrice = decimal.TryParse(Console.ReadLine()?.Trim(), out var mn) ? mn : null;

        Console.Write("  Max price: ");
        decimal? maxPrice = decimal.TryParse(Console.ReadLine()?.Trim(), out var mx) ? mx : null;

        Console.Write("  Year: ");
        int? year = int.TryParse(Console.ReadLine()?.Trim(), out var yr) ? yr : null;

        var criteria = new SearchCriteria(
            TitleContains: string.IsNullOrWhiteSpace(title) ? null : title,
            AuthorContains: string.IsNullOrWhiteSpace(author) ? null : author,
            Genre: genre,
            MinPrice: minPrice,
            MaxPrice: maxPrice,
            Year: year);

        var results = await _service.SearchAsync(criteria);
        Console.WriteLine($"\n  Found {results.Count} result(s):");
        DisplayComicList(results);
        ConsoleHelper.WaitForKey();
    }

    // ── Add ──────────────────────────────────────────────────────────────

    private async Task AddComicAsync()
    {
        ConsoleHelper.PrintSubHeader("Add New Comic");

        try
        {
            var comic = new ComicBook
            {
                Title       = ConsoleHelper.GetRequiredInput("Title"),
                Author      = ConsoleHelper.GetRequiredInput("Author"),
                Artist      = ConsoleHelper.GetRequiredInput("Artist"),
                Publisher   = ConsoleHelper.GetRequiredInput("Publisher"),
                Genre       = ConsoleHelper.GetEnumChoice<Genre>("Genre"),
                Price       = ConsoleHelper.GetDecimalInput("Price ($)"),
                StockQuantity = ConsoleHelper.GetIntInput("Stock quantity", 0),
                ISBN        = ConsoleHelper.GetRequiredInput("ISBN"),
                Year        = ConsoleHelper.GetIntInput("Year", 1900, DateTime.Now.Year + 1),
                IssueNumber = ConsoleHelper.GetIntInput("Issue number", 1),
                Condition   = ConsoleHelper.GetEnumChoice<ComicCondition>("Condition"),
                Description = ConsoleHelper.GetRequiredInput("Description")
            };

            await _service.AddComicAsync(comic);
            ConsoleHelper.PrintSuccess($"Added \"{comic.Title}\" (ID: {comic.Id.ToString()[..8]})");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.WaitForKey();
    }

    // ── Update ───────────────────────────────────────────────────────────

    private async Task UpdateComicAsync()
    {
        var comics = await _service.GetAllAsync();
        if (comics.Count == 0) { ConsoleHelper.PrintInfo("No comics to update."); return; }

        DisplayComicList(comics);
        int idx = ConsoleHelper.GetMenuChoice("Select comic #", 1, comics.Count) - 1;
        var existing = comics[idx];

        ConsoleHelper.PrintSubHeader($"Updating \"{existing.Title}\" (Enter to keep current value)");

        try
        {
            existing.Title       = ConsoleHelper.GetOptionalInput("Title", existing.Title);
            existing.Author      = ConsoleHelper.GetOptionalInput("Author", existing.Author);
            existing.Artist      = ConsoleHelper.GetOptionalInput("Artist", existing.Artist);
            existing.Publisher   = ConsoleHelper.GetOptionalInput("Publisher", existing.Publisher);
            existing.Description = ConsoleHelper.GetOptionalInput("Description", existing.Description);

            Console.Write($"  Change genre? Current: {existing.Genre} (y/n): ");
            if (Console.ReadLine()?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) == true)
                existing.Genre = ConsoleHelper.GetEnumChoice<Genre>("Genre");

            Console.Write($"  New price [{existing.Price:F2}]: ");
            if (decimal.TryParse(Console.ReadLine()?.Trim(), out var p) && p > 0) existing.Price = p;

            Console.Write($"  New year [{existing.Year}]: ");
            if (int.TryParse(Console.ReadLine()?.Trim(), out var y) && y >= 1900) existing.Year = y;

            await _service.UpdateComicAsync(existing);
            ConsoleHelper.PrintSuccess("Comic updated.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.WaitForKey();
    }

    // ── Remove ───────────────────────────────────────────────────────────

    private async Task RemoveComicAsync()
    {
        var comics = await _service.GetAllAsync();
        if (comics.Count == 0) { ConsoleHelper.PrintInfo("No comics to remove."); return; }

        DisplayComicList(comics);
        int idx = ConsoleHelper.GetMenuChoice("Select comic # to remove", 1, comics.Count) - 1;
        var target = comics[idx];

        if (!ConsoleHelper.Confirm($"Remove \"{target.Title}\"?"))
        {
            ConsoleHelper.PrintInfo("Cancelled.");
            return;
        }

        try
        {
            await _service.RemoveComicAsync(target.Id);
            ConsoleHelper.PrintSuccess($"Removed \"{target.Title}\".");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.WaitForKey();
    }

    // ── Display helper ──────────────────────────────────────────────────

    private static void DisplayComicList(IReadOnlyList<ComicBook> comics)
    {
        var headers = new[] { "#", "Title", "Author", "Genre", "Price", "Stock", "Condition" };
        var rows = comics.Select((c, i) => new[]
        {
            (i + 1).ToString(),
            $"{c.Title} #{c.IssueNumber}",
            c.Author,
            c.Genre.ToString(),
            $"${c.Price:F2}",
            c.StockQuantity.ToString(),
            c.Condition.ToString()
        }).ToList();

        ConsoleHelper.PrintTable(headers, rows);
    }
}
