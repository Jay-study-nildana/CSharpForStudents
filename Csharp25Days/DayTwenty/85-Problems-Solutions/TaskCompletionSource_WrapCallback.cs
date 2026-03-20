// TaskCompletionSource_WrapCallback.cs
// Wraps a callback-style API using TaskCompletionSource<T>.

using System;
using System.Threading.Tasks;

class CallbackApi
{
    // Simulated callback API that fires after delay
    public static void BeginOperation(Action<string> onSuccess, Action<Exception> onError)
    {
        Task.Run(async () =>
        {
            await Task.Delay(200);
            onSuccess?.Invoke("callback result");
        });
    }
}

class Wrapper
{
    public static Task<string> OperationAsync()
    {
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        try
        {
            CallbackApi.BeginOperation(
                result => tcs.TrySetResult(result),
                ex => tcs.TrySetException(ex)
            );
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }

        return tcs.Task;
    }

    public static async Task Main()
    {
        string r = await OperationAsync();
        Console.WriteLine($"Got: {r}");
    }
}