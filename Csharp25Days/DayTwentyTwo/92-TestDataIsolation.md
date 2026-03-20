# Test Data Isolation & Using In-Memory Stores

Purpose
- Understand why test data isolation matters.
- Learn patterns for using in-memory stores (SQLite in-memory, EF Core InMemory) safely.
- See practical setup/teardown fixtures and seeding approaches in C# (xUnit + EF Core).
- Learn trade-offs and when to prefer which in-memory strategy.

Why test data isolation matters
Isolated test data ensures tests are deterministic and independent. If tests share mutable state (a database, file, or queue), one test can change the environment and cause others to fail intermittently (flakiness). Isolation makes test runs reproducible, easier to debug, and safe to run in parallel.

Goals of isolation
- Each test starts with a known, minimal dataset.
- Tests don't require manual cleanup.
- Tests can run in parallel without collisions.
- Test failures are easy to reproduce locally and in CI.

Common isolation strategies
- Per-test ephemeral databases (in-memory or containerized).
- Shared fixture with transactional rollback per test.
- Unique identifiers (GUIDs) for test records.
- Programmatic seeding and explicit cleanup if necessary.

In-memory stores: options and trade-offs
1. EF Core InMemory provider
   - Fast and simple.
   - Not a relational database; does not enforce foreign keys, SQL translation, or LINQ translation behavior exactly.
   - Good for unit-like tests that need a lightweight fake repository.

2. SQLite in-memory (relational)
   - Uses a real relational engine; supports constraints, SQL translation, transactions.
   - Keep the connection open to persist the in-memory DB for the fixture lifetime.
   - Recommended when you need relational semantics but want an ephemeral DB.

3. In-memory file systems / queues
   - Libraries or simple in-memory implementations can replace file I/O or message brokers for tests.
   - Useful for fast tests that exercise logic without external infra.

4. Containerized DBs (Testcontainers/Docker)
   - Most realistic; run the same engine as production.
   - Slower and heavier but best for full integration tests.

Pattern: Per-test SQLite in-memory fixture (recommended for realistic behavior)
- Use a fixture that opens an in-memory SQLite connection and keeps it open.
- Create schema and seed minimal data in the fixture constructor.
- Use IClassFixture<T> to share the connection/DB across tests in a class.

```csharp
// Fixture that creates a relational in-memory DB
public class OrdersIntegrationFixture : IDisposable
{
    public DbConnection Connection { get; }
    public AppDbContext Context { get; }

    public OrdersIntegrationFixture()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open(); // Keep DB alive

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(Connection)
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();

        // Seed minimal, deterministic data
        Context.Customers.Add(new Customer { Id = 1, Email = "student@example.com" });
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context?.Dispose();
        Connection?.Close();
        Connection?.Dispose();
    }
}

// Use in tests
public class OrdersIntegrationTests : IClassFixture<OrdersIntegrationFixture>
{
    private readonly OrdersIntegrationFixture _f;
    public OrdersIntegrationTests(OrdersIntegrationFixture f) => _f = f;

    [Fact]
    public void AddOrder_PersistsToDb()
    {
        // Arrange
        var repo = new EfOrderRepository(_f.Context);

        // Act
        var id = repo.Add(new Order { CustomerId = 1, Total = 42m });

        // Assert
        var persisted = _f.Context.Orders.Find(id);
        Assert.NotNull(persisted);
    }
}
```

Pattern: Transaction-per-test (fast, avoids cleanup)
- Begin a transaction at test start and roll it back at teardown. This works well when the database supports nested/ambient transactions.
- For EF Core you can create a DbTransaction and pass the connection/transaction to the context.

```csharp
[Fact]
public void Test_WithTransactionRollback()
{
    using var transaction = _fixture.Connection.BeginTransaction();
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(_fixture.Connection)
        .Options;

    using var context = new AppDbContext(options);
    context.Database.UseTransaction(transaction);

    // Act: perform DB operations
    context.Orders.Add(new Order { CustomerId = 1, Total = 5m });
    context.SaveChanges();

    // Assert within transaction
    Assert.Equal(1, context.Orders.Count());

    // Rollback by disposing transaction (or explicitly calling Rollback)
    transaction.Rollback();
}
```

EF Core InMemory provider: quick example and caveats
- Use when you only need a lightweight fake repository and don't depend on relational behavior.

```csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // per-test DB
    .Options;

using var context = new AppDbContext(options);
context.Orders.Add(new Order { Total = 10m });
context.SaveChanges();
```

Caveat: EF Core InMemory may hide SQL translation or constraint bugs. For behavior that depends on SQL (joins, groupings, constraints), prefer SQLite in-memory or containerized DB.

Seeding and minimal data
- Seed only what the test needs—small datasets are faster and simpler.
- Use test factories/helpers to create domain objects with defaults—override only required fields.
- Use deterministic values (fixed timestamps or IClock abstraction) to avoid time-related flakiness.

Unique IDs and parallelism
- Generate unique identifiers per test (Guid.NewGuid(), test-specific prefixes) to avoid collisions when tests run in parallel or against shared resources.

Cleaning up external resources
- For external resources (files, queues, external services), ensure teardown removes test artifacts.
- In CI, prefer ephemeral resources (temporary directories, test containers) that are destroyed at the end of the job.

Choosing strategies by purpose
- Unit tests / logic: use mocks or EF Core InMemory for speed.
- Component-level tests needing relational semantics: use SQLite in-memory.
- Full integration tests (schema, performance): use production-like DB in a container, run less frequently (nightly).

Practical recommendations
- Keep integration suites small and focused; avoid turning every test into an integration test.
- Tag integration tests (Trait/Category) and run them in separate CI stages.
- Use fixtures for expensive setup (schema creation) and transaction-per-test or per-test DB instances for isolation.
- Document seeding, cleanup, and credentials in a test data plan for your capstone homework.

Closing note
Good test data isolation reduces flakiness and improves confidence in test results. Start with lightweight isolated tests for speed, and introduce relational or real-infra integration tests only where necessary to validate behavior that mocks cannot guarantee.

Further reading
- EF Core testing: https://learn.microsoft.com/ef/core/testing
- xUnit fixtures: https://xunit.net/docs/shared-context
- Testcontainers .NET: https://dotnet.testcontainers.org