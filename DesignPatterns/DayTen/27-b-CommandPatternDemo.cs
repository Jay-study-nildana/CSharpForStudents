// CommandPatternDemo.cs
// Console demo of the Command Pattern with undo/redo.

using System;
using System.Collections.Generic;
using System.Linq;

// Top-level statements
var doc = new Document();
var manager = new CommandManager();

// Enhanced interactive loop with descriptive input/output and command history
while (true)
{
    Console.WriteLine($"\nCurrent document content: '{doc.Text}'");
    Console.WriteLine("Available commands: insert, undo, redo, history, show, help, exit");
    Console.Write("Enter command: ");
    var cmd = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
    if (cmd == "exit") break;

    if (cmd == "help")
    {
        Console.WriteLine("insert  - insert text at a specified index (shows preview & asks confirmation)");
        Console.WriteLine("undo    - undo the last executed command (if any)");
        Console.WriteLine("redo    - redo the last undone command (if any)");
        Console.WriteLine("history - list descriptions of executed (undo stack) and redo stack");
        Console.WriteLine("show    - display current document contents");
        Console.WriteLine("exit    - quit the demo");
        continue;
    }

    if (cmd == "show")
    {
        Console.WriteLine($"\n--- Document ---\n{doc.Text}\n----------------\n");
        continue;
    }

    if (cmd == "history")
    {
        Console.WriteLine("\nUndo stack (most recent first):");
        foreach (var d in manager.GetUndoDescriptions())
            Console.WriteLine(" - " + d);

        Console.WriteLine("Redo stack (most recent first):");
        foreach (var d in manager.GetRedoDescriptions())
            Console.WriteLine(" - " + d);
        continue;
    }

    if (cmd == "insert")
    {
        Console.Write("Text to insert: ");
        var text = Console.ReadLine() ?? string.Empty;
        Console.Write($"Index (0..{doc.Text.Length}): ");
        var idxInput = Console.ReadLine();
        if (!int.TryParse(idxInput, out var idx) || idx < 0 || idx > doc.Text.Length)
        {
            Console.WriteLine("Invalid index. Insert aborted.");
            continue;
        }

        // Preview the result and ask for confirmation.
        var preview = doc.Text.Insert(idx, text);
        Console.WriteLine($"\nPreview after insert:\n'{preview}'\n");
        Console.Write("Apply this change? (y/n): ");
        var confirm = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
        if (confirm != "y" && confirm != "yes")
        {
            Console.WriteLine("Insert canceled.");
            continue;
        }

        var insertCmd = new InsertTextCommand(doc, idx, text);
        try
        {
            manager.ExecuteCommand(insertCmd);
            Console.WriteLine($"Inserted '{text}' at index {idx}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Command failed: {ex.Message}");
        }
        continue;
    }

    if (cmd == "undo")
    {
        if (!manager.CanUndo)
        {
            Console.WriteLine("Nothing to undo.");
            continue;
        }
        Console.WriteLine($"Undoing: {manager.PeekUndoDescription()}");
        manager.Undo();
        Console.WriteLine("Undo completed.");
        continue;
    }

    if (cmd == "redo")
    {
        if (!manager.CanRedo)
        {
            Console.WriteLine("Nothing to redo.");
            continue;
        }
        Console.WriteLine($"Redoing: {manager.PeekRedoDescription()}");
        manager.Redo();
        Console.WriteLine("Redo completed.");
        continue;
    }

    Console.WriteLine("Unknown command. Type 'help' for options.");
}


public interface ICommand
{
    void Execute();
    void Unexecute(); // For undo; can throw or be a no-op if not reversible
    string Description { get; }
}

// Receiver: performs work
public class Document
{
    public string Text { get; private set; } = "";
    public void Insert(int index, string text)
    {
        Text = Text.Insert(index, text);
    }
    public void Remove(int index, int length)
    {
        Text = Text.Remove(index, length);
    }
}

// Concrete Command
public class InsertTextCommand : ICommand
{
    private readonly Document _doc;
    private readonly int _index;
    private readonly string _text;
    public string Description => $"Insert '{_text}' at {_index}";
    public InsertTextCommand(Document doc, int index, string text)
    {
        _doc = doc; _index = index; _text = text;
    }
    public void Execute() => _doc.Insert(_index, _text);
    public void Unexecute() => _doc.Remove(_index, _text.Length);
}

// Invoker with undo/redo
public class CommandManager
{
    private readonly Stack<ICommand> _undo = new();
    private readonly Stack<ICommand> _redo = new();

    public void ExecuteCommand(ICommand cmd)
    {
        cmd.Execute();
        _undo.Push(cmd);
        _redo.Clear();
    }

    public void Undo()
    {
        if (_undo.Count == 0) return;
        var cmd = _undo.Pop();
        cmd.Unexecute();
        _redo.Push(cmd);
    }

    public void Redo()
    {
        if (_redo.Count == 0) return;
        var cmd = _redo.Pop();
        cmd.Execute();
        _undo.Push(cmd);
    }

    // Helpers for UI/inspection
    public bool CanUndo => _undo.Count > 0;
    public bool CanRedo => _redo.Count > 0;
    public IEnumerable<string> GetUndoDescriptions() => _undo.Reverse().Select(c => c.Description);
    public IEnumerable<string> GetRedoDescriptions() => _redo.Reverse().Select(c => c.Description);
    public string PeekUndoDescription() => _undo.Count > 0 ? _undo.Peek().Description : string.Empty;
    public string PeekRedoDescription() => _redo.Count > 0 ? _redo.Peek().Description : string.Empty;
}

