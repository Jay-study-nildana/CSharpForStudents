// AsyncAllTheWay_API.cs
// Demonstrates "async all the way" in a web-like API handler versus a bad blocking version.

using System;
using System.Threading.Tasks;

class ApiExample
{
    // Simulated async data fetch
    static async Task<string> GetDataAsync()
    {
        await Task.Delay(200); // I/O
        return "payload";
    }

    // BAD: synchronous, blocks thread-pool and harms throughput
    public static string BadHandler()
    {
        // Blocking on an async method
        return GetDataAsync().Result; // blocks
    }

    // GOOD: async all the way — non-blocking
    public static async Task<string> GoodHandler()
    {
        return await GetDataAsync();
    }

    public static async Task Main()
    {
        Console.WriteLine("BadHandler (blocking):");
        try
        {
            Console.WriteLine(BadHandler());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BadHandler threw: {ex.Message}");
        }

        Console.WriteLine("GoodHandler (async):");
        Console.WriteLine(await GoodHandler());
    }
}