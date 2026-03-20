// 10-ThreadPoolStarvation_LongRunningTasks.cs
// Demonstrate scheduling long-running CPU work without starving thread-pool for async continuations.

using System;
using System.Threading.Tasks;

public class LongRunningDemo
{
    // Dangerous: many Task.Run (default) long-running tasks may occupy thread-pool threads
    public Task StartManyShortTasksBad(int count)
    {
        var tasks = new Task[count];
        for (int i = 0; i < count; i++)
        {
            tasks[i] = Task.Run(() => { ThreadWork(); }); // may use thread-pool threads
        }
        return Task.WhenAll(tasks);
    }

    // Better: mark as LongRunning to request dedicated thread per task
    public Task StartManyLongRunningGood(int count)
    {
        var tasks = new Task[count];
        for (int i = 0; i < count; i++)
        {
            tasks[i] = Task.Factory.StartNew(() => ThreadWork(), TaskCreationOptions.LongRunning);
        }
        return Task.WhenAll(tasks);
    }

    private void ThreadWork()
    {
        // simulate CPU-bound long work
        Thread.SpinWait(5000000); // heavy CPU work
    }
}

/*
Why:
- Long-running CPU tasks can monopolize the thread pool and delay async continuations.
- TaskCreationOptions.LongRunning asks for dedicated threads to avoid starving the pool.
- Alternatively, use a dedicated thread or a custom TaskScheduler for heavy background workers.
*/