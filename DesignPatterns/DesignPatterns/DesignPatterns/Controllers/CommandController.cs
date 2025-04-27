using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Command encapsulates a request as an object, thereby allowing for parameterization and queuing of requests.


namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        [HttpPost]
        public IActionResult ExecuteCommands([FromBody] List<string> commands)
        {
            var receiver = new Receiver();
            var invoker = new Invoker();

            foreach (var command in commands)
            {
                invoker.AddCommand(new ConcreteCommand(receiver, command));
            }

            invoker.ExecuteCommands();

            return Ok("Commands executed.");
        }
    }

    // ICommand.cs
    public interface ICommand
    {
        void Execute();
    }

    // Receiver.cs
    public class Receiver
    {
        public void Action(string command) => Console.WriteLine($"Receiver executed: {command}");
    }

    // ConcreteCommand.cs
    public class ConcreteCommand : ICommand
    {
        private readonly Receiver _receiver;
        private readonly string _command;

        public ConcreteCommand(Receiver receiver, string command)
        {
            _receiver = receiver;
            _command = command;
        }

        public void Execute() => _receiver.Action(_command);
    }

    // Invoker.cs
    public class Invoker
    {
        private readonly List<ICommand> _commands = new();

        public void AddCommand(ICommand command) => _commands.Add(command);

        public void ExecuteCommands()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }
    }
}
