// 07-OrderServiceWithUoW.cs
// Purpose: Implement OrderService that coordinates multiple repositories via IUnitOfWork to perform transactional operations.
// DI/Lifetime: OrderService is typically Scoped; depends on Scoped IUnitOfWork. For tests inject InMemoryUnitOfWork (Transient).
// Testability note: Tests can assert UnitOfWork.Committed and repository state after operations.

using System;

public class OrderService
{
    private readonly IUnitOfWork _uow;

    public OrderService(IUnitOfWork uow) => _uow = uow;

    /// <summary>
    /// Place an order: add order, adjust customer/inventory, and commit transaction.
    /// If an exception occurs, we call rollback to simulate transaction rollback.
    /// </summary>
    public void PlaceOrder(Order order)
    {
        try
        {
            // Example business operations:
            _uow.Orders.Add(order);

            // Simulate inventory/update operations on other repositories:
            // _uow.Inventory.Decrement(...); // omitted inventory repo for brevity

            // Commit all changes as a unit
            _uow.Commit();
        }
        catch (Exception)
        {
            // If any exception occurs, rollback
            _uow.Rollback();
            throw;
        }
    }

    /// <summary>Example showing rollback on failure.</summary>
    public void PlaceOrderWithFailure(Order order)
    {
        try
        {
            _uow.Orders.Add(order);
            // Simulate failure in a later step:
            throw new InvalidOperationException("Simulated downstream failure");
            // _uow.Commit(); // not reached
        }
        catch
        {
            _uow.Rollback();
            // rethrow or handle
            throw;
        }
    }
}

/*
Usage (conceptual):
var uow = new InMemoryUnitOfWork();
var svc = new OrderService(uow);
var order = new Order { CustomerId = 1, Lines = {...} };
svc.PlaceOrder(order);
Assert.True(uow.Committed);
*/