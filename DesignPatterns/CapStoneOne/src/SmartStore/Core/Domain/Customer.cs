namespace SmartStore.Core.Domain;

public enum CustomerType { Regular, Premium, Vip }

/// <summary>Domain entity representing a store customer.</summary>
public class Customer
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public CustomerType Type { get; init; }

    public override string ToString() => $"{Name} ({Type})";
}
