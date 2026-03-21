using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Day10.CommandCoR
{
    public interface ICommand
    {
        void Execute();
        string Description { get; }
    }

    public class SimpleCommand : ICommand
    {
        private readonly string _message;
        public string Description => $"Simple: {_message}";
        public SimpleCommand(string message) => _message = message;
        public void Execute() => Console.WriteLine($"Executing command: {_message}");
    }

    public class CommandQueue
    {
        private readonly BlockingCollection<ICommand> _queue = new();

        public void Enqueue(ICommand cmd) => _queue.Add(cmd);
        public ICommand? Dequeue(CancellationToken ct) => _queue.TryTake(out var cmd, Timeout.Infinite, ct) ? cmd : null;
        public void Complete() => _queue.CompleteAdding();
    }

    public class Worker
    {
        private readonly CommandQueue _queue;
        public Worker(CommandQueue q) => _queue = q;

        public void Run(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    var cmd = _queue.Dequeue(ct);
                    if (cmd == null) break;
                    try
                    {
                        cmd.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Transient failure: {ex.Message} - requeueing");
                        _queue.Enqueue(cmd); // naive retry
                    }
                }
            }
            catch (OperationCanceledException) { /* shutting down */ }
        }
    }

    class Program
    {
        static void Main()
        {
            var queue = new CommandQueue();
            var worker = new Worker(queue);
            var cts = new CancellationTokenSource();

            // Start worker on background thread
            var t = Task.Run(() => worker.Run(cts.Token));

            // Enqueue commands
            queue.Enqueue(new SimpleCommand("A"));
            queue.Enqueue(new SimpleCommand("B"));
            queue.Enqueue(new SimpleCommand("C"));

            // Let worker process then shut down
            Task.Delay(500).Wait();
            queue.Complete();
            cts.CancelAfter(200);
            t.Wait();
            Console.WriteLine("Worker finished");
        }
    }
}