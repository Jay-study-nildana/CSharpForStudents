using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.Models;

namespace DCSuperHeroes.Application.Services;

public static class LeagueAnalyticsBuilder
{
    public static LeagueDashboard Build(
        IReadOnlyList<Hero> heroes,
        IReadOnlyList<Mission> missions,
        IReadOnlyList<MissionAssignment> assignments)
    {
        var mostActiveCity = heroes.Count == 0
            ? "N/A"
            : heroes
                .GroupBy(hero => hero.City)
                .OrderByDescending(group => group.Sum(hero => hero.CompletedMissionCount))
                .ThenBy(group => group.Key)
                .Select(group => group.Key)
                .FirstOrDefault() ?? "N/A";

        var highestOpenThreat = missions
            .Where(mission => mission.IsOpen)
            .Select(mission => (ThreatLevel?)mission.ThreatLevel)
            .OrderByDescending(level => level)
            .FirstOrDefault();

        var heroesByRank = Enum.GetValues<MembershipRank>()
            .ToDictionary(rank => rank, rank => heroes.Count(hero => hero.Rank == rank));

        var missionsByThreat = Enum.GetValues<ThreatLevel>()
            .ToDictionary(level => level, level => missions.Count(mission => mission.ThreatLevel == level));

        return new LeagueDashboard(
            TotalHeroes: heroes.Count,
            AvailableHeroes: heroes.Count(hero => hero.IsAvailable),
            OpenMissions: missions.Count(mission => mission.IsOpen),
            CompletedMissions: missions.Count(mission => mission.Status == MissionStatus.Completed),
            ActiveAssignments: assignments.Count,
            AveragePowerLevel: heroes.Count == 0 ? 0m : decimal.Round((decimal)heroes.Average(hero => hero.PowerLevel), 2),
            MostActiveCity: mostActiveCity,
            HighestOpenThreat: highestOpenThreat,
            HeroesByRank: heroesByRank,
            MissionsByThreat: missionsByThreat);
    }
}