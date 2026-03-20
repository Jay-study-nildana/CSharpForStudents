// TaskRun_ForIO_vsCPU.cs
// Shows correct use of Task.Run for CPU-bound work and incorrect wrapping of I/O.

using System;
using System.Threading.Tasks;

class TaskRunExamples
{
    // CPU-bound synchronous compute
    static int ComputePricey(int n)
    {
        // expensive CPU work
        var sum = 0;
        for (var i = 0; i < n; i++) sum += i;
        return sum;
    }

    // Correct: offload CPU-bound work to thread pool
    public static Task<int> ComputePriceyAsync(int n)
    {
        return Task.Run(() => ComputePricey(n));
    }

    // Simulated I/O-bound async method
    public static async Task<string> ReadRemoteAsync()
    {
        await Task.Delay(200); // simulate non-blocking I/O
        return "payload";
    }

    // Wrong: wrapping I/O-bound method in Task.Run (wastes a thread)
    public static Task<string> WrongWrapIOAsync()
    {
        // This consumes a thread-pool thread unnecessarily; prefer calling ReadRemoteAsync directly.
        return Task.Run(() => ReadRemoteAsync().GetAwaiter().GetResult());
    }

    public static async Task Main()
    {
        int cpuResult = await ComputePriceyAsync(1000000);
        Console.WriteLine($"CPU result: {cpuResult}");

        string ioResult = await ReadRemoteAsync(); // preferred
        Console.WriteLine($"I/O result: {ioResult}");

        string wrong = await WrongWrapIOAsync(); // works but costlier
        Console.WriteLine($"Wrong wrapped I/O result: {wrong}");
    }
}