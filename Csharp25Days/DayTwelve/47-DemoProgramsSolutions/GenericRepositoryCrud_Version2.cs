// GenericRepositoryCrud.cs
// Problem: GenericRepositoryCrud
// Simple in-memory repository InMemoryRepository<T, TId> using Dictionary<TId, T>.
// Complexity: O(1) average for add/get/update/remove.

using System;
using System.Collections.Generic;

class InMemoryRepository<T, TId> where TId : notnull
{
    private readonly Dictionary<TId, T> _store = new();
    private readonly Func<T, TId> _idSelector;

    public InMemoryRepository(Func<T, TId> idSelector) => _idSelector = idSelector;

    public void Add(T item) => _store[_idSelector(item)] = item;
    public bool TryGet(TId id, out T? item) => _store.TryGetValue(id, out item);
    public bool Remove(TId id) => _store.Remove(id);
    public void Update(T item) => _store[_idSelector(item)] = item;
    public IEnumerable<T> GetAll() => _store.Values;
}

record TaskItem(Guid Id, string Title);

class ProgramRepo
{
    static void Main()
    {
        var repo = new InMemoryRepository<TaskItem, Guid>(t => t.Id);
        var t = new TaskItem(Guid.NewGuid(), "Write docs");
        repo.Add(t);
        repo.Update(t with { Title = "Write docs + tests" });
        if (repo.TryGet(t.Id, out var got)) Console.WriteLine(got.Title);
        repo.Remove(t.Id);
    }
}