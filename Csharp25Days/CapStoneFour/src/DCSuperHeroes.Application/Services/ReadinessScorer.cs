using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Models;

namespace DCSuperHeroes.Application.Services;

public static class ReadinessScorer
{
    public static TeamRecommendation CreateRecommendation(Hero hero, Mission mission)
    {
        var score = hero.CalculateMissionReadiness(mission);
        var reason = $"{hero.DescribeSpecialty()} | City: {hero.City} | Available: {(hero.IsAvailable ? "Yes" : "No")}";

        return new TeamRecommendation(hero.Id, hero.Alias, score, reason);
    }
}