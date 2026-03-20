// ReadWriteBinaryFile.cs
// Problem: ReadWriteBinaryFile
// Write/read an int[] to/from a binary file using BinaryWriter/BinaryReader.
// Complexity: O(n) where n = number of elements. Note: BinaryWriter writes values in little-endian on current platform.

using System;
using System.IO;

class ReadWriteBinaryFile
{
    public static void WriteInts(string path, int[] data)
    {
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        using var w = new BinaryWriter(fs);
        w.Write(data.Length);
        foreach (var v in data) w.Write(v); // current platform endianness (little-endian on x86/x64)
        w.Flush();
        fs.Flush(true);
    }

    public static int[] ReadInts(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var r = new BinaryReader(fs);
        int len = r.ReadInt32();
        var res = new int[len];
        for (int i = 0; i < len; i++) res[i] = r.ReadInt32();
        return res;
    }

    static void Main()
    {
        string path = "ints.bin";
        var data = new[] { 1, 2, 3, 42, 1000 };
        WriteInts(path, data);
        var read = ReadInts(path);
        Console.WriteLine("Read ints: " + string.Join(", ", read));
    }
}