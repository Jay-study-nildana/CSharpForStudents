// 03-DIManagedSingletonVsManual.cs
// Problem: Compare DI-managed singleton vs manual singleton.
// This file shows both approaches and lists tradeoffs in comments.

using System;
using Microsoft.Extensions.DependencyInjection;

public interface ILoggerService
{
    void Log(string message);
}

// Concrete logger
public class AppLogger : ILoggerService
{
    public void Log(string message) => Console.WriteLine($"[AppLogger] {message}");
}

/* Approach A: DI-managed singleton (preferred in DI apps)
   Registration (conceptual):
   services.AddSingleton<ILoggerService, AppLogger>();
   Then constructor-inject ILoggerService where needed.
*/

/* Approach B: Manual singleton */
public class ManualLogger : ILoggerService
{
    private static readonly Lazy<ManualLogger> _lazy = new(() => new ManualLogger());
    private ManualLogger() { }
    public static ManualLogger Instance => _lazy.Value;
    public void Log(string message) => Console.WriteLine($"[ManualLogger] {message}");
}

/*
Tradeoffs:
- Testability: DI-managed makes it easy to replace with mocks; manual singleton is harder to replace/reset.
- Lifecycle control: DI container manages disposal if the concrete type implements IDisposable; manual requires custom cleanup.
- Dependencies: DI allows constructor injection of dependencies; manual singletons must obtain/initialize dependencies themselves.
- Global access: manual singletons often encourage hidden dependencies; DI encourages explicit dependency graphs.
- Complexity: manual singleton is simple for tiny apps, but scales poorly in complex systems.
*/