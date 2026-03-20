using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Models;

public sealed record HeroSearchCriteria(
    string? SearchText = null,
    string? City = null,
    MembershipRank? Rank = null,
    HeroArchetype? Archetype = null,
    bool OnlyAvailable = false,
    int? MinimumPowerLevel = null);