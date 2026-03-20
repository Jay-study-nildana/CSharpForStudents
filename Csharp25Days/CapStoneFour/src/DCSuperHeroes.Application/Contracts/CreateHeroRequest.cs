using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Application.Contracts;

public sealed record CreateHeroRequest(
    string Alias,
    string RealName,
    string City,
    MembershipRank Rank,
    HeroArchetype Archetype,
    int PowerLevel,
    int Intelligence,
    int Teamwork,
    string SpecialtyDetail);