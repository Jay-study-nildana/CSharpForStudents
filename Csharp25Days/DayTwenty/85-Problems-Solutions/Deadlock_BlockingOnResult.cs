// Deadlock_BlockingOnResult.cs
// Minimal demonstration of a deadlock when blocking on async with .Result
// and the fixed version using async/await "all the way".

using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Simulated UI SynchronizationContext: single-threaded scheduler
    static void Main()
    {
        var uiThread = new Thread(UIThreadStart) { IsBackground = false };
        uiThread.Start();
    }

    static void UIThreadStart()
    {
        // Install a simple SynchronizationContext that forces continuations back to this thread.
        SynchronizationContext.SetSynchronizationContext(new SingleThreadSyncContext(Thread.CurrentThread));

        Console.WriteLine("UI thread starting...");

        // BAD: blocking call on UI thread — can deadlock
        try
        {
            Console.WriteLine("Calling BadGetAsync().Result (may deadlock) ...");
            var res = BadGetAsync().Result; // blocking call
            Console.WriteLine($"Bad result: {res}");
        }
        catch (AggregateException ex)
        {
            Console.WriteLine($"Bad path exception: {ex.Flatten().InnerException}");
        }

        // GOOD: use async all the way
        Console.WriteLine("Calling GoodUseAsync() — uses await correctly.");
        GoodUseAsync().GetAwaiter().GetResult();

        Console.WriteLine("Done. Press Enter to quit.");
        Console.ReadLine();
    }

    // Async method that by default tries to resume on the captured context
    static async Task<string> BadGetAsync()
    {
        await Task.Delay(100).ConfigureAwait(true); // default capture (true for illustration)
        return "done";
    }

    static async Task GoodUseAsync()
    {
        var res = await BadGetAsync().ConfigureAwait(false); // library code would use false
        Console.WriteLine($"Good result: {res}");
    }
}

// Very small single-threaded SynchronizationContext for demonstration.
// It queues work but only executes when Post/Send is processed on the thread.
class SingleThreadSyncContext : SynchronizationContext
{
    private readonly Thread _owner;
    public SingleThreadSyncContext(Thread owner) => _owner = owner;

    public override void Post(SendOrPostCallback d, object state)
    {
        // For demo we run synchronously on the owner thread (simulate marshal back).
        if (Thread.CurrentThread == _owner)
        {
            d(state);
            return;
        }

        // In more complete implementations you'd queue and pump; simplified here.
        throw new InvalidOperationException("Posting to UI from background not supported in this shim.");
    }
}