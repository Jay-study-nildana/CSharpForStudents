using System;
using System.Collections.Generic;

namespace Day09.ObserverMediator
{
    // Problem: Mediator coordinating simple UI components.
    public interface IComponent
    {
        void Notify(string evt, object? data = null);
    }

    public class UIMediator
    {
        private readonly Dictionary<string, IComponent> _components = new();

        public void Register(string name, IComponent component) => _components[name] = component;

        public void Send(string sender, string evt, object? data = null)
        {
            // Centralized rules
            if (evt == "save")
            {
                _components.TryGetValue("status", out var status);
                status?.Notify("show", "Saving...");
            }
            else if (evt == "saved")
            {
                _components.TryGetValue("toolbar", out var toolbar);
                toolbar?.Notify("enable", "publish");
                _components.TryGetValue("status", out var status2);
                status2?.Notify("show", "Saved");
            }
            else if (evt == "error")
            {
                _components.TryGetValue("status", out var status3);
                status3?.Notify("show", $"Error: {data}");
            }
        }
    }

    public class Editor : IComponent
    {
        private readonly UIMediator _mediator;
        public Editor(UIMediator mediator) => _mediator = mediator;
        public void Save() => _mediator.Send("editor", "save");
        public void Notify(string evt, object? data = null)
        {
            // Editor may react to mediator messages if needed
            Console.WriteLine($"[Editor] Notified: {evt} {(data ?? "")}");
        }

        public void OnSaved() => _mediator.Send("editor", "saved");
    }

    public class Toolbar : IComponent
    {
        public void Notify(string evt, object? data = null)
        {
            Console.WriteLine($"[Toolbar] {evt} {(data ?? "")}");
        }
    }

    public class StatusBar : IComponent
    {
        public void Notify(string evt, object? data = null)
        {
            if (evt == "show") Console.WriteLine($"[StatusBar] {data}");
        }
    }

    class Program
    {
        static void Main()
        {
            var mediator = new UIMediator();
            var editor = new Editor(mediator);
            var toolbar = new Toolbar();
            var status = new StatusBar();

            mediator.Register("editor", editor);
            mediator.Register("toolbar", toolbar);
            mediator.Register("status", status);

            editor.Save();     // shows "Saving..."
            // simulate save completion
            editor.OnSaved();  // toolbar enable + "Saved"
        }
    }
}