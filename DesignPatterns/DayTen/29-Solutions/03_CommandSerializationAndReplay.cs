using System;
using System.Text.Json;

namespace Day10.CommandCoR
{
    // DTO for command serialization
    public class InsertDto
    {
        public string Type { get; set; } = "InsertText";
        public int Index { get; set; }
        public string Text { get; set; } = "";
    }

    public class Document
    {
        public string Text { get; private set; } = "";
        public void Insert(int index, string text) => Text = Text.Insert(index, text);
        public void Remove(int index, int length) => Text = Text.Remove(index, length);
    }

    public interface ICommand
    {
        void Execute();
        void Unexecute();
    }

    public class InsertTextCommand : ICommand
    {
        private readonly Document _doc;
        private readonly int _index;
        private readonly string _text;

        public InsertTextCommand(Document doc, int index, string text)
        {
            _doc = doc; _index = index; _text = text;
        }

        public void Execute() => _doc.Insert(_index, _text);
        public void Unexecute() => _doc.Remove(_index, _text.Length);
    }

    class Program
    {
        static void Main()
        {
            var doc = new Document();
            var dto = new InsertDto { Index = 0, Text = "Serialized" };

            // Serialize DTO to JSON (this is what you'd persist)
            var json = JsonSerializer.Serialize(dto);
            Console.WriteLine($"Serialized command DTO: {json}");

            // Rehydrate: parse JSON and create concrete command
            var readDto = JsonSerializer.Deserialize<InsertDto>(json);
            if (readDto != null && readDto.Type == "InsertText")
            {
                var cmd = new InsertTextCommand(doc, readDto.Index, readDto.Text);
                cmd.Execute();
                Console.WriteLine($"After replay: '{doc.Text}'");
            }
        }
    }
}