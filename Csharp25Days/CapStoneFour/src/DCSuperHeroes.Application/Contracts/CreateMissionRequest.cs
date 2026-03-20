using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Application.Contracts;

public sealed record CreateMissionRequest(
    string CodeName,
    string Location,
    ThreatLevel ThreatLevel,
    int RequiredTeamSize,
    bool RequiresMysticSupport,
    bool RequiresStealth,
    DateTime StartsAtUtc,
    int DurationHours,
    string Notes);