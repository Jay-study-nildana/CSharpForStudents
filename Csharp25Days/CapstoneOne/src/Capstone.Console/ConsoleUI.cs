using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Capstone.Core.Models;
using Capstone.Core.Services;

namespace Capstone.ConsoleApp
{
    // Interactive console UI. Keeps I/O separated from business logic in TaskService.
    public class ConsoleUI
    {
        private readonly TaskService _service;

        public ConsoleUI(TaskService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            // Subscribe to domain events provided by the service
            _service.TaskCompleted += Service_TaskCompleted;
        }

        private void Service_TaskCompleted(object? sender, TaskItem e)
        {
            Console.WriteLine($"[Event] Task completed: {e.Title} ({e.Id})");
        }

        public async Task RunAsync()
        {
            await _service.InitializeAsync();

            var quit = false;
            while (!quit)
            {
                ShowMenu();
                var choice = Console.ReadLine();
                switch (choice?.Trim())
                {
                    case "1": await AddTaskAsync(); break;
                    case "2": ListTasks(false); break;
                    case "3": ListTasks(true); break;
                    case "4": CompleteTask(); break;
                    case "5": Search(); break;
                    case "6": await SaveAndExitAsync(); quit = true; break;
                    case "7": await AddRandomTask(); break;
                    case "8": await SeedTasksFromFile(); break;
                    case "x": quit = true; break;
                    default: Console.WriteLine("Unknown option"); break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== Task Tracker ===");
            Console.WriteLine("1) Add Task");
            Console.WriteLine("2) List active tasks");
            Console.WriteLine("3) List all tasks");
            Console.WriteLine("4) Complete task");
            Console.WriteLine("5) Search tasks");
            Console.WriteLine("6) Save and Exit");
            Console.WriteLine("7) Add Random Task");
            Console.WriteLine("8) Seed Tasks From File");
            Console.WriteLine("x) Exit (without saving)");
            Console.Write("Choice: ");
        }

        private Task AddTaskAsync()
        {
            Console.Write("Title: ");
            var title = Console.ReadLine() ?? string.Empty;
            Console.Write("Description (optional): ");
            var description = Console.ReadLine();
            Console.Write("Priority (low/medium/high) default medium: ");
            var pr = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            var priority = pr switch { "low" => Priority.Low, "high" => Priority.High, _ => Priority.Medium };
            Console.Write("Due date (YYYY-MM-DD) optional: ");
            var dueInput = Console.ReadLine();
            DateTime? due = null;
            if (DateTime.TryParse(dueInput, out var dt)) due = dt;
            Console.Write("Tags (comma separated): ");
            var tagsInput = Console.ReadLine();
            var tags = string.IsNullOrWhiteSpace(tagsInput) ? null : tagsInput.Split(',').Select(t => t.Trim()).Where(t => t.Length > 0);

            try
            {
                var task = _service.Create(title, description, priority, due, tags);
                Console.WriteLine($"Created task: {task.Title} ({task.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create task: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        // Adds a randomly generated task without prompting the user.
        private Task AddRandomTask()
        {
            var rand = Random.Shared;
            var verbs = new[] { "Fix", "Implement", "Review", "Update", "Write", "Refactor", "Test" };
            var nouns = new[] { "logger", "service", "module", "report", "importer", "parser", "handler" };
            var adjs = new[] { "urgent", "important", "quick", "deprecated", "experimental", "minor" };

            var title = $"{verbs[rand.Next(verbs.Length)]} {adjs[rand.Next(adjs.Length)]} {nouns[rand.Next(nouns.Length)]}";
            var description = $"Auto-generated task created at {DateTime.UtcNow:u}";
            var priority = (Priority)rand.Next(0, 3);
            DateTime? due = null;
            if (rand.NextDouble() < 0.6)
            {
                due = DateTime.UtcNow.AddDays(rand.Next(1, 31));
            }

            var tags = new[] { "auto", "random", adjs[rand.Next(adjs.Length)] };

            var task = _service.Create(title, description, priority, due, tags);
            Console.WriteLine($"[Random] Created task: {task.Title} ({task.Id})");

            return Task.CompletedTask;
        }

        // Read a JSON seed file from several likely locations, import tasks, and persist them.
        private async Task SeedTasksFromFile()
        {
            var candidates = new[]
            {
                Path.Combine(Environment.CurrentDirectory, "seed_tasks.json"),
                Path.Combine(Environment.CurrentDirectory, "data", "seed_tasks.json"),
                Path.Combine(Environment.CurrentDirectory, "src", "Capstone.Console", "Data", "seed_tasks.json"),
                Path.Combine(AppContext.BaseDirectory, "seed_tasks.json"),
                Path.Combine(AppContext.BaseDirectory, "Data", "seed_tasks.json"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "src", "Capstone.Console", "Data", "seed_tasks.json")
            };

            string? found = null;
            foreach (var cand in candidates)
            {
                try
                {
                    var full = Path.GetFullPath(cand);
                    if (File.Exists(full)) { found = full; break; }
                }
                catch { /* ignore invalid paths */ }
            }

            if (found == null)
            {
                Console.WriteLine("Seed file not found. Checked locations:");
                foreach (var c in candidates) Console.WriteLine(" - " + c);
                return;
            }

            try
            {
                using var stream = File.OpenRead(found);
                var items = await JsonSerializer.DeserializeAsync<List<TaskItem>>(stream);
                if (items == null || items.Count == 0)
                {
                    Console.WriteLine("No tasks found in the seed file.");
                    return;
                }

                var added = _service.Import(items);
                await _service.SaveAsync();
                Console.WriteLine($"Imported {added} tasks from {Path.GetFileName(found)} and saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to import seed file: {ex.Message}");
            }
        }

        private void ListTasks(bool includeCompleted)
        {
            var list = _service.GetAll(includeCompleted);
            foreach (var t in list)
            {
                Console.WriteLine(t.ToString());
            }
        }

        private void CompleteTask()
        {
            Console.Write("Task id: ");
            var idStr = Console.ReadLine();
            if (Guid.TryParse(idStr, out var id))
            {
                try
                {
                    _service.Complete(id);
                    Console.WriteLine("Marked complete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to complete task: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid id format.");
            }
        }

        private void Search()
        {
            Console.Write("Query: ");
            var q = Console.ReadLine() ?? string.Empty;
            var results = _service.Search(q);
            foreach (var t in results)
            {
                Console.WriteLine(t.ToString());
            }
        }

        private async Task SaveAndExitAsync()
        {
            try
            {
                await _service.SaveAsync();
                Console.WriteLine("Saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save failed: {ex.Message}");
            }
        }
    }
}
