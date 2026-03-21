# Day 7 — Structural Patterns: Composite & Bridge

## Objectives
- Understand the Composite pattern for treating individual (leaf) and composed (composite) objects uniformly.
- Understand the Bridge pattern to decouple an abstraction from its implementation so both can vary independently.
- See traversal, operations, and separation of responsibilities in practice.
- Demo: menu/tree UI with Composite; rendering abstraction separated from concrete renderers using Bridge.
- Lab & homework: design a composite for a hierarchical domain and sketch a bridge for multiple platform renderers; draw UML and reason about extensibility.

---

## 1. Composite — Concept & Key Points
Composite lets clients treat single objects and compositions of objects uniformly by defining a common component interface. Use Composite when you have tree-like structures (documents, UI components, file systems, menus).

- Component: declares the interface for objects in the composition (operations, e.g., Render(), Add(), Remove()).
- Leaf: represents end objects with no children.
- Composite: has children and implements child-management; forwards operations to children.

Key ideas:
- Uniformity: code that uses components doesn’t care whether it’s a leaf or composite.
- Traversal: composites typically implement traversal (e.g., Depth-First) by delegating to children.
- Operations: operations are applied recursively.

C# example — simple menu/tree UI:

```csharp
// Component
public abstract class MenuComponent {
    public virtual void Add(MenuComponent c) => throw new NotSupportedException();
    public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
    public abstract void Render(int depth);
}

// Leaf
public class MenuItem : MenuComponent {
    public string Title { get; }
    public MenuItem(string title) => Title = title;
    public override void Render(int depth) {
        Console.WriteLine(new string(' ', depth * 2) + "- " + Title);
    }
}

// Composite
public class MenuGroup : MenuComponent {
    private readonly List<MenuComponent> _children = new();
    public string Title { get; }
    public MenuGroup(string title) => Title = title;
    public override void Add(MenuComponent c) => _children.Add(c);
    public override void Remove(MenuComponent c) => _children.Remove(c);
    public override void Render(int depth) {
        Console.WriteLine(new string(' ', depth * 2) + "+ " + Title);
        foreach (var child in _children) child.Render(depth + 1);
    }
}
```

Usage:
```csharp
var root = new MenuGroup("Root");
root.Add(new MenuItem("Home"));
var settings = new MenuGroup("Settings");
settings.Add(new MenuItem("General"));
settings.Add(new MenuItem("Security"));
root.Add(settings);
root.Render(0);
```

This prints a tree and shows uniform treatment: MenuItem and MenuGroup both implement Render.

---

## 2. Bridge — Concept & Key Points
Bridge separates an abstraction from its implementation. Use Bridge when you want to vary an abstraction and implementation independently, especially when multiple implementations target different platforms or subsystems (UI renderers, persistence layers).

- Abstraction: defines high-level operations and holds a reference to an Implementor.
- Implementor: defines low-level operations (e.g., DrawText, DrawRect).
- ConcreteImplementor: platform-specific implementations (Console, HTML, WPF, Skia, etc.).

Bridge decouples so you can add new implementations without changing the abstraction, and vice versa.

C# example — rendering abstraction and concrete renderers:

```csharp
// Implementor
public interface IRenderer {
    void DrawText(string text, int x, int y);
    void DrawBox(int x, int y, int width, int height);
}

// Concrete Implementor A
public class ConsoleRenderer : IRenderer {
    public void DrawText(string text, int x, int y)
        => Console.WriteLine($"Console: Text '{text}' at ({x},{y})");
    public void DrawBox(int x, int y, int width, int height)
        => Console.WriteLine($"Console: Box at ({x},{y}) {width}x{height}");
}

// Concrete Implementor B
public class HtmlRenderer : IRenderer {
    public void DrawText(string text, int x, int y)
        => Console.WriteLine($"<div style='position:absolute;left:{x}px;top:{y}px'>{text}</div>");
    public void DrawBox(int x, int y, int width, int height)
        => Console.WriteLine($"<div style='position:absolute;left:{x}px;top:{y}px;width:{width}px;height:{height}px;border:1px solid'> </div>");
}

// Abstraction
public abstract class Widget {
    protected readonly IRenderer Renderer;
    protected Widget(IRenderer renderer) => Renderer = renderer;
    public abstract void Draw();
}

// Refined Abstraction
public class Button : Widget {
    public string Label { get; }
    public Button(string label, IRenderer renderer) : base(renderer) => Label = label;
    public override void Draw() {
        Renderer.DrawBox(0, 0, 80, 30);
        Renderer.DrawText(Label, 10, 10);
    }
}
```

Usage:
```csharp
IRenderer console = new ConsoleRenderer();
Widget btn = new Button("OK", console);
btn.Draw();

IRenderer html = new HtmlRenderer();
Widget htmlBtn = new Button("Submit", html);
htmlBtn.Draw();
```

Notice: the Button abstraction never needs to know how drawing is implemented.

---

## 3. Demo Idea (Menu + Renderer)
- Build the Composite menu structure (MenuGroup/MenuItem).
- Give each MenuComponent a Render method that accepts an IRenderer (Bridge).
- Demonstrate swapping renderers at runtime (ConsoleRenderer, HtmlRenderer).
- Show how the tree is traversed and each node delegates drawing to the renderer.

Small hybrid snippet idea:
```csharp
public abstract class MenuComponent {
    public abstract void Render(IRenderer renderer, int depth);
}
```
Then both Composite and Leaf use renderer.DrawText / DrawBox.

---

## 4. Lab & Homework
Lab:
- Design a Composite for a hierarchical domain (e.g., document sections, UI components, file system tree). Implement Add/Remove/Render and traversal.
- Sketch a Bridge: define an abstraction (e.g., ComponentRenderer) and at least two Implementors (e.g., ConsoleRenderer, SvgRenderer). Wire them so the composite accepts a renderer.

Homework:
- Draw UML diagrams for both patterns:
  - Composite UML: Component, Leaf, Composite, associations (composite contains components).
  - Bridge UML: Abstraction, RefinedAbstraction, Implementor, ConcreteImplementor.
- Describe how adding a feature affects each:
  - Adding a new leaf type or component operation: Composite may require changes in the base Component interface and updates in leaf/composite; clients remain unchanged.
  - Adding a new rendering backend: Bridge allows adding a new ConcreteImplementor without changing the abstraction (Widget/MenuComponent). Adding a new Widget (abstraction) is also independent of implementors.

---

## 5. Quick Comparison & When to Use
- Use Composite when you need to represent part-whole hierarchies and treat parts uniformly.
- Use Bridge when you want to separate an abstraction from its implementation (platform independence, multiple backends).
- They can be combined: a Composite structure that delegates rendering through a Bridge is a common and powerful pattern.

---

## 6. Final Tips for Students
- Keep Component interfaces small and stable; prefer optional methods or explicit child-management interfaces to avoid NotSupportedException magic.
- In Bridge, focus on a minimal Implementor interface that provides primitive operations; compose higher-level behaviors in the Abstraction.
- Write unit tests for traversal and for each ConcreteImplementor's contract.

End of Day 7 summary — practice implementing both patterns and try combining them in a short demo app.