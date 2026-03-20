// ValidationPipelineShortCircuit.cs
// Problem: ValidationPipelineShortCircuit
// Compose validators and short-circuit on first failure (fail-fast validation pipeline).

using System;
using System.Collections.Generic;

record NewOrderDto(string? CustomerId, List<(int ProductId, int Qty)> Items);

class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; } = new();
    public static ValidationResult Ok() => new ValidationResult();
    public static ValidationResult Fail(string msg) => new ValidationResult { Errors = { msg } };
}

class ValidationPipelineShortCircuit
{
    static ValidationResult NotEmptyCustomer(NewOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CustomerId)) return ValidationResult.Fail("CustomerId required.");
        return ValidationResult.Ok();
    }

    static ValidationResult AtLeastOneItem(NewOrderDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0) return ValidationResult.Fail("At least one item required.");
        return ValidationResult.Ok();
    }

    static ValidationResult PositiveQuantities(NewOrderDto dto)
    {
        foreach (var it in dto.Items)
            if (it.Qty <= 0) return ValidationResult.Fail("Quantities must be positive.");
        return ValidationResult.Ok();
    }

    static ValidationResult RunValidators(NewOrderDto dto, IEnumerable<Func<NewOrderDto, ValidationResult>> validators)
    {
        foreach (var v in validators)
        {
            var r = v(dto);
            if (!r.IsValid) return r; // short-circuit fail-fast
        }
        return ValidationResult.Ok();
    }

    static void Main()
    {
        var dto = new NewOrderDto(null, new List<(int,int)>{ (1,2) });
        var validators = new Func<NewOrderDto, ValidationResult>[] { NotEmptyCustomer, AtLeastOneItem, PositiveQuantities };
        var result = RunValidators(dto, validators);
        if (!result.IsValid) Console.WriteLine("Validation failed: " + string.Join(", ", result.Errors));
        else Console.WriteLine("Valid order");
    }
}