namespace ComicBookShop.Core.Models;

/// <summary>
/// Value type representing a price range with validation.
/// Demonstrates struct, IEquatable, operator overloading (Day 10).
/// </summary>
public readonly struct PriceRange : IEquatable<PriceRange>
{
    public decimal Min { get; }
    public decimal Max { get; }

    public PriceRange(decimal min, decimal max)
    {
        if (min < 0)
            throw new ArgumentException("Min price cannot be negative.", nameof(min));
        if (max < min)
            throw new ArgumentException("Max price cannot be less than min price.", nameof(max));

        Min = min;
        Max = max;
    }

    /// <summary>Returns true if the given price falls within this range (inclusive).</summary>
    public bool Contains(decimal price) => price >= Min && price <= Max;

    public bool Equals(PriceRange other) => Min == other.Min && Max == other.Max;
    public override bool Equals(object? obj) => obj is PriceRange other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Min, Max);
    public override string ToString() => $"${Min:F2} – ${Max:F2}";

    public static bool operator ==(PriceRange left, PriceRange right) => left.Equals(right);
    public static bool operator !=(PriceRange left, PriceRange right) => !left.Equals(right);
}
