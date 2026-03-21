// 01-AbstractFactory_UI.cs
// Abstract Factory for UI components (IButton, IWindow, IUiFactory).
// DI/Lifetime: Register the chosen IUiFactory with IServiceCollection (Singleton if stateless).
// Testability: Inject a test factory or mock IUiFactory in unit tests.

using System;

public interface IButton { void Render(); }
public interface IWindow { void Open(); }

public interface IUiFactory
{
    IButton CreateButton();
    IWindow CreateWindow();
}

// Windows family
public class WindowsButton : IButton { public void Render() => Console.WriteLine("Render WindowsButton"); }
public class WindowsWindow : IWindow { public void Open() => Console.WriteLine("Open WindowsWindow"); }
public class WindowsFactory : IUiFactory
{
    public IButton CreateButton() => new WindowsButton();
    public IWindow CreateWindow() => new WindowsWindow();
}

// Mac family
public class MacButton : IButton { public void Render() => Console.WriteLine("Render MacButton"); }
public class MacWindow : IWindow { public void Open() => Console.WriteLine("Open MacWindow"); }
public class MacFactory : IUiFactory
{
    public IButton CreateButton() => new MacButton();
    public IWindow CreateWindow() => new MacWindow();
}

// Client uses the abstract factory
public class Application
{
    private readonly IUiFactory _uiFactory;
    public Application(IUiFactory uiFactory) => _uiFactory = uiFactory;

    public void Start()
    {
        var w = _uiFactory.CreateWindow();
        var b = _uiFactory.CreateButton();
        w.Open();
        b.Render();
    }
}

/*
DI registration example (conceptual):
// services.AddSingleton<IUiFactory, WindowsFactory>(); // or MacFactory per config
*/