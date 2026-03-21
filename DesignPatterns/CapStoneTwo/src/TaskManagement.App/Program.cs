using Microsoft.Extensions.DependencyInjection;
using TaskManagement.App.UI;
using TaskManagement.Core;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Patterns.Behavioral;
using TaskManagement.Core.Patterns.Creational;
using TaskManagement.Core.Patterns.Structural;
using TaskManagement.Infrastructure.Adapters;
using TaskManagement.Infrastructure.Logging;
using TaskManagement.Infrastructure.Persistence;

// ─── Composition Root ─────────────────────────────────────────────────────────
// This is the ONLY place where concrete types are wired together.
// Everything else depends only on interfaces (Dependency Inversion Principle).

var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
var logDir  = Path.Combine(AppContext.BaseDirectory, "logs");

var services = new ServiceCollection();

// ── Singleton: Logger ─────────────────────────────────────────────────────────
services.AddSingleton<IAppLogger>(_ => new AppLogger(logDir));

// ── Unit of Work + Repositories ──────────────────────────────────────────────
services.AddSingleton<IUnitOfWork>(_ => new JsonFileUnitOfWork(dataDir));

// ── Strategy: default sort strategy ──────────────────────────────────────────
services.AddSingleton<IPriorityStrategy, PriorityDescendingStrategy>();

// ── Core Task Service (raw, no decorators) ────────────────────────────────────
services.AddSingleton<TaskService>();

// ── Decorator chain: Validation → Logging → Core ─────────────────────────────
// Registered as ITaskService so consumers get the fully-decorated version
services.AddSingleton<ITaskService>(sp =>
{
    var core      = sp.GetRequiredService<TaskService>();
    var logger    = sp.GetRequiredService<IAppLogger>();
    var validator = ValidationChainBuilder.Build();

    ITaskService decorated = core;
    decorated = new ValidationTaskServiceDecorator(decorated, validator);
    decorated = new LoggingTaskServiceDecorator(decorated, logger);
    return decorated;
});

// ── Observer / Event Bus ──────────────────────────────────────────────────────
services.AddSingleton<TaskEventBus>(sp =>
{
    var bus    = new TaskEventBus();
    var logger = sp.GetRequiredService<IAppLogger>();
    bus.Subscribe(new LoggingTaskEventListener(logger));
    bus.Subscribe(new ConsoleAlertEventListener());
    return bus;
});

// ── Abstract Factory: Notification ───────────────────────────────────────────
// Console channel by default; swap to EmailNotificationProviderFactory to change
services.AddSingleton<INotificationProviderFactory>(sp =>
    new ConsoleNotificationProviderFactory(sp.GetRequiredService<IAppLogger>()));

services.AddSingleton<INotificationSender>(sp =>
    sp.GetRequiredService<INotificationProviderFactory>().CreateSender());

// ── Facade ────────────────────────────────────────────────────────────────────
services.AddSingleton<WorkflowFacade>();

// ── Command History (undo/redo) ───────────────────────────────────────────────
services.AddSingleton<CommandHistory>();

// ── Mediator ─────────────────────────────────────────────────────────────────
services.AddSingleton<IWorkflowMediator, WorkflowMediator>();

// ── Bridge: Console Report Renderer ──────────────────────────────────────────
services.AddSingleton<IReportRenderer, ConsoleReportRenderer>();
services.AddSingleton<TaskReportGenerator>();

// ── Console Menu ─────────────────────────────────────────────────────────────
services.AddSingleton<ConsoleMenu>();

var sp = services.BuildServiceProvider();

// ── Demo: import legacy tasks via Adapter ────────────────────────────────────
var logger  = sp.GetRequiredService<IAppLogger>();
var uow     = sp.GetRequiredService<IUnitOfWork>();

logger.Log("Application starting.");

var existing = await uow.Tasks.GetAllAsync();
if (!existing.Any())
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  [Adapter] Importing tasks from legacy CSV source...");
    Console.ResetColor();

    var adapter  = new LegacyTaskAdapter(new HardCodedLegacyCsvSource());
    var eventBus = sp.GetRequiredService<TaskEventBus>();
    foreach (var task in adapter.ImportTasks())
    {
        await uow.Tasks.AddAsync(task);
        eventBus.PublishCreated(task);
    }
    await uow.SaveChangesAsync();
    Console.WriteLine("  [Adapter] Import complete.\n");
}

// ── Demo: Composite tree view ─────────────────────────────────────────────────
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("  [Composite] Building project tree view...");
Console.ResetColor();

var allTasks  = (await uow.Tasks.GetAllAsync()).ToList();
var project   = new TaskManagement.Core.Domain.TaskGroup("Project Alpha");
var sprintOne = new TaskManagement.Core.Domain.TaskGroup("Sprint 1");
var sprintTwo = new TaskManagement.Core.Domain.TaskGroup("Sprint 2");

foreach (var t in allTasks.Take(2))  sprintOne.Add(new TaskManagement.Core.Domain.TaskLeaf(t));
foreach (var t in allTasks.Skip(2))  sprintTwo.Add(new TaskManagement.Core.Domain.TaskLeaf(t));
project.Add(sprintOne);
project.Add(sprintTwo);
project.Display();

// ── Demo: Template Method processors ─────────────────────────────────────────
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("\n  [Template Method] Processing first two tasks...");
Console.ResetColor();

var bugProcessor     = new BugTaskProcessor(logger);
var featureProcessor = new FeatureTaskProcessor(logger);

if (allTasks.Count > 0) await bugProcessor.ProcessAsync(allTasks[0]);
if (allTasks.Count > 1) await featureProcessor.ProcessAsync(allTasks[1]);

// ── Start interactive console menu ───────────────────────────────────────────
var menu = sp.GetRequiredService<ConsoleMenu>();
await menu.RunAsync();

logger.Log("Application exiting.");
