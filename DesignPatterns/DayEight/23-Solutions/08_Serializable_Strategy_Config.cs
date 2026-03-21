// 08_Serializable_Strategy_Config.cs
// Serialize strategy selection as JSON (strategy key + params), then rehydrate the strategy via factory.

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Day08.StrategySerialization08
{
    public interface IPriceStrategy { decimal ApplyDiscount(decimal p); string Name { get; } }

    public class PercentStrategy : IPriceStrategy
    {
        public string Name => $"Percent({_percent * 100}%)";
        private readonly decimal _percent;
        public PercentStrategy(decimal percent) => _percent = percent;
        public decimal ApplyDiscount(decimal p) => Math.Round(p * (1 - _percent), 2);
    }

    public static class StrategyFactory
    {
        public static IPriceStrategy FromConfig(Dictionary<string, object> cfg)
        {
            var key = cfg["type"] as string ?? "percent";
            return key switch
            {
                "percent" => new PercentStrategy(Convert.ToDecimal(cfg["percent"])),
                _ => new PercentStrategy(0m)
            };
        }
    }

    class Program
    {
        static void Main()
        {
            var config = new Dictionary<string, object> { ["type"] = "percent", ["percent"] = 0.2m };
            var json = JsonSerializer.Serialize(config);
            Console.WriteLine("Serialized config: " + json);

            // Deserialize and rehydrate
            var des = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            var cfg = new Dictionary<string, object>();
            foreach (var kv in des!)
            {
                if (kv.Value.ValueKind == JsonValueKind.String) cfg[kv.Key] = kv.Value.GetString()!;
                else if (kv.Value.ValueKind == JsonValueKind.Number) cfg[kv.Key] = kv.Value.GetDecimal();
                else cfg[kv.Key] = kv.Value.ToString()!;
            }

            var strat = StrategyFactory.FromConfig(cfg);
            Console.WriteLine($"Rehydrated strategy: {strat.Name}, price(100) => {strat.ApplyDiscount(100m):C}");
        }
    }
}