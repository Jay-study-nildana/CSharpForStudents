// 01_SimpleMenuComposite.cs
// Basic Composite implementation for a menu/tree UI.
// Compile with: dotnet run (in a project) or add to an existing console app.

using System;
using System.Collections.Generic;

namespace Day07.CompositeExamples
{
    // Abstract Component
    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent c) => throw new NotSupportedException();
        public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
        public abstract void Render(int depth);
        public virtual string Title => GetType().Name;
    }

    // Leaf
    public class MenuItem : MenuComponent
    {
        public string Name { get; }
        public MenuItem(string name) => Name = name;
        public override void Render(int depth)
        {
            Console.WriteLine(new string(' ', depth * 2) + "- " + Name);
        }
        public override string Title => Name;
    }

    // Composite
    public class MenuGroup : MenuComponent
    {
        private readonly List<MenuComponent> _children = new();
        public string Name { get; }
        public MenuGroup(string name) => Name = name;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);
        public override void Render(int depth)
        {
            Console.WriteLine(new string(' ', depth * 2) + "+ " + Name);
            foreach (var child in _children) child.Render(depth + 1);
        }
        public override string Title => Name;
    }

    // Demo
    class Program
    {
        static void Main()
        {
            var root = new MenuGroup("Root");
            root.Add(new MenuItem("Home"));
            var settings = new MenuGroup("Settings");
            settings.Add(new MenuItem("General"));
            settings.Add(new MenuItem("Security"));
            var about = new MenuGroup("About");
            about.Add(new MenuItem("Team"));
            about.Add(new MenuItem("License"));
            root.Add(settings);
            root.Add(about);

            Console.WriteLine("Menu tree:");
            root.Render(0);
        }
    }
}