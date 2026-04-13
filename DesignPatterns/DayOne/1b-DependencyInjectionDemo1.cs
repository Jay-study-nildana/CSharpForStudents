using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IDoSomething,DoSomethingImpl>();

var provider = services.BuildServiceProvider();

var dosomething = provider.GetRequiredService<IDoSomething>();
dosomething.DoSomething();

//Console.WriteLine("Hello, World!");

public interface IDoSomething
{
    void DoSomething();
}

public class DoSomethingImpl : IDoSomething
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something...");
    }
}



