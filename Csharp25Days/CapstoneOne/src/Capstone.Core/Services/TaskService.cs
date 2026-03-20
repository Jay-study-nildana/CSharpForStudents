using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Core.Models;
using Capstone.Core.Storage;

namespace Capstone.Core.Services
{
    // Business logic around TaskItem. Keeps an in-memory list and persists via IStorage<T>.
    public class TaskService
    {
        private readonly IStorage<TaskItem> _storage;
        private readonly List<TaskItem> _tasks = new List<TaskItem>();

        // Event fired when a task moves to completed. Demonstrates events/delegates.
        public event EventHandler<TaskItem>? TaskCompleted;

        public TaskService(IStorage<TaskItem> storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        // Load persisted tasks into memory (async boundary)
        public async Task InitializeAsync()
        {
            var loaded = await _storage.LoadAsync();
            _tasks.Clear();
            _tasks.AddRange(loaded);
        }

        // Create a new task and add to the in-memory collection
        public TaskItem Create(string title, string? description = null, Priority priority = Priority.Medium, DateTime? dueDate = null, IEnumerable<string>? tags = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required", nameof(title));

            var t = new TaskItem
            {
                Title = title.Trim(),
                Description = description?.Trim(),
                Priority = priority,
                DueDate = dueDate,
                Tags = tags?.ToList() ?? new List<string>(),
                CreatedAt = DateTime.UtcNow
            };

            _tasks.Add(t);
            return t;
        }

        // Query APIs: show how LINQ and ordering are used
        public IEnumerable<TaskItem> GetAll(bool includeCompleted = true)
        {
            var q = _tasks.AsEnumerable();
            if (!includeCompleted) q = q.Where(t => !t.IsCompleted);
            return q.OrderByDescending(t => t.CreatedAt).ToList();
        }

        public IEnumerable<TaskItem> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return GetAll();
            query = query.Trim();
            // Demonstrates LINQ and lambda usage
            return _tasks.Where(t => t.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                                  || (t.Description != null && t.Description.Contains(query, StringComparison.OrdinalIgnoreCase)))
                         .OrderByDescending(t => t.Priority)
                         .ThenBy(t => t.DueDate)
                         .ToList();
        }

        // Mark a task completed and raise event
        public void Complete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id) ?? throw new KeyNotFoundException("Task not found");
            if (task.IsCompleted) return;
            task.IsCompleted = true;
            task.CompletedAt = DateTime.UtcNow;
            TaskCompleted?.Invoke(this, task);
        }

        // Persist the in-memory collection (async)
        public async Task SaveAsync() => await _storage.SaveAsync(_tasks);

        // Import external TaskItem instances into the current in-memory store.
        // Returns the number of items that were added (duplicates by Id are ignored).
        public int Import(IEnumerable<TaskItem>? items)
        {
            if (items == null) return 0;
            var added = 0;
            foreach (var item in items)
            {
                if (!_tasks.Any(t => t.Id == item.Id))
                {
                    _tasks.Add(item);
                    added++;
                }
            }
            return added;
        }
    }
}
