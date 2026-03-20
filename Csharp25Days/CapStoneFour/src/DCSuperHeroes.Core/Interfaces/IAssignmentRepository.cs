using DCSuperHeroes.Core.Entities;

namespace DCSuperHeroes.Core.Interfaces;

public interface IAssignmentRepository : IRepository<MissionAssignment>
{
    Task<IReadOnlyList<MissionAssignment>> GetByMissionIdAsync(Guid missionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MissionAssignment>> GetByHeroIdAsync(Guid heroId, CancellationToken cancellationToken = default);
}