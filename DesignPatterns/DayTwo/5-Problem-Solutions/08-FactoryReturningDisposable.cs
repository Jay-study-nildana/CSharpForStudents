// 08-FactoryReturningDisposable.cs
// Problem: Factory that returns disposable resource instances and proper consumer usage.

using System;

public interface IDatabaseConnection : IDisposable
{
    void Execute(string sql);
}

public class DatabaseConnection : IDatabaseConnection
{
    private readonly string _connString;
    public DatabaseConnection(string connString) => _connString = connString;
    public void Execute(string sql) => Console.WriteLine($"Executing on {_connString}: {sql}");
    public void Dispose() => Console.WriteLine("DatabaseConnection disposed.");
}

// Factory that creates transient disposable connections
public class ConnectionFactory
{
    private readonly string _connString;
    public ConnectionFactory(string connString) => _connString = connString;

    public IDatabaseConnection Create() => new DatabaseConnection(_connString);
}

// Consumer demonstrates correct usage
public class Repository
{
    private readonly ConnectionFactory _factory;
    public Repository(ConnectionFactory factory) => _factory = factory;

    public void Save(string payload)
    {
        // Each call gets its own disposable resource that must be disposed
        using var conn = _factory.Create();
        conn.Execute($"INSERT {payload}");
    }
}

/*
Lifetime rationale:
- Resources returned by the factory are transient and should be created/disposed per-use.
- Do not cache disposable per-request resources in singletons; prefer per-operation creation or DI scopes.
*/