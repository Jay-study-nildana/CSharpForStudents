using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: EventBus / EventAggregator with topic-based subscriptions.
    public class EventBus
    {
        private readonly Dictionary<string, List<Action<object?>>> _topics = new();

        public void Subscribe(string topic, Action<object?> handler)
        {
            if (!_topics.TryGetValue(topic, out var list))
            {
                list = new List<Action<object?>>();
                _topics[topic] = list;
            }
            list.Add(handler);
        }

        public void Unsubscribe(string topic, Action<object?> handler)
        {
            if (_topics.TryGetValue(topic, out var list)) list.Remove(handler);
        }

        public void Publish(string topic, object? payload = null)
        {
            if (_topics.TryGetValue(topic, out var list))
            {
                foreach (var h in list.ToArray())
                {
                    try { h(payload); } catch (Exception ex) { Console.WriteLine($"Handler error: {ex.Message}"); }
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var bus = new EventBus();

            bus.Subscribe("user:login", p => Console.WriteLine($"[Auth] Welcome {(p as string)}"));
            bus.Subscribe("doc:updated", p => Console.WriteLine($"[Indexer] Doc updated: {p}"));

            bus.Publish("user:login", "alice");
            bus.Publish("doc:updated", "doc-42");
        }
    }
}