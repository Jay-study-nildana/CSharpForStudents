// 03_Composite_With_Iterator.cs
// Make MenuGroup enumerable (depth-first) so 'foreach' works over the entire subtree.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Day07.CompositeIterator
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

    public class MenuGroup : MenuComponent, IEnumerable<MenuComponent>
    {
        private readonly List<MenuComponent> _children = new();
        public override string Title { get; }
        public MenuGroup(string title) => Title = title;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);

        // Recursive depth-first iterator
        public IEnumerator<MenuComponent> GetEnumerator()
        {
            yield return this;
            foreach (var child in _children)
            {
                if (child is MenuGroup mg)
                {
                    foreach (var nested in mg) yield return nested;
                }
                else yield return child;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class Program
    {
        static void Main()
        {
            var root = new MenuGroup("Root");
            root.Add(new MenuItem("Home"));
            var g = new MenuGroup("Settings");
            g.Add(new MenuItem("General"));
            g.Add(new MenuItem("Security"));
            root.Add(g);
            root.Add(new MenuItem("About"));

            Console.WriteLine("Iterating depth-first with foreach:");
            foreach (var node in root)
            {
                Console.WriteLine(" - " + node.Title);
            }

            // LINQ usage
            var leafCount = root.OfType<MenuItem>().Count();
            Console.WriteLine($"\nLeaf count: {leafCount}");
        }
    }
}