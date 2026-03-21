// 09-Proxy_RemoteStub.cs
// Intent: RemoteServiceProxy that caches a connection handle and handles reconnection on transient failure.
// DI/Lifetime: Proxy Transient/Scoped; connection handles often IDisposable and scoped per operation; avoid long-lived singletons holding stale connections.
// Testability: Use RemoteStub to simulate latency/failure and assert proxy behavior.

using System;
using System.Threading;

public interface IRemoteService
{
    string Fetch(string resource);
}

public class RemoteStub : IRemoteService
{
    private readonly Random _rnd = new();

    public string Fetch(string resource)
    {
        // Simulate latency and occasional transient failure
        Thread.Sleep(200);
        if (_rnd.NextDouble() < 0.2) throw new Exception("Transient remote error");
        return $"Data:{resource}";
    }
}

public class RemoteServiceProxy : IRemoteService
{
    private readonly Func<IRemoteService> _createRemote;
    private IRemoteService _remote;
    private int _attemptsBeforeReconnect = 1;

    public RemoteServiceProxy(Func<IRemoteService> createRemote)
    {
        _createRemote = createRemote;
    }

    public string Fetch(string resource)
    {
        EnsureConnected();
        try
        {
            return _remote.Fetch(resource);
        }
        catch (Exception)
        {
            // On failure, reset remote to force reconnection on next call
            _remote = null;
            throw;
        }
    }

    private void EnsureConnected()
    {
        if (_remote == null) _remote = _createRemote();
    }
}

/*
Notes on retry/backoff:
- Proxy handles connection lifecycle (lazy connect, reset on failure) but retry/backoff policy is better placed in a higher-level facade or using a resilience library (Polly).
- Keeping retry in proxy can be acceptable for simple reconnection attempts, but avoid embedding application-level retry semantics that callers expect.
*/