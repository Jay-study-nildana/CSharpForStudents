// FailFastStartupConfig.cs
// Problem: FailFastStartupConfig
// Validate configuration at startup and throw InvalidOperationException on missing required settings.

using System;
using System.Collections.Generic;

class Bootstrapper
{
    public static void ValidateConfig(IDictionary<string,string?> config)
    {
        if (!config.TryGetValue("ConnectionString", out var cs) || string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Missing required configuration: ConnectionString");
        if (!config.TryGetValue("ApiKey", out var api) || string.IsNullOrWhiteSpace(api))
            throw new InvalidOperationException("Missing required configuration: ApiKey");
        // other checks...
    }
}

class FailFastStartupConfig
{
    static void Main()
    {
        var goodConfig = new Dictionary<string,string?> { ["ConnectionString"] = "Server=x;", ["ApiKey"] = "abc" };
        var badConfig = new Dictionary<string,string?> { ["ApiKey"] = "abc" };

        try
        {
            Bootstrapper.ValidateConfig(goodConfig);
            Console.WriteLine("Good config: startup OK");
            Bootstrapper.ValidateConfig(badConfig); // will throw
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Startup failed fast: {ex.Message}");
            // In a real app, exit process or prevent service registration
        }
    }
}