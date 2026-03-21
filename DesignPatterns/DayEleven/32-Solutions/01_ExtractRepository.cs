using System;
using System.Collections.Generic;

//
// Problem: Extract Repository
// Test plan: Verify Save and GetById use repository and not inline persistence; demo prints same result.
// Demonstrates: IOrderRepository extraction and InMemoryOrderRepository.
//

namespace Day11.RefactorLab
{
    public class Order { public int Id; public string Item; public int Qty; }

    // BEFORE: service had inline storage logic (simulated).
    // AFTER: extract into repository interface.

    public interface IOrderRepository
    {
        void Save(Order order);
        Order? GetById(int id);
    }

    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly Dictionary<int, Order> _store = new();
        public void Save(Order order) => _store[order.Id] = order;
        public Order? GetById(int id) => _store.TryGetValue(id, out var o) ? o : null;
    }

    public class OrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) => _repo = repo;

        public void PlaceOrder(Order order)
        {
            // business logic...
            _repo.Save(order);
            Console.WriteLine($"Order {order.Id} saved via repository.");
        }

        public void PrintOrder(int id)
        {
            var o = _repo.GetById(id);
            Console.WriteLine(o == null ? "Not found" : $"Order {o.Id}: {o.Item} x{o.Qty}");
        }
    }

    class Program
    {
        static void Main()
        {
            var repo = new InMemoryOrderRepository();
            var svc = new OrderService(repo);

            svc.PlaceOrder(new Order { Id = 1, Item = "Widget", Qty = 3 });
            svc.PrintOrder(1);
        }
    }
}