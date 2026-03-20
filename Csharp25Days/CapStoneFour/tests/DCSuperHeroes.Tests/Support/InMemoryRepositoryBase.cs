using DCSuperHeroes.Core.Common;
using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Tests.Support;

public abstract class InMemoryRepositoryBase<T> : IRepository<T> where T : BaseEntity
{
    protected readonly List<T> Items = [];

    public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<T>>(Items.ToList());

    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(item => item.Id == id));

    public Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var index = Items.FindIndex(item => item.Id == entity.Id);
        if (index >= 0)
        {
            Items[index] = entity;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Items.RemoveAll(item => item.Id == id);
        return Task.CompletedTask;
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.Count);
}