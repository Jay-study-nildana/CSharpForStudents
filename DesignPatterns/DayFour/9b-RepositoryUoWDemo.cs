// RepositoryUnitOfWorkDemo.cs
// Console demo of Repository and Unit of Work patterns with in-memory storage.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

// Top-level statements
using var uow = new InMemoryUnitOfWork();

while (true)
{
    Console.WriteLine("Choose action: add-order, list-orders, add-customer, list-customers, commit, rollback, exit");
    var cmd = Console.ReadLine();
    if (cmd == "exit") break;

    if (cmd == "add-order")
    {
        Console.Write("Order description: ");
        var desc = Console.ReadLine();
        uow.Orders.Add(new Order { Description = desc });
        Console.WriteLine("Order added.");
    }
    else if (cmd == "list-orders")
    {
        foreach (var o in uow.Orders.GetAll())
            Console.WriteLine($"Order {o.Id}: {o.Description}");
    }
    else if (cmd == "add-customer")
    {
        Console.Write("Customer name: ");
        var name = Console.ReadLine();
        uow.Customers.Add(new Customer { Name = name });
        Console.WriteLine("Customer added.");
    }
    else if (cmd == "list-customers")
    {
        foreach (var c in uow.Customers.GetAll())
            Console.WriteLine($"Customer {c.Id}: {c.Name}");
    }
    else if (cmd == "commit")
    {
        uow.Commit();
    }
    else if (cmd == "rollback")
    {
        uow.Rollback();
    }
}

/*
- This demo shows how repositories abstract data access for entities.
- The Unit of Work coordinates multiple repositories and transactional boundaries.
- All data is in-memory for simplicity.
*/


// Generic repository interface
public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}

// Domain entities
public class Order { 
    public int Id { get; set; } 
    public string Description { get; set; } 
}
public class Customer { 
    public int Id { get; set; } 
    public string Name { get; set; } 
}

// Specific repository interfaces
public interface IOrderRepository : IRepository<Order> { }
public interface ICustomerRepository : IRepository<Customer> { }

// In-memory repository implementation
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _data = new();
    private int _nextId = 1;

    public T GetById(int id)
    {
        var prop = typeof(T).GetProperty("Id");
        return _data.FirstOrDefault(e => (int)prop.GetValue(e) == id);
    }
    public IEnumerable<T> GetAll() => _data;
    public void Add(T entity)
    {
        var prop = typeof(T).GetProperty("Id");
        if ((int)prop.GetValue(entity) == 0)
            prop.SetValue(entity, _nextId++);
        _data.Add(entity);
    }
    public void Update(T entity)
    {
        var prop = typeof(T).GetProperty("Id");
        int id = (int)prop.GetValue(entity);
        var idx = _data.FindIndex(e => (int)prop.GetValue(e) == id);
        if (idx >= 0) _data[idx] = entity;
    }
    public void Remove(T entity)
    {
        var prop = typeof(T).GetProperty("Id");
        int id = (int)prop.GetValue(entity);
        _data.RemoveAll(e => (int)prop.GetValue(e) == id);
    }
}

// Concrete repositories
// they're empty by design — they exist to provide concrete, domain-specific types while inheriting all behavior from the generic base.
// Details:
// InMemoryOrderRepository / InMemoryCustomerRepository inherit the full implementation from InMemoryRepository<T>,
// so no extra code is required for basic CRUD.
// They implement the domain-specific interfaces (IOrderRepository, ICustomerRepository).
// That makes it easy to register and resolve the specific repository types in DI
// (and keeps consumer code depending on meaningful interfaces instead of a generic type).
// Having these concrete classes gives you a place to add entity-specific behavior later (custom queries, validation, overrides)
// without changing the generic base.
// They act as clear domain markers — the code is more readable: IOrderRepository → InMemoryOrderRepository
// rather than wiring a generic type everywhere.
// They make testing/mocking and future swapping of implementations
// (e.g., to a database-backed repository) straightforward by replacing the concrete implementation in DI.
public class InMemoryOrderRepository : InMemoryRepository<Order>, IOrderRepository 
{
    
}
public class InMemoryCustomerRepository : InMemoryRepository<Customer>, ICustomerRepository 
{

}

// Unit of Work interface
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    ICustomerRepository Customers { get; }
    void Commit();
    void Rollback();
}

// In-memory Unit of Work implementation
public class InMemoryUnitOfWork : IUnitOfWork
{
    public IOrderRepository Orders { get; } = new InMemoryOrderRepository();
    public ICustomerRepository Customers { get; } = new InMemoryCustomerRepository();
    public void Commit() => Console.WriteLine("Committing changes (simulated).");
    public void Rollback() => Console.WriteLine("Rolling back changes (simulated).");
    public void Dispose() { }
}

