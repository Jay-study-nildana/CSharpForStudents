using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Day09.ObserverMediator
{
    // Problem: Weak-reference observers to avoid leaks when subscribers are not unsubscribed.
    public class WeakSubject<T>
    {
        private readonly List<WeakReference<Action<T>>> _subscribers = new();

        public void Subscribe(Action<T> handler)
        {
            _subscribers.Add(new WeakReference<Action<T>>(handler));
        }

        public void Publish(T payload)
        {
            // Clean dead references as we go
            var alive = new List<WeakReference<Action<T>>>();
            foreach (var weak in _subscribers)
            {
                if (weak.TryGetTarget(out var target))
                {
                    try { target(payload); } catch (Exception ex) { Console.WriteLine($"Handler error: {ex.Message}"); }
                    alive.Add(weak);
                }
            }
            _subscribers.Clear();
            _subscribers.AddRange(alive);
        }
    }

    class Program
    {
        static void Main()
        {
            var subject = new WeakSubject<string>();

            void CreateAndSubscribe()
            {
                var local = new Subscriber("TempSubscriber");
                subject.Subscribe(local.Handle);
                // After this method returns, 'local' has no rooted references besides the weak reference
            }

            CreateAndSubscribe();

            // Force garbage collection to simulate subscriber being collected.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Now publish: weak subscriber should not be called (no exception)
            subject.Publish("Will any temp subscriber receive this?");

            // Subscribe a long-lived subscriber
            var persistent = new Subscriber("Persistent");
            subject.Subscribe(persistent.Handle);

            subject.Publish("Hello persistent subscriber!");
        }

        private class Subscriber
        {
            private readonly string _name;
            public Subscriber(string name) => _name = name;
            public void Handle(string msg) => Console.WriteLine($"[{_name}] Received: {msg}");
            ~Subscriber() => Console.WriteLine($"[{_name}] Finalized");
        }
    }
}