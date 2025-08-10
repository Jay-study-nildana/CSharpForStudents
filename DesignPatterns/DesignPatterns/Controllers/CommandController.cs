using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Command encapsulates a request as an object, thereby allowing for parameterization and queuing of requests.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/command"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class CommandController : ControllerBase
    {
        // Handles HTTP POST requests to "api/command"
        // Expects a list of command strings in the request body
        [HttpPost]
        public IActionResult ExecuteCommands([FromBody] List<string> commands)
        {
            var receiver = new Receiver(); // The object that performs the actual work
            var invoker = new Invoker();   // The object that stores and executes commands

            // For each command string, create a command object and add it to the invoker
            foreach (var command in commands)
            {
                invoker.AddCommand(new ConcreteCommand(receiver, command));
            }

            // Execute all stored commands
            invoker.ExecuteCommands();

            // Return a confirmation response
            return Ok("Commands executed.");
        }
    }

    // Defines the interface for command objects
    public interface ICommand
    {
        // Method to execute the command
        void Execute();
    }

    // The receiver class contains the actual business logic to be executed
    public class Receiver
    {
        // Performs an action based on the command string
        public void Action(string command) => Console.WriteLine($"Receiver executed: {command}");
    }

    // Concrete implementation of a command
    public class ConcreteCommand : ICommand
    {
        private readonly Receiver _receiver; // Reference to the receiver
        private readonly string _command;    // The command to execute

        // Constructor sets the receiver and command string
        public ConcreteCommand(Receiver receiver, string command)
        {
            _receiver = receiver;
            _command = command;
        }

        // Executes the command by calling the receiver's action
        public void Execute() => _receiver.Action(_command);
    }

    // The invoker class stores and executes commands
    public class Invoker
    {
        private readonly List<ICommand> _commands = new(); // List of commands to execute

        // Adds a command to the list
        public void AddCommand(ICommand command) => _commands.Add(command);

        // Executes all commands in the list
        public void ExecuteCommands()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }
    }
}