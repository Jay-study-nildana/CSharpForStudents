// Problem: TaskLookupById
// Provide O(1) lookup by id using Dictionary<Guid,Task>.
// Complexity: O(1) average for lookup/insert/delete.

using System;
using System.Collections.Generic;

record TaskItem(Guid Id, string Title);

class TaskLookupById
{
    private readonly Dictionary<Guid, TaskItem> _tasks = new();

    public void Add(TaskItem t) => _tasks[t.Id] = t;
    public bool TryGet(Guid id, out TaskItem task) => _tasks.TryGetValue(id, out task);
    public bool Remove(Guid id) => _tasks.Remove(id);
    public void UpdateTitle(Guid id, string newTitle)
    {
        if (_tasks.TryGetValue(id, out var t))
            _tasks[id] = t with { Title = newTitle };
    }

    static void Main()
    {
        var store = new TaskLookupById();
        var t = new TaskItem(Guid.NewGuid(), "Write doc");
        store.Add(t);
        store.UpdateTitle(t.Id, "Write docs and tests");
        if (store.TryGet(t.Id, out var fetched)) Console.WriteLine(fetched.Title);
        store.Remove(t.Id);
    }
}