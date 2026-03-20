using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Tests.Support;

public sealed class InMemoryMissionRepository : InMemoryRepositoryBase<Mission>, IMissionRepository
{
    public Task<IReadOnlyList<Mission>> GetByStatusAsync(MissionStatus status, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Mission>>(Items.Where(mission => mission.Status == status).ToList());

    public Task<IReadOnlyList<Mission>> GetOpenMissionsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Mission>>(Items.Where(mission => mission.IsOpen).ToList());
}