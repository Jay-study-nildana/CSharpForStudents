// ConstructorInjectionRefactor.cs
using System;

namespace DI.Exercises
{
    // Original anti-pattern (brief): class that 'new's its dependency
    /*
    public class OrderProcessorBad
    {
        public void Process() {
            var repo = new OrderRepository(); // hard-coded creation
            // use repo...
        }
    }
    */

    // Refactored: constructor injection with interface
    public interface IOrderRepository
    {
        void Save(string order);
    }

    public class OrderRepository : IOrderRepository
    {
        public void Save(string order) => Console.WriteLine($"Saved: {order}");
    }

    public class OrderProcessor
    {
        private readonly IOrderRepository _repo;
        public OrderProcessor(IOrderRepository repo) => _repo = repo;
        public void Process(string order) => _repo.Save(order);
    }

    // Usage example:
    // var repo = new OrderRepository();
    // var processor = new OrderProcessor(repo);
    // processor.Process("order-123");

    // Note: Refactoring to constructor injection decouples OrderProcessor from concrete
    // implementations and enables easy substitution with fakes/mocks for unit testing.
}