using DCSuperHeroes.Application.Contracts;
using DCSuperHeroes.Application.Interfaces;
using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.Events;
using DCSuperHeroes.Core.Exceptions;
using DCSuperHeroes.Core.Interfaces;
using DCSuperHeroes.Core.Models;
using DCSuperHeroes.Core.ValueObjects;

namespace DCSuperHeroes.Application.Services;

public sealed class JusticeLeagueService : IJusticeLeagueService
{
    private readonly IHeroRepository _heroRepository;
    private readonly IMissionRepository _missionRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ILeagueLogger _logger;
    private readonly SemaphoreSlim _coordinationLock = new(1, 1);

    public JusticeLeagueService(
        IHeroRepository heroRepository,
        IMissionRepository missionRepository,
        IAssignmentRepository assignmentRepository,
        ILeagueLogger logger)
    {
        _heroRepository = heroRepository;
        _missionRepository = missionRepository;
        _assignmentRepository = assignmentRepository;
        _logger = logger;
    }

    public event EventHandler<MissionAssignedEventArgs>? MissionAssigned;
    public event EventHandler<MissionCompletedEventArgs>? MissionCompleted;

    public async Task SeedSampleDataAsync(CancellationToken cancellationToken = default)
    {
        if (await _heroRepository.CountAsync(cancellationToken) > 0)
        {
            return;
        }

        var heroes = new Hero[]
        {
            new TechHero
            {
                Alias = "Batman",
                RealName = "Bruce Wayne",
                City = "Gotham",
                Rank = MembershipRank.Founder,
                PowerLevel = 80,
                Intelligence = 99,
                Teamwork = 90,
                PrimaryGear = "Batcomputer-linked tactical gear",
                CompletedMissionCount = 18
            },
            new MetahumanHero
            {
                Alias = "Superman",
                RealName = "Clark Kent",
                City = "Metropolis",
                Rank = MembershipRank.Founder,
                PowerLevel = 100,
                Intelligence = 88,
                Teamwork = 95,
                SignaturePower = "Solar-charged strength",
                CompletedMissionCount = 22
            },
            new MysticHero
            {
                Alias = "Wonder Woman",
                RealName = "Diana Prince",
                City = "Washington",
                Rank = MembershipRank.Founder,
                PowerLevel = 95,
                Intelligence = 86,
                Teamwork = 94,
                ArtifactName = "Lasso of Truth",
                CompletedMissionCount = 20
            },
            new MetahumanHero
            {
                Alias = "The Flash",
                RealName = "Barry Allen",
                City = "Central City",
                Rank = MembershipRank.LeagueMember,
                PowerLevel = 92,
                Intelligence = 84,
                Teamwork = 88,
                SignaturePower = "Speed Force manipulation",
                CompletedMissionCount = 14
            },
            new MysticHero
            {
                Alias = "Zatanna",
                RealName = "Zatanna Zatara",
                City = "San Francisco",
                Rank = MembershipRank.LeagueMember,
                PowerLevel = 89,
                Intelligence = 85,
                Teamwork = 83,
                ArtifactName = "Backward spellcasting grimoire",
                CompletedMissionCount = 11
            }
        };

        foreach (var hero in heroes)
        {
            await _heroRepository.AddAsync(hero, cancellationToken);
        }

        var missions = new[]
        {
            new Mission
            {
                CodeName = "Apokolips Shadow",
                Location = "Metropolis",
                ThreatLevel = ThreatLevel.Crisis,
                RequiredTeamSize = 3,
                RequiresMysticSupport = false,
                RequiresStealth = false,
                Window = new MissionWindow(DateTime.UtcNow.AddDays(1), 6),
                Notes = "Track a boom tube energy surge near S.T.A.R. Labs."
            },
            new Mission
            {
                CodeName = "Gotham Whisper",
                Location = "Gotham",
                ThreatLevel = ThreatLevel.High,
                RequiredTeamSize = 2,
                RequiresMysticSupport = false,
                RequiresStealth = true,
                Window = new MissionWindow(DateTime.UtcNow.AddDays(2), 4),
                Notes = "Investigate Court of Owls movement under the Narrows."
            },
            new Mission
            {
                CodeName = "Oblivion Sigil",
                Location = "The Watchtower",
                ThreatLevel = ThreatLevel.High,
                RequiredTeamSize = 2,
                RequiresMysticSupport = true,
                RequiresStealth = false,
                Window = new MissionWindow(DateTime.UtcNow.AddDays(3), 5),
                Notes = "Contain a magical breach radiating from a relic vault."
            }
        };

        foreach (var mission in missions)
        {
            await _missionRepository.AddAsync(mission, cancellationToken);
        }

        await _logger.LogInfoAsync("Seeded DC Comics sample data.", cancellationToken);
    }

    public async Task<Hero> RegisterHeroAsync(CreateHeroRequest request, CancellationToken cancellationToken = default)
    {
        ValidateHeroRequest(request);

        Hero hero = request.Archetype switch
        {
            HeroArchetype.Metahuman => new MetahumanHero { SignaturePower = request.SpecialtyDetail },
            HeroArchetype.Tech => new TechHero { PrimaryGear = request.SpecialtyDetail },
            HeroArchetype.Mystic => new MysticHero { ArtifactName = request.SpecialtyDetail },
            _ => throw new ValidationException("Unsupported hero archetype.")
        };

        hero.Alias = request.Alias.Trim();
        hero.RealName = request.RealName.Trim();
        hero.City = request.City.Trim();
        hero.Rank = request.Rank;
        hero.PowerLevel = request.PowerLevel;
        hero.Intelligence = request.Intelligence;
        hero.Teamwork = request.Teamwork;
        hero.IsAvailable = true;

        await _heroRepository.AddAsync(hero, cancellationToken);
        await _logger.LogInfoAsync($"Registered hero {hero.Alias}.", cancellationToken);

        return hero;
    }

    public async Task<Mission> CreateMissionAsync(CreateMissionRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.CodeName))
        {
            throw new ValidationException("Mission code name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Location))
        {
            throw new ValidationException("Mission location is required.");
        }

        if (request.RequiredTeamSize <= 0)
        {
            throw new ValidationException("Required team size must be greater than zero.");
        }

        if (request.DurationHours <= 0)
        {
            throw new ValidationException("Mission duration must be greater than zero.");
        }

        var mission = new Mission
        {
            CodeName = request.CodeName.Trim(),
            Location = request.Location.Trim(),
            ThreatLevel = request.ThreatLevel,
            RequiredTeamSize = request.RequiredTeamSize,
            RequiresMysticSupport = request.RequiresMysticSupport,
            RequiresStealth = request.RequiresStealth,
            Window = new MissionWindow(request.StartsAtUtc, request.DurationHours),
            Notes = request.Notes.Trim()
        };

        await _missionRepository.AddAsync(mission, cancellationToken);
        await _logger.LogInfoAsync($"Created mission {mission.CodeName}.", cancellationToken);

        return mission;
    }

    public async Task<MissionAssignment> AssignHeroAsync(AssignmentRequest request, CancellationToken cancellationToken = default)
    {
        await _coordinationLock.WaitAsync(cancellationToken);

        try
        {
            var mission = await GetRequiredMissionAsync(request.MissionId, cancellationToken);
            var hero = await GetRequiredHeroAsync(request.HeroId, cancellationToken);
            var assignments = await _assignmentRepository.GetByMissionIdAsync(request.MissionId, cancellationToken);

            if (!mission.IsOpen)
            {
                throw new RosterConflictException("You can only assign heroes to open missions.");
            }

            if (!hero.IsAvailable)
            {
                throw new RosterConflictException($"{hero.Alias} is currently unavailable.");
            }

            if (assignments.Any(assignment => assignment.HeroId == hero.Id))
            {
                throw new RosterConflictException($"{hero.Alias} is already assigned to this mission.");
            }

            if (assignments.Count >= mission.RequiredTeamSize)
            {
                throw new RosterConflictException("Mission roster is already full.");
            }

            var readiness = hero.CalculateMissionReadiness(mission);
            var assignment = new MissionAssignment
            {
                MissionId = mission.Id,
                HeroId = hero.Id,
                Role = request.Role,
                AssignedReadinessScore = readiness
            };

            hero.IsAvailable = false;
            mission.UpdateStatusFromAssignments(assignments.Count + 1);

            await _assignmentRepository.AddAsync(assignment, cancellationToken);
            await _heroRepository.UpdateAsync(hero, cancellationToken);
            await _missionRepository.UpdateAsync(mission, cancellationToken);
            await _logger.LogInfoAsync($"Assigned {hero.Alias} to mission {mission.CodeName} as {request.Role}.", cancellationToken);

            MissionAssigned?.Invoke(this, new MissionAssignedEventArgs(mission, hero, assignment));
            return assignment;
        }
        finally
        {
            _coordinationLock.Release();
        }
    }

    public async Task<Mission> LaunchMissionAsync(Guid missionId, CancellationToken cancellationToken = default)
    {
        var mission = await GetRequiredMissionAsync(missionId, cancellationToken);
        var assignments = await _assignmentRepository.GetByMissionIdAsync(missionId, cancellationToken);

        if (assignments.Count < mission.RequiredTeamSize)
        {
            throw new RosterConflictException("Mission needs a full team before launch.");
        }

        mission.Status = MissionStatus.Active;
        await _missionRepository.UpdateAsync(mission, cancellationToken);
        await _logger.LogInfoAsync($"Launched mission {mission.CodeName}.", cancellationToken);

        return mission;
    }

    public async Task<Mission> CompleteMissionAsync(Guid missionId, string outcomeSummary, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(outcomeSummary))
        {
            throw new ValidationException("Outcome summary is required.");
        }

        await _coordinationLock.WaitAsync(cancellationToken);

        try
        {
            var mission = await GetRequiredMissionAsync(missionId, cancellationToken);
            var assignments = await _assignmentRepository.GetByMissionIdAsync(missionId, cancellationToken);

            if (assignments.Count == 0)
            {
                throw new RosterConflictException("Cannot complete a mission that has no assigned heroes.");
            }

            var heroes = new List<Hero>();
            foreach (var assignment in assignments)
            {
                var hero = await GetRequiredHeroAsync(assignment.HeroId, cancellationToken);
                hero.IsAvailable = true;
                hero.CompletedMissionCount++;
                await _heroRepository.UpdateAsync(hero, cancellationToken);
                heroes.Add(hero);
            }

            mission.Status = MissionStatus.Completed;
            mission.OutcomeSummary = outcomeSummary.Trim();
            await _missionRepository.UpdateAsync(mission, cancellationToken);
            await _logger.LogInfoAsync($"Completed mission {mission.CodeName}.", cancellationToken);

            MissionCompleted?.Invoke(this, new MissionCompletedEventArgs(mission, heroes));
            return mission;
        }
        finally
        {
            _coordinationLock.Release();
        }
    }

    public Task<IReadOnlyList<Hero>> GetHeroesAsync(HeroSearchCriteria criteria, CancellationToken cancellationToken = default) =>
        _heroRepository.SearchAsync(criteria, cancellationToken);

    public async Task<IReadOnlyList<Hero>> GetTopHeroesAsync(int count, CancellationToken cancellationToken = default)
    {
        var heroes = await _heroRepository.GetAllAsync(cancellationToken);

        return heroes
            .OrderByDescending(hero => hero.CompletedMissionCount)
            .ThenByDescending(hero => hero.PowerLevel)
            .ThenBy(hero => hero.Alias)
            .Take(Math.Max(count, 1))
            .ToList();
    }

    public async Task<IReadOnlyList<MissionBriefing>> GetMissionBriefingsAsync(CancellationToken cancellationToken = default)
    {
        var missions = await _missionRepository.GetAllAsync(cancellationToken);
        var assignments = await _assignmentRepository.GetAllAsync(cancellationToken);
        var heroes = await _heroRepository.GetAllAsync(cancellationToken);

        var heroLookup = heroes.ToDictionary(hero => hero.Id, hero => hero.Alias);

        return missions
            .OrderBy(mission => mission.Window.StartsAtUtc)
            .ThenByDescending(mission => mission.ThreatLevel)
            .Select(mission =>
            {
                var roster = assignments
                    .Where(assignment => assignment.MissionId == mission.Id)
                    .Select(assignment => heroLookup.GetValueOrDefault(assignment.HeroId, "Unknown"))
                    .OrderBy(alias => alias)
                    .ToList();

                return new MissionBriefing(
                    MissionId: mission.Id,
                    CodeName: mission.CodeName,
                    Location: mission.Location,
                    ThreatLevel: mission.ThreatLevel,
                    Status: mission.Status,
                    RequiredTeamSize: mission.RequiredTeamSize,
                    AssignedHeroes: roster.Count,
                    AssignedRoster: roster.Count == 0 ? "Unassigned" : string.Join(", ", roster),
                    StartsAtUtc: mission.Window.StartsAtUtc,
                    RequiresMysticSupport: mission.RequiresMysticSupport,
                    RequiresStealth: mission.RequiresStealth);
            })
            .ToList();
    }

    public async Task<IReadOnlyList<TeamRecommendation>> RecommendTeamAsync(Guid missionId, int count, CancellationToken cancellationToken = default)
    {
        var mission = await GetRequiredMissionAsync(missionId, cancellationToken);
        var heroes = await _heroRepository.GetAvailableAsync(cancellationToken);

        return heroes
            .Select(hero => ReadinessScorer.CreateRecommendation(hero, mission))
            .OrderByDescending(recommendation => recommendation.ReadinessScore)
            .ThenBy(recommendation => recommendation.Alias)
            .Take(Math.Max(count, 1))
            .ToList();
    }

    public async Task<LeagueDashboard> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var heroes = await _heroRepository.GetAllAsync(cancellationToken);
        var missions = await _missionRepository.GetAllAsync(cancellationToken);
        var assignments = await _assignmentRepository.GetAllAsync(cancellationToken);

        return LeagueAnalyticsBuilder.Build(heroes, missions, assignments);
    }

    private static void ValidateHeroRequest(CreateHeroRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Alias))
        {
            throw new ValidationException("Hero alias is required.");
        }

        if (string.IsNullOrWhiteSpace(request.RealName))
        {
            throw new ValidationException("Hero real name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.City))
        {
            throw new ValidationException("Hero city is required.");
        }

        if (string.IsNullOrWhiteSpace(request.SpecialtyDetail))
        {
            throw new ValidationException("Hero specialty detail is required.");
        }

        ValidateStat(request.PowerLevel, nameof(request.PowerLevel));
        ValidateStat(request.Intelligence, nameof(request.Intelligence));
        ValidateStat(request.Teamwork, nameof(request.Teamwork));
    }

    private static void ValidateStat(int value, string fieldName)
    {
        if (value is < 1 or > 100)
        {
            throw new ValidationException($"{fieldName} must be between 1 and 100.");
        }
    }

    private async Task<Hero> GetRequiredHeroAsync(Guid heroId, CancellationToken cancellationToken)
    {
        return await _heroRepository.GetByIdAsync(heroId, cancellationToken)
            ?? throw new EntityNotFoundException("Hero", heroId);
    }

    private async Task<Mission> GetRequiredMissionAsync(Guid missionId, CancellationToken cancellationToken)
    {
        return await _missionRepository.GetByIdAsync(missionId, cancellationToken)
            ?? throw new EntityNotFoundException("Mission", missionId);
    }
}