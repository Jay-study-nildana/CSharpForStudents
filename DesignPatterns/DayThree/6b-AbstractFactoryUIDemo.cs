// AbstractFactoryDemo.cs
// Console demo of the Abstract Factory pattern for UI families.

using System;

// Top-level statements
while (true)
{
    Console.WriteLine("Choose UI family: windows, mac, or exit");
    var choice = Console.ReadLine();
    if (choice == "exit") break;
    IUiFactory? factory = choice switch
    {
        "windows" => new WindowsFactory(),
        "mac" => new MacFactory(),
        _ => null
    };
    if (factory == null)
    {
        Console.WriteLine("Invalid choice.");
        continue;
    }
    var app = new Application(factory);
    app.Start();
}


public interface IButton { void Render(); }
public interface IWindow { void Open(); }

public interface IUiFactory
{
    IButton CreateButton();
    IWindow CreateWindow();
}

// Windows family
public class WindowsButton : IButton { public void Render() => Console.WriteLine("Render Windows Button"); }
public class WindowsWindow : IWindow { public void Open() => Console.WriteLine("Open Windows Window"); }
public class WindowsFactory : IUiFactory
{
    public IButton CreateButton() => new WindowsButton();
    public IWindow CreateWindow() => new WindowsWindow();
}

// Mac family
public class MacButton : IButton { public void Render() => Console.WriteLine("Render Mac Button"); }
public class MacWindow : IWindow { public void Open() => Console.WriteLine("Open Mac Window"); }
public class MacFactory : IUiFactory
{
    public IButton CreateButton() => new MacButton();
    public IWindow CreateWindow() => new MacWindow();
}

// Client uses the abstract factory
public class Application
{
    private readonly IUiFactory _factory;
    public Application(IUiFactory factory) => _factory = factory;
    public void Start()
    {
        var win = _factory.CreateWindow();
        var btn = _factory.CreateButton();
        win.Open(); btn.Render();
    }
}

