// SafeResourceHandling.cs
// Problem: SafeResourceHandling
// Show resource cleanup using `using` and `try/finally`. Demonstrate FileNotFound handling.

using System;
using System.IO;
using System.Text;

class SafeResourceHandling
{
    // Using variant (recommended)
    public static string ReadAllTextUsing(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var sr = new StreamReader(fs, Encoding.UTF8);
        return sr.ReadToEnd();
    }

    // try/finally variant
    public static string ReadAllTextTryFinally(string path)
    {
        FileStream? fs = null;
        StreamReader? sr = null;
        try
        {
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(fs, Encoding.UTF8);
            return sr.ReadToEnd();
        }
        finally
        {
            sr?.Dispose();
            fs?.Dispose();
        }
    }

    static void Main()
    {
        var path = "nonexistent.txt";
        try
        {
            var text = ReadAllTextUsing(path);
            Console.WriteLine(text);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File missing: {ex.FileName}");
            // handle or rethrow depending on context
        }
    }
}