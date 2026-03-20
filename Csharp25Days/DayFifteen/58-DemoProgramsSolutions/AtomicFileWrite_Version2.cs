// AtomicFileWrite.cs
// Problem: AtomicFileWrite
// Write text atomically: write to temp file in same directory, flush, then replace target (File.Replace or File.Move).
// Complexity: O(n) I/O. Ensures target is not left partially written on crash.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class AtomicFileWrite
{
    public static async Task AtomicWriteTextAsync(string targetPath, string content)
    {
        var dir = Path.GetDirectoryName(targetPath) ?? ".";
        var tempPath = Path.Combine(dir, Path.GetRandomFileName());
        await File.WriteAllTextAsync(tempPath, content, Encoding.UTF8);

        // Flush by opening the file and flushing OS buffers (best effort)
        using (var fs = new FileStream(tempPath, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            fs.Flush(true);
        }

        try
        {
            if (File.Exists(targetPath))
            {
                // Try to replace atomically (Windows supports File.Replace)
                File.Replace(tempPath, targetPath, null);
            }
            else
            {
                File.Move(tempPath, targetPath);
            }
        }
        catch (PlatformNotSupportedException)
        {
            // Fallback: move over (may not be atomic across volumes)
            if (File.Exists(targetPath)) File.Delete(targetPath);
            File.Move(tempPath, targetPath);
        }
    }

    static async Task Main()
    {
        string path = "atomic.txt";
        await AtomicWriteTextAsync(path, "Hello atomic world\n");
        Console.WriteLine(await File.ReadAllTextAsync(path));
    }
}