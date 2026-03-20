# Day 15 — File I/O & Serialization: Practice Problems (C# / .NET)

Instructions
- Solve each problem using idiomatic C# (.NET 6+). Use System.Text.Json for JSON tasks and XmlSerializer for XML tasks unless noted otherwise.
- Include brief comments that explain the chosen approach and expected complexity.
- Provide a Main() in each solution that demonstrates usage on small example data.
- Aim for clear, safe file I/O (use `using`, handle common exceptions, prefer streaming for large files, and avoid insecure serializers).

Problems

1) ReadWriteTextFile  
Write methods to read all text from a UTF‑8 file and to append a line to a UTF‑8 file. Demonstrate reading and appending. Discuss complexity and safety (encoding, resource disposal).

2) ReadWriteBinaryFile  
Write methods to write an array of integers to a binary file and read them back using `BinaryWriter`/`BinaryReader`. Discuss endianness and complexity.

3) JsonSerializeDeserialize  
Serialize a simple `Product { Id, Name, Price }` list to JSON and deserialize it back using `System.Text.Json`. Demonstrate custom `JsonSerializerOptions` (camelCase, ignore nulls). Discuss schema evolution considerations.

4) XmlSerializeDeserialize  
Serialize and deserialize the same `Product` list using `XmlSerializer`. Show how to persist to a file and read back. Note when XML is preferred and complexity.

5) AtomicFileWrite  
Implement an atomic write helper for text files: write to a temp file in the same directory, flush, and replace the target atomically (use `File.Replace` or `File.Move` fallback). Demonstrate safe replacement and describe why this avoids partial writes.

6) FileLockingAndSharing  
Demonstrate opening a file for exclusive write (`FileShare.None`) and a reader that uses `FileShare.Read`. Show how a conflicting open raises an `IOException` and how to handle/retry. Discuss `FileShare` semantics.

7) ComputeFileChecksum  
Compute a SHA256 checksum for a file (streaming) and verify it after writing. Discuss why checksums help detect corruption and complexity (I/O bound).

8) StreamLargeFileLines  
Process a very large text file line-by-line without loading it fully into memory using `File.ReadLines` or a `StreamReader`. Provide an example that counts lines containing a word. Discuss streaming complexity and memory use.

9) SimpleFileBasedPersistence  
Design and implement a minimal file-based persistence for `TaskItem { Id (Guid), Title, IsDone }`. Options: one JSON file per entity vs one JSON array file. Implement Add, GetAll, and Save operations (pick one approach and justify). Discuss backup/restore implications.

10) BackupAndRestoreFiles  
Implement a simple backup helper that copies a set of files to a timestamped backup folder and a restore that validates checksums before restoring. Demonstrate backing up and restoring a small set of files and discuss atomicity and safety concerns.

Deliverables
- One `.md` file listing the 10 problems (this file).
- Ten C# solution files, one per problem, with file names matching the problem titles (sanitized, no spaces).