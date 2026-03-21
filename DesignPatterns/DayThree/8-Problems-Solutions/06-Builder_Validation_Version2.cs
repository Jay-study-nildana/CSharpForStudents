// 06-Builder_Validation.cs
// Builder that validates required fields for Product before building.
// Testability: Validation makes it easy to assert failure modes in unit tests.

using System;

public class Product
{
    public string Name { get; }
    public decimal Price { get; }

    internal Product(string name, decimal price) { Name = name; Price = price; }
}

public class ProductBuilder
{
    private string _name;
    private decimal? _price;

    public ProductBuilder WithName(string name) { _name = name; return this; }
    public ProductBuilder WithPrice(decimal price) { _price = price; return this; }

    public Product Build()
    {
        var errors = new System.Text.StringBuilder();
        if (string.IsNullOrWhiteSpace(_name)) errors.AppendLine("Name is required.");
        if (!_price.HasValue) errors.AppendLine("Price is required.");
        else if (_price.Value < 0) errors.AppendLine("Price must be non-negative.");

        if (errors.Length > 0) throw new InvalidOperationException("Product invalid: " + errors.ToString().Trim());

        return new Product(_name, _price.Value);
    }
}