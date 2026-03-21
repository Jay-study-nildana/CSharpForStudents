// 02-Decorator_StackingOrder.cs
// Intent: Demonstrate how stacking order of decorators affects runtime behavior.
// DI/Lifetime: Compose decorators in DI factory; ensure lifetimes align (Scoped with scoped dependencies).
// Testability: Compose with fakes to observe ordering effects.

using System;
using System.Collections.Generic;

// Reuse IService and RealService from 01-Decorator_Basic.cs

public class ValidationDecorator : ServiceDecoratorBase
{
    public ValidationDecorator(IService inner) : base(inner) { }
    public override string GetData(int id)
    {
        if (id <= 0) throw new ArgumentException("id must be positive");
        return _inner.GetData(id);
    }
}

public class CachingDecorator : ServiceDecoratorBase
{
    private readonly Dictionary<int, string> _cache = new();
    public CachingDecorator(IService inner) : base(inner) { }
    public override string GetData(int id)
    {
        if (_cache.TryGetValue(id, out var v)) return v;
        var result = _inner.GetData(id);
        _cache[id] = result;
        return result;
    }
}

public static class StackingExamples
{
    public static void ExampleA()
    {
        // Order: Validation -> Caching -> RealService
        // Validation runs first; caching wraps after validation.
        IService service = new ValidationDecorator(new CachingDecorator(new RealService()));
        Console.WriteLine(service.GetData(1));
    }

    public static void ExampleB()
    {
        // Order: Caching -> Validation -> RealService
        // Caching may bypass validation if cached key exists.
        IService service = new CachingDecorator(new ValidationDecorator(new RealService()));
        Console.WriteLine(service.GetData(1));
    }
}

/*
Explanation:
- In ExampleA, validation always runs before caching checks; invalid inputs rejected prior to cache lookups.
- In ExampleB, caching may return a cached value without validation, which could be undesirable.
- Choose ordering based on semantics: validation typically should be outermost to enforce invariants.
*/