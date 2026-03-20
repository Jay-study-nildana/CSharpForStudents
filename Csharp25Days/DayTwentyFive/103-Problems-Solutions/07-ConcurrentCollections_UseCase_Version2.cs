// 07-ConcurrentCollections_UseCase.cs
// Use ConcurrentDictionary for many threads adding and reading frequently.

using System;
using System.Collections.Concurrent;

public class Product { public string Id; public string Name; }

public class ProductRegistry
{
    private readonly ConcurrentDictionary<string, Product> _products = new();

    public void AddOrUpdate(Product p) => _products.AddOrUpdate(p.Id, p, (_, __) => p);

    public bool TryGet(string id, out Product product) => _products.TryGetValue(id, out product);

    public int Count => _products.Count;
}

/*
Why prefer ConcurrentDictionary:
- Internally optimized for concurrent reads/writes.
- Avoids manual lock complexity and scaling bottlenecks.
- Good when operations are mostly independent (by key).
*/