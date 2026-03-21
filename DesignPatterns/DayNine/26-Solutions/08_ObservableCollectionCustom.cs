using System;
using System.Collections.Generic;
using System.Threading;

namespace Day09.ObserverMediator
{
    // Problem: Minimal ObservableCollection<T> with CollectionChanged events.
    public enum CollectionChangeAction { Add, Remove }

    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public CollectionChangeAction Action { get; }
        public T Item { get; }

        public CollectionChangedEventArgs(CollectionChangeAction action, T item)
        {
            Action = action;
            Item = item;
        }
    }

    public class SimpleObservableCollection<T>
    {
        private readonly List<T> _items = new();
        private readonly object _lock = new();

        public event EventHandler<CollectionChangedEventArgs<T>>? CollectionChanged;

        public void Add(T item)
        {
            lock (_lock)
            {
                _items.Add(item);
            }
            CollectionChanged?.Invoke(this, new CollectionChangedEventArgs<T>(CollectionChangeAction.Add, item));
        }

        public bool Remove(T item)
        {
            var removed = false;
            lock (_lock)
            {
                removed = _items.Remove(item);
            }
            if (removed) CollectionChanged?.Invoke(this, new CollectionChangedEventArgs<T>(CollectionChangeAction.Remove, item));
            return removed;
        }

        public IReadOnlyList<T> Snapshot()
        {
            lock (_lock) return _items.ToArray();
        }
    }

    class Program
    {
        static void Main()
        {
            var col = new SimpleObservableCollection<string>();
            col.CollectionChanged += (s, e) =>
            {
                Console.WriteLine($"Collection changed: {e.Action} {e.Item}");
            };

            col.Add("A");
            col.Add("B");
            col.Remove("A");
            Console.WriteLine("Snapshot: " + string.Join(",", col.Snapshot()));
        }
    }
}