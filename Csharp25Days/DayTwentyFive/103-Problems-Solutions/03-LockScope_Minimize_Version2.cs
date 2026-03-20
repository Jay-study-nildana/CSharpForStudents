// 03-LockScope_Minimize.cs
// Do minimal work under lock (only mutate in-memory list); perform I/O outside lock.

using System;
using System.Collections.Generic;
using System.IO;

public class LogService
{
    private readonly object _sync = new();
    private List<string> _items = new();

    public void AddAndPersist(string s)
    {
        bool shouldPersist = false;
        // capture what we need under lock, but do not do I/O inside lock
        lock (_sync)
        {
            _items.Add(s);
            shouldPersist = true; // marker for outside work
        }

        if (shouldPersist)
        {
            // Do I/O outside lock to avoid blocking other threads
            File.AppendAllText("log.txt", s + Environment.NewLine);
        }
    }
}