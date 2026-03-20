using LibraryManagement.Core.Interfaces;
using LibraryManagement.CLI.Menus;

namespace LibraryManagement.CLI.Menus;

/// <summary>Top-level navigation menu — the application shell.</summary>
public sealed class MainMenu
{
    private readonly ILibraryService _service;
    private readonly IAppLogger      _logger;
    private readonly BookMenu        _bookMenu;
    private readonly MemberMenu      _memberMenu;
    private readonly LoanMenu        _loanMenu;

    public MainMenu(ILibraryService service, IAppLogger logger)
    {
        _service    = service;
        _logger     = logger;
        _bookMenu   = new BookMenu(service);
        _memberMenu = new MemberMenu(service);
        _loanMenu   = new LoanMenu(service);
    }

    public async Task RunAsync()
    {
        Console.Clear();
        PrintBanner();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        MAIN MENU                 ║");
            Console.WriteLine("╠══════════════════════════════════╣");
            Console.WriteLine("║  1. Books                        ║");
            Console.WriteLine("║  2. Members                      ║");
            Console.WriteLine("║  3. Loans                        ║");
            Console.WriteLine("║  0. Exit                         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.Write("Select: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": await _bookMenu.RunAsync();   break;
                case "2": await _memberMenu.RunAsync(); break;
                case "3": await _loanMenu.RunAsync();   break;
                case "0":
                    Console.WriteLine("\nGoodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose 0–3.");
                    break;
            }
        }
    }

    private static void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   Library Management System — CLI Edition  ║");
        Console.WriteLine("║   C# & .NET Fundamentals — Capstone Project║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.ResetColor();
    }
}
