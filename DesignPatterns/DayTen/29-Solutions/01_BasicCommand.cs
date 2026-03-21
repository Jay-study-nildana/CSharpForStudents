using System;

namespace Day10.CommandCoR
{
    // Basic ICommand with Execute/Unexecute
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
        public void Insert(int index, string text) => Text = Text.Insert(index, text);
        public void Remove(int index, int length) => Text = Text.Remove(index, length);
    }

    // Concrete command
    public class InsertTextCommand : ICommand
    {
        private readonly Document _doc;
        private readonly int _index;
        private readonly string _text;
        public string Description => $"Insert '{_text}' at {_index}";

        public InsertTextCommand(Document doc, int index, string text)
        {
            _doc = doc;
            _index = index;
            _text = text;
        }

        public void Execute() => _doc.Insert(_index, _text);
        public void Unexecute() => _doc.Remove(_index, _text.Length);
    }

    class Program
    {
        static void Main()
        {
            var doc = new Document();
            var cmd = new InsertTextCommand(doc, 0, "Hello");
            Console.WriteLine($"Before: '{doc.Text}'");
            cmd.Execute();
            Console.WriteLine($"After Execute: '{doc.Text}'");
            cmd.Unexecute();
            Console.WriteLine($"After Unexecute: '{doc.Text}'");
        }
    }
}