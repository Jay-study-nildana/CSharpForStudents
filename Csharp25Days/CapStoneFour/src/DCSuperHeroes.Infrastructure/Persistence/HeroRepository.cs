using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Interfaces;
using DCSuperHeroes.Core.Models;

namespace DCSuperHeroes.Infrastructure.Persistence;

public sealed class HeroRepository : JsonRepository<Hero>, IHeroRepository
{
    public HeroRepository(string dataDirectory)
        : base(dataDirectory, "heroes.json")
    {
    }

    public async Task<IReadOnlyList<Hero>> SearchAsync(HeroSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        var heroes = await GetAllAsync(cancellationToken);
        IEnumerable<Hero> query = heroes;

        if (!string.IsNullOrWhiteSpace(criteria.SearchText))
        {
            query = query.Where(hero =>
                hero.Alias.Contains(criteria.SearchText, StringComparison.OrdinalIgnoreCase) ||
                hero.RealName.Contains(criteria.SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(criteria.City))
        {
            query = query.Where(hero => hero.City.Equals(criteria.City, StringComparison.OrdinalIgnoreCase));
        }

        if (criteria.Rank is not null)
        {
            query = query.Where(hero => hero.Rank == criteria.Rank);
        }

        if (criteria.Archetype is not null)
        {
            query = query.Where(hero => hero.Archetype == criteria.Archetype);
        }

        if (criteria.OnlyAvailable)
        {
            query = query.Where(hero => hero.IsAvailable);
        }

        if (criteria.MinimumPowerLevel is not null)
        {
            query = query.Where(hero => hero.PowerLevel >= criteria.MinimumPowerLevel);
        }

        return query
            .OrderByDescending(hero => hero.CompletedMissionCount)
            .ThenBy(hero => hero.Alias)
            .ToList();
    }

    public async Task<IReadOnlyList<Hero>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        var heroes = await GetAllAsync(cancellationToken);
        return heroes.Where(hero => hero.IsAvailable).OrderBy(hero => hero.Alias).ToList();
    }
}