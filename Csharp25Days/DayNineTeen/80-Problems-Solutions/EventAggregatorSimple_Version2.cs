// EventAggregatorSimple.cs
// Solution: minimal in-process EventAggregator for publish/subscribe.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Day19.Solutions
{
    public class EventAggregator
    {
        private readonly ConcurrentDictionary<Type, List<Delegate>> _subs = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var list = _subs.GetOrAdd(typeof(T), _ => new List<Delegate>());
            lock (list) { list.Add(handler); }
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            if (_subs.TryGetValue(typeof(T), out var list))
            {
                lock (list) { list.Remove(handler); }
            }
        }

        public void Publish<T>(T @event)
        {
            if (_subs.TryGetValue(typeof(T), out var list))
            {
                Delegate[] snapshot;
                lock (list) { snapshot = list.ToArray(); }
                foreach (Action<T> handler in snapshot)
                {
                    try { handler(@event); } catch { /* log and continue */ }
                }
            }
        }
    }

    public static class EventAggregatorSimple
    {
        public static void Run()
        {
            var agg = new EventAggregator();
            agg.Subscribe<string>(s => Console.WriteLine($"Subscriber A got: {s}"));
            agg.Subscribe<string>(s => Console.WriteLine($"Subscriber B got: {s}"));

            agg.Publish("Hello PubSub"); // both handlers invoked
        }
    }
}