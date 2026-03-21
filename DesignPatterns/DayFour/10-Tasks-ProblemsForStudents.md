# Day 4 — Exercises: Repository & Unit of Work (10 Problems)

Instructions
- Complete the 10 problems below. Each problem focuses on Repository and Unit of Work patterns, testing strategies, DI/lifetime choices, and common anti-patterns.
- Implement each solution in a separate C# file named exactly as the problem title (prefix with two-digit ID). Example: Problem "01-OrderRepositoryInterfaces.cs" → solution file "01-OrderRepositoryInterfaces.cs".
- At the top of each solution file include a short comment summarizing the pattern intent, DI registration/lifetime recommendation, and one testability note.
- Aim for clear interfaces, small focused examples, and simple in-memory implementations where appropriate so you can run quick tests in class.

Problems

1) 01-OrderRepositoryInterfaces.cs  
Problem: Define a minimal generic `IRepository<T>` interface and a specialized `IOrderRepository` for an Order domain (methods: GetById, GetAll, Add, Update, Remove, plus `GetByCustomerId`). Include XML-like comments describing responsibilities.

2) 02-InMemoryRepository.cs  
Problem: Implement an in-memory `InMemoryRepository<T>` that implements `IRepository<T>` using a `List<T>` and a simple integer id strategy (assume entities have an `int Id` property). Provide thread-safety notes in comments.

3) 03-OrderEntityAndDto.cs  
Problem: Define simple domain classes: `Order`, `OrderLine`, and a read-model `OrderDto` used for queries (Id, CustomerId, TotalAmount, Status). Include a small helper to map `Order` -> `OrderDto`.

4) 04-InMemoryOrderRepository.cs  
Problem: Implement `InMemoryOrderRepository` that implements `IOrderRepository` using `InMemoryRepository<Order>` as backing store and adds `GetByCustomerId` implementation. Include simple query (LINQ) semantics.

5) 05-IUnitOfWorkInterface.cs  
Problem: Define an `IUnitOfWork` interface exposing `IOrderRepository Orders { get; }` and `IRepository<Customer> Customers { get; }`, plus `Commit()` and `Rollback()`. Make it `IDisposable` friendly.

6) 06-InMemoryUnitOfWork.cs  
Problem: Implement `InMemoryUnitOfWork` that returns in-memory repositories, collects changes, and simulates `Commit()` and `Rollback()` behavior. Provide a way to inspect pending changes for tests.

7) 07-OrderServiceWithUoW.cs  
Problem: Implement an `OrderService` that uses `IUnitOfWork` to place an order and demonstrate transactional behavior: add order, decrement inventory (simulate via comments or repository), then commit; simulate an exception and show rollback behavior in code comments or sample usage.

8) 08-QueryableAntiPatternAndRefactor.cs  
Problem: Demonstrate the anti-pattern of exposing `IQueryable<T>` from a repository in a simple example (commented). Then refactor to provide a safe query method returning `IEnumerable<OrderDto>` and a paged query API. Explain tradeoffs in comments.

9) 09-RepositoryUnitTestsExample.cs  
Problem: Provide example unit-test style code (conceptual, not framework-bound) that shows how to test `OrderService` using a `FakeUnitOfWork` or mocked `IOrderRepository`. Include assertions/comments demonstrating what to verify (calls, state after commit, rollback).

10) 10-RepositoryVsDbContextNotes.cs  
Problem: Create a concise design-note (text) file summarizing when to use Repository+UoW vs direct DbContext usage. Include a short checklist (5 questions) teams should ask when choosing an approach and practical DI registration recommendations.

Deliverables
- Day4_Exercises.md (this file)  
- Ten solution files named exactly as the problem titles (01–10).