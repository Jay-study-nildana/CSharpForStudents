using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: Push-style observer where subject pushes payload to subscribers.
    public class Subject<T>
    {
        private readonly List<Action<T>> _subscribers = new();

        public void Subscribe(Action<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _subscribers.Add(handler);
        }

        public void Unsubscribe(Action<T> handler)
        {
            _subscribers.Remove(handler);
        }

        public void Notify(T payload)
        {
            // Make a copy to be safe if subscribers modify during iteration
            foreach (var h in _subscribers.ToArray())
            {
                try { h(payload); }
                catch (Exception ex) { Console.WriteLine($"Subscriber threw: {ex.Message}"); }
            }
        }
    }

    public record Notification(string User, string Message);

    class Program
    {
        static void Main()
        {
            var subject = new Subject<Notification>();

            subject.Subscribe(n => Console.WriteLine($"[Logger] {n.User}: {n.Message}"));
            subject.Subscribe(n => Console.WriteLine($"[UI] Displaying: {n.Message} (from {n.User})"));

            subject.Notify(new Notification("Alice", "Hello, team."));
            subject.Notify(new Notification("Bob", "Build complete."));
        }
    }
}