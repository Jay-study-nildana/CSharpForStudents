using System;
using System.Collections.Generic;

class AbstractRepository_Pattern
{
    // Problem: abstract Repository<T> and a concrete in-memory implementation
    public abstract class Repository<T>
    {
        public abstract T Get(int id);
        public abstract void Save(int id, T item);
    }

    public class InMemoryRepository<T> : Repository<T>
    {
        private readonly Dictionary<int, T> _store = new();
        public override T Get(int id) => _store.TryGetValue(id, out var v) ? v : default;
        public override void Save(int id, T item) => _store[id] = item;
    }

    static void Main()
    {
        var repo = new InMemoryRepository<string>();
        repo.Save(1, "hello");
        Console.WriteLine($"Fetched: {repo.Get(1)}");

        // Abstract base enforces contract; concrete class provides storage.
        // Tests can use InMemoryRepository or another fake implementation.
    }
}