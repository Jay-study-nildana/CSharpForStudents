using System.Text.Json.Serialization;
using DCSuperHeroes.Core.Common;
using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(MetahumanHero), "metahuman")]
[JsonDerivedType(typeof(TechHero), "tech")]
[JsonDerivedType(typeof(MysticHero), "mystic")]
public abstract class Hero : BaseEntity
{
    public string Alias { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public MembershipRank Rank { get; set; } = MembershipRank.Reserve;
    public int PowerLevel { get; set; }
    public int Intelligence { get; set; }
    public int Teamwork { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int CompletedMissionCount { get; set; }

    [JsonIgnore]
    public abstract HeroArchetype Archetype { get; }

    public decimal CalculateMissionReadiness(Mission mission)
    {
        var score = ComputeBaseReadiness();
        score += ComputeThreatBonus(mission.ThreatLevel);
        score += GetSpecialtyBonus(mission);
        score += CompletedMissionCount * 0.75m;
        score += IsAvailable ? 5m : -35m;

        return decimal.Clamp(Math.Round(score, 2), 0m, 100m);
    }

    public virtual string DescribeSpecialty() => Archetype.ToString();

    protected decimal ComputeBaseReadiness() =>
        (PowerLevel * 0.35m) +
        (Intelligence * 0.25m) +
        (Teamwork * 0.20m);

    protected virtual decimal ComputeThreatBonus(ThreatLevel level) => level switch
    {
        ThreatLevel.Low => 2m,
        ThreatLevel.Medium => 4m,
        ThreatLevel.High => 6m,
        ThreatLevel.Crisis => 8m,
        _ => 0m
    };

    protected abstract decimal GetSpecialtyBonus(Mission mission);
}