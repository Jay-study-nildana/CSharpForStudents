using System.Text.Json;
using LibraryManagement.CLI;
using LibraryManagement.CLI.Menus;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Services;
using LibraryManagement.Infrastructure.Logging;
using LibraryManagement.Infrastructure.Repositories;

// ── 1. Load configuration from appsettings.json (Day 18) ────────────────────
var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
AppSettings settings;

if (File.Exists(settingsPath))
{
    var json = await File.ReadAllTextAsync(settingsPath);
    settings = JsonSerializer.Deserialize<AppSettings>(json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
        ?? new AppSettings();
}
else
{
    settings = new AppSettings();
}

// Resolve paths: prefer a 'data/' folder next to where the command is run (dotnet run
// from solution root), then fall back to next to the executable (published build).
var dataDir = ResolveDirectory(settings.DataDirectory);
var logDir  = ResolveDirectory(settings.LogDirectory);

static string ResolveDirectory(string relativePath)
{
    var fromCwd = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath));
    if (Directory.Exists(fromCwd))
        return fromCwd;

    // Fallback: next to the executable (e.g. after dotnet publish)
    return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relativePath));
}

// ── 2. Manual dependency injection (Day 17) ──────────────────────────────────
//    Compose the object graph by hand, wiring interfaces to concrete types.
var logger           = new ConsoleFileLogger(logDir);
var bookRepository   = new JsonBookRepository(dataDir);
var memberRepository = new JsonMemberRepository(dataDir);
var loanRepository   = new JsonLoanRepository(dataDir);

var libraryService = new LibraryService(
    bookRepository, memberRepository, loanRepository, logger);

// ── 3. Subscribe to domain events (Day 19) ───────────────────────────────────
//    Lambda event handlers react to service-level notifications.
libraryService.BookBorrowed += (_, args) =>
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(
        $"  ► EVENT  Book \"{args.Book.Title}\" borrowed by {args.Member.Name}. " +
        $"Return by {args.Loan.DueDate:yyyy-MM-dd}.");
    Console.ResetColor();
};

libraryService.BookReturned += (_, args) =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(
        $"  ► EVENT  \"{args.Book.Title}\" returned by {args.Member.Name}. Thank you!");
    Console.ResetColor();
};

// ── 4. Run the application ────────────────────────────────────────────────────
var app = new MainMenu(libraryService, logger);
await app.RunAsync();
