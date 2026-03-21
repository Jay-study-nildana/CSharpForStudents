# Day 4 — Data Access Patterns: Repository & Unit of Work

This lesson describes two complementary patterns for organizing data access: Repository and Unit of Work (UoW). The goal is to decouple domain logic from persistence details, make code more testable, and control transactional boundaries.

## High-level intent
- Repository: Provide an abstraction for querying and persisting aggregates or domain entities so domain code doesn't depend on a particular ORM or database API.
- Unit of Work: Coordinate changes across multiple repositories and commit them as a single transaction, tracking a set of changes and handling commit/rollback.

Together they improve separation of concerns, enable easier testing (mocking/faking persistence), and centralize transaction management.

---

## Repository — responsibilities and design
A Repository represents a collection-like interface for a specific aggregate root or entity type. Typical responsibilities:
- Query operations (GetById, Find, List, paging).
- Persistence operations (Add, Update, Remove).
- Translate domain operations into persistence calls (but avoid leaking ORM types in the domain interface).

Common pitfalls:
- Overloading repository with complex queries (consider Query Objects or Specification pattern).
- Returning IQueryable<T> from repository commonly couples callers to the ORM; prefer domain-level DTOs or well-defined query methods.

Minimal generic repository interface:
```csharp
public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
```

Example: a specialized repository for Orders (query methods can be tailored):
```csharp
public interface IOrderRepository : IRepository<Order>
{
    IEnumerable<Order> GetByCustomerId(int customerId);
    IEnumerable<Order> GetPendingOrders();
}
```

Implementation choices:
- In a simple app, repository methods can wrap DbContext operations.
- For complex querying, expose explicit query methods (e.g., GetByFilter) or return read-model DTOs instead of IQueryable<T>.

---

## Unit of Work — intent and contract
The Unit of Work aggregates multiple repository changes into a single transaction. It typically:
- Exposes access to repositories (or provides repository creation),
- Manages a transaction scope,
- Provides Commit and Rollback operations.

Minimal UoW interface:
```csharp
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    IRepository<Customer> Customers { get; }
    void Commit();     // commit transaction
    void Rollback();   // rollback (optional)
}
```

Usage pattern in a service:
```csharp
public class OrderService
{
    private readonly IUnitOfWork _uow;

    public OrderService(IUnitOfWork uow) => _uow = uow;

    public void PlaceOrder(Order order)
    {
        _uow.Orders.Add(order);
        // maybe modify customers, inventory, etc.
        _uow.Commit(); // persists all changes together
    }
}
```

Under the hood with EF Core, Commit typically calls `DbContext.SaveChanges()` inside a transaction.

---

## Mapping to EF Core (practical notes)
- DbContext already implements a unit-of-work-like behavior (change tracker + SaveChanges). You can either:
  - Treat DbContext as the UoW (expose repositories that use DbContext and register DbContext as Scoped in DI), or
  - Wrap DbContext in a UoW that exposes repository properties and a Commit method.

Example of a simple EF-backed repository:
```csharp
public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _ctx;
    public EfRepository(DbContext ctx) => _ctx = ctx;
    public T GetById(int id) => _ctx.Set<T>().Find(id);
    public IEnumerable<T> GetAll() => _ctx.Set<T>().ToList();
    public void Add(T entity) => _ctx.Set<T>().Add(entity);
    public void Update(T entity) => _ctx.Set<T>().Update(entity);
    public void Remove(T entity) => _ctx.Set<T>().Remove(entity);
}
```

Simple UoW wrapper around DbContext:
```csharp
public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    public IOrderRepository Orders { get; }
    public IRepository<Customer> Customers { get; }

    public EfUnitOfWork(AppDbContext ctx)
    {
        _ctx = ctx;
        Orders = new EfOrderRepository(_ctx);
        Customers = new EfRepository<Customer>(_ctx);
    }

    public void Commit() => _ctx.SaveChanges();
    public void Rollback() => _ctx.ChangeTracker.Clear(); // or use transactions
    public void Dispose() => _ctx.Dispose();
}
```

DI registration recommendations:
- Register DbContext as Scoped (per web request).  
- Register UnitOfWork and repositories as Scoped.  
Example registration (conceptual):
```csharp
services.AddDbContext<AppDbContext>(options => ...);
services.AddScoped<IUnitOfWork, EfUnitOfWork>();
services.AddScoped<IOrderRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Orders);
```

---

## Testing strategies
- Mock repositories and UoW interfaces for unit tests of services. Use lightweight fakes for integration tests.
- Because real DbContext has I/O, prefer:
  - Unit tests: inject mock IOrderRepository and verify service behavior, or
  - Integration tests: use an in-memory provider or a local test database.
- Example pseudo-test using a fake repository:
```csharp
// Arrange
var fakeOrders = new InMemoryOrderRepository();
var uow = new FakeUnitOfWork { Orders = fakeOrders };
var svc = new OrderService(uow);

// Act
svc.PlaceOrder(new Order { ... });

// Assert
Assert.Contains(fakeOrders.GetAll(), o => o.Id == expectedId);
```

Be careful: using EF InMemory provider is useful but has behavioral differences from relational providers (e.g., SQL behavior, transactions).

---

## When to use Repository+UoW vs direct ORM use
- Use Repository+UoW when you need:
  - Clear separation between domain and persistence,
  - Easier mocking and unit testing,
  - Multiple persistence strategies or a desire to encapsulate queries.
- Direct DbContext usage is acceptable when:
  - You accept some coupling to EF in your domain layer,
  - The team is comfortable with EF patterns and you prefer fewer abstractions.

Tradeoffs:
- Repository+UoW adds abstraction and maintenance overhead but increases testability and portability.
- Direct ORM use can reduce boilerplate and better leverage ORM features (LINQ queries, eager loading), but can make testing and swapping ORMs harder.

---

## Lab (in-class)
1. Define domain: Customer and Order (Order has multiple OrderLines). Create IOrderRepository and IRepository<T> interfaces with appropriate methods.
2. Design an IUnitOfWork that exposes repositories and Commit/Rollback.
3. Implement simple in-memory repositories and an InMemoryUnitOfWork for tests.
4. Write test scenarios:
   - PlaceOrder: adds order and commits; verify order persisted in repo.
   - PlaceOrder failure: simulate exception before commit; verify no partial persistence.

---

## Homework
Write a one-page comparison of Repository+UoW vs using DbContext directly. Include:
- When you'd pick each approach,
- How you would test services that place orders,
- One example of an anti-pattern to avoid (e.g., exposing IQueryable<T> from repository).

Further reading
- Microsoft docs: EF Core DbContext, transactions, and testing guidance  
- Martin Fowler: Repository pattern notes  
- Articles on testing data access layers and patterns for complex domains