// 01-RaceCondition_Counter.cs
// Two safe implementations: one using lock, one using Interlocked.

using System;
using System.Threading;

public class CounterWithLock
{
    private int _value;
    private readonly object _sync = new();

    public void Increment()
    {
        lock (_sync)
        {
            _value++; // protected critical section
        }
    }

    public int Value
    {
        get { lock (_sync) { return _value; } }
    }
}

public class CounterWithInterlocked
{
    private int _value;

    public void Increment() => Interlocked.Increment(ref _value); // atomic

    public int Value => Volatile.Read(ref _value);
}

/*
Trade-offs:
- lock: flexible for complex invariants, slightly higher overhead.
- Interlocked: low-overhead for simple primitive updates, limited to atomic ops.
*/