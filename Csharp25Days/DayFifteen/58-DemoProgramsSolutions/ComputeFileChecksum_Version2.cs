// ComputeFileChecksum.cs
// Problem: ComputeFileChecksum
// Compute SHA256 checksum of a file using streaming (no full file in memory).
// Complexity: O(n) I/O bound. Useful to detect corruption.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class ComputeFileChecksum
{
    public static async Task<byte[]> ComputeSha256Async(string path)
    {
        using var stream = File.OpenRead(path);
        using var sha = SHA256.Create();
        return await sha.ComputeHashAsync(stream);
    }

    public static string BytesToHex(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();

    static async Task Main()
    {
        string path = "checksum_example.txt";
        await File.WriteAllTextAsync(path, "content for checksum", Encoding.UTF8);
        var hash = await ComputeSha256Async(path);
        Console.WriteLine("SHA256: " + BytesToHex(hash));
    }
}