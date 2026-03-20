using DCSuperHeroes.Application.Contracts;
using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Events;
using DCSuperHeroes.Core.Models;

namespace DCSuperHeroes.Application.Interfaces;

public interface IJusticeLeagueService
{
    event EventHandler<MissionAssignedEventArgs>? MissionAssigned;
    event EventHandler<MissionCompletedEventArgs>? MissionCompleted;

    Task SeedSampleDataAsync(CancellationToken cancellationToken = default);
    Task<Hero> RegisterHeroAsync(CreateHeroRequest request, CancellationToken cancellationToken = default);
    Task<Mission> CreateMissionAsync(CreateMissionRequest request, CancellationToken cancellationToken = default);
    Task<MissionAssignment> AssignHeroAsync(AssignmentRequest request, CancellationToken cancellationToken = default);
    Task<Mission> LaunchMissionAsync(Guid missionId, CancellationToken cancellationToken = default);
    Task<Mission> CompleteMissionAsync(Guid missionId, string outcomeSummary, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Hero>> GetHeroesAsync(HeroSearchCriteria criteria, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Hero>> GetTopHeroesAsync(int count, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MissionBriefing>> GetMissionBriefingsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamRecommendation>> RecommendTeamAsync(Guid missionId, int count, CancellationToken cancellationToken = default);
    Task<LeagueDashboard> GetDashboardAsync(CancellationToken cancellationToken = default);
}