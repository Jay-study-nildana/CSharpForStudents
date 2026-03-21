using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Day10.CommandCoR
{
    public enum HandlerResult { Continue, Handled, Rejected }

    public class Request { public string Payload { get; set; } = ""; }

    public interface IAsyncHandler
    {
        Task<HandlerResult> HandleAsync(Request req, List<string> runLog);
        IAsyncHandler SetNext(IAsyncHandler next);
    }

    public abstract class AsyncHandlerBase : IAsyncHandler
    {
        protected IAsyncHandler? NextHandler;
        public IAsyncHandler SetNext(IAsyncHandler next) { NextHandler = next; return next; }
        public abstract Task<HandlerResult> HandleAsync(Request req, List<string> runLog);
    }

    public class AsyncValidator : AsyncHandlerBase
    {
        public override async Task<HandlerResult> HandleAsync(Request req, List<string> runLog)
        {
            runLog.Add("AsyncValidator");
            await Task.Delay(50); // simulate async validation
            if (string.IsNullOrWhiteSpace(req.Payload)) return HandlerResult.Rejected;
            return NextHandler != null ? await NextHandler.HandleAsync(req, runLog) : HandlerResult.Continue;
        }
    }

    public class AsyncProcessor : AsyncHandlerBase
    {
        public override async Task<HandlerResult> HandleAsync(Request req, List<string> runLog)
        {
            runLog.Add("AsyncProcessor");
            await Task.Delay(100); // simulate I/O
            Console.WriteLine($"[AsyncProcessor] Processed: {req.Payload}");
            return HandlerResult.Handled;
        }
    }

    class Program
    {
        static async Task Main()
        {
            var validator = new AsyncValidator();
            var processor = new AsyncProcessor();
            validator.SetNext(processor);

            var log = new List<string>();
            Console.WriteLine("Running async chain...");
            var result = await validator.HandleAsync(new Request { Payload = "async work" }, log);
            Console.WriteLine($"Result: {result}, Handlers: {string.Join(", ", log)}");

            // Short-circuit scenario
            log.Clear();
            var result2 = await validator.HandleAsync(new Request { Payload = "" }, log);
            Console.WriteLine($"Result: {result2}, Handlers: {string.Join(", ", log)}");
        }
    }
}