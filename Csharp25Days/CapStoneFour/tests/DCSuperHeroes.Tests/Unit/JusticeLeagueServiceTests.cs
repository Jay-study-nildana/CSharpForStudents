using DCSuperHeroes.Application.Contracts;
using DCSuperHeroes.Application.Services;
using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.Models;
using DCSuperHeroes.Core.ValueObjects;
using DCSuperHeroes.Tests.Support;

namespace DCSuperHeroes.Tests.Unit;

public sealed class JusticeLeagueServiceTests
{
    [Fact]
    public async Task RegisterHeroAsync_CreatesRequestedHeroType()
    {
        var service = CreateService(out var heroes, out _, out _, out _);

        var request = new CreateHeroRequest(
            Alias: "Green Lantern",
            RealName: "John Stewart",
            City: "Coast City",
            Rank: MembershipRank.LeagueMember,
            Archetype: HeroArchetype.Metahuman,
            PowerLevel: 88,
            Intelligence: 82,
            Teamwork: 91,
            SpecialtyDetail: "Power ring constructs");

        var hero = await service.RegisterHeroAsync(request);

        Assert.IsType<MetahumanHero>(hero);
        Assert.Single(await heroes.GetAllAsync());
    }

    [Fact]
    public async Task AssignHeroAsync_MarksHeroUnavailable_AndMissionReadyWhenFull()
    {
        var service = CreateService(out var heroes, out var missions, out var assignments, out _);
        var hero = new TechHero
        {
            Alias = "Batman",
            RealName = "Bruce Wayne",
            City = "Gotham",
            Rank = MembershipRank.Founder,
            PowerLevel = 80,
            Intelligence = 99,
            Teamwork = 90,
            PrimaryGear = "Utility belt"
        };
        var mission = new Mission
        {
            CodeName = "Silent Knight",
            Location = "Gotham",
            ThreatLevel = ThreatLevel.High,
            RequiredTeamSize = 1,
            RequiresStealth = true,
            Window = new MissionWindow(DateTime.UtcNow, 2)
        };

        await heroes.AddAsync(hero);
        await missions.AddAsync(mission);

        await service.AssignHeroAsync(new AssignmentRequest(mission.Id, hero.Id, SupportRole.Strategist));

        var storedHero = await heroes.GetByIdAsync(hero.Id);
        var storedMission = await missions.GetByIdAsync(mission.Id);
        Assert.False(storedHero!.IsAvailable);
        Assert.Equal(MissionStatus.Ready, storedMission!.Status);
        Assert.Single(await assignments.GetByMissionIdAsync(mission.Id));
    }

    [Fact]
    public async Task RecommendTeamAsync_PrefersMysticHeroForMysticMission()
    {
        var service = CreateService(out var heroes, out var missions, out _, out _);
        var tech = new TechHero
        {
            Alias = "Batman",
            RealName = "Bruce Wayne",
            City = "Gotham",
            Rank = MembershipRank.Founder,
            PowerLevel = 90,
            Intelligence = 90,
            Teamwork = 90,
            PrimaryGear = "Suit"
        };
        var mystic = new MysticHero
        {
            Alias = "Zatanna",
            RealName = "Zatanna Zatara",
            City = "San Francisco",
            Rank = MembershipRank.LeagueMember,
            PowerLevel = 90,
            Intelligence = 90,
            Teamwork = 90,
            ArtifactName = "Spellbook"
        };
        var mission = new Mission
        {
            CodeName = "Arcane Rift",
            Location = "The Watchtower",
            ThreatLevel = ThreatLevel.High,
            RequiredTeamSize = 2,
            RequiresMysticSupport = true,
            Window = new MissionWindow(DateTime.UtcNow, 3)
        };

        await heroes.AddAsync(tech);
        await heroes.AddAsync(mystic);
        await missions.AddAsync(mission);

        var recommendations = await service.RecommendTeamAsync(mission.Id, 2);

        Assert.Equal("Zatanna", recommendations[0].Alias);
    }

    [Fact]
    public async Task CompleteMissionAsync_RestoresAvailabilityAndTracksCompletion()
    {
        var service = CreateService(out var heroes, out var missions, out _, out _);
        var hero = new MetahumanHero
        {
            Alias = "Superman",
            RealName = "Clark Kent",
            City = "Metropolis",
            Rank = MembershipRank.Founder,
            PowerLevel = 100,
            Intelligence = 88,
            Teamwork = 95,
            SignaturePower = "Flight"
        };
        var mission = new Mission
        {
            CodeName = "Doomsday Drill",
            Location = "Metropolis",
            ThreatLevel = ThreatLevel.Crisis,
            RequiredTeamSize = 1,
            Window = new MissionWindow(DateTime.UtcNow, 2)
        };

        await heroes.AddAsync(hero);
        await missions.AddAsync(mission);
        await service.AssignHeroAsync(new AssignmentRequest(mission.Id, hero.Id, SupportRole.Powerhouse));
        await service.LaunchMissionAsync(mission.Id);

        var completed = await service.CompleteMissionAsync(mission.Id, "Threat neutralized.");
        var updatedHero = await heroes.GetByIdAsync(hero.Id);

        Assert.True(updatedHero!.IsAvailable);
        Assert.Equal(1, updatedHero.CompletedMissionCount);
        Assert.Equal(MissionStatus.Completed, completed.Status);
    }

    [Fact]
    public async Task GetDashboardAsync_AggregatesLeagueMetrics()
    {
        var service = CreateService(out var heroes, out var missions, out _, out _);
        await heroes.AddAsync(new TechHero
        {
            Alias = "Batman",
            RealName = "Bruce Wayne",
            City = "Gotham",
            Rank = MembershipRank.Founder,
            PowerLevel = 80,
            Intelligence = 99,
            Teamwork = 90,
            PrimaryGear = "Gear",
            CompletedMissionCount = 10
        });
        await heroes.AddAsync(new MysticHero
        {
            Alias = "Zatanna",
            RealName = "Zatanna Zatara",
            City = "Gotham",
            Rank = MembershipRank.LeagueMember,
            PowerLevel = 89,
            Intelligence = 85,
            Teamwork = 83,
            ArtifactName = "Grimoire"
        });
        await missions.AddAsync(new Mission
        {
            CodeName = "Rook",
            Location = "Gotham",
            ThreatLevel = ThreatLevel.High,
            RequiredTeamSize = 2,
            Window = new MissionWindow(DateTime.UtcNow, 1)
        });

        var dashboard = await service.GetDashboardAsync();

        Assert.Equal(2, dashboard.TotalHeroes);
        Assert.Equal("Gotham", dashboard.MostActiveCity);
        Assert.Equal(ThreatLevel.High, dashboard.HighestOpenThreat);
    }

    private static JusticeLeagueService CreateService(
        out InMemoryHeroRepository heroes,
        out InMemoryMissionRepository missions,
        out InMemoryAssignmentRepository assignments,
        out TestLeagueLogger logger)
    {
        heroes = new InMemoryHeroRepository();
        missions = new InMemoryMissionRepository();
        assignments = new InMemoryAssignmentRepository();
        logger = new TestLeagueLogger();

        return new JusticeLeagueService(heroes, missions, assignments, logger);
    }
}