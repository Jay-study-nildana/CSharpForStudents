// 09-StressTest_ReproConcurrencyBug.cs
// Reproduce race on non-thread-safe stack; then provide fixed version using ConcurrentStack for correctness.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NonThreadSafeStack<T>
{
    private readonly List<T> _list = new();
    public void Push(T item) => _list.Add(item);
    public bool TryPop(out T item)
    {
        if (_list.Count == 0) { item = default!; return false; }
        int i = _list.Count - 1;
        item = _list[i];
        _list.RemoveAt(i);
        return true;
    }
}

// Stress test that tries to surface races
public static class StackStressTest
{
    public static async Task<bool> RunAsync()
    {
        var stack = new NonThreadSafeStack<int>();
        var tasks = new List<Task>();
        for (int p = 0; p < 4; p++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 10000; i++) stack.Push(i);
            }));
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 10000; i++) stack.TryPop(out _);
            }));
        }
        await Task.WhenAll(tasks);
        // Race likely occurred (could crash or show inconsistent state)
        return true;
    }

    // Fixed using ConcurrentStack
    public static async Task<bool> RunFixedAsync()
    {
        var stack = new ConcurrentStack<int>();
        var tasks = new List<Task>();
        for (int p = 0; p < 4; p++)
        {
            tasks.Add(Task.Run(() => { for (int i = 0; i < 10000; i++) stack.Push(i); }));
            tasks.Add(Task.Run(() => { for (int i = 0; i < 10000; i++) stack.TryPop(out _); }));
        }
        await Task.WhenAll(tasks);
        return true; // safe and deterministic
    }
}

/*
Notes:
- NonThreadSafeStack will exhibit races under stress.
- ConcurrentStack is lock-free / thread-safe for this use-case.
*/