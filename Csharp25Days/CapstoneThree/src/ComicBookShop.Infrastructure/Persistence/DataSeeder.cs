using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.Infrastructure.Persistence;

/// <summary>
/// Seeds initial sample data when the data store is empty.
/// Demonstrates collections initialisation and data-driven setup (Day 11).
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(
        IRepository<ComicBook> comicRepo,
        IRepository<Customer> customerRepo,
        IAppLogger logger)
    {
        var existingComics = await comicRepo.CountAsync();
        if (existingComics > 0)
        {
            logger.LogInformation("Data store already populated — skipping seed.");
            return;
        }

        foreach (var comic in GetSampleComics())
            await comicRepo.AddAsync(comic);

        foreach (var customer in GetSampleCustomers())
            await customerRepo.AddAsync(customer);

        logger.LogInformation("Seeded {Comics} comics and {Customers} customers.",
            GetSampleComics().Count, GetSampleCustomers().Count);
    }

    // ── Sample comics ───────────────────────────────────────────────────

    private static List<ComicBook> GetSampleComics() => new()
    {
        new ComicBook
        {
            Title = "The Amazing Spider-Man", Author = "Stan Lee", Artist = "Steve Ditko",
            Publisher = "Marvel", Genre = Genre.Superhero, Price = 12.99m,
            StockQuantity = 25, ISBN = "978-0-7851-5757-1", Year = 1963,
            IssueNumber = 1, Condition = ComicCondition.NearMint,
            Description = "The origin story of Spider-Man."
        },
        new ComicBook
        {
            Title = "Batman: The Dark Knight Returns", Author = "Frank Miller", Artist = "Frank Miller",
            Publisher = "DC Comics", Genre = Genre.Action, Price = 14.99m,
            StockQuantity = 18, ISBN = "978-1-5638-9342-7", Year = 1986,
            IssueNumber = 1, Condition = ComicCondition.Mint,
            Description = "An older Bruce Wayne returns to fight crime in Gotham."
        },
        new ComicBook
        {
            Title = "X-Men: Days of Future Past", Author = "Chris Claremont", Artist = "John Byrne",
            Publisher = "Marvel", Genre = Genre.SciFi, Price = 11.50m,
            StockQuantity = 12, ISBN = "978-0-7851-6422-7", Year = 1981,
            IssueNumber = 141, Condition = ComicCondition.VeryFine,
            Description = "A dystopian future timeline where mutants are hunted."
        },
        new ComicBook
        {
            Title = "Saga", Author = "Brian K. Vaughan", Artist = "Fiona Staples",
            Publisher = "Image Comics", Genre = Genre.Fantasy, Price = 9.99m,
            StockQuantity = 30, ISBN = "978-1-6070-6601-9", Year = 2012,
            IssueNumber = 1, Condition = ComicCondition.NearMint,
            Description = "An epic space opera / fantasy about two lovers from warring races."
        },
        new ComicBook
        {
            Title = "The Walking Dead", Author = "Robert Kirkman", Artist = "Tony Moore",
            Publisher = "Image Comics", Genre = Genre.Horror, Price = 10.99m,
            StockQuantity = 20, ISBN = "978-1-5824-0672-5", Year = 2003,
            IssueNumber = 1, Condition = ComicCondition.Fine,
            Description = "Survivors navigate a zombie apocalypse."
        },
        new ComicBook
        {
            Title = "Archie", Author = "Bob Montana", Artist = "Bob Montana",
            Publisher = "Archie Comics", Genre = Genre.Comedy, Price = 5.99m,
            StockQuantity = 35, ISBN = "978-1-6279-8912-3", Year = 1942,
            IssueNumber = 1, Condition = ComicCondition.Good,
            Description = "Classic teen humour in Riverdale."
        },
        new ComicBook
        {
            Title = "Blankets", Author = "Craig Thompson", Artist = "Craig Thompson",
            Publisher = "Top Shelf", Genre = Genre.Romance, Price = 18.99m,
            StockQuantity = 8, ISBN = "978-1-8913-0225-9", Year = 2003,
            IssueNumber = 1, Condition = ComicCondition.Mint,
            Description = "A coming-of-age memoir and love story."
        },
        new ComicBook
        {
            Title = "The Sandman", Author = "Neil Gaiman", Artist = "Sam Kieth",
            Publisher = "DC / Vertigo", Genre = Genre.Mystery, Price = 16.99m,
            StockQuantity = 15, ISBN = "978-1-4012-2575-9", Year = 1989,
            IssueNumber = 1, Condition = ComicCondition.NearMint,
            Description = "Morpheus, the King of Dreams, escapes captivity and reclaims his realm."
        },
        new ComicBook
        {
            Title = "Akira", Author = "Katsuhiro Otomo", Artist = "Katsuhiro Otomo",
            Publisher = "Kodansha", Genre = Genre.Manga, Price = 24.99m,
            StockQuantity = 10, ISBN = "978-1-9351-2920-5", Year = 1982,
            IssueNumber = 1, Condition = ComicCondition.VeryFine,
            Description = "Neo-Tokyo biker gang caught in a government conspiracy."
        },
        new ComicBook
        {
            Title = "Watchmen", Author = "Alan Moore", Artist = "Dave Gibbons",
            Publisher = "DC Comics", Genre = Genre.Drama, Price = 19.99m,
            StockQuantity = 22, ISBN = "978-1-4012-4534-4", Year = 1986,
            IssueNumber = 1, Condition = ComicCondition.Mint,
            Description = "Deconstruction of the superhero genre in an alternate 1985."
        },
        new ComicBook
        {
            Title = "Spawn", Author = "Todd McFarlane", Artist = "Todd McFarlane",
            Publisher = "Image Comics", Genre = Genre.Action, Price = 8.99m,
            StockQuantity = 14, ISBN = "978-1-6070-6000-0", Year = 1992,
            IssueNumber = 1, Condition = ComicCondition.Fine,
            Description = "A murdered CIA operative returns from the dead as a hellspawn."
        },
        new ComicBook
        {
            Title = "Hellboy: Seed of Destruction", Author = "Mike Mignola", Artist = "Mike Mignola",
            Publisher = "Dark Horse", Genre = Genre.Horror, Price = 13.99m,
            StockQuantity = 4, ISBN = "978-1-5930-7094-6", Year = 1994,
            IssueNumber = 1, Condition = ComicCondition.NearMint,
            Description = "A demon raised by humans investigates the paranormal."
        },
        new ComicBook
        {
            Title = "One Piece", Author = "Eiichiro Oda", Artist = "Eiichiro Oda",
            Publisher = "Shueisha", Genre = Genre.Manga, Price = 9.99m,
            StockQuantity = 40, ISBN = "978-1-5693-1901-2", Year = 1997,
            IssueNumber = 1, Condition = ComicCondition.NearMint,
            Description = "Monkey D. Luffy and crew search for the legendary One Piece treasure."
        },
        new ComicBook
        {
            Title = "Bone", Author = "Jeff Smith", Artist = "Jeff Smith",
            Publisher = "Cartoon Books", Genre = Genre.Fantasy, Price = 11.99m,
            StockQuantity = 3, ISBN = "978-1-8889-6314-4", Year = 1991,
            IssueNumber = 1, Condition = ComicCondition.Good,
            Description = "Three cousins lost in a vast uncharted desert befriend locals."
        },
        new ComicBook
        {
            Title = "Scott Pilgrim", Author = "Bryan Lee O'Malley", Artist = "Bryan Lee O'Malley",
            Publisher = "Oni Press", Genre = Genre.Comedy, Price = 10.99m,
            StockQuantity = 16, ISBN = "978-1-9320-6422-4", Year = 2004,
            IssueNumber = 1, Condition = ComicCondition.VeryFine,
            Description = "A slacker must defeat his new girlfriend's seven evil exes."
        }
    };

    // ── Sample customers ────────────────────────────────────────────────

    private static List<Customer> GetSampleCustomers() => new()
    {
        new Customer
        {
            FirstName = "Tony", LastName = "Parker",
            Email = "tony.parker@example.com", Phone = "555-0101",
            Membership = MembershipTier.Bronze, TotalSpent = 45.00m
        },
        new Customer
        {
            FirstName = "Diana", LastName = "Prince",
            Email = "diana.prince@example.com", Phone = "555-0102",
            Membership = MembershipTier.Silver, TotalSpent = 320.50m
        },
        new Customer
        {
            FirstName = "Bruce", LastName = "Wayne",
            Email = "bruce.wayne@example.com", Phone = "555-0103",
            Membership = MembershipTier.Gold, TotalSpent = 1250.00m
        },
        new Customer
        {
            FirstName = "Clark", LastName = "Kent",
            Email = "clark.kent@example.com", Phone = "555-0104",
            Membership = MembershipTier.Platinum, TotalSpent = 3500.75m
        },
        new Customer
        {
            FirstName = "Peter", LastName = "Parker",
            Email = "peter.parker@example.com", Phone = "555-0105",
            Membership = MembershipTier.Bronze, TotalSpent = 28.99m
        }
    };
}
