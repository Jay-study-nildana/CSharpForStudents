using DCSuperHeroes.Core.Entities;
using DCSuperHeroes.Core.Models;

namespace DCSuperHeroes.Core.Interfaces;

public interface IHeroRepository : IRepository<Hero>
{
    Task<IReadOnlyList<Hero>> SearchAsync(HeroSearchCriteria criteria, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Hero>> GetAvailableAsync(CancellationToken cancellationToken = default);
}