# Integration Testing — Contrast with Unit Testing

Objectives
- Understand the difference between unit and integration tests.
- Learn approaches for deterministic integration tests and test-data management.
- See practical C# examples: a mocked unit test and an EF Core integration test using an in-memory SQLite database.
- Learn setup/teardown, seeding, and cleanup patterns.

Quick summary
- Unit tests: fast, isolated, deterministic. Replace external dependencies with test doubles (mocks, stubs, fakes).
- Integration tests: verify how multiple components work together and with real infrastructure (DB, file system, HTTP). They are slower and more brittle, so manage them carefully—use them to complement, not replace, unit tests.

When to prefer unit tests
- Verifying business logic in a single class or method.
- Validating branching, calculations, or small algorithms.
- Running in tight CI loops (pre-commit, PR build).  
Unit-test benefits: very fast, easy to run locally, and provide quick feedback.

When to prefer integration tests
- Validating actual database schema, SQL behavior, transactions, or migrations.
- Testing HTTP endpoints end-to-end (controller → service → DB).
- Verifying third-party SDK interactions in a sandbox or using test containers.  
Integration tests provide higher confidence for cross-cutting behavior that mocks cannot reproduce.

Determinism and test-data isolation
- Make tests deterministic: control time, randomness, and external network access.
- Give each integration test a clean data environment:
  - Use disposable test databases (SQLite in-memory with an open connection, Docker containers, test schemas).
  - Seed only the minimal required data.
  - Use unique identifiers (GUIDs, timestamps) to avoid collisions.
- Separate integration tests from unit tests in CI (e.g., run integration tests nightly or in a special pipeline) because of their longer runtime and external dependencies.

Example patterns for setup/teardown
- xUnit IClassFixture<T> or ICollectionFixture<T> for shared initialization between tests.
- Use IDisposable on fixtures to tear down resources (close DB connection, delete temporary files).
- For web apps, use WebApplicationFactory<TEntryPoint> (ASP.NET Core) and configure test services to use test DBs.

Unit test example (xUnit + Moq)
- Goal: fast verification of OrderProcessor behavior — mock payment and repo.

```csharp
// Unit test: isolate dependencies with Moq
[Fact]
public void Process_SuccessfulPayment_SavesOrder()
{
    var order = new Order { Total = 50m, CustomerEmail = "c@ex.com" };
    var mockPayment = new Mock<IPaymentGateway>();
    var mockRepo = new Mock<IOrderRepository>();
    var mockEmail = new Mock<IEmailSender>();

    mockPayment.Setup(p => p.Charge(It.IsAny<string>(), order.Total)).Returns(true);

    var sut = new OrderProcessor(mockPayment.Object, mockRepo.Object, mockEmail.Object);

    var result = sut.Process(order, "token-1");

    Assert.True(result);
    mockRepo.Verify(r => r.Save(order), Times.Once);
    mockEmail.Verify(e => e.Send(order.CustomerEmail, "Order received", It.IsAny<string>()), Times.Once);
}
```

Integration test example (EF Core + SQLite in-memory)
- Use an in-memory SQLite with a persistent open connection to exercise the real relational provider behavior (foreign keys, SQL translation). Keep the connection open for the fixture lifetime so the in-memory DB persists across contexts.

```csharp
// Integration test: EF Core + SQLite in-memory
public class OrdersIntegrationFixture : IDisposable
{
    public DbConnection Connection { get; }
    public AppDbContext Context { get; }

    public OrdersIntegrationFixture()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open(); // Keep DB alive for the lifetime of the fixture

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(Connection)
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
        // Seed minimal data
        Context.Customers.Add(new Customer { Id = 1, Email = "c@ex.com" });
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context?.Dispose();
        Connection?.Close();
        Connection?.Dispose();
    }
}

public class OrdersIntegrationTests : IClassFixture<OrdersIntegrationFixture>
{
    private readonly OrdersIntegrationFixture _fixture;

    public OrdersIntegrationTests(OrdersIntegrationFixture fixture) => _fixture = fixture;

    [Fact]
    public void CreateOrder_PersistsToDatabaseAndReturnsId()
    {
        // Arrange
        var repo = new EfOrderRepository(_fixture.Context);

        // Act
        var order = new Order { CustomerId = 1, Total = 25m };
        var id = repo.Add(order);

        // Assert
        var persisted = _fixture.Context.Orders.Find(id);
        Assert.NotNull(persisted);
        Assert.Equal(25m, persisted.Total);
    }
}
```

Notes on test-data management
- Seed only what is necessary; prefer programmatic seeding in the fixture rather than large SQL dumps.
- Prefer isolated DB instances: in-memory provider, ephemeral schemas, or per-test containers (Testcontainers or Docker).
- Avoid shared mutable state between tests; if using a shared DB, reset tables between tests (truncate or rollback transactional tests).
- Transaction-per-test approach: begin a transaction in setup and rollback in teardown—this can be fast and keeps schema intact. For EF Core with SQLite in-memory, keeping connection open is common.

Assertions and scope
- Integration tests should assert observable outcomes at the system boundary: DB rows exist with expected columns, API returns expected HTTP status and JSON, background job enqueued correct message.
- Avoid testing implementation details like specific repository method invocations (that belongs to unit tests).

Common pitfalls
- Using EF Core InMemory provider for behavior-sensitive tests: it does not enforce relational constraints or translate LINQ the same way as a relational provider—prefer SQLite in-memory or a containerized DB for realistic behavior.
- Flaky tests due to external network calls: mock external HTTP using tools like WireMock.Net or use sandbox environments and retry logic.
- Long-running integration suites in pull-request builds: keep PR pipelines light (unit & smoke integration tests); run full integration suites in scheduled or gate pipelines.

Practical CI strategy
- Run unit tests on every push/PR for rapid feedback.
- Run integration tests in a separate pipeline or as nightly builds.
- Mark integration tests with a trait/Category so CI can select them.

Closing recommendations
- Start with unit tests to cover logic and contract-level behavior.
- Add focused integration tests for cross-component scenarios and infra-specific behavior.
- Invest in fixtures and deterministic seeding to make integration tests reliable and maintainable.

References
- xUnit: https://xunit.net
- EF Core testing: https://learn.microsoft.com/ef/core/testing
- ASP.NET Core WebApplicationFactory: https://learn.microsoft.com/aspnet/core/test/integration-tests