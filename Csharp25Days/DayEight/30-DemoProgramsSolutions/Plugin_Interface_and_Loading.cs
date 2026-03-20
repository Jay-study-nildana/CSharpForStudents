using System;
using System.Collections.Generic;

class Plugin_Interface_and_Loading
{
    public interface IPlugin
    {
        string Name { get; }
        void Run();
    }

    public class HelloPlugin : IPlugin
    {
        public string Name => "Hello";
        public void Run() => Console.WriteLine("HelloPlugin: Hello!");
    }

    public class TimePlugin : IPlugin
    {
        public string Name => "Time";
        public void Run() => Console.WriteLine($"TimePlugin: now = {DateTime.Now}");
    }

    static void Main()
    {
        // Simulate plugin discovery
        var plugins = new List<IPlugin> { new HelloPlugin(), new TimePlugin() };
        foreach (var p in plugins)
        {
            Console.WriteLine($"Running plugin: {p.Name}");
            p.Run();
        }

        // Interfaces + polymorphism make a simple plugin model: loader deals with IPlugin only.
    }
}