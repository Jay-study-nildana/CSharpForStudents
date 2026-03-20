using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Interfaces;

public interface IMissionRepository : IRepository<Mission>
{
    Task<IReadOnlyList<Mission>> GetByStatusAsync(MissionStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Mission>> GetOpenMissionsAsync(CancellationToken cancellationToken = default);
}