using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Models;

public sealed record LeagueDashboard(
    int TotalHeroes,
    int AvailableHeroes,
    int OpenMissions,
    int CompletedMissions,
    int ActiveAssignments,
    decimal AveragePowerLevel,
    string MostActiveCity,
    ThreatLevel? HighestOpenThreat,
    IReadOnlyDictionary<MembershipRank, int> HeroesByRank,
    IReadOnlyDictionary<ThreatLevel, int> MissionsByThreat);