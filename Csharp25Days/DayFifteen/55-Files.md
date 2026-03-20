# Day 15 — File I/O Basics: Reading & Writing Text and Binary Files (C# / .NET)

This note covers conceptual guidance and common C# patterns for reading and writing text and binary files safely and efficiently. Focus on choice (text vs binary), correct APIs, resource cleanup, concurrency considerations (locking / sharing), and basic techniques for atomic writes and data integrity.

Why choose text or binary?
- Text files (UTF-8/UTF-16, CSV, logs, JSON, XML): human-readable, easier to debug and edit. Use when interoperability and inspectability matter.
- Binary files (custom binary formats, images, compressed blobs, protobuf/MessagePack): compact and faster to serialize/deserialize for structured data; required when format or performance demands binary representation.

Always pick an encoding explicitly for text (prefer UTF-8 without BOM for cross-platform interoperability).

Safe patterns for text I/O
- Small files: convenience methods are fine:
  - `File.ReadAllText(path, Encoding.UTF8)` / `File.WriteAllText(path, content, Encoding.UTF8)`
  - `File.ReadAllLines` / `File.WriteAllLines`

- Large files or streaming: avoid loading entire file into memory:
  - `File.ReadLines(path)` yields lines lazily.
  - Use `StreamReader`/`StreamWriter` for controlled buffered I/O and to support async operations.

Example — streaming text lines (async):
```csharp
using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
using var reader = new StreamReader(fs, Encoding.UTF8);
string? line;
while ((line = await reader.ReadLineAsync()) != null)
{
    ProcessLine(line);
}
```

Key text tips
- Always specify `Encoding` explicitly (UTF8/UTF8Encoding without BOM).
- Use `ReadLineAsync` / `WriteAsync` for I/O-bound concurrency.
- Use `File.ReadLines` (deferred execution) when processing line-by-line.

Binary I/O basics
- Use `FileStream` for raw byte access; wrap in `BinaryWriter` / `BinaryReader` for primitive types.
- Decide on endianness and be consistent (BitConverter is platform-endian — prefer explicit byte order if files may move between architectures).
- For structured, versioned binary data prefer stable libraries (protobuf, MessagePack) instead of ad-hoc binary layout.

Example — writing and reading primitives:
```csharp
// Writing
using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
using var writer = new BinaryWriter(fs);
writer.Write(42);                // int
writer.Write("Hello");           // length-prefixed string

// Reading
using var rfs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
using var reader = new BinaryReader(rfs);
int value = reader.ReadInt32();
string msg = reader.ReadString();
```

Random access and seeking
- `FileStream.Seek(offset, SeekOrigin)` supports random access.
- Use for databases, indices, or file formats with offsets.

Performance considerations
- Choose a reasonable buffer size (default 4 KB or larger for sequential scans).
- Use `FileOptions.SequentialScan` hint for large sequential reads:
  ```csharp
  new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 81920, options: FileOptions.SequentialScan);
  ```
- For very hot I/O paths consider `Span<byte>` and `Memory<T>` APIs or memory-mapped files (`MemoryMappedFile`) for large shared datasets.

Concurrency, locking and FileShare
- `FileShare` controls whether other processes/threads may read/write concurrently.
  - `FileShare.Read` — allow other readers, deny writers.
  - `FileShare.None` — exclusive access.
- If a file is already locked, attempts to open with incompatible `FileShare`/`FileAccess` throw `IOException`. Catch and handle that explicitly.

Example — exclusive write:
```csharp
try
{
    using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
    // write safely; no other process can open for write/read (depending on OS)
}
catch (IOException ex)
{
    // handle file-in-use (retry, inform user, or fail)
}
```

Atomic writes and durable updates
- To avoid partial files (e.g., crash during write), use an atomic replace strategy:
  1. Write to a temporary file in the same directory (ensures same filesystem/volume).
  2. Flush and optionally flush to disk.
  3. Replace the target using an atomic rename/replace API (`File.Replace` or `File.Move`/`File.Replace` depending on platform).
- On Windows `File.Replace` can atomically replace and optionally create a backup. On POSIX, `File.Move` within same filesystem is atomic; cross-platform behavior differs — writing to temp then `File.Move(temp, target, overwrite: true)` in .NET core is a common approach.

Example — atomic text write (safe replace):
```csharp
string tempPath = Path.Combine(Path.GetDirectoryName(path)!, Path.GetRandomFileName());
await File.WriteAllTextAsync(tempPath, content, Encoding.UTF8);

// Ensure data flushed to disk
using (var fs = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
{
    fs.Flush(true); // flush OS buffers to disk (platform-specific)
}

// Replace target atomically (on same volume)
if (File.Exists(path))
    File.Replace(tempPath, path, null); // may throw on some platforms
else
    File.Move(tempPath, path);
```
Notes:
- `Flush(true)` (or `FileStream.Flush(flushToDisk: true)`) requests the OS to flush to permanent storage; portability and guarantees vary.
- Consider backup files and versioning when atomic replace is not feasible.

Data integrity and checksums
- For critical data, compute and store a checksum (SHA-256) or use cryptographic signatures to detect corruption.
- Verify checksum after read before trusting content.

Example — compute SHA256:
```csharp
using var stream = File.OpenRead(path);
using var sha = System.Security.Cryptography.SHA256.Create();
byte[] hash = sha.ComputeHash(stream);
```

Error handling patterns
- Use `using` or `try/finally` to ensure `Dispose()` runs and streams are closed.
- Catch specific exceptions (`FileNotFoundException`, `UnauthorizedAccessException`, `IOException`) and handle appropriately (retry/backoff, user message, fail-fast).
- For expected parse errors (bad content), prefer returning a `Result`/`Try` pattern rather than throwing for flow control.

When to use files vs database
- Files are simple and fine for logs, exports, small local persistence, or when human inspection is valuable.
- Use a database when you need concurrency, transactions, indexing, complex queries, or ACID guarantees. File-based approaches require you to implement locking, concurrency policies, and backups yourself.

Backup & restore basics (conceptual)
- Keep copies in a separate directory or remote store.
- Use checksums and versioning to detect corruption and allow rollbacks.
- For restore, validate files (checksum, schema) before replacing live data.

Summary checklist
- Choose text vs binary based on readability vs compactness and performance.
- Always specify encoding for text; prefer UTF-8 for cross-platform interoperability.
- Stream large files; avoid ReadAllText for huge files.
- Use `using` / `try/finally` to dispose streams.
- Control concurrency with `FileShare` and handle `IOException` when files are in use.
- Implement atomic writes via temp files + atomic replace; flush to disk for durability when necessary.
- Protect critical data with checksums and backups.
- For complex concurrency/transactional needs, prefer a database.

Practice: try implementing a safe save method (text and binary) that writes atomically and verifies the written checksum before replacing the original — this covers text/binary APIs, atomic replace, flush, and integrity checks.