using DCSuperHeroes.Application.Interfaces;

namespace DCSuperHeroes.Cli.Menus;

public sealed class MainMenu
{
    private readonly HeroesMenu _heroesMenu;
    private readonly MissionsMenu _missionsMenu;
    private readonly ReportsMenu _reportsMenu;

    public MainMenu(IJusticeLeagueService service, int recommendationCount)
    {
        _heroesMenu = new HeroesMenu(service);
        _missionsMenu = new MissionsMenu(service, recommendationCount);
        _reportsMenu = new ReportsMenu(service);
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            PrintBanner();

            Console.WriteLine("1. Hero Registry");
            Console.WriteLine("2. Mission Control");
            Console.WriteLine("3. Reports and Analytics");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    await _heroesMenu.RunAsync();
                    break;
                case "2":
                    await _missionsMenu.RunAsync();
                    break;
                case "3":
                    await _reportsMenu.RunAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    private static void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===============================================");
        Console.WriteLine(" DC Comics Super Heroes: Watchtower Console");
        Console.WriteLine("===============================================");
        Console.ResetColor();
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }
}