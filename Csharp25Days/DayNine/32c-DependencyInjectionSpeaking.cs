using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

//swap dependency injection for happy or sad saying by commenting and uncommenting the lines below
//for happy saying
//serviceCollection.AddSingleton<ISaySomething>(new HappySaySomething());
//for sad saying
serviceCollection.AddSingleton<ISaySomething>(new SadSaySomething());

//instead of AddSingleton , we could also use AddTransient or AddScoped, but for this example, it doesn't matter which one we use because we are only resolving the service once.

//AddSingleton will create a single instance of the service and reuse it throughout the application,
//while AddTransient will create a new instance every time it is requested,
//and AddScoped will create a new instance for each scope (e.g. per web request).

//either way, we can inject the same PersonSpeaks class and it will
//use the ISaySomething implementation that we registered above
serviceCollection.AddSingleton<PersonSpeaks>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var personSpeaks = serviceProvider.GetRequiredService<PersonSpeaks>();
personSpeaks.Speaking.SaySomething();


public interface ISaySomething
{
    void SaySomething();
}

public class HappySaySomething : ISaySomething
{
    public void SaySomething()
    {
        Console.WriteLine("Happy!");
    }
}

public class SadSaySomething : ISaySomething
{
    public void SaySomething()
    {
        Console.WriteLine("Sad!");
    }
}

public class PersonSpeaks
{
    public ISaySomething Speaking { get; }
    public PersonSpeaks(ISaySomething saySomething)
    {
        Speaking = saySomething;
    }
}