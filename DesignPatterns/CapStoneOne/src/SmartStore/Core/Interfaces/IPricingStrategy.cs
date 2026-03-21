namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// STRATEGY PATTERN — interface
// -------------------------------------------------------
// Defines the algorithm contract for pricing/discounts.
// Concrete strategies are swappable at runtime.
// -------------------------------------------------------
public interface IPricingStrategy
{
    string Name { get; }
    decimal CalculateDiscount(Order order);
}
