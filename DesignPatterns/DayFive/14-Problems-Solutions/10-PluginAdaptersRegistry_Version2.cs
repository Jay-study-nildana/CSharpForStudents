// 10-PluginAdaptersRegistry.cs
// Intent: Registry for payment adapters (providers). Enables runtime selection of adapter implementations by key.
// DI/Lifetime: Registry Singleton; adapters registered may be Transient/Scoped. For plugin discovery, register factories that create adapters.
// Testability: In tests register fake adapters with the registry to simulate provider behavior.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class PaymentAdapterRegistry
{
    private readonly ConcurrentDictionary<string, Func<IPaymentGateway>> _map = new(StringComparer.OrdinalIgnoreCase);

    // Register a factory that can create an adapter instance on demand
    public void Register(string key, Func<IPaymentGateway> adapterFactory) => _map[key] = adapterFactory;

    public bool TryGet(string key, out IPaymentGateway adapter)
    {
        adapter = null;
        if (_map.TryGetValue(key, out var factory))
        {
            adapter = factory();
            return true;
        }
        return false;
    }

    public IEnumerable<string> RegisteredKeys => _map.Keys;
}

// Usage: orchestrator that uses registry to pick provider at runtime
public class PaymentOrchestrator
{
    private readonly PaymentAdapterRegistry _registry;
    public PaymentOrchestrator(PaymentAdapterRegistry registry) => _registry = registry;

    public PaymentResult Process(string providerKey, decimal amount, string currency)
    {
        if (!_registry.TryGet(providerKey, out var adapter))
        {
            return new PaymentResult { Success = false, Error = "ProviderNotFound" };
        }

        try
        {
            var ok = adapter.Charge(amount, currency);
            return new PaymentResult { Success = ok, Error = ok ? null : "Declined" };
        }
        catch (Exception ex)
        {
            return new PaymentResult { Success = false, Error = ex.Message };
        }
    }
}

/*
Plugin loading notes:
- In simple apps, register known adapters at startup with registry.Register("legacy", () => new LegacyPaymentAdapter(...));
- For plugin systems, discover adapter types via reflection/MEF and register factories that construct adapters (allowing DI resolution inside factory).
- Keep registry singleton and adapter factories lightweight; create adapter instances per request if adapters hold transient dependencies.
*/