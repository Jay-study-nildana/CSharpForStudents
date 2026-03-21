// 07_Extending_With_New_Leaf.cs
// Add a new leaf type MenuToggle demonstrating minimal impact to existing composite.

using System;
using System.Collections.Generic;

namespace Day07.ExtendingComposite
{
    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent c) => throw new NotSupportedException();
        public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
        public abstract void Render(int depth);
    }

    public class MenuItem : MenuComponent
    {
        public string Title { get; }
        public MenuItem(string title) => Title = title;
        public override void Render(int depth) => Console.WriteLine(new string(' ', depth * 2) + "- " + Title);
    }

    // New leaf: MenuToggle (on/off state)
    public class MenuToggle : MenuComponent
    {
        public string Title { get; }
        public bool IsOn { get; private set; }
        public MenuToggle(string title, bool initial) { Title = title; IsOn = initial; }
        public void Toggle() => IsOn = !IsOn;
        public override void Render(int depth) => Console.WriteLine(new string(' ', depth * 2) + $"* {Title} [{(IsOn ? "On" : "Off")}]");
    }

    public class MenuGroup : MenuComponent
    {
        private readonly List<MenuComponent> _children = new();
        public string Title { get; }
        public MenuGroup(string title) => Title = title;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);
        public override void Render(int depth)
        {
            Console.WriteLine(new string(' ', depth * 2) + "+ " + Title);
            foreach (var c in _children) c.Render(depth + 1);
        }
    }

    class Program
    {
        static void Main()
        {
            var root = new MenuGroup("Root");
            root.Add(new MenuItem("Home"));
            var view = new MenuGroup("View");
            var toggle = new MenuToggle("Show Grid", false);
            view.Add(toggle);
            root.Add(view);

            Console.WriteLine("Before toggle:");
            root.Render(0);

            toggle.Toggle();
            Console.WriteLine("\nAfter toggle:");
            root.Render(0);

            // Note in comments: No change required to MenuGroup; new leaf only implements Render.
        }
    }
}