// 05_Bridge_Renderer_Abstract.cs
// Bridge pattern: IRenderer (Implementor), ConsoleRenderer/HtmlRenderer (ConcreteImplementor),
// Abstraction: Widget, RefinedAbstraction: Button.

using System;

namespace Day07.Bridge
{
    // Implementor
    public interface IRenderer
    {
        void DrawText(string text, int x, int y);
        void DrawBox(int x, int y, int width, int height);
    }

    // ConcreteImplementor: Console
    public class ConsoleRenderer : IRenderer
    {
        public void DrawText(string text, int x, int y)
            => Console.WriteLine($"Console: Text '{text}' at ({x},{y})");
        public void DrawBox(int x, int y, int width, int height)
            => Console.WriteLine($"Console: Box at ({x},{y}) {width}x{height}");
    }

    // ConcreteImplementor: HTML-like
    public class HtmlRenderer : IRenderer
    {
        public void DrawText(string text, int x, int y)
            => Console.WriteLine($"<div style='position:absolute;left:{x}px;top:{y}px'>{text}</div>");
        public void DrawBox(int x, int y, int width, int height)
            => Console.WriteLine($"<div style='position:absolute;left:{x}px;top:{y}px;width:{width}px;height:{height}px;border:1px solid'></div>");
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

    class Program
    {
        static void Main()
        {
            IRenderer console = new ConsoleRenderer();
            Widget btn1 = new Button("OK", console);
            btn1.Draw();

            IRenderer html = new HtmlRenderer();
            Widget btn2 = new Button("Submit", html);
            btn2.Draw();
        }
    }
}