// 02_MenuCompositeTraversal.cs
// Adds traversal helpers (depth-first and breadth-first) to the composite.
// Demonstrates traversal order.

using System;
using System.Collections.Generic;

namespace Day07.CompositeTraversal
{
    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent c) => throw new NotSupportedException();
        public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
        public abstract string Title { get; }
    }

    public class MenuItem : MenuComponent
    {
        public override string Title { get; }
        public MenuItem(string title) => Title = title;
    }

    public class MenuGroup : MenuComponent
    {
        private readonly List<MenuComponent> _children = new();
        public override string Title { get; }
        public MenuGroup(string title) => Title = title;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);

        // Depth-first (pre-order)
        public void DepthFirstTraversal(Action<MenuComponent> action)
        {
            action(this);
            foreach (var child in _children)
            {
                if (child is MenuGroup g) g.DepthFirstTraversal(action);
                else action(child);
            }
        }

        // Breadth-first
        public void BreadthFirstTraversal(Action<MenuComponent> action)
        {
            var queue = new Queue<MenuComponent>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                action(current);
                if (current is MenuGroup mg)
                {
                    foreach (var c in mg._children) queue.Enqueue(c);
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var root = new MenuGroup("Root");
            root.Add(new MenuItem("Home"));
            var settings = new MenuGroup("Settings");
            settings.Add(new MenuItem("General"));
            settings.Add(new MenuItem("Security"));
            root.Add(settings);
            var about = new MenuGroup("About");
            about.Add(new MenuItem("Team"));
            root.Add(about);

            Console.WriteLine("Depth-first:");
            root.DepthFirstTraversal(c => Console.WriteLine(" - " + c.Title));

            Console.WriteLine("\nBreadth-first:");
            root.BreadthFirstTraversal(c => Console.WriteLine(" - " + c.Title));
        }
    }
}