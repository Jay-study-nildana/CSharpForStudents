using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Entities;

public sealed class MetahumanHero : Hero
{
    public string SignaturePower { get; set; } = string.Empty;

    public override HeroArchetype Archetype => HeroArchetype.Metahuman;

    public override string DescribeSpecialty() => $"Metahuman power: {SignaturePower}";

    protected override decimal GetSpecialtyBonus(Mission mission)
    {
        var bonus = mission.ThreatLevel switch
        {
            ThreatLevel.High => 8m,
            ThreatLevel.Crisis => 12m,
            _ => 4m
        };

        if (!mission.RequiresStealth)
        {
            bonus += 3m;
        }

        return bonus;
    }
}