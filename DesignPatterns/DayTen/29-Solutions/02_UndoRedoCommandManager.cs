using System;
using System.Collections.Generic;

namespace Day10.CommandCoR
{
    public interface ICommand
    {
        void Execute();
        void Unexecute();
        bool CanUnexecute { get; }
        string Description { get; }
    }

    // Simple receiver
    public class Counter
    {
        public int Value { get; private set; }
        public void Add(int x) => Value += x;
        public void Sub(int x) => Value -= x;
    }

    public class AddCommand : ICommand
    {
        private readonly Counter _counter;
        private readonly int _amount;
        public bool CanUnexecute => true;
        public string Description => $"Add {_amount}";

        public AddCommand(Counter c, int amount) { _counter = c; _amount = amount; }
        public void Execute() => _counter.Add(_amount);
        public void Unexecute() => _counter.Sub(_amount);
    }

    public class CommandManager
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void ExecuteCommand(ICommand cmd)
        {
            cmd.Execute();
            if (cmd.CanUnexecute)
            {
                _undo.Push(cmd);
                _redo.Clear();
            }
            else
            {
                // Non-reversible: don't push to undo; clear redo as command is new action
                _redo.Clear();
            }
            Console.WriteLine($"Executed: {cmd.Description}");
        }

        public void Undo()
        {
            if (_undo.Count == 0) { Console.WriteLine("Nothing to undo"); return; }
            var cmd = _undo.Pop();
            cmd.Unexecute();
            _redo.Push(cmd);
            Console.WriteLine($"Undone: {cmd.Description}");
        }

        public void Redo()
        {
            if (_redo.Count == 0) { Console.WriteLine("Nothing to redo"); return; }
            var cmd = _redo.Pop();
            cmd.Execute();
            _undo.Push(cmd);
            Console.WriteLine($"Redone: {cmd.Description}");
        }
    }

    class Program
    {
        static void Main()
        {
            var counter = new Counter();
            var mgr = new CommandManager();

            mgr.ExecuteCommand(new AddCommand(counter, 5)); Console.WriteLine($"Counter: {counter.Value}");
            mgr.ExecuteCommand(new AddCommand(counter, 3)); Console.WriteLine($"Counter: {counter.Value}");

            mgr.Undo(); Console.WriteLine($"Counter after Undo: {counter.Value}");
            mgr.Redo(); Console.WriteLine($"Counter after Redo: {counter.Value}");
        }
    }
}