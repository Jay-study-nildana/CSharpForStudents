// BoundedRecentActivityGeneric.cs
// Problem: BoundedRecentActivityGeneric
// Keep the last N items (newest first). Add O(1), GetRecent O(n) to enumerate.
// Uses LinkedList<T> to efficiently add/remove ends.

using System;
using System.Collections.Generic;

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

    // Returns newest -> oldest
    public IEnumerable<T> GetRecent() => _list;

    static void Main()
    {
        var recent = new RecentActivity<string>(3);
        recent.Add("a"); recent.Add("b"); recent.Add("c"); recent.Add("d");
        Console.WriteLine(string.Join(", ", recent.GetRecent())); // d, c, b
    }
}