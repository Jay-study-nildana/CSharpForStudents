using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.ValueObjects;

namespace DCSuperHeroes.Tests.Unit;

public sealed class HeroReadinessTests
{
    [Fact]
    public void TechHero_GetsBonusOnStealthMission()
    {
        var techHero = new TechHero
        {
            Alias = "Batman",
            PowerLevel = 90,
            Intelligence = 90,
            Teamwork = 90,
            City = "Gotham",
            PrimaryGear = "Suit"
        };
        var metahuman = new MetahumanHero
        {
            Alias = "Superman",
            PowerLevel = 90,
            Intelligence = 90,
            Teamwork = 90,
            City = "Metropolis",
            SignaturePower = "Strength"
        };
        var mission = new Mission
        {
            CodeName = "Silent Wing",
            Location = "Gotham",
            ThreatLevel = ThreatLevel.High,
            RequiresStealth = true,
            Window = new MissionWindow(DateTime.UtcNow, 1)
        };

        Assert.True(techHero.CalculateMissionReadiness(mission) > metahuman.CalculateMissionReadiness(mission));
    }

    [Fact]
    public void MysticHero_GetsBonusOnMysticMission()
    {
        var mystic = new MysticHero
        {
            Alias = "Zatanna",
            PowerLevel = 80,
            Intelligence = 85,
            Teamwork = 82,
            ArtifactName = "Spellbook"
        };
        var tech = new TechHero
        {
            Alias = "Batman",
            PowerLevel = 80,
            Intelligence = 85,
            Teamwork = 82,
            PrimaryGear = "Suit"
        };
        var mission = new Mission
        {
            CodeName = "Occult Storm",
            Location = "The Watchtower",
            ThreatLevel = ThreatLevel.High,
            RequiresMysticSupport = true,
            Window = new MissionWindow(DateTime.UtcNow, 1)
        };

        Assert.True(mystic.CalculateMissionReadiness(mission) > tech.CalculateMissionReadiness(mission));
    }

    [Fact]
    public void UnavailableHero_IsPenalized()
    {
        var hero = new MetahumanHero
        {
            Alias = "The Flash",
            PowerLevel = 92,
            Intelligence = 84,
            Teamwork = 88,
            SignaturePower = "Speed Force",
            IsAvailable = false
        };
        var mission = new Mission
        {
            CodeName = "Red Shift",
            Location = "Central City",
            ThreatLevel = ThreatLevel.Medium,
            Window = new MissionWindow(DateTime.UtcNow, 1)
        };

        Assert.True(hero.CalculateMissionReadiness(mission) < 70m);
    }
}