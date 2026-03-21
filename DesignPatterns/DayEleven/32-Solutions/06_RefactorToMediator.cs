using System;
using System.Collections.Generic;

//
// Problem: Refactor to Mediator
// Test plan: components register and mediator coordinates save -> status -> autosave; demo prints sequence.
// Demonstrates: Mediator pattern centralizing interaction.
//

namespace Day11.RefactorLab
{
    public interface IComponent
    {
        void Notify(string evt, object? data = null);
    }

    public class UIMediator
    {
        private readonly Dictionary<string, IComponent> _components = new();
        public void Register(string name, IComponent c) => _components[name] = c;
        public void Send(string sender, string evt, object? data = null)
        {
            if (evt == "save")
            {
                _components.TryGetValue("status", out var s); s?.Notify("show", "Saving...");
                _components.TryGetValue("autosave", out var a); a?.Notify("trigger", data);
            }
            else if (evt == "saved")
            {
                _components.TryGetValue("status", out var s2); s2?.Notify("show", "Saved");
            }
        }
    }

    public class Editor : IComponent
    {
        private readonly UIMediator _m;
        public Editor(UIMediator m) { _m = m; }
        public void Save() => _m.Send("editor", "save");
        public void Notify(string evt, object? data = null) => Console.WriteLine($"[Editor] Received {evt}");
    }

    public class Autosave : IComponent { public void Notify(string evt, object? data = null) => Console.WriteLine("[Autosave] Triggered"); }
    public class StatusBar : IComponent { public void Notify(string evt, object? data = null) => Console.WriteLine($"[Status] {data}"); }

    class Program
    {
        static void Main()
        {
            var mediator = new UIMediator();
            var editor = new Editor(mediator);
            var autosave = new Autosave();
            var status = new StatusBar();

            mediator.Register("editor", editor);
            mediator.Register("autosave", autosave);
            mediator.Register("status", status);

            editor.Save();
            mediator.Send("editor", "saved");
        }
    }
}