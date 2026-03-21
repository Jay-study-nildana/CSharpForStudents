using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: Pull-style observer where observers are signalled and then pull the subject state.
    public class PullSubject
    {
        private readonly List<Action> _observers = new();
        private int _counter;

        public void Subscribe(Action observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            _observers.Add(observer);
        }

        public void Unsubscribe(Action observer) => _observers.Remove(observer);

        public void Increment()
        {
            _counter++;
            Notify();
        }

        public int GetState() => _counter;

        private void Notify()
        {
            foreach (var o in _observers.ToArray())
            {
                try { o(); } catch (Exception ex) { Console.WriteLine($"Observer exception: {ex.Message}"); }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var subj = new PullSubject();

            subj.Subscribe(() =>
            {
                var state = subj.GetState();
                Console.WriteLine($"Observer A pulled state: {state}");
            });

            subj.Subscribe(() =>
            {
                var state = subj.GetState();
                Console.WriteLine($"Observer B pulled state (double-check): {state}");
            });

            subj.Increment();
            subj.Increment();
        }
    }
}