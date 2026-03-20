using System;
using System.Collections.Generic;

class Plugin_Discovery_Interface
{
    // Problem: define IPlugin and simulate discovery/execution
    public interface IPlugin { string Name { get; } void Run(); }

    public class HelloPlugin : IPlugin { public string Name => "Hello"; public void Run() => Console.WriteLine("Hello plugin running"); }
    public class TimePlugin : IPlugin { public string Name => "Time"; public void Run() => Console.WriteLine($"Time: {DateTime.Now}"); }

    static void Main()
    {
        // Simulated discovery
        List<IPlugin> plugins = new() { new HelloPlugin(), new TimePlugin() };
        foreach (var p in plugins)
        {
            Console.WriteLine($"Executing plugin: {p.Name}");
            p.Run();
        }

        // Interfaces let plugin loader treat implementations uniformly and enable dynamic composition.
    }
}