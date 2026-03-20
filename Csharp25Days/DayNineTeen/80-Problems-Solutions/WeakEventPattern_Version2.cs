// WeakEventPattern.cs
// Solution: simple weak-event helper using WeakReference to avoid strong reference leaks.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day19.Solutions
{
    // Very small weak-event helper for Action<T>
    public class WeakEvent<T>
    {
        private readonly List<WeakReference> _handlers = new();

        public void Subscribe(Action<T> handler)
        {
            _handlers.Add(new WeakReference(handler));
        }

        public void Publish(T payload)
        {
            // Clean dead references while invoking
            var alive = new List<WeakReference>();
            foreach (var wr in _handlers)
            {
                if (wr.Target is Action<T> act)
                {
                    try { act(payload); } catch { /* log and continue */ }
                    alive.Add(wr);
                }
            }
            _handlers.Clear();
            _handlers.AddRange(alive);
        }
    }

    public class WeakSubscriber
    {
        private readonly string _name;
        public WeakSubscriber(string name) => _name = name;
        public void Handle(string message) => Console.WriteLine($"{_name} handled: {message}");
    }

    public static class WeakEventPatternDemo
    {
        public static void Run()
        {
            var we = new WeakEvent<string>();

            var sub = new WeakSubscriber("S1");
            we.Subscribe(sub.Handle);

            we.Publish("first"); // S1 handled

            // Drop strong reference to subscriber and force GC
            // In practice GC is non-deterministic; this demonstrates intent
            // after sub = null and GC, the weak reference can be collected.
            // Note: Forcing GC in examples (not recommended in production) to demonstrate:
            // sub = null;
            // GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            // we.Publish("second"); // may no longer call S1
        }
    }
}