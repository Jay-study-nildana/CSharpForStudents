using System;
using System.Collections.Generic;

namespace Day10.CommandCoR
{
    // Command interface
    public interface ICommand
    {
        void Execute();
        void Unexecute();
        string Name { get; }
    }

    // Basic command: change a numeric value on a receiver
    public class ChangeValueCommand : ICommand
    {
        private readonly Receiver _recv;
        private readonly int _delta;
        public string Name => $"Change by {_delta}";
        public ChangeValueCommand(Receiver r, int delta) { _recv = r; _delta = delta; }
        public void Execute() => _recv.Value += _delta;
        public void Unexecute() => _recv.Value -= _delta;
    }

    public class Receiver { public int Value { get; set; } = 0; }

    // Handler chain for commands: validators/processors may reject/modify command
    public enum HandlerResult { Continue, Handled, Rejected }

    public abstract class CommandHandler
    {
        protected CommandHandler? Next { get; private set; }
        public CommandHandler SetNext(CommandHandler next) { Next = next; return next; }
        public abstract HandlerResult Handle(ICommand cmd);
    }

    public class ValidationHandler : CommandHandler
    {
        public override HandlerResult Handle(ICommand cmd)
        {
            // reject commands with negative delta in name (simple convention)
            if (cmd.Name.Contains(" -"))
            {
                Console.WriteLine("[Validation] Rejected negative change");
                return HandlerResult.Rejected;
            }
            Console.WriteLine("[Validation] Passed");
            return Next?.Handle(cmd) ?? HandlerResult.Continue;
        }
    }

    public class ExecutionHandler : CommandHandler
    {
        public override HandlerResult Handle(ICommand cmd)
        {
            cmd.Execute();
            Console.WriteLine($"[Execution] Executed command: {cmd.Name}");
            return HandlerResult.Handled;
        }
    }

    // Manager that routes command through handlers and tracks undo only if executed
    public class CommandPipelineManager
    {
        private readonly CommandHandler _entry;
        private readonly Stack<ICommand> _undo = new();

        public CommandPipelineManager(CommandHandler entry) { _entry = entry; }

        public void Send(ICommand cmd)
        {
            var result = _entry.Handle(cmd);
            if (result == HandlerResult.Handled)
            {
                _undo.Push(cmd);
            }
            else if (result == HandlerResult.Rejected)
            {
                Console.WriteLine($"[Manager] Command '{cmd.Name}' rejected by pipeline");
            }
        }

        public void Undo()
        {
            if (_undo.Count == 0) { Console.WriteLine("Nothing to undo"); return; }
            var cmd = _undo.Pop();
            cmd.Unexecute();
            Console.WriteLine($"Undone: {cmd.Name}");
        }
    }

    class Program
    {
        static void Main()
        {
            var receiver = new Receiver();
            var validator = new ValidationHandler();
            var executor = new ExecutionHandler();
            validator.SetNext(executor);

            var manager = new CommandPipelineManager(validator);

            var c1 = new ChangeValueCommand(receiver, 10);
            var c2 = new ChangeValueCommand(receiver, -5); // name will contain "-5", validation rejects

            Console.WriteLine($"Initial value: {receiver.Value}");
            manager.Send(c1);
            Console.WriteLine($"After c1: {receiver.Value}");
            manager.Send(c2); // expected to be rejected
            Console.WriteLine($"After c2 attempt: {receiver.Value}");
            manager.Undo();
            Console.WriteLine($"After undo: {receiver.Value}");
        }
    }
}