// ReadWriteTextFile.cs
// Problem: ReadWriteTextFile
// ReadAllText (UTF-8) and AppendLine (UTF-8) helpers.
// Complexity: O(n) for reading/writing where n = file size. Uses streaming and `using` for safety.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class ReadWriteTextFile
{
    public static string ReadAllTextUtf8(string path)
    {
        // Read all text with explicit UTF-8 encoding
        return File.ReadAllText(path, Encoding.UTF8);
    }

    public static async Task AppendLineUtf8Async(string path, string line)
    {
        // Append a line using StreamWriter (async) and UTF-8
        using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
        using var writer = new StreamWriter(stream, Encoding.UTF8);
        await writer.WriteLineAsync(line);
        await writer.FlushAsync();
    }

    static async Task Main()
    {
        string path = "text_example.txt";
        // Ensure file exists
        File.WriteAllText(path, "Line1\nLine2\n", Encoding.UTF8);

        Console.WriteLine("Before append:");
        Console.WriteLine(ReadAllTextUtf8(path));

        await AppendLineUtf8Async(path, "Appended line");

        Console.WriteLine("After append:");
        Console.WriteLine(ReadAllTextUtf8(path));
    }
}