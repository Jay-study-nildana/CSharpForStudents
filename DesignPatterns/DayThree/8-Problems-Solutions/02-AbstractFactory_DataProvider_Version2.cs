// 02-AbstractFactory_DataProvider.cs
// Abstract Factory for data provider families: IConnection & ICommand.
// DI/Lifetime: provider factories are typically stateless; register as Singleton or Transient as appropriate.
// Testability: inject IProviderFactory and use in-memory factory for tests.

using System;

public interface IConnection : IDisposable
{
    void Open();
}

public interface ICommand
{
    void Execute(string sql);
}

public interface IProviderFactory
{
    IConnection CreateConnection();
    ICommand CreateCommand(IConnection conn);
}

// SQL family
public class SqlConnection : IConnection
{
    public void Open() => Console.WriteLine("SqlConnection.Open");
    public void Dispose() => Console.WriteLine("SqlConnection.Dispose");
}
public class SqlCommand : ICommand
{
    private readonly IConnection _conn;
    public SqlCommand(IConnection conn) => _conn = conn;
    public void Execute(string sql) => Console.WriteLine($"SqlCommand.Execute: {sql}");
}
public class SqlProviderFactory : IProviderFactory
{
    public IConnection CreateConnection() => new SqlConnection();
    public ICommand CreateCommand(IConnection conn) => new SqlCommand(conn);
}

// InMemory family
public class InMemoryConnection : IConnection
{
    public void Open() => Console.WriteLine("InMemoryConnection.Open");
    public void Dispose() => Console.WriteLine("InMemoryConnection.Dispose");
}
public class InMemoryCommand : ICommand
{
    private readonly IConnection _conn;
    public InMemoryCommand(IConnection conn) => _conn = conn;
    public void Execute(string sql) => Console.WriteLine($"InMemoryCommand.Execute: {sql}");
}
public class InMemoryProviderFactory : IProviderFactory
{
    public IConnection CreateConnection() => new InMemoryConnection();
    public ICommand CreateCommand(IConnection conn) => new InMemoryCommand(conn);
}

// Client
public class DataAccess
{
    private readonly IProviderFactory _factory;
    public DataAccess(IProviderFactory factory) => _factory = factory;

    public void Run(string sql)
    {
        using var conn = _factory.CreateConnection();
        conn.Open();
        var cmd = _factory.CreateCommand(conn);
        cmd.Execute(sql);
    }
}