// StreamLargeFileLines.cs
// Problem: StreamLargeFileLines
// Count lines containing a word using streaming (File.ReadLines) to avoid loading whole file.
// Complexity: O(n) time, O(1) memory for streaming processing.

using System;
using System.IO;
using System.Linq;

class StreamLargeFileLines
{
    public static long CountLinesContaining(string path, string word)
    {
        // File.ReadLines returns deferred enumerable and reads lazily
        return File.ReadLines(path).LongCount(line => line.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    static void Main()
    {
        string path = "large_example.txt";
        // Create a file with many lines (small example)
        File.WriteAllLines(path, Enumerable.Range(1, 1000).Select(i => i % 10 == 0 ? "match line" : "nope"));
        long count = CountLinesContaining(path, "match");
        Console.WriteLine($"Lines containing 'match': {count}");
    }
}