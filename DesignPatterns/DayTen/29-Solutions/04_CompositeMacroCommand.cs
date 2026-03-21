using System;
using System.Collections.Generic;

namespace Day10.CommandCoR
{
    public interface ICommand
    {
        void Execute();
        void Unexecute();
        string Description { get; }
    }

    // Receiver
    public class Document
    {
        public string Text { get; private set; } = "";
        public void Insert(int idx, string s) => Text = Text.Insert(idx, s);
        public void Remove(int idx, int len) => Text = Text.Remove(idx, len);
    }

    public class InsertCommand : ICommand
    {
        private readonly Document _doc;
        private readonly int _idx;
        private readonly string _text;
        public string Description => $"Insert '{_text}'";

        public InsertCommand(Document d, int idx, string text) { _doc = d; _idx = idx; _text = text; }
        public void Execute() => _doc.Insert(_idx, _text);
        public void Unexecute() => _doc.Remove(_idx, _text.Length);
    }

    // Composite (Macro) command
    public class CompositeCommand : ICommand
    {
        private readonly List<ICommand> _children = new();
        public string Description => $"Macro({_children.Count} cmds)";
        public void Add(ICommand cmd) => _children.Add(cmd);
        public void Execute()
        {
            foreach (var c in _children) c.Execute();
        }

        public void Unexecute()
        {
            // reverse order
            for (int i = _children.Count - 1; i >= 0; i--) _children[i].Unexecute();
        }
    }

    class Program
    {
        static void Main()
        {
            var doc = new Document();
            var macro = new CompositeCommand();
            macro.Add(new InsertCommand(doc, 0, "Hello"));
            macro.Add(new InsertCommand(doc, 5, " World"));
            Console.WriteLine($"Before: '{doc.Text}'");
            macro.Execute();
            Console.WriteLine($"After Execute: '{doc.Text}'");
            macro.Unexecute();
            Console.WriteLine($"After Unexecute: '{doc.Text}'");
        }
    }
}