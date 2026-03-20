using System;
using System.Collections.Generic;

namespace Capstone.Core.Models
{
    // Simple priority enumeration used by tasks.
    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    // TaskItem is a plain domain model used across the app.
    public class TaskItem
    {
        // Unique id for lookups and persistence
        public Guid Id { get; set; } = Guid.NewGuid();

        // Human-facing title (required)
        public string Title { get; set; } = string.Empty;

        // Optional longer description
        public string? Description { get; set; }

        // Basic priority ranking
        public Priority Priority { get; set; } = Priority.Medium;

        // Completion tracking
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }

        // Creation timestamp and optional due date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        // Simple tag list for filtering/searching
        public List<string> Tags { get; set; } = new List<string>();

        // Human-friendly display used by the console UI
        public override string ToString()
        {
            var due = DueDate.HasValue ? DueDate.Value.ToString("u") : "none";
            var tags = Tags.Count > 0 ? string.Join(",", Tags) : "none";
            return $"{Title} (Id: {Id}) - Priority: {Priority}, Completed: {IsCompleted}, Due: {due}, Tags: {tags}";
        }
    }
}
