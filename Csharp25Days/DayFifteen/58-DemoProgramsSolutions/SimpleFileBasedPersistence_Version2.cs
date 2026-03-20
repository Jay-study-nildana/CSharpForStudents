// SimpleFileBasedPersistence.cs
// Problem: SimpleFileBasedPersistence
// Implement simple file-based persistence for TaskItem using one JSON file per entity.
// Methods: Add, GetAll, Save (per-entity file). Justification: per-file allows simple concurrency and easy restore per entity.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

record TaskItem(Guid Id, string Title, bool IsDone);

class FileTaskStore
{
    private readonly string _dir;
    private readonly JsonSerializerOptions _opts = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };

    public FileTaskStore(string directory)
    {
        _dir = directory;
        Directory.CreateDirectory(_dir);
    }

    public void Add(TaskItem t)
    {
        var path = Path.Combine(_dir, $"{t.Id}.json");
        File.WriteAllText(path, JsonSerializer.Serialize(t, _opts));
    }

    public IEnumerable<TaskItem> GetAll()
    {
        foreach (var file in Directory.EnumerateFiles(_dir, "*.json"))
        {
            var json = File.ReadAllText(file);
            var t = JsonSerializer.Deserialize<TaskItem>(json, _opts);
            if (t != null) yield return t;
        }
    }

    public void Delete(Guid id)
    {
        var path = Path.Combine(_dir, $"{id}.json");
        if (File.Exists(path)) File.Delete(path);
    }
}

class SimpleFileBasedPersistence
{
    static void Main()
    {
        var store = new FileTaskStore("tasks");
        var t1 = new TaskItem(Guid.NewGuid(), "Write docs", false);
        var t2 = new TaskItem(Guid.NewGuid(), "Run tests", true);
        store.Add(t1); store.Add(t2);

        Console.WriteLine("Persisted tasks:");
        foreach (var t in store.GetAll()) Console.WriteLine($"{t.Id} - {t.Title} (Done: {t.IsDone})");

        // Backup/restore discussion: per-file allows per-entity restore and simpler incremental backup.
    }
}