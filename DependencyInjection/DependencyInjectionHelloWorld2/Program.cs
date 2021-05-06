using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

//IMPORTANT NOTE : For the sake of brevity and easy scrolling of code, 
//I have put every class, interface and implementation in this one single file
//in an actual project, obviously, all these things will be in their own file and folders.

namespace DependencyInjectionHelloWorld2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World - Dependency Injection Begins!");

            MessageWriter tempMessageWriter = new MessageWriter();
            tempMessageWriter.Write("I am finally learning dependency injection after having avoided it for 9 years. Oh my god!");

            CreateHostBuilder(args).Build().Run();

            //with DI
            CreateHostBuilderWithDI(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                    {
                        //here, the worker is being injected to the code
                        //but we cannot change the worker implementation
                        //to change worker behavior, we need to dig deeper into the worker class
                        //creating the mess that comes with using concrete dependencies.
                        services.AddHostedService<Worker>();
                    }

                );

        //with DI
        public static IHostBuilder CreateHostBuilderWithDI(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                    {
                        //this is the key part 
                        //the worker service is being included
                        //and it depends on an interface, which is recommended
                        //and then, the interface is being attached with the concrete implementation
                        //only at this point.
                        //that means, if we decide to use a different IMessageWriter
                        //implementation we simply chanage that here
                        //the Worker class need not be changed, as it only depends on the interface
                        //and not on any actual class.
                        //So, the dependency is being injected, at run time. 
                        services.AddHostedService<WorkerWithDI>()
                                .AddScoped<IMessageWriter, MessageWriterWithDI>();
                    });
    }
}

//without DI things.

public class MessageWriter
{
    public void Write(string message)
    {
        Console.WriteLine($"MessageWriter.Write(message: \"{message}\")");
    }
}

// The class creates and directly depends on the MessageWriter class. Hard-coded dependencies, are problematic.
// and should be avoided for the following reasons:
// To replace MessageWriter with a different implementation, the Worker class must be modified.
// If MessageWriter has dependencies, they must also be configured by the Worker class. 
//In a large project with multiple classes depending on MessageWriter, the configuration code becomes scattered across the app.
// This implementation is difficult to unit test. 
//The app should use a mock or stub MessageWriter class, which isn't possible with this approach.
public class Worker : BackgroundService
{
    private readonly MessageWriter _messageWriter = new MessageWriter();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _messageWriter.Write($"Worker running at: {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}


//With DI things

public interface IMessageWriter
{
    void Write(string message);
}

public class MessageWriterWithDI : IMessageWriter
{
    public void Write(string message)
    {
        Console.WriteLine($"MessageWriterWithDI.Write(message: \"{message}\")");
    }
}

public class WorkerWithDI : BackgroundService
{
    private readonly IMessageWriter _messageWriter;

    public WorkerWithDI(IMessageWriter messageWriter) =>
        _messageWriter = messageWriter;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _messageWriter.Write($"Worker running at: {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}