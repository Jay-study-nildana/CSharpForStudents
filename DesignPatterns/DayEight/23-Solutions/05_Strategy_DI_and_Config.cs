// 05_Strategy_DI_and_Config.cs
// Simulated DI/config factory that maps config keys to strategies.
// Demonstrates selecting a strategy by config name.

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Day08.StrategyDI05
{
    public interface IPriceStrategy { decimal ApplyDiscount(decimal basePrice); string Name { get; } }

    public class NoDiscount : IPriceStrategy { public string Name => "NoDiscount"; public decimal ApplyDiscount(decimal basePrice) => basePrice; }
    public class Percent : IPriceStrategy { public string Name => "Percent10"; private readonly decimal _p = 0.10m; public decimal ApplyDiscount(decimal basePrice) => Math.Round(basePrice * (1 - _p), 2); }

    public static class StrategyFactory
    {
        public static IPriceStrategy Create(string configKey)
        {
            return configKey switch
            {
                "none" => new NoDiscount(),
                "pct10" => new Percent(),
                _ => new NoDiscount()
            };
        }
    }

    class Program
    {
        static void Main()
        {
            // Simulate reading config
            var json = "{\"discount\":\"pct10\"}";
            var doc = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            var key = doc!["discount"];

            var strategy = StrategyFactory.Create(key);
            Console.WriteLine($"Selected strategy: {strategy.Name}");
            Console.WriteLine($"Price for 200 => {strategy.ApplyDiscount(200m):C}");
        }
    }
}