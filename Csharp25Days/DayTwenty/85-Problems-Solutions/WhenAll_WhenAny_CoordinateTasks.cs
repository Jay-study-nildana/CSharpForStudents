// WhenAll_WhenAny_CoordinateTasks.cs
// Demonstrates Task.WhenAny to use the fastest result and Task.WhenAll to gather all,
// with partial failure handling.

using System;
using System.Linq;
using System.Threading.Tasks;

class WhenDemo
{
    static async Task<string> FetchAsync(string name, int delay, bool fail = false)
    {
        await Task.Delay(delay);
        if (fail) throw new InvalidOperationException($"fetch {name} failed");
        return $"result-{name}";
    }

    public static async Task Main()
    {
        var t1 = FetchAsync("A", 300);
        var t2 = FetchAsync("B", 100);
        var t3 = FetchAsync("C", 500, fail: true);

        // Proceed with the fastest result
        var first = await Task.WhenAny(t1, t2, t3);
        try
        {
            Console.WriteLine($"First completed: {await first}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"First completed with exception: {ex.Message}");
        }

        // Gather all results; handle partial failures
        Task<string>[] all = new[] { t1, t2, t3 };
        try
        {
            var results = await Task.WhenAll(all); // will throw AggregateException-like behavior
            Console.WriteLine("All succeeded: " + string.Join(", ", results));
        }
        catch
        {
            // inspect each task individually
            for (int i = 0; i < all.Length; i++)
            {
                var t = all[i];
                if (t.IsFaulted) Console.WriteLine($"Task {i} faulted: {t.Exception?.InnerException?.Message}");
                else if (t.IsCanceled) Console.WriteLine($"Task {i} canceled");
                else Console.WriteLine($"Task {i} result: {t.Result}");
            }
        }
    }
}