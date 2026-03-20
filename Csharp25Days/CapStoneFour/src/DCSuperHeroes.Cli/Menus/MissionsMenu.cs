using DCSuperHeroes.Application.Contracts;
using DCSuperHeroes.Application.Interfaces;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Cli.Support;

namespace DCSuperHeroes.Cli.Menus;

public sealed class MissionsMenu
{
    private readonly IJusticeLeagueService _service;
    private readonly int _recommendationCount;

    public MissionsMenu(IJusticeLeagueService service, int recommendationCount)
    {
        _service = service;
        _recommendationCount = recommendationCount;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mission Control");
            Console.WriteLine("1. List missions");
            Console.WriteLine("2. Create a mission");
            Console.WriteLine("3. Recommend a team");
            Console.WriteLine("4. Assign a hero");
            Console.WriteLine("5. Launch mission");
            Console.WriteLine("6. Complete mission");
            Console.WriteLine("0. Back");
            Console.Write("Select an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    await ListMissionsAsync();
                    break;
                case "2":
                    await CreateMissionAsync();
                    break;
                case "3":
                    await RecommendTeamAsync();
                    break;
                case "4":
                    await AssignHeroAsync();
                    break;
                case "5":
                    await LaunchMissionAsync();
                    break;
                case "6":
                    await CompleteMissionAsync();
                    break;
                case "0":
                    return;
                default:
                    break;
            }
        }
    }

    private async Task ListMissionsAsync()
    {
        var missions = await _service.GetMissionBriefingsAsync();
        ConsoleTable.Print(
            headers: ["Mission", "Threat", "Status", "Location", "Team", "Roster", "Starts"],
            rows: missions.Select(mission => new[]
            {
                $"{mission.CodeName} ({mission.MissionId})",
                mission.ThreatLevel.ToString(),
                mission.Status.ToString(),
                mission.Location,
                $"{mission.AssignedHeroes}/{mission.RequiredTeamSize}",
                mission.AssignedRoster,
                mission.StartsAtUtc.ToString("yyyy-MM-dd HH:mm")
            }));
        InputHelpers.Pause();
    }

    private async Task CreateMissionAsync()
    {
        try
        {
            Console.Write("Code name: ");
            var codeName = Console.ReadLine() ?? string.Empty;
            Console.Write("Location: ");
            var location = Console.ReadLine() ?? string.Empty;
            var threat = InputHelpers.ReadEnum<ThreatLevel>("Threat level");
            var teamSize = InputHelpers.ReadInt("Required team size: ");
            var requiresMystic = InputHelpers.ReadBoolean("Requires mystic support (y/n): ");
            var requiresStealth = InputHelpers.ReadBoolean("Requires stealth (y/n): ");
            var startsAt = InputHelpers.ReadDateTime("Start UTC (yyyy-MM-dd HH:mm): ");
            var duration = InputHelpers.ReadInt("Duration in hours: ");
            Console.Write("Notes: ");
            var notes = Console.ReadLine() ?? string.Empty;

            var request = new CreateMissionRequest(codeName, location, threat, teamSize, requiresMystic, requiresStealth, startsAt, duration, notes);
            var mission = await _service.CreateMissionAsync(request);
            Console.WriteLine($"Created mission {mission.CodeName} with ID {mission.Id}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        InputHelpers.Pause();
    }

    private async Task RecommendTeamAsync()
    {
        try
        {
            var missionId = InputHelpers.ReadGuid("Mission ID: ");
            var recommendations = await _service.RecommendTeamAsync(missionId, _recommendationCount);
            ConsoleTable.Print(
                headers: ["Hero", "Score", "Reason"],
                rows: recommendations.Select(recommendation => new[]
                {
                    $"{recommendation.Alias} ({recommendation.HeroId})",
                    recommendation.ReadinessScore.ToString("0.##"),
                    recommendation.Reason
                }));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        InputHelpers.Pause();
    }

    private async Task AssignHeroAsync()
    {
        try
        {
            var missionId = InputHelpers.ReadGuid("Mission ID: ");
            var heroId = InputHelpers.ReadGuid("Hero ID: ");
            var role = InputHelpers.ReadEnum<SupportRole>("Support role");
            await _service.AssignHeroAsync(new AssignmentRequest(missionId, heroId, role));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        InputHelpers.Pause();
    }

    private async Task LaunchMissionAsync()
    {
        try
        {
            var missionId = InputHelpers.ReadGuid("Mission ID: ");
            var mission = await _service.LaunchMissionAsync(missionId);
            Console.WriteLine($"Mission {mission.CodeName} is now {mission.Status}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        InputHelpers.Pause();
    }

    private async Task CompleteMissionAsync()
    {
        try
        {
            var missionId = InputHelpers.ReadGuid("Mission ID: ");
            Console.Write("Outcome summary: ");
            var summary = Console.ReadLine() ?? string.Empty;
            var mission = await _service.CompleteMissionAsync(missionId, summary);
            Console.WriteLine($"Mission {mission.CodeName} completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        InputHelpers.Pause();
    }
}