using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Entities;

public sealed class TechHero : Hero
{
    public string PrimaryGear { get; set; } = string.Empty;

    public override HeroArchetype Archetype => HeroArchetype.Tech;

    public override string DescribeSpecialty() => $"Tech specialist: {PrimaryGear}";

    protected override decimal GetSpecialtyBonus(Mission mission)
    {
        var bonus = mission.RequiresStealth ? 10m : 4m;

        if (mission.Location.Contains("Gotham", StringComparison.OrdinalIgnoreCase) ||
            mission.Location.Contains("Watchtower", StringComparison.OrdinalIgnoreCase))
        {
            bonus += 4m;
        }

        return bonus;
    }
}