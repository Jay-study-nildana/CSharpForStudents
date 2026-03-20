// Problem: RecentActivityBoundedQueue
// Keep last N events. Use Queue<T> or LinkedList<T> + limit.
// Complexity: Add O(1), GetRecent O(n) to copy or O(1) to iterate.

using System;
using System.Collections.Generic;
using System.Linq;

class RecentActivity<T>
{
    private readonly int _capacity;
    private readonly LinkedList<T> _list = new();

    public RecentActivity(int capacity) => _capacity = Math.Max(1, capacity);

    public void Add(T item)
    {
        _list.AddFirst(item);
        if (_list.Count > _capacity) _list.RemoveLast();
    }

    public IEnumerable<T> GetRecent() => _list; // already from newest to oldest

    static void Main()
    {
        var recent = new RecentActivity<string>(3);
        recent.Add("a"); recent.Add("b"); recent.Add("c"); recent.Add("d");
        Console.WriteLine(string.Join(", ", recent.GetRecent())); // d, c, b
    }
}