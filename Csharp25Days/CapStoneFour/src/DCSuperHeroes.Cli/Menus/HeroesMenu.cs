using DCSuperHeroes.Application.Contracts;
using DCSuperHeroes.Application.Interfaces;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.Models;
using DCSuperHeroes.Cli.Support;

namespace DCSuperHeroes.Cli.Menus;

public sealed class HeroesMenu
{
    private readonly IJusticeLeagueService _service;

    public HeroesMenu(IJusticeLeagueService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Hero Registry");
            Console.WriteLine("1. List all heroes");
            Console.WriteLine("2. List available heroes");
            Console.WriteLine("3. Search heroes");
            Console.WriteLine("4. Register a hero");
            Console.WriteLine("0. Back");
            Console.Write("Select an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    await ListHeroesAsync(new HeroSearchCriteria());
                    break;
                case "2":
                    await ListHeroesAsync(new HeroSearchCriteria(OnlyAvailable: true));
                    break;
                case "3":
                    await SearchHeroesAsync();
                    break;
                case "4":
                    await RegisterHeroAsync();
                    break;
                case "0":
                    return;
                default:
                    break;
            }
        }
    }

    private async Task ListHeroesAsync(HeroSearchCriteria criteria)
    {
        var heroes = await _service.GetHeroesAsync(criteria);
        ConsoleTable.Print(
            headers: ["Alias", "Archetype", "City", "Rank", "Ready", "Available", "Completed"],
            rows: heroes.Select(hero => new[]
            {
                hero.Alias,
                hero.Archetype.ToString(),
                hero.City,
                hero.Rank.ToString(),
                hero.PowerLevel.ToString(),
                hero.IsAvailable ? "Yes" : "No",
                hero.CompletedMissionCount.ToString()
            }));
        InputHelpers.Pause();
    }

    private async Task SearchHeroesAsync()
    {
        Console.Write("Alias or real name contains: ");
        var searchText = Console.ReadLine();
        Console.Write("City (optional): ");
        var city = Console.ReadLine();

        await ListHeroesAsync(new HeroSearchCriteria(SearchText: searchText, City: string.IsNullOrWhiteSpace(city) ? null : city));
    }

    private async Task RegisterHeroAsync()
    {
        try
        {
            Console.Write("Alias: ");
            var alias = Console.ReadLine() ?? string.Empty;
            Console.Write("Real name: ");
            var realName = Console.ReadLine() ?? string.Empty;
            Console.Write("City: ");
            var city = Console.ReadLine() ?? string.Empty;
            var rank = InputHelpers.ReadEnum<MembershipRank>("Rank");
            var archetype = InputHelpers.ReadEnum<HeroArchetype>("Archetype");
            var power = InputHelpers.ReadInt("Power level (1-100): ");
            var intelligence = InputHelpers.ReadInt("Intelligence (1-100): ");
            var teamwork = InputHelpers.ReadInt("Teamwork (1-100): ");
            Console.Write("Specialty detail: ");
            var specialty = Console.ReadLine() ?? string.Empty;

            var request = new CreateHeroRequest(alias, realName, city, rank, archetype, power, intelligence, teamwork, specialty);
            var hero = await _service.RegisterHeroAsync(request);
            Console.WriteLine($"Registered {hero.Alias} with ID {hero.Id}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        InputHelpers.Pause();
    }
}