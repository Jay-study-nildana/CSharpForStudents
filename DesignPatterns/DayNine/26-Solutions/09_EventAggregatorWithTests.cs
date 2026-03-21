using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: Event aggregator with simple built-in assertions (no external test runner).
    public class EventAggregator
    {
        private readonly Dictionary<string, List<Action<object?>>> _handlers = new();

        public void Subscribe(string topic, Action<object?> handler)
        {
            if (!_handlers.TryGetValue(topic, out var list))
            {
                list = new List<Action<object?>>();
                _handlers[topic] = list;
            }
            list.Add(handler);
        }

        public void Publish(string topic, object? payload = null)
        {
            if (_handlers.TryGetValue(topic, out var list))
            {
                foreach (var h in list.ToArray())
                {
                    h(payload);
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var ea = new EventAggregator();
            var log = new List<string>();

            ea.Subscribe("seq", p => log.Add($"A:{p}"));
            ea.Subscribe("seq", p => log.Add($"B:{p}"));

            ea.Publish("seq", 1);
            ea.Publish("seq", 2);

            // Simple assertions:
            Assert(log.Count == 4, "Expected 4 events");
            Assert(log[0] == "A:1", "First event must be A:1");
            Assert(log[1] == "B:1", "Second event must be B:1");
            Console.WriteLine("All assertions passed.");
        }

        static void Assert(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException($"Assertion failed: {message}");
        }
    }
}