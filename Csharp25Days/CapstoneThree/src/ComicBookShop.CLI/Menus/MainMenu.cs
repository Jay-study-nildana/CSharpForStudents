using ComicBookShop.CLI.Helpers;

namespace ComicBookShop.CLI.Menus;

/// <summary>Top-level menu that routes to sub-menus.</summary>
public class MainMenu
{
    private readonly ComicBookMenu _comicMenu;
    private readonly CustomerMenu _customerMenu;
    private readonly OrderMenu _orderMenu;
    private readonly ReportMenu _reportMenu;
    private readonly string _shopName;

    public MainMenu(
        ComicBookMenu comicMenu,
        CustomerMenu customerMenu,
        OrderMenu orderMenu,
        ReportMenu reportMenu,
        string shopName)
    {
        _comicMenu = comicMenu;
        _customerMenu = customerMenu;
        _orderMenu = orderMenu;
        _reportMenu = reportMenu;
        _shopName = shopName;
    }

    public async Task RunAsync()
    {
        PrintBanner();

        while (true)
        {
            ConsoleHelper.PrintHeader($"{_shopName} — Main Menu");
            Console.WriteLine("  1. Comics Management");
            Console.WriteLine("  2. Customer Management");
            Console.WriteLine("  3. Orders");
            Console.WriteLine("  4. Reports");
            Console.WriteLine("  5. Exit");

            switch (ConsoleHelper.GetMenuChoice("Select option", 1, 5))
            {
                case 1: await _comicMenu.ShowAsync(); break;
                case 2: await _customerMenu.ShowAsync(); break;
                case 3: await _orderMenu.ShowAsync(); break;
                case 4: await _reportMenu.ShowAsync(); break;
                case 5:
                    ConsoleHelper.PrintSuccess("Thank you for visiting! Goodbye.");
                    return;
            }
        }
    }

    private void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"
    ╔═══════════════════════════════════════════════════╗
    ║                                                   ║
    ║     KAPOW!  COMIC  BOOK  SHOP                     ║
    ║     ─────────────────────────                     ║
    ║     Your neighbourhood comic store, now in CLI!   ║
    ║                                                   ║
    ╚═══════════════════════════════════════════════════╝");
        Console.ResetColor();
    }
}
