// ConfigureAwait_InLibrary.cs
// Demonstrates a library-style async method using ConfigureAwait(false)
// and explains where to use it.

using System;
using System.Net.Http;
using System.Threading.Tasks;

class LibraryExample
{
    // Library method - should avoid capturing caller's context
    public static async Task<string> FetchContentAsync(string url)
    {
        using var client = new HttpClient();
        // Avoid capturing the caller's SynchronizationContext in library code.
        return await client.GetStringAsync(url).ConfigureAwait(false);
    }

    // Application-level usage where we need the context (e.g., UI update)
    public static async Task ApplicationUsageAsync()
    {
        // On UI app you'd capture context here so that UI updates work after await.
        string content = await FetchContentAsync("https://example.com"); // caller's context is captured (unless ConfigureAwait(false) used here)
        Console.WriteLine($"Length: {content.Length}");
        // If this were a UI app, you could update UI elements here safely.
    }

    public static async Task Main()
    {
        await ApplicationUsageAsync();
    }
}