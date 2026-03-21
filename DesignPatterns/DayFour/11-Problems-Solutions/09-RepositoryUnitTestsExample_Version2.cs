// 09-RepositoryUnitTestsExample.cs
// Purpose: Provide conceptual unit-test code demonstrating how to test OrderService with a fake UnitOfWork and assertions.
// DI/Lifetime: Inject InMemoryUnitOfWork or mock IUnitOfWork into the service in tests.
// Testability note: Focus tests on behavior and calls to Commit/Rollback, and resulting repository state.

using System;
using System.Linq;

public static class RepositoryUnitTestsExample
{
    // Conceptual test (pseudo-code). Replace assertions with your test framework (xUnit/NUnit/MSTest).
    public static void Test_PlaceOrder_Commits()
    {
        // Arrange
        var uow = new InMemoryUnitOfWork();
        var svc = new OrderService(uow);
        var order = new Order { CustomerId = 42, Lines = { new OrderLine { Sku = "A", Quantity = 1, UnitPrice = 10 } } };

        // Act
        svc.PlaceOrder(order);

        // Assert
        // 1) UnitOfWork should have been committed
        if (!uow.Committed) throw new Exception("Expected commit to be called.");

        // 2) Order should be present in the Orders repository
        var all = uow.Orders.GetAll();
        if (!all.Any(o => o.CustomerId == 42)) throw new Exception("Order not found in repository.");
    }

    public static void Test_PlaceOrder_Exception_RollsBack()
    {
        // Arrange
        var uow = new InMemoryUnitOfWork();
        var svc = new OrderService(uow);
        var order = new Order { CustomerId = 99, Lines = { new OrderLine { Sku = "X", Quantity = 1, UnitPrice = 1 } } };

        // Act & Assert
        try
        {
            svc.PlaceOrderWithFailure(order);
            throw new Exception("Expected exception was not thrown.");
        }
        catch (InvalidOperationException)
        {
            // expected
        }

        if (!uow.RolledBack) throw new Exception("Expected rollback to be triggered.");
        // Note: In our simple InMemoryUnitOfWork we didn't revert repo changes; in a full transactional sim we'd assert absence.
    }
}