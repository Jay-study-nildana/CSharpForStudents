using System;
using System.Collections.Generic;

namespace Day10.CommandCoR
{
    public enum HandlerResult { Continue, Handled, Rejected }

    public class Request { public string Payload { get; set; } = ""; }

    public abstract class Handler
    {
        protected Handler? Next { get; private set; }
        public Handler SetNext(Handler next) { Next = next; return next; }
        public abstract HandlerResult Handle(Request req, List<string> log);
    }

    public class LoggingHandler : Handler
    {
        public override HandlerResult Handle(Request req, List<string> log)
        {
            log.Add("LoggingHandler");
            Console.WriteLine("[Logging] Logged payload");
            return Next?.Handle(req, log) ?? HandlerResult.Continue;
        }
    }

    public class ShortCircuitHandler : Handler
    {
        public override HandlerResult Handle(Request req, List<string> log)
        {
            log.Add("ShortCircuitHandler");
            if (req.Payload.Contains("stop"))
            {
                Console.WriteLine("[ShortCircuit] Stopping chain");
                return HandlerResult.Handled; // short-circuit: stop further handlers
            }
            return Next?.Handle(req, log) ?? HandlerResult.Continue;
        }
    }

    public class FinalHandler : Handler
    {
        public override HandlerResult Handle(Request req, List<string> log)
        {
            log.Add("FinalHandler");
            Console.WriteLine("[Final] Handled payload");
            return HandlerResult.Handled;
        }
    }

    class Program
    {
        static void RunScenario(string payload)
        {
            var log = new List<string>();
            var logger = new LoggingHandler();
            var stopper = new ShortCircuitHandler();
            var final = new FinalHandler();
            logger.SetNext(stopper).SetNext(final);

            Console.WriteLine($"\n--- Scenario: '{payload}' ---");
            var result = logger.Handle(new Request { Payload = payload }, log);
            Console.WriteLine($"Result: {result}");
            Console.WriteLine("Handlers run: " + string.Join(", ", log));
        }

        static void Main()
        {
            RunScenario("allow this");
            RunScenario("please stop here"); // contains "stop" => short-circuit
        }
    }
}