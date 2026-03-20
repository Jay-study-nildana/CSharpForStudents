using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Core.Models;
using Capstone.Core.Services;
using Capstone.Core.Storage;
using Xunit;

namespace Capstone.Tests
{
    // Very small in-memory storage used as a test double for unit tests.
    public class InMemoryStorage : IStorage<TaskItem>
    {
        private readonly List<TaskItem> _items = new List<TaskItem>();
        public Task<IEnumerable<TaskItem>> LoadAsync() => Task.FromResult<IEnumerable<TaskItem>>(_items);
        public Task SaveAsync(IEnumerable<TaskItem> items)
        {
            _items.Clear();
            _items.AddRange(items);
            return Task.CompletedTask;
        }
    }

    public class TaskServiceTests
    {
        [Fact]
        public async Task CreateAndSave_LoadsBack()
        {
            var storage = new InMemoryStorage();
            var svc = new TaskService(storage);

            // Create and persist
            var t = svc.Create("test", "desc");
            await svc.SaveAsync();

            // New service instance simulates app restart and re-load
            var svc2 = new TaskService(storage);
            await svc2.InitializeAsync();
            var all = svc2.GetAll().ToList();

            Assert.Single(all);
            Assert.Equal("test", all[0].Title);
        }

        [Fact]
        public void Complete_RaisesEventAndMarksCompleted()
        {
            var storage = new InMemoryStorage();
            var svc = new TaskService(storage);
            var t = svc.Create("t1");

            var raised = false;
            svc.TaskCompleted += (s, item) =>
            {
                raised = true;
                Assert.Equal(t.Id, item.Id);
            };

            svc.Complete(t.Id);

            Assert.True(t.IsCompleted);
            Assert.True(raised);
        }

        [Fact]
        public void Search_FiltersAndOrdersResults()
        {
            var storage = new InMemoryStorage();
            var svc = new TaskService(storage);

            var now = DateTime.UtcNow;
            var t1 = svc.Create("Fix critical bug", "Fix urgent crash", Priority.High, now.AddDays(5));
            var t2 = svc.Create("Fix minor issue", "Small cosmetic fix", Priority.Low, now.AddDays(1));
            var t3 = svc.Create("Implement feature", "Fix legacy behavior in module", Priority.Medium, now.AddDays(2));

            var results = svc.Search("fix").ToList();
            Assert.Equal(3, results.Count);
            // High priority comes first
            Assert.Equal(t1.Id, results[0].Id);
            // Medium then low
            Assert.Equal(t3.Id, results[1].Id);
            Assert.Equal(t2.Id, results[2].Id);
        }

        [Fact]
        public void GetAll_ExcludeCompleted()
        {
            var storage = new InMemoryStorage();
            var svc = new TaskService(storage);
            var a = svc.Create("one");
            var b = svc.Create("two");
            svc.Complete(a.Id);

            var active = svc.GetAll(includeCompleted: false).ToList();
            Assert.Single(active);
            Assert.Equal(b.Id, active[0].Id);
        }

        [Fact]
        public async Task Import_AddsOnlyNewItems()
        {
            var storage = new InMemoryStorage();
            var svc = new TaskService(storage);

            // Persist an existing item
            var existing = svc.Create("existing");
            await svc.SaveAsync();

            // New service loads existing items
            var svc2 = new TaskService(storage);
            await svc2.InitializeAsync();

            var duplicate = new TaskItem { Id = existing.Id, Title = "duplicate" };
            var newTask = new TaskItem { Title = "new task" };
            var added = svc2.Import(new[] { duplicate, newTask });
            Assert.Equal(1, added);

            await svc2.SaveAsync();

            var svc3 = new TaskService(storage);
            await svc3.InitializeAsync();
            var all = svc3.GetAll().ToList();
            Assert.Equal(2, all.Count);
            var titles = all.Select(t => t.Title).ToList();
            Assert.Contains("existing", titles);
            Assert.Contains("new task", titles);
        }
    }
}
