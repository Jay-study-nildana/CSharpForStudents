// 04-Decorator_Validation.cs
// Intent: Implement a validation decorator that enforces input invariants before delegating.
// DI/Lifetime: Validation decorator stateless; Transient/Scoped fine.
// Testability: Use fake inner to ensure validation prevents inner calls on invalid inputs.

using System;

public class ValidationDecoratorSimple : ServiceDecoratorBase
{
    public ValidationDecoratorSimple(IService inner) : base(inner) { }

    public override string GetData(int id)
    {
        if (id <= 0) throw new ArgumentException("id must be positive", nameof(id));
        return _inner.GetData(id);
    }
}

/*
Usage:
var svc = new ValidationDecoratorSimple(new RealService());
svc.GetData(0); // throws ArgumentException without calling RealService
*/