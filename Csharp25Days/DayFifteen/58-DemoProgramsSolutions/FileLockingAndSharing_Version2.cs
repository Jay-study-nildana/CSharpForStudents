// FileLockingAndSharing.cs
// Problem: FileLockingAndSharing
// Demonstrate exclusive write (FileShare.None) and a reader that attempts to open concurrently.
// Complexity: O(1) for opening; conflicts raise IOException which should be handled (retry/backoff).

using System;
using System.IO;
using System.Threading;

class FileLockingAndSharing
{
    static void WriterExclusive(string path)
    {
        using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        using var w = new StreamWriter(fs);
        w.WriteLine($"Written at {DateTime.UtcNow}");
        w.Flush();
        Console.WriteLine("Writer: wrote and holds exclusive lock for 3s");
        Thread.Sleep(3000); // hold lock to simulate long write
    }

    static void ReaderAttempt(string path)
    {
        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var r = new StreamReader(fs);
            Console.WriteLine("Reader: opened file successfully, content:");
            Console.WriteLine(r.ReadToEnd());
        }
        catch (IOException ex)
        {
            Console.WriteLine("Reader: failed to open file due to lock: " + ex.Message);
            // Option: retry with backoff
        }
    }

    static void Main()
    {
        string path = "locktest.txt";
        File.WriteAllText(path, "Initial content\n");
        var writerThread = new Thread(() => WriterExclusive(path));
        writerThread.Start();

        // Slight delay so writer obtains lock
        Thread.Sleep(200);

        // Reader runs while writer holds exclusive lock
        ReaderAttempt(path);

        writerThread.Join();
        // Now reader can open
        ReaderAttempt(path);
    }
}