using System;
using System.Collections.Generic;

class Composite_Notifier
{
    // Problem: Composite notifier that delegates to children
    public interface INotifier { void Notify(string message); }

    public class EmailNotifier : INotifier { public void Notify(string message) => Console.WriteLine($"Email: {message}"); }
    public class SmsNotifier : INotifier { public void Notify(string message) => Console.WriteLine($"SMS: {message}"); }

    public class CompositeNotifier : INotifier
    {
        private readonly List<INotifier> _children = new();
        public void Add(INotifier n) => _children.Add(n);
        public void Notify(string message) { foreach (var c in _children) c.Notify(message); }
    }

    static void Main()
    {
        var composite = new CompositeNotifier();
        composite.Add(new EmailNotifier());
        composite.Add(new SmsNotifier());
        composite.Notify("System alert!");

        // Composite treats a group of notifiers as a single INotifier, simplifying client code.
    }
}