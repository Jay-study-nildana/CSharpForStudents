// StrategyPatternDemo.cs
// Interactive console demo of the Strategy Pattern with interchangeable discount algorithms.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;


// Top-level statements
var services = new ServiceCollection();

// Register strategy implementations:
// `NoDiscountStrategy` is stateless (pure function of input -> output).
// Making it a singleton is safe and efficient: one instance can be reused
// across the app without risk of shared mutable state or thread-safety issues.
services.AddSingleton<NoDiscountStrategy>();

// `TieredDiscountStrategy` is also stateless and deterministic. A singleton
// avoids redundant allocations and ensures a single canonical implementation
// is used everywhere. Use singleton for lightweight, thread-safe services.
services.AddSingleton<TieredDiscountStrategy>();

// `DiscountContext` holds the current strategy instance and therefore carries
// mutable state (the selected strategy). Registering it as transient means a
// new `DiscountContext` will be produced on each resolve, avoiding accidental
// sharing of context state across callers. Note: transient = new per resolve
// (so callers should resolve a new context for each logical operation).
services.AddTransient<DiscountContext>();

var provider = services.BuildServiceProvider();

var strategies = new Dictionary<string, Func<IDiscountStrategy>>
{
    { "none", () => provider.GetRequiredService<NoDiscountStrategy>() },
    { "tiered", () => provider.GetRequiredService<TieredDiscountStrategy>() },
    { "percent", () => new PercentageDiscountStrategy(ReadDecimal("Enter percent (e.g., 10): ")) },
    { "fixed", () => new FixedAmountDiscountStrategy(ReadDecimal("Enter amount (e.g., 20): ")) }
};

var context = provider.GetRequiredService<DiscountContext>();
context.SetStrategy(strategies["none"]());

while (true)
{
    Console.WriteLine("Choose strategy: none, percent, fixed, tiered, or exit");
    var choice = Console.ReadLine();
    if (choice == "exit") break;
    if (!strategies.ContainsKey(choice))
    {
        Console.WriteLine("Invalid strategy.");
        continue;
    }
    context.SetStrategy(strategies[choice]());
    var price = ReadDecimal("Enter original price: ");
    var discounted = context.ApplyDiscount(price);
    Console.WriteLine($"Discounted price: {discounted:C}");
}

static decimal ReadDecimal(string prompt)
{
    Console.Write(prompt);
    // Declare `value` here so it remains in scope after the while loop.
    decimal value;
    while (!decimal.TryParse(Console.ReadLine(), out value))
        Console.Write("Invalid. Try again: ");
    return value;
}

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
}

public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice) => originalPrice;
}

public class PercentageDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _percent;
    public PercentageDiscountStrategy(decimal percent) => _percent = percent;
    public decimal ApplyDiscount(decimal originalPrice) => originalPrice * (1 - _percent / 100);
}

public class FixedAmountDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _amount;
    public FixedAmountDiscountStrategy(decimal amount) => _amount = amount;
    public decimal ApplyDiscount(decimal originalPrice) => Math.Max(0, originalPrice - _amount);
}

public class TieredDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice)
    {
        if (originalPrice > 200) return originalPrice * 0.7m;
        if (originalPrice > 100) return originalPrice * 0.8m;
        return originalPrice * 0.9m;
    }
}

public class DiscountContext
{
    private IDiscountStrategy _strategy;
    public DiscountContext(IDiscountStrategy strategy) => _strategy = strategy;
    public void SetStrategy(IDiscountStrategy strategy) => _strategy = strategy;
    public decimal ApplyDiscount(decimal price) => _strategy.ApplyDiscount(price);
}