// BackupAndRestoreFiles.cs
// Problem: BackupAndRestoreFiles
// Copy files into timestamped backup folder and restore verifying SHA256 checksums before overwrite.
// Complexity: O(total bytes) I/O. Ensures backups are separate snapshots; verify integrity using checksums.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class BackupAndRestoreFiles
{
    public static async Task<string> BackupFilesAsync(IEnumerable<string> paths, string backupRoot)
    {
        var ts = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var dest = Path.Combine(backupRoot, ts);
        Directory.CreateDirectory(dest);

        var manifest = new List<(string RelPath, string Checksum)>();

        foreach (var p in paths)
        {
            var name = Path.GetFileName(p);
            var destPath = Path.Combine(dest, name);
            File.Copy(p, destPath, overwrite: true);
            var checksum = await ComputeSha256Async(destPath);
            manifest.Add((name, BytesToHex(checksum)));
        }

        // Write manifest
        await File.WriteAllTextAsync(Path.Combine(dest, "manifest.txt"), string.Join("\n", manifest.Select(m => $"{m.RelPath}\t{m.Checksum}")));
        return dest;
    }

    public static async Task RestoreAsync(string backupFolder, string restoreTargetDir)
    {
        var manifestPath = Path.Combine(backupFolder, "manifest.txt");
        if (!File.Exists(manifestPath)) throw new InvalidOperationException("Missing manifest");
        var lines = await File.ReadAllLinesAsync(manifestPath);
        foreach (var line in lines)
        {
            var parts = line.Split('\t', 2);
            if (parts.Length != 2) continue;
            var name = parts[0];
            var expected = parts[1];
            var src = Path.Combine(backupFolder, name);
            var checksum = await ComputeSha256Async(src);
            var hex = BytesToHex(checksum);
            if (!string.Equals(hex, expected, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Checksum mismatch for {name}");
            var dest = Path.Combine(restoreTargetDir, name);
            File.Copy(src, dest, overwrite: true);
        }
    }

    static async Task<byte[]> ComputeSha256Async(string path)
    {
        using var stream = File.OpenRead(path);
        using var sha = SHA256.Create();
        return await sha.ComputeHashAsync(stream);
    }

    static string BytesToHex(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();

    static async Task Main()
    {
        Directory.CreateDirectory("backup_demo");
        // create sample files
        File.WriteAllText("data1.txt", "one");
        File.WriteAllText("data2.txt", "two");

        var backupFolder = await BackupFilesAsync(new[] { "data1.txt", "data2.txt" }, "backup_demo");
        Console.WriteLine("Backup created at: " + backupFolder);

        // restore into restore_dir
        Directory.CreateDirectory("restore_dir");
        await RestoreAsync(backupFolder, "restore_dir");
        Console.WriteLine("Restore complete. Files in restore_dir: " + string.Join(", ", Directory.GetFiles("restore_dir").Select(Path.GetFileName)));
    }
}