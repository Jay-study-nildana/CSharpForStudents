namespace DCSuperHeroes.Core.Models;

public sealed record TeamRecommendation(
    Guid HeroId,
    string Alias,
    decimal ReadinessScore,
    string Reason);