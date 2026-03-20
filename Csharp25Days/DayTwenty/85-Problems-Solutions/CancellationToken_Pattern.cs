// CancellationToken_Pattern.cs
// Shows cooperative cancellation with CancellationToken and handling OperationCanceledException.

using System;
using System.Threading;
using System.Threading.Tasks;

class CancellationDemo
{
    public static async Task<int> WorkAsync(CancellationToken ct)
    {
        // Pass cancellation token to cancellable operations
        for (int i = 0; i < 10; i++)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(200, ct); // cancellable delay
        }
        return 42;
    }

    public static async Task Main()
    {
        using var cts = new CancellationTokenSource();
        var task = WorkAsync(cts.Token);

        // Cancel after a short delay
        _ = Task.Run(async () =>
        {
            await Task.Delay(600);
            cts.Cancel();
            Console.WriteLine("Cancellation requested.");
        });

        try
        {
            int result = await task;
            Console.WriteLine($"Completed: {result}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was canceled (caught OperationCanceledException).");
        }
    }
}