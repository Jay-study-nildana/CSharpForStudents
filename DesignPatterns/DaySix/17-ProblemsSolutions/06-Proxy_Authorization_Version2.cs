// 06-Proxy_Authorization.cs
// Intent: Protection proxy that verifies authorization before delegating to the subject.
// DI/Lifetime: Proxy transient/scoped; authorization delegate injected so tests can supply different behaviors.
// Testability: Provide a fake authorization delegate to simulate allowed/denied cases.

using System;

public interface IDataProvider
{
    string GetSecret(int id);
}

public class RealDataProvider : IDataProvider
{
    public string GetSecret(int id) => $"Secret#{id}";
}

public class ProtectionProxy : IDataProvider
{
    private readonly IDataProvider _inner;
    private readonly Func<string, bool> _isAuthorized; // e.g., (permission) => true/false
    private readonly string _requiredPermission;

    public ProtectionProxy(IDataProvider inner, Func<string, bool> isAuthorized, string requiredPermission)
    {
        _inner = inner;
        _isAuthorized = isAuthorized;
        _requiredPermission = requiredPermission;
    }

    public string GetSecret(int id)
    {
        if (!_isAuthorized(_requiredPermission)) throw new UnauthorizedAccessException("Permission denied");
        return _inner.GetSecret(id);
    }
}

/*
Test example (conceptual):
var provider = new ProtectionProxy(new RealDataProvider(), perm => false, "read:secret");
provider.GetSecret(1); // throws UnauthorizedAccessException
*/