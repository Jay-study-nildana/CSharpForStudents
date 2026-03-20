using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Infrastructure.Persistence;

namespace DCSuperHeroes.Tests.Integration;

public sealed class JsonRepositoryTests : IDisposable
{
    private readonly string _tempDirectory;

    public JsonRepositoryTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"dc-heroes-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDirectory);
    }

    [Fact]
    public async Task HeroRepository_RoundTripsPolymorphicHero()
    {
        var repository = new HeroRepository(_tempDirectory);
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

        await repository.AddAsync(hero);

        var loaded = await repository.GetByIdAsync(hero.Id);

        Assert.NotNull(loaded);
        Assert.IsType<TechHero>(loaded);
    }

    [Fact]
    public async Task MissionRepository_UpdatePersistsChanges()
    {
        var repository = new MissionRepository(_tempDirectory);
        var mission = new Mission
        {
            CodeName = "Black Dawn",
            Location = "Gotham",
            ThreatLevel = ThreatLevel.High,
            RequiredTeamSize = 2
        };

        await repository.AddAsync(mission);
        mission.Status = MissionStatus.Ready;
        await repository.UpdateAsync(mission);

        var loaded = await repository.GetByIdAsync(mission.Id);
        Assert.Equal(MissionStatus.Ready, loaded!.Status);
    }

    [Fact]
    public async Task AssignmentRepository_DeleteRemovesEntity()
    {
        var repository = new AssignmentRepository(_tempDirectory);
        var assignment = new MissionAssignment
        {
            MissionId = Guid.NewGuid(),
            HeroId = Guid.NewGuid(),
            Role = SupportRole.Leader,
            AssignedReadinessScore = 88m
        };

        await repository.AddAsync(assignment);
        await repository.DeleteAsync(assignment.Id);

        Assert.Equal(0, await repository.CountAsync());
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, true);
        }
    }
}