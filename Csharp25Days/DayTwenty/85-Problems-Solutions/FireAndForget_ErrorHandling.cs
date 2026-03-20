// FireAndForget_ErrorHandling.cs
// Safe fire-and-forget helper that observes/logs exceptions.

using System;
using System.Threading.Tasks;

static class FireAndForgetHelper
{
    // Basic logger (replace with real logging in apps)
    static void Log(Exception ex) => Console.WriteLine($"[Log] {ex.GetType()}: {ex.Message}");

    // Fire-and-forget wrapper ensures exceptions are observed
    public static void FireAndForgetSafe(this Task task)
    {
        // Attach continuation on thread-pool to observe exceptions
        _ = task.ContinueWith(t =>
        {
            if (t.IsFaulted && t.Exception != null)
                Log(t.Exception.Flatten().InnerException ?? t.Exception);
        }, TaskScheduler.Default);
    }
}

class Demo
{
    static async Task FaultyAsync()
    {
        await Task.Delay(50);
        throw new InvalidOperationException("boom");
    }

    public static async Task Main()
    {
        // Launch without awaiting — but use helper to log errors
        FaultyAsync().FireAndForgetSafe();

        Console.WriteLine("Fire-and-forget launched.");
        await Task.Delay(200); // wait to allow background task to run
    }
}