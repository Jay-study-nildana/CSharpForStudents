using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: EventBus with predicate filtering per subscriber.
    public class FilteredEventBus
    {
        private readonly Dictionary<string, List<(Func<object?, bool> filter, Action<object?> handler)>> _topics = new();

        public void Subscribe(string topic, Func<object?, bool>? filter, Action<object?> handler)
        {
            if (!_topics.TryGetValue(topic, out var list))
            {
                list = new List<(Func<object?, bool>, Action<object?>)>();
                _topics[topic] = list;
            }
            list.Add((filter ?? (_ => true), handler));
        }

        public void Publish(string topic, object? payload = null)
        {
            if (!_topics.TryGetValue(topic, out var list)) return;
            foreach (var (filter, handler) in list.ToArray())
            {
                try
                {
                    if (filter(payload)) handler(payload);
                }
                catch (Exception ex) { Console.WriteLine($"Filtered handler error: {ex.Message}"); }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var bus = new FilteredEventBus();

            // subscriber only interested in integers > 10
            bus.Subscribe("numbers", p => p is int n && n > 10, p => Console.WriteLine($"Large number: {p}"));

            // subscriber for string messages
            bus.Subscribe("numbers", p => p is string, p => Console.WriteLine($"String payload: {p}"));

            bus.Publish("numbers", 5);
            bus.Publish("numbers", 42);
            bus.Publish("numbers", "forty-two");
        }
    }
}