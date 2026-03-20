using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Models;

public sealed record MissionBriefing(
    Guid MissionId,
    string CodeName,
    string Location,
    ThreatLevel ThreatLevel,
    MissionStatus Status,
    int RequiredTeamSize,
    int AssignedHeroes,
    string AssignedRoster,
    DateTime StartsAtUtc,
    bool RequiresMysticSupport,
    bool RequiresStealth);