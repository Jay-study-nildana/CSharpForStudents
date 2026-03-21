using System;
using System.Collections.Generic;

namespace Day10.CommandCoR
{
    public interface ICommand
    {
        void Execute();
        void Unexecute(); // may throw or be flagged unsupported
        bool IsReversible { get; }
        string Description { get; }
    }

    public class SendEmailCommand : ICommand
    {
        private readonly string _to;
        private readonly string _body;
        public bool IsReversible => false;
        public string Description => $"SendEmail to {_to}";

        public SendEmailCommand(string to, string body) { _to = to; _body = body; }
        public void Execute() => Console.WriteLine($"[Email] Sent to {_to}: {_body}");
        public void Unexecute() => throw new NotSupportedException("Cannot undo sent email");
    }

    public class CommandManager
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void ExecuteCommand(ICommand cmd)
        {
            cmd.Execute();
            if (cmd.IsReversible) _undo.Push(cmd);
            else Console.WriteLine($"Command '{cmd.Description}' is non-reversible; not pushed to undo stack.");
            _redo.Clear();
        }

        public void Undo()
        {
            if (_undo.Count == 0) { Console.WriteLine("Nothing to undo"); return; }
            var cmd = _undo.Pop();
            try
            {
                cmd.Unexecute();
                _redo.Push(cmd);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Safe-undo failed: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var mgr = new CommandManager();
            mgr.ExecuteCommand(new SendEmailCommand("bob@example.com", "Hello Bob"));
            mgr.Undo(); // safe behavior: nothing to undo, or handled message
        }
    }
}