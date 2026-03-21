// 10_UnitTests_For_Composite_Bridge.cs
// Simple console-based assertions to verify key behaviors of Composite and Bridge.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Day07.Tests
{
    static class Assert
    {
        public static void IsTrue(bool cond, string message = "Assertion failed")
        {
            if (!cond) throw new Exception(message);
        }

        public static void AreEqual<T>(T expected, T actual, string message = "")
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                throw new Exception($"Assert.AreEqual failed. Expected: {expected}, Actual: {actual}. {message}");
        }
    }

    // Reuse small implementations inline for testing
    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent c) => throw new NotSupportedException();
        public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
        public abstract string Title { get; }
    }
    public class MenuItem : MenuComponent { public override string Title { get; } public MenuItem(string t) => Title = t; }
    public class MenuGroup : MenuComponent
    {
        private readonly List<MenuComponent> _children = new();
        public override string Title { get; }
        public MenuGroup(string title) => Title = title;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);
        public IEnumerable<MenuComponent> Children => _children;
    }

    public interface IRenderer { void DrawText(string s, int x, int y); void DrawBox(int x, int y, int w, int h); }
    public class TestRenderer : IRenderer
    {
        public readonly List<string> Events = new();
        public void DrawText(string s, int x, int y) => Events.Add($"T:{s}@{x},{y}");
        public void DrawBox(int x, int y, int w, int h) => Events.Add($"B:{x},{y},{w}x{h}");
    }
    public abstract class Widget { protected readonly IRenderer R; protected Widget(IRenderer r) => R = r; public abstract void Draw(); }
    public class Button : Widget { public string Label { get; } public Button(string l, IRenderer r) : base(r) => Label = l; public override void Draw() { R.DrawBox(0, 0, 10, 4); R.DrawText(Label, 1, 2); } }

    class Program
    {
        static void Main()
        {
            // Composite add/remove
            var root = new MenuGroup("root");
            var child = new MenuItem("leaf");
            root.Add(child);
            Assert.AreEqual(1, ((List<MenuComponent>)root.Children).Count, "Add failed");
            root.Remove(child);
            Assert.AreEqual(0, ((List<MenuComponent>)root.Children).Count, "Remove failed");

            // Traversal order check (depth-first via simple build)
            var g = new MenuGroup("g");
            g.Add(new MenuItem("A"));
            var inner = new MenuGroup("inner");
            inner.Add(new MenuItem("B"));
            g.Add(inner);

            var visited = new List<string>();
            // Depth-first simulation
            void Dfs(MenuComponent m)
            {
                visited.Add(m.Title);
                if (m is MenuGroup mg)
                    foreach (var c in mg.Children) Dfs(c);
            }
            Dfs(g);
            Assert.AreEqual("g", visited[0]);
            Assert.AreEqual("A", visited[1]);
            Assert.AreEqual("inner", visited[2]);
            Assert.AreEqual("B", visited[3]);

            // Bridge draw validation
            var tr = new TestRenderer();
            var btn = new Button("Go", tr);
            btn.Draw();
            Assert.IsTrue(tr.Events.Count == 2, "Button should emit two draw events");
            Assert.IsTrue(tr.Events[1].StartsWith("T:Go"), "Text not drawn correctly");

            // Serialization round-trip (simple DTO)
            var dto = new { Type = "Group", Title = "root" };
            var json = JsonSerializer.Serialize(dto);
            var back = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            Assert.IsTrue(back != null && back.ContainsKey("Type"));

            Console.WriteLine("All tests passed.");
        }
    }
}