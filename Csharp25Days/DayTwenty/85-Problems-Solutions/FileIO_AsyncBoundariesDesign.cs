// FileIO_AsyncBoundariesDesign.cs
// Minimal example of reading multiple files asynchronously, processing on thread-pool, and uploading async.
// Illustrates async boundaries: file I/O async, CPU processing with Task.Run, upload async, and cancellation support.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Pipeline
{
    // Simulated async file read
    static async Task<string> ReadFileAsync(string path, CancellationToken ct)
    {
        using var sr = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes($"content-of-{Path.GetFileName(path)}")));
        // Simulate async read using ReadToEndAsync
        return await sr.ReadToEndAsync().ConfigureAwait(false);
    }

    // CPU-bound processing
    static string Process(string content)
    {
        // expensive CPU-bound transformation
        return content.ToUpperInvariant();
    }

    // Simulated async upload
    static async Task UploadAsync(string result, CancellationToken ct)
    {
        // simulated network delay
        await Task.Delay(100, ct).ConfigureAwait(false);
        Console.WriteLine($"Uploaded: {result.Substring(0, Math.Min(10, result.Length))}...");
    }

    public static async Task RunPipelineAsync(string[] files, CancellationToken ct)
    {
        // Start multiple file reads concurrently
        var readTasks = files.Select(f => ReadFileAsync(f, ct)).ToArray();

        // Await all reads without capturing context (library/data layer)
        string[] contents = await Task.WhenAll(readTasks).ConfigureAwait(false);

        // Process each result using Task.Run (CPU-bound)
        var processed = await Task.WhenAll(contents.Select(c => Task.Run(() => Process(c), ct))).ConfigureAwait(false);

        // Upload results concurrently using async uploads
        var uploadTasks = processed.Select(p => UploadAsync(p, ct));
        await Task.WhenAll(uploadTasks).ConfigureAwait(false);
    }

    public static async Task Main()
    {
        var files = Enumerable.Range(1, 5).Select(i => $"file{i}.txt").ToArray();
        using var cts = new CancellationTokenSource(1000); // cancel if too slow

        try
        {
            await RunPipelineAsync(files, cts.Token);
            Console.WriteLine("Pipeline completed.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Pipeline canceled.");
        }
    }
}