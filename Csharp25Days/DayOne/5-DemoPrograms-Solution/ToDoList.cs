using System;
using System.Collections.Generic;

namespace ToDoDemo
{
    // Simple task model for the demo
    record ToDoItem(int Id, string Description, DateTime CreatedAt, bool IsDone = false);

    class Program
    {
        static void Main()
        {
            var tasks = new List<ToDoItem>();
            var nextId = 1;

            while (true)
            {
                ShowMenu();
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddTask(tasks, ref nextId);
                        break;
                    case "2":
                        ListTasks(tasks);
                        break;
                    case "3":
                        RemoveTask(tasks);
                        break;
                    case "4":
                        ToggleComplete(tasks);
                        break;
                    case "5":
                        ClearTasks(tasks);
                        break;
                    case "0":
                        Console.WriteLine("Exiting. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Unknown option. Enter a number from the menu.");
                        break;
                }

                Console.WriteLine(); // spacing between iterations
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== In-Memory To-Do List ===");
            Console.WriteLine("1) Add task");
            Console.WriteLine("2) List tasks");
            Console.WriteLine("3) Remove task by id");
            Console.WriteLine("4) Toggle complete by id");
            Console.WriteLine("5) Clear all tasks");
            Console.WriteLine("0) Exit");
        }

        static void AddTask(List<ToDoItem> tasks, ref int nextId)
        {
            Console.Write("Enter task description: ");
            var desc = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(desc))
            {
                Console.WriteLine("Task description cannot be empty.");
                return;
            }

            tasks.Add(new ToDoItem(nextId++, desc, DateTime.Now));
            Console.WriteLine("Task added.");
        }

        static void ListTasks(List<ToDoItem> tasks)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("(No tasks)");
                return;
            }

            Console.WriteLine("ID  | Done | Created              | Description");
            Console.WriteLine("----+------+----------------------+-------------------------");
            foreach (var t in tasks)
            {
                var doneMark = t.IsDone ? "X" : " ";
                Console.WriteLine($"{t.Id,-3} |  {doneMark}   | {t.CreatedAt:yyyy-MM-dd HH:mm} | {t.Description}");
            }
        }

        static void RemoveTask(List<ToDoItem> tasks)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks to remove.");
                return;
            }

            Console.Write("Enter task id to remove: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id.");
                return;
            }

            var idx = tasks.FindIndex(t => t.Id == id);
            if (idx < 0)
            {
                Console.WriteLine($"Task with id {id} not found.");
                return;
            }

            tasks.RemoveAt(idx);
            Console.WriteLine($"Removed task {id}.");
        }

        static void ToggleComplete(List<ToDoItem> tasks)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            Console.Write("Enter task id to toggle complete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id.");
                return;
            }

            var idx = tasks.FindIndex(t => t.Id == id);
            if (idx < 0)
            {
                Console.WriteLine($"Task with id {id} not found.");
                return;
            }

            var t = tasks[idx];
            // Replace record with toggled IsDone (records are immutable in this example)
            tasks[idx] = t with { IsDone = !t.IsDone };
            Console.WriteLine($"Task {id} marked {(tasks[idx].IsDone ? "complete" : "incomplete")}.");
        }

        static void ClearTasks(List<ToDoItem> tasks)
        {
            Console.Write("Are you sure you want to clear all tasks? (y/N): ");
            var ans = Console.ReadLine()?.Trim().ToLower();
            if (ans == "y" || ans == "yes")
            {
                tasks.Clear();
                Console.WriteLine("All tasks cleared.");
            }
            else
            {
                Console.WriteLine("Clear cancelled.");
            }
        }
    }
}