using System;
using System.Collections.Generic;

//
// Problem: Add Decorator for Caching
// Test plan: call GetById multiple times and show underlying repo is only hit once for same id.
// Demonstrates: decorator pattern for cross-cutting caching concern.
//

namespace Day11.RefactorLab
{
    public class Product { public int Id; public string Name = ""; }

    public interface IProductRepository { Product? GetById(int id); }

    public class SlowProductRepository : IProductRepository
    {
        public Product? GetById(int id)
        {
            Console.WriteLine("Underlying repo fetching...");
            // simulate data
            return new Product { Id = id, Name = $"Product-{id}" };
        }
    }

    public class CachingProductRepositoryDecorator : IProductRepository
    {
        private readonly IProductRepository _inner;
        private readonly Dictionary<int, Product> _cache = new();
        public CachingProductRepositoryDecorator(IProductRepository inner) => _inner = inner;
        public Product? GetById(int id)
        {
            if (_cache.TryGetValue(id, out var p)) { Console.WriteLine("Cache hit"); return p; }
            var prod = _inner.GetById(id);
            if (prod != null) _cache[id] = prod;
            return prod;
        }
    }

    class Program
    {
        static void Main()
        {
            IProductRepository repo = new SlowProductRepository();
            repo = new CachingProductRepositoryDecorator(repo);

            var p1 = repo.GetById(1);
            var p2 = repo.GetById(1); // should hit cache
        }
    }
}