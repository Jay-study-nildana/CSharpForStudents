using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: Components are fully decoupled; only mediator knows routing rules.
    public interface IDevice
    {
        void Receive(string message, object? payload = null);
    }

    public class CentralMediator
    {
        private readonly Dictionary<string, IDevice> _devices = new();

        public void Register(string name, IDevice device) => _devices[name] = device;

        public void Broadcast(string sender, string message, object? payload = null)
        {
            // Example complex rule: when "save" occurs, inform autosave + status + telemetry
            if (message == "save")
            {
                _devices.TryGetValue("autosave", out var asv);
                asv?.Receive("trigger", payload);

                _devices.TryGetValue("status", out var st);
                st?.Receive("show", "Saving...");

                _devices.TryGetValue("telemetry", out var tel);
                tel?.Receive("event", new { action = "save", when = DateTime.UtcNow });
            }
            else
            {
                // default broadcast to everyone except sender
                foreach (var kv in _devices)
                {
                    if (kv.Key != sender) kv.Value.Receive(message, payload);
                }
            }
        }
    }

    public class Autosave : IDevice
    {
        public void Receive(string message, object? payload = null)
        {
            if (message == "trigger") Console.WriteLine("[Autosave] Saving draft...");
        }
    }

    public class Status : IDevice
    {
        public void Receive(string message, object? payload = null)
        {
            if (message == "show") Console.WriteLine($"[Status] {payload}");
        }
    }

    public class Telemetry : IDevice
    {
        public void Receive(string message, object? payload = null)
        {
            if (message == "event") Console.WriteLine($"[Telemetry] Logged: {payload}");
        }
    }

    class Program
    {
        static void Main()
        {
            var mediator = new CentralMediator();
            mediator.Register("autosave", new Autosave());
            mediator.Register("status", new Status());
            mediator.Register("telemetry", new Telemetry());

            // Editor triggers save through mediator (editor not modeled here)
            mediator.Broadcast("editor", "save", new { docId = 42 });

            // Another generic message broadcast
            mediator.Broadcast("editor", "focusChanged", "Editor focused");
        }
    }
}