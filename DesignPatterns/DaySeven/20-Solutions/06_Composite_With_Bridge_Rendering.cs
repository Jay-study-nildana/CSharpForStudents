// 06_Composite_With_Bridge_Rendering.cs
// Combine Composite and Bridge: MenuComponent.Render(IRenderer, int depth)
// Each component uses renderer primitives to draw.

using System;
using System.Collections.Generic;

namespace Day07.CompositeWithBridge
{
    public interface IRenderer
    {
        void DrawText(string text, int x, int y);
        void DrawBox(int x, int y, int width, int height);
    }

    public class ConsoleRenderer : IRenderer
    {
        public void DrawText(string text, int x, int y)
            => Console.WriteLine(new string(' ', y) + text);
        public void DrawBox(int x, int y, int width, int height)
            => Console.WriteLine(new string(' ', y) + $"[box {width}x{height}]");
    }

    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent c) => throw new NotSupportedException();
        public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
        public abstract void Render(IRenderer renderer, int depth);
    }

    public class MenuItem : MenuComponent
    {
        public string Name { get; }
        public MenuItem(string name) => Name = name;
        public override void Render(IRenderer renderer, int depth)
        {
            renderer.DrawText(new string(' ', depth * 2) + "- " + Name, 0, depth);
        }
    }

    public class MenuGroup : MenuComponent
    {
        private readonly List<MenuComponent> _children = new();
        public string Name { get; }
        public MenuGroup(string name) => Name = name;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);
        public override void Render(IRenderer renderer, int depth)
        {
            renderer.DrawText(new string(' ', depth * 2) + "+ " + Name, 0, depth);
            foreach (var child in _children) child.Render(renderer, depth + 1);
        }
    }

    class Program
    {
        static void Main()
        {
            var consoleRenderer = new ConsoleRenderer();
            var root = new MenuGroup("Root");
            root.Add(new MenuItem("Home"));
            var settings = new MenuGroup("Settings");
            settings.Add(new MenuItem("General"));
            settings.Add(new MenuItem("Security"));
            root.Add(settings);

            Console.WriteLine("Render with ConsoleRenderer:");
            root.Render(consoleRenderer, 0);

            // Could add an HtmlRenderer and re-render without touching MenuComponent
        }
    }
}