// 08_Adding_New_Renderer.cs
// Add a new Svg-like renderer demonstrating Bridge: new renderer added without changing Widget/Composite.

using System;

namespace Day07.AddingRenderer
{
    public interface IRenderer
    {
        void DrawText(string text, int x, int y);
        void DrawBox(int x, int y, int width, int height);
    }

    public class SvgRenderer : IRenderer
    {
        public void DrawText(string text, int x, int y)
            => Console.WriteLine($"<text x='{x}' y='{y}'>{System.Security.SecurityElement.Escape(text)}</text>");
        public void DrawBox(int x, int y, int width, int height)
            => Console.WriteLine($"<rect x='{x}' y='{y}' width='{width}' height='{height}' stroke='black' fill='transparent'/>");
    }

    public class ConsoleRenderer : IRenderer
    {
        public void DrawText(string text, int x, int y) => Console.WriteLine($"T[{x},{y}]: {text}");
        public void DrawBox(int x, int y, int width, int height) => Console.WriteLine($"B[{x},{y}] {width}x{height}");
    }

    public abstract class Widget
    {
        protected readonly IRenderer Renderer;
        protected Widget(IRenderer renderer) => Renderer = renderer;
        public abstract void Draw();
    }

    public class Button : Widget
    {
        public string Label { get; }
        public Button(string label, IRenderer r) : base(r) => Label = label;
        public override void Draw()
        {
            Renderer.DrawBox(0, 0, 100, 40);
            Renderer.DrawText(Label, 10, 20);
        }
    }

    class Program
    {
        static void Main()
        {
            IRenderer svg = new SvgRenderer();
            var svgBtn = new Button("Save", svg);
            Console.WriteLine("SVG rendering:");
            svgBtn.Draw();

            IRenderer console = new ConsoleRenderer();
            var cBtn = new Button("Cancel", console);
            Console.WriteLine("\nConsole rendering:");
            cBtn.Draw();
        }
    }
}