using System;
using System.Collections.Generic;

//
// Problem: EventBus / Observer Integration
// Test plan: subscribe multiple handlers and ensure they receive events; demo prints both handlers invoked.
// Demonstrates: lightweight event aggregator for decoupling.
//

namespace Day11.RefactorLab
{
    public class OrderPlaced { public int OrderId; public string Customer; }

    public interface IEventBus
    {
        void Publish<T>(T evt);
        void Subscribe<T>(Action<T> handler);
    }

    public class SimpleEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();
        public void Publish<T>(T evt)
        {
            if (_handlers.TryGetValue(typeof(T), out var list))
            {
                foreach (Action<T> h in list) h(evt);
            }
        }
        public void Subscribe<T>(Action<T> handler)
        {
            var list = _handlers.TryGetValue(typeof(T), out var l) ? l : new List<Delegate>();
            list.Add(handler);
            _handlers[typeof(T)] = list;
        }
    }

    class Program
    {
        static void Main()
        {
            var bus = new SimpleEventBus();
            bus.Subscribe<OrderPlaced>(e => Console.WriteLine($"Email sent to {e.Customer} for order {e.OrderId}"));
            bus.Subscribe<OrderPlaced>(e => Console.WriteLine($"Analytics logged for order {e.OrderId}"));

            bus.Publish(new OrderPlaced { OrderId = 1001, Customer = "Alice" });
        }
    }
}