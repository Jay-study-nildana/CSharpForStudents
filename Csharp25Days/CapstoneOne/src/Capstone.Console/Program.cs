using System;
using System.IO;
using System.Threading.Tasks;
using Capstone.Core.Storage;
using Capstone.Core.Services;
using Capstone.Core.Utils;

namespace Capstone.ConsoleApp
{
    // Application entrypoint wires up storage, services and the console UI.
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Ensure data directory exists and pick a file path.
                var dataFile = AppConfig.DefaultDataFile;
                Directory.CreateDirectory(Path.GetDirectoryName(dataFile) ?? ".");

                var storage = new FileStorage<Capstone.Core.Models.TaskItem>(dataFile);
                var service = new TaskService(storage);
                var ui = new ConsoleUI(service);

                await ui.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }
        }
    }
}
