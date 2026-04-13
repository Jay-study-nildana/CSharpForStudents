// BridgePatternDemo.cs
// Console demo of the Bridge Pattern for rendering widgets.

using System;

// Top-level statements
while (true)
{
    Console.WriteLine("Choose renderer: console, html, or exit");
    var choice = Console.ReadLine();
    if (choice == "exit") break;
    IRenderer? renderer = choice switch
    {
        "console" => new ConsoleRenderer(),
        "html" => new HtmlRenderer(),
        _ => null
    };
    if (renderer == null)
    {
        Console.WriteLine("Invalid renderer.");
        continue;
    }
    Console.Write("Enter button label: ");
    var label = Console.ReadLine();
    var button = new Button(label, renderer);
    button.Draw();
    Console.WriteLine();
}


// Implementor
public interface IRenderer
{
    void DrawText(string text, int x, int y);
    void DrawBox(int x, int y, int width, int height);
}

// Concrete Implementor A
public class ConsoleRenderer : IRenderer
{
    public void DrawText(string text, int x, int y)
        => Console.WriteLine($"Console: Text '{text}' at ({x},{y})");
    public void DrawBox(int x, int y, int width, int height)
        => Console.WriteLine($"Console: Box at ({x},{y}) {width}x{height}");
}

// Concrete Implementor B
public class HtmlRenderer : IRenderer
{
    public void DrawText(string text, int x, int y)
        => Console.WriteLine($"<div style='position:absolute;left:{x}px;top:{y}px'>{text}</div>");
    public void DrawBox(int x, int y, int width, int height)
        => Console.WriteLine($"<div style='position:absolute;left:{x}px;top:{y}px;width:{width}px;height:{height}px;border:1px solid'> </div>");
}

// Abstraction
public abstract class Widget
{
    protected readonly IRenderer Renderer;
    protected Widget(IRenderer renderer) => Renderer = renderer;
    public abstract void Draw();
}

// Refined Abstraction
public class Button : Widget
{
    public string Label { get; }
    public Button(string label, IRenderer renderer) : base(renderer) => Label = label;
    public override void Draw()
    {
        Renderer.DrawBox(0, 0, 80, 30);
        Renderer.DrawText(Label, 10, 10);
    }
}

