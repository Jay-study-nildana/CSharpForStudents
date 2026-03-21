namespace SmartStore.Core.Domain;

/// <summary>Domain entity representing a store product.</summary>
public class Product
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Stock { get; set; }
    public string Category { get; init; } = string.Empty;

    public override string ToString() => $"[{Id}] {Name,-20} ${Price,8:F2}  Stock: {Stock,3}  ({Category})";
}
