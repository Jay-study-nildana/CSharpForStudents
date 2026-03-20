using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Infrastructure.Persistence;

public sealed class AssignmentRepository : JsonRepository<MissionAssignment>, IAssignmentRepository
{
    public AssignmentRepository(string dataDirectory)
        : base(dataDirectory, "assignments.json")
    {
    }

    public async Task<IReadOnlyList<MissionAssignment>> GetByMissionIdAsync(Guid missionId, CancellationToken cancellationToken = default)
    {
        var assignments = await GetAllAsync(cancellationToken);
        return assignments.Where(assignment => assignment.MissionId == missionId).OrderBy(assignment => assignment.AssignedAtUtc).ToList();
    }

    public async Task<IReadOnlyList<MissionAssignment>> GetByHeroIdAsync(Guid heroId, CancellationToken cancellationToken = default)
    {
        var assignments = await GetAllAsync(cancellationToken);
        return assignments.Where(assignment => assignment.HeroId == heroId).OrderByDescending(assignment => assignment.AssignedAtUtc).ToList();
    }
}