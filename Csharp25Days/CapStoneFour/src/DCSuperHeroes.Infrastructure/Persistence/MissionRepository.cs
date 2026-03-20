using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Infrastructure.Persistence;

public sealed class MissionRepository : JsonRepository<Mission>, IMissionRepository
{
    public MissionRepository(string dataDirectory)
        : base(dataDirectory, "missions.json")
    {
    }

    public async Task<IReadOnlyList<Mission>> GetByStatusAsync(MissionStatus status, CancellationToken cancellationToken = default)
    {
        var missions = await GetAllAsync(cancellationToken);
        return missions.Where(mission => mission.Status == status).OrderBy(mission => mission.Window.StartsAtUtc).ToList();
    }

    public async Task<IReadOnlyList<Mission>> GetOpenMissionsAsync(CancellationToken cancellationToken = default)
    {
        var missions = await GetAllAsync(cancellationToken);
        return missions.Where(mission => mission.IsOpen).OrderByDescending(mission => mission.ThreatLevel).ToList();
    }
}