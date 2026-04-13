// MediatorPatternDemo.cs
// Console demo of the Mediator Pattern for UI coordination.

using System;
using System.Collections.Generic;

// Top-level statements
var mediator = new UIMediator();
var editor = new Editor(mediator);
var toolbar = new Toolbar();
var status = new StatusBar();
mediator.Register("editor", editor);
mediator.Register("toolbar", toolbar);
mediator.Register("status", status);

// Interactive REPL for mediator demo
Console.WriteLine("Mediator demo interactive. Type 'help' for commands.");
while (true)
{
    Console.WriteLine("\nCommands: save, send, error, publish, status, list, register, unregister, help, exit");
    Console.Write("cmd: ");
    var cmd = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(cmd)) continue;
    cmd = cmd.Trim().ToLowerInvariant();
    if (cmd == "exit") break;

    if (cmd == "help")
    {
        Console.WriteLine("save        - ask the editor to save (goes through mediator)");
        Console.WriteLine("send        - manually send an event via the mediator");
        Console.WriteLine("error       - simulate an error event (shows in status)");
        Console.WriteLine("publish     - simulate a publish event (toolbar enable/disable)");
        Console.WriteLine("status      - show a custom status message");
        Console.WriteLine("list        - list registered components");
        Console.WriteLine("register    - register a simple anonymous StatusBar component");
        Console.WriteLine("unregister  - unregister a component by name");
        continue;
    }

    if (cmd == "save")
    {
        editor.Save();
        continue;
    }

    if (cmd == "send")
    {
        Console.Write("Sender name: "); var sender = Console.ReadLine();
        Console.Write("Event name: "); var ev = Console.ReadLine();
        Console.Write("Optional data (text): "); var data = Console.ReadLine();
        mediator.Send(sender, ev, new { text = data });
        continue;
    }

    if (cmd == "error")
    {
        Console.Write("Error message: "); var msg = Console.ReadLine();
        mediator.Send("system", "error", new { text = msg });
        continue;
    }

    if (cmd == "publish")
    {
        Console.Write("Topic to publish: "); var topic = Console.ReadLine();
        mediator.Send("toolbar", "publish", topic);
        continue;
    }

    if (cmd == "status")
    {
        Console.Write("Status text: "); var text = Console.ReadLine();
        mediator.Send("user", "show", new { text });
        continue;
    }

    if (cmd == "list")
    {
        var names = mediator.GetRegisteredComponents();
        Console.WriteLine("Registered components: " + string.Join(", ", names));
        continue;
    }

    if (cmd == "register")
    {
        Console.Write("Name for component: "); var name = Console.ReadLine();
        var comp = new StatusBar();
        mediator.Register(name, comp);
        Console.WriteLine($"Registered StatusBar as '{name}'");
        continue;
    }

    if (cmd == "unregister")
    {
        Console.Write("Name to unregister: "); var name = Console.ReadLine();
        mediator.Unregister(name);
        Console.WriteLine($"Unregistered '{name}' (if existed)");
        continue;
    }

    Console.WriteLine("Unknown command. Type 'help' for options.");
}


public interface IComponent { void Notify(string @event, object data = null); }

public class UIMediator
{
    private readonly Dictionary<string, IComponent> _components = new();
    public void Register(string name, IComponent comp) => _components[name] = comp;
    public void Send(string sender, string @event, object data = null)
    {
        // Centralized routing rules for demo purposes.
        try
        {
            if (@event == "save")
                _components["status"]?.Notify("show", new { text = "Saving..." });
            else if (@event == "saved")
            {
                _components["toolbar"]?.Notify("enable", "publish");
                _components["status"]?.Notify("show", new { text = "Saved" });
            }
            else if (@event == "error")
            {
                _components["status"]?.Notify("show", new { text = $"Error: {(data as dynamic)?.text ?? ""}" });
            }
            else if (@event == "publish")
            {
                // Example: toolbar asked to publish, toolbar shows a busy status
                _components["status"]?.Notify("show", new { text = $"Publishing: {data}" });
            }
            else if (@event == "show")
            {
                _components["status"]?.Notify("show", data);
            }
            else
            {
                // Default: broadcast to all registered components
                foreach (var kv in _components)
                    kv.Value?.Notify(@event, data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mediator routing error: {ex.Message}");
        }
    }

    public IEnumerable<string> GetRegisteredComponents() => _components.Keys;
    public void Unregister(string name) => _components.Remove(name);
}

// Example UI components
public class Editor : IComponent
{
    private readonly UIMediator _mediator;
    public Editor(UIMediator mediator) => _mediator = mediator;
    public void Save()
    {
        Console.WriteLine("[Editor] Save requested");
        _mediator.Send("editor", "save");
        // Simulate save
        System.Threading.Thread.Sleep(500);
        _mediator.Send("editor", "saved");
    }
    public void Notify(string @event, object data = null) { /* Not used in this demo */ }
}

public class Toolbar : IComponent
{
    public void Notify(string @event, object data = null)
    {
        if (@event == "enable")
            Console.WriteLine($"[Toolbar] Enabled: {data}");
        else if (@event == "disable")
            Console.WriteLine($"[Toolbar] Disabled: {data}");
        else if (@event == "publish")
            Console.WriteLine($"[Toolbar] Publishing: {data}");
    }
}

public class StatusBar : IComponent
{
    public void Notify(string @event, object data = null)
    {
        if (@event == "show")
            Console.WriteLine($"[StatusBar] {((dynamic)data).text}");
        else if (@event == "clear")
            Console.WriteLine("[StatusBar] (cleared)");
    }
}

