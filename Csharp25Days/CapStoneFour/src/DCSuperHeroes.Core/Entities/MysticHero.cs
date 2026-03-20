using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Entities;

public sealed class MysticHero : Hero
{
    public string ArtifactName { get; set; } = string.Empty;

    public override HeroArchetype Archetype => HeroArchetype.Mystic;

    public override string DescribeSpecialty() => $"Mystic artifact: {ArtifactName}";

    protected override decimal GetSpecialtyBonus(Mission mission)
    {
        var bonus = mission.RequiresMysticSupport ? 14m : 5m;

        if (mission.ThreatLevel == ThreatLevel.Crisis)
        {
            bonus += 3m;
        }

        return bonus;
    }
}