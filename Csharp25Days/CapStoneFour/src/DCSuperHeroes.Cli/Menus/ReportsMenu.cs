using DCSuperHeroes.Application.Interfaces;
using DCSuperHeroes.Cli.Support;

namespace DCSuperHeroes.Cli.Menus;

public sealed class ReportsMenu
{
    private readonly IJusticeLeagueService _service;

    public ReportsMenu(IJusticeLeagueService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Reports and Analytics");
            Console.WriteLine("1. View dashboard");
            Console.WriteLine("2. View top heroes");
            Console.WriteLine("3. Reseed sample data");
            Console.WriteLine("0. Back");
            Console.Write("Select an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    await ShowDashboardAsync();
                    break;
                case "2":
                    await ShowTopHeroesAsync();
                    break;
                case "3":
                    await _service.SeedSampleDataAsync();
                    Console.WriteLine("Seed operation completed.");
                    InputHelpers.Pause();
                    break;
                case "0":
                    return;
                default:
                    break;
            }
        }
    }

    private async Task ShowDashboardAsync()
    {
        var dashboard = await _service.GetDashboardAsync();

        Console.WriteLine($"Total heroes      : {dashboard.TotalHeroes}");
        Console.WriteLine($"Available heroes  : {dashboard.AvailableHeroes}");
        Console.WriteLine($"Open missions     : {dashboard.OpenMissions}");
        Console.WriteLine($"Completed missions: {dashboard.CompletedMissions}");
        Console.WriteLine($"Assignments       : {dashboard.ActiveAssignments}");
        Console.WriteLine($"Average power     : {dashboard.AveragePowerLevel:0.##}");
        Console.WriteLine($"Most active city  : {dashboard.MostActiveCity}");
        Console.WriteLine($"Highest open threat: {dashboard.HighestOpenThreat?.ToString() ?? "None"}");
        Console.WriteLine();

        ConsoleTable.Print(
            headers: ["Rank", "Heroes"],
            rows: dashboard.HeroesByRank.Select(entry => new[] { entry.Key.ToString(), entry.Value.ToString() }));
        Console.WriteLine();
        ConsoleTable.Print(
            headers: ["Threat", "Missions"],
            rows: dashboard.MissionsByThreat.Select(entry => new[] { entry.Key.ToString(), entry.Value.ToString() }));

        InputHelpers.Pause();
    }

    private async Task ShowTopHeroesAsync()
    {
        var count = InputHelpers.ReadInt("How many heroes: ");
        var heroes = await _service.GetTopHeroesAsync(count);

        ConsoleTable.Print(
            headers: ["Hero", "Completed", "Power", "City"],
            rows: heroes.Select(hero => new[]
            {
                hero.Alias,
                hero.CompletedMissionCount.ToString(),
                hero.PowerLevel.ToString(),
                hero.City
            }));

        InputHelpers.Pause();
    }
}