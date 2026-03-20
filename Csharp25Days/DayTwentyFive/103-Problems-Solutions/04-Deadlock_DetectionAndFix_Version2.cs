// 04-Deadlock_DetectionAndFix.cs
// Enforce consistent lock ordering to avoid deadlocks.

using System;
using System.Threading;

public class DeadlockFix
{
    private readonly object _a = new();
    private readonly object _b = new();

    // Acquire locks always in same order: _a then _b
    public void Operation1()
    {
        lock (_a)
        {
            // do work that needs both
            lock (_b)
            {
                /* safe: consistent ordering */
            }
        }
    }

    public void Operation2()
    {
        // also acquire _a then _b to preserve global order
        lock (_a)
        {
            lock (_b)
            {
                /* safe: consistent ordering */
            }
        }
    }

    // Alternative: use TryEnter with timeout and a back-off to avoid circular waits.
    public bool TryDoWorkWithTimeout(int timeoutMs)
    {
        bool gotA = false, gotB = false;
        try
        {
            gotA = Monitor.TryEnter(_a, timeoutMs);
            if (!gotA) return false;
            gotB = Monitor.TryEnter(_b, timeoutMs);
            if (!gotB) return false;
            // do work
            return true;
        }
        finally
        {
            if (gotB) Monitor.Exit(_b);
            if (gotA) Monitor.Exit(_a);
        }
    }
}

/*
Why this prevents deadlocks:
- Consistent lock ordering prevents cycles in lock graph.
- TryEnter with timeout avoids permanent waits and allows retry/back-off.
*/