using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Tests.Support;

public sealed class InMemoryAssignmentRepository : InMemoryRepositoryBase<MissionAssignment>, IAssignmentRepository
{
    public Task<IReadOnlyList<MissionAssignment>> GetByMissionIdAsync(Guid missionId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MissionAssignment>>(Items.Where(assignment => assignment.MissionId == missionId).ToList());

    public Task<IReadOnlyList<MissionAssignment>> GetByHeroIdAsync(Guid heroId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MissionAssignment>>(Items.Where(assignment => assignment.HeroId == heroId).ToList());
}