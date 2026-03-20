using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Interfaces;
using DCSuperHeroes.Core.Models;

namespace DCSuperHeroes.Tests.Support;

public sealed class InMemoryHeroRepository : InMemoryRepositoryBase<Hero>, IHeroRepository
{
    public Task<IReadOnlyList<Hero>> SearchAsync(HeroSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        IEnumerable<Hero> query = Items;

        if (!string.IsNullOrWhiteSpace(criteria.SearchText))
        {
            query = query.Where(hero => hero.Alias.Contains(criteria.SearchText, StringComparison.OrdinalIgnoreCase));
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

        return Task.FromResult<IReadOnlyList<Hero>>(query.ToList());
    }

    public Task<IReadOnlyList<Hero>> GetAvailableAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Hero>>(Items.Where(hero => hero.IsAvailable).ToList());
}