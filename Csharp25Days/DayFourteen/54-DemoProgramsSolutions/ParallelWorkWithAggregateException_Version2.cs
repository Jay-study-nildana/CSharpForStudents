// ParallelWorkWithAggregateException.cs
// Problem: ParallelWorkWithAggregateException
// Run several tasks where some throw, show handling of AggregateException via WaitAll and via await patterns.

using System;
using System.Linq;
using System.Threading.Tasks;

class ParallelWorkWithAggregateException
{
    static Task WorkAsync(int id)
    {
        return Task.Run(() =>
        {
            if (id % 2 == 0) throw new InvalidOperationException($"Fail {id}");
            Console.WriteLine($"Work {id} done");
        });
    }

    static void HandleWithWaitAll()
    {
        var tasks = Enumerable.Range(1, 4).Select(WorkAsync).ToArray();
        try
        {
            // Using WaitAll will wrap exceptions in AggregateException
            Task.WaitAll(tasks);
        }
        catch (AggregateException ae)
        {
            Console.WriteLine("AggregateException caught via WaitAll:");
            foreach (var ex in ae.InnerExceptions) Console.WriteLine($" - {ex.GetType().Name}: {ex.Message}");
        }
    }

    static async Task HandleWithAwait()
    {
        var tasks = Enumerable.Range(1, 4).Select(WorkAsync).ToArray();
        try
        {
            // Awaiting WhenAll will rethrow a single exception if one faulted;
            // but the thrown exception is the *first* observed; to inspect all, examine Task.Exception.
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Await threw: " + ex.Message);
            var agg = tasks.Where(t => t.IsFaulted).Select(t => t.Exception).Where(e => e!=null).ToArray();
            foreach (var a in agg) foreach (var inner in a!.InnerExceptions) Console.WriteLine($" - {inner.Message}");
        }
    }

    static async Task MainAsync()
    {
        HandleWithWaitAll();
        await HandleWithAwait();
    }

    static void Main() => MainAsync().GetAwaiter().GetResult();
}