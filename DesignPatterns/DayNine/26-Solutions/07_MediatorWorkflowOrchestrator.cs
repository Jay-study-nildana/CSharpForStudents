using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: Mediator orchestrating a multi-step workflow with error handling.

    public class WorkflowMediator
    {
        private readonly Dictionary<string, IWorker> _workers = new();

        public void Register(string name, IWorker worker) => _workers[name] = worker;

        public void Start(string input)
        {
            try
            {
                _workers["validator"]?.DoWork(input, this);
                _workers["processor"]?.DoWork(input, this);
                _workers["notifier"]?.DoWork(input, this);
                Console.WriteLine("[Mediator] Workflow completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Mediator] Workflow failed: {ex.Message}");
                _workers.TryGetValue("notifier", out var notifier);
                notifier?.HandleError(ex.Message);
            }
        }

        // Optionally mediator can allow callbacks from workers
        public void NotifyProgress(string step, string message) => Console.WriteLine($"[Mediator] {step}: {message}");
    }

    public interface IWorker
    {
        void DoWork(string input, WorkflowMediator mediator);
        void HandleError(string reason);
    }

    public class Validator : IWorker
    {
        public void DoWork(string input, WorkflowMediator mediator)
        {
            mediator.NotifyProgress("Validator", "Validating input");
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Input is empty");
        }

        public void HandleError(string reason) => Console.WriteLine($"[Validator] Error handled: {reason}");
    }

    public class Processor : IWorker
    {
        public void DoWork(string input, WorkflowMediator mediator)
        {
            mediator.NotifyProgress("Processor", "Processing...");
            // Simulate processing; could throw
            if (input.Contains("fail")) throw new InvalidOperationException("Processing failed");
            mediator.NotifyProgress("Processor", "Processing done");
        }

        public void HandleError(string reason) => Console.WriteLine($"[Processor] Error logged: {reason}");
    }

    public class Notifier : IWorker
    {
        public void DoWork(string input, WorkflowMediator mediator)
        {
            mediator.NotifyProgress("Notifier", "Sending notification");
            Console.WriteLine($"[Notifier] Notified about: {input}");
        }

        public void HandleError(string reason) => Console.WriteLine($"[Notifier] Sending failure notification: {reason}");
    }

    class Program
    {
        static void Main()
        {
            var mediator = new WorkflowMediator();
            mediator.Register("validator", new Validator());
            mediator.Register("processor", new Processor());
            mediator.Register("notifier", new Notifier());

            Console.WriteLine("=== Successful run ===");
            mediator.Start("good input");

            Console.WriteLine("\n=== Failure run (processing) ===");
            mediator.Start("this will fail");

            Console.WriteLine("\n=== Failure run (validation) ===");
            mediator.Start("");
        }
    }
}