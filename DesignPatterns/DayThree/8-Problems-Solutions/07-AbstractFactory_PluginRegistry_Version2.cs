// 07-AbstractFactory_PluginRegistry.cs
// Plugin registry mapping string keys to IProviderFactory instances.
// DI/Lifetime: Registry can be Singleton; factories may be Singleton or created on registration.
// Testability: Register fake factories for unit tests or simulate missing plugins.

using System;
using System.Collections.Generic;

public class PluginFactoryRegistry
{
    private readonly Dictionary<string, IProviderFactory> _map = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string key, IProviderFactory factory) => _map[key] = factory;
    public bool TryGet(string key, out IProviderFactory factory) => _map.TryGetValue(key, out factory);
}

// Plugin client that asks registry for a factory by key
public class PluginClient
{
    private readonly PluginFactoryRegistry _registry;
    public PluginClient(PluginFactoryRegistry registry) => _registry = registry;

    public void Execute(string key)
    {
        if (_registry.TryGet(key, out var factory))
        {
            using var conn = factory.CreateConnection();
            conn.Open();
            var cmd = factory.CreateCommand(conn);
            cmd.Execute($"Plugin {key} operation");
        }
        else
        {
            Console.WriteLine($"Factory for plugin '{key}' not found.");
        }
    }
}

/*
Note: Real plugin loaders can discover factories via reflection and register them at startup.
*/