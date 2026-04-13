// ChainOfResponsibilityDemo.cs
// Console demo of the Chain of Responsibility Pattern.

using System;

// Top-level statements
var validation = new ValidationHandler();
var logging = new LoggingHandler();
var processing = new ProcessingHandler();
validation.SetNext(logging).SetNext(processing);

Console.WriteLine("Chain of Responsibility interactive demo. Type 'help' for commands.");
while (true)
{
    Console.WriteLine("\nCommands: send, empty, list, toggle, show, help, exit");
    Console.Write("cmd: ");
    var cmd = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(cmd)) continue;
    cmd = cmd.Trim().ToLowerInvariant();
    if (cmd == "exit") break;

    if (cmd == "help")
    {
        Console.WriteLine("send   - send a custom payload through the chain");
        Console.WriteLine("empty  - send an empty payload to demonstrate validation short-circuit");
        Console.WriteLine("list   - list handlers in the chain with enabled state");
        Console.WriteLine("toggle - toggle a handler on/off by index (use 'list' to see indexes)");
        Console.WriteLine("show   - print a descriptive run for a sample payload");
        Console.WriteLine("exit   - quit the demo");
        continue;
    }

    if (cmd == "send")
    {
        Console.Write("Enter payload: ");
        var payload = Console.ReadLine();
        var req = new Request { Payload = payload };
        Console.WriteLine("\n-- Processing request --");
        var handled = validation.Process(req);
        if (!handled) Console.WriteLine("Result: Request was NOT handled by any handler in the chain.");
        else Console.WriteLine("Result: Request was handled by the chain.");
        continue;
    }

    if (cmd == "empty")
    {
        var req = new Request { Payload = string.Empty };
        Console.WriteLine("\n-- Processing empty request (should be rejected by validation) --");
        var handled = validation.Process(req);
        if (!handled) Console.WriteLine("Result: Request was NOT handled by any handler in the chain.");
        else Console.WriteLine("Result: Request was handled by the chain.");
        continue;
    }

    if (cmd == "list")
    {
        Console.WriteLine("Handlers in chain:");
        var idx = 0;
        foreach (var desc in validation.DescribeChain())
        {
            Console.WriteLine($"[{idx}] {desc}");
            idx++;
        }
        continue;
    }

    if (cmd == "toggle")
    {
        Console.Write("Handler index to toggle (use 'list' to view): ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var index)) { Console.WriteLine("Invalid index"); continue; }
        var nodes = validation.GetChainNodes().ToArray();
        if (index < 0 || index >= nodes.Length) { Console.WriteLine("Index out of range"); continue; }
        var node = nodes[index];
        node.Enabled = !node.Enabled;
        Console.WriteLine($"Toggled '{node.Name}' to {(node.Enabled ? "Enabled" : "Disabled")}");
        continue;
    }

    if (cmd == "show")
    {
        Console.Write("Enter sample payload: ");
        var payload = Console.ReadLine();
        var req = new Request { Payload = payload };
        Console.WriteLine("\n-- Step-by-step processing --");
        validation.Process(req);
        Console.WriteLine("-- End processing --");
        continue;
    }

    Console.WriteLine("Unknown command. Type 'help' for options.");
}


public class Request { public string Payload { get; set; } }

// Handler base
public abstract class Handler
{
    protected Handler? Next { get; private set; }
    public bool Enabled { get; set; } = true;
    public string Name { get; protected set; } = "Handler";
    public Handler SetNext(Handler next) { Next = next; return next; }

    // Process the request: returns true if handled by any handler in the chain
    public bool Process(Request req)
    {
        if (!Enabled)
        {
            // Skip to next if disabled
            return Next?.Process(req) ?? false;
        }
        var handled = Handle(req);
        if (handled) return true;
        return Next?.Process(req) ?? false;
    }

    // Return descriptions for each node in the chain for introspection
    public IEnumerable<string> DescribeChain()
    {
        var current = (Handler?)this;
        while (current != null)
        {
            yield return $"{current.Name} (Enabled={current.Enabled})";
            current = current.Next;
        }
    }

    // Return nodes for operations like toggling by index
    public IEnumerable<Handler> GetChainNodes()
    {
        var current = (Handler?)this;
        while (current != null)
        {
            yield return current;
            current = current.Next;
        }
    }

    // Concrete handlers implement this to perform checks/work.
    // Return true if this handler handled the request and chain should stop.
    public abstract bool Handle(Request req);
}

// Concrete handlers
public class ValidationHandler : Handler
{
    public ValidationHandler() { Name = "ValidationHandler"; }
    public override bool Handle(Request req)
    {
        Console.WriteLine("[Validation] Checking payload...");
        if (string.IsNullOrWhiteSpace(req.Payload))
        {
            Console.WriteLine("[Validation] Failed: payload is empty. Stopping chain.");
            return true; // handled (rejected)
        }
        Console.WriteLine("[Validation] Passed.");
        return false; // not handled, continue
    }
}

public class LoggingHandler : Handler
{
    public LoggingHandler() { Name = "LoggingHandler"; }
    public override bool Handle(Request req)
    {
        Console.WriteLine($"[Logging] Payload: {req.Payload}");
        return false; // logging does not terminate the chain
    }
}

public class ProcessingHandler : Handler
{
    public ProcessingHandler() { Name = "ProcessingHandler"; }
    public override bool Handle(Request req)
    {
        Console.WriteLine($"[Processing] Processing payload: {req.Payload}");
        Console.WriteLine("[Processing] Work complete.");
        return true; // handled successfully, stop chain
    }
}

