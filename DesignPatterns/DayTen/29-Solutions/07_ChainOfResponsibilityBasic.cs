using System;

namespace Day10.CommandCoR
{
    // Request result enum
    public enum HandlerResult { Continue, Handled, Rejected }

    public class Request { public string Payload { get; set; } = ""; }

    public abstract class Handler
    {
        protected Handler? Next { get; private set; }
        public Handler SetNext(Handler next) { Next = next; return next; }
        public abstract HandlerResult Handle(Request req);
    }

    public class ValidationHandler : Handler
    {
        public override HandlerResult Handle(Request req)
        {
            if (string.IsNullOrWhiteSpace(req.Payload))
            {
                Console.WriteLine("[Validation] Rejected: empty payload");
                return HandlerResult.Rejected;
            }
            Console.WriteLine("[Validation] Passed");
            return Next?.Handle(req) ?? HandlerResult.Continue;
        }
    }

    public class AuthorizationHandler : Handler
    {
        public override HandlerResult Handle(Request req)
        {
            // trivial example: payload containing "deny" is unauthorized
            if (req.Payload.Contains("deny"))
            {
                Console.WriteLine("[Auth] Rejected");
                return HandlerResult.Rejected;
            }
            Console.WriteLine("[Auth] Authorized");
            return Next?.Handle(req) ?? HandlerResult.Continue;
        }
    }

    public class ProcessingHandler : Handler
    {
        public override HandlerResult Handle(Request req)
        {
            Console.WriteLine($"[Processor] Processing: {req.Payload}");
            return HandlerResult.Handled;
        }
    }

    class Program
    {
        static void Main()
        {
            var validator = new ValidationHandler();
            var auth = new AuthorizationHandler();
            var processor = new ProcessingHandler();
            validator.SetNext(auth).SetNext(processor);

            Console.WriteLine("=== Valid request ===");
            validator.Handle(new Request { Payload = "do something" });

            Console.WriteLine("\n=== Invalid (empty) request ===");
            validator.Handle(new Request { Payload = "" });

            Console.WriteLine("\n=== Unauthorized request ===");
            validator.Handle(new Request { Payload = "please deny me" });
        }
    }
}