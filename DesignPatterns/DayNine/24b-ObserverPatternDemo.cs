// ObserverPatternDemo.cs
// Console demo of Observer Pattern: Push, Pull, and Event Bus styles.

using System;
using System.Collections.Generic;

// Top-level statements
Console.WriteLine("--- Push-style Observer Demo ---");
var pushSubject = new SubjectPush<(string user, string message)>();
pushSubject.Subscribe(p => Console.WriteLine($"Push received: {p.user}: {p.message}"));
pushSubject.Notify(("Alice", "Hello from push!"));

Console.WriteLine("\n--- Pull-style Observer Demo ---");
var pullSubject = new SubjectPull();
pullSubject.Subscribe(() => Console.WriteLine($"Pulled state: {pullSubject.GetState()}"));
pullSubject.Increment();
pullSubject.Increment();

Console.WriteLine("\n--- Event Bus Demo ---");
var bus = new EventBus();
bus.On("user:login", payload => Console.WriteLine($"User login event: {payload}"));
bus.On("doc:updated", payload => Console.WriteLine($"Document updated: {payload}"));
bus.Emit("user:login", "Bob");
bus.Emit("doc:updated", new { id = 42, title = "Observer Pattern" });

// Interactive section - enhanced for each observer variant -----------------
// We keep small registries of created delegates so the user can subscribe
// and unsubscribe by id. This makes the demo feel more like a real system.

int pushNextId = 1;
var pushHandlers = new Dictionary<int, Action<(string user, string message)>>();

int pullNextId = 1;
var pullHandlers = new Dictionary<int, Action>();

int busNextId = 1;
var busHandlers = new Dictionary<string, Dictionary<int, Action<object>>>();

while (true)
{
    Console.WriteLine("\nChoose demo: push, pull, bus, info, or exit");
    var choice = Console.ReadLine();
    if (choice == "exit") break;

    if (choice == "info")
    {
        Console.WriteLine("Available demos:\n - push: manage push-style observers\n - pull: manage pull-style observers\n - bus: manage event-bus subscriptions\n");
        continue;
    }

    if (choice == "push")
    {
        while (true)
        {
            Console.WriteLine("\nPush demo - commands: add, remove, list, notify, back");
            Console.Write("cmd: ");
            var cmd = Console.ReadLine();
            if (cmd == "back") break;

            if (cmd == "add")
            {
                var id = pushNextId++;
                // Create a handler that prints the payload and its subscriber id.
                Action<(string user, string message)> handler = p =>
                    Console.WriteLine($"[Push subscriber {id}] {p.user}: {p.message}");
                pushHandlers[id] = handler;
                pushSubject.Subscribe(handler);
                Console.WriteLine($"Added push subscriber {id}");
            }
            else if (cmd == "remove")
            {
                Console.Write("Subscriber id: ");
                if (!int.TryParse(Console.ReadLine(), out var id) || !pushHandlers.ContainsKey(id))
                {
                    Console.WriteLine("Invalid id");
                    continue;
                }
                var handler = pushHandlers[id];
                pushSubject.Unsubscribe(handler);
                pushHandlers.Remove(id);
                Console.WriteLine($"Removed push subscriber {id}");
            }
            else if (cmd == "list")
            {
                if (pushHandlers.Count == 0) Console.WriteLine("No push subscribers");
                else Console.WriteLine("Push subscribers: " + string.Join(", ", pushHandlers.Keys));
            }
            else if (cmd == "notify")
            {
                Console.Write("User: "); var user = Console.ReadLine();
                Console.Write("Message: "); var msg = Console.ReadLine();
                pushSubject.Notify((user, msg));
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
    else if (choice == "pull")
    {
        while (true)
        {
            Console.WriteLine("\nPull demo - commands: add, remove, list, inc, state, back");
            Console.Write("cmd: ");
            var cmd = Console.ReadLine();
            if (cmd == "back") break;

            if (cmd == "add")
            {
                var id = pullNextId++;
                Action handler = () => Console.WriteLine($"[Pull observer {id}] Pulled state: {pullSubject.GetState()}");
                pullHandlers[id] = handler;
                pullSubject.Subscribe(handler);
                Console.WriteLine($"Added pull observer {id}");
            }
            else if (cmd == "remove")
            {
                Console.Write("Observer id: ");
                if (!int.TryParse(Console.ReadLine(), out var id) || !pullHandlers.ContainsKey(id))
                {
                    Console.WriteLine("Invalid id");
                    continue;
                }
                var handler = pullHandlers[id];
                // Note: SubjectPull doesn't expose Unsubscribe publicly in demo;
                // it does, so we can call it.
                pullSubject.Subscribe(() => { }); // noop to keep API usage obvious
                pullSubject.Subscribe(handler); // ensure it's present
                pullSubject.Unsubscribe(handler);
                pullHandlers.Remove(id);
                Console.WriteLine($"Removed pull observer {id}");
            }
            else if (cmd == "list")
            {
                if (pullHandlers.Count == 0) Console.WriteLine("No pull observers");
                else Console.WriteLine("Pull observers: " + string.Join(", ", pullHandlers.Keys));
            }
            else if (cmd == "inc")
            {
                pullSubject.Increment();
            }
            else if (cmd == "state")
            {
                Console.WriteLine($"Current state: {pullSubject.GetState()}");
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
    else if (choice == "bus")
    {
        while (true)
        {
            Console.WriteLine("\nBus demo - commands: subscribe, unsubscribe, list, emit, back");
            Console.Write("cmd: ");
            var cmd = Console.ReadLine();
            if (cmd == "back") break;

            if (cmd == "subscribe")
            {
                Console.Write("Topic: "); var topic = Console.ReadLine();
                var id = busNextId++;
                Action<object> handler = payload => Console.WriteLine($"[Bus subscriber {id} @ {topic}] Payload: {payload}");
                if (!busHandlers.ContainsKey(topic)) busHandlers[topic] = new Dictionary<int, Action<object>>();
                busHandlers[topic][id] = handler;
                bus.On(topic, handler);
                Console.WriteLine($"Subscribed id {id} to topic '{topic}'");
            }
            else if (cmd == "unsubscribe")
            {
                Console.Write("Topic: "); var topic = Console.ReadLine();
                Console.Write("Subscriber id: ");
                if (!int.TryParse(Console.ReadLine(), out var id) || !busHandlers.ContainsKey(topic) || !busHandlers[topic].ContainsKey(id))
                {
                    Console.WriteLine("Invalid topic or id");
                    continue;
                }
                var handler = busHandlers[topic][id];
                bus.Off(topic, handler);
                busHandlers[topic].Remove(id);
                Console.WriteLine($"Unsubscribed {id} from '{topic}'");
            }
            else if (cmd == "list")
            {
                if (busHandlers.Count == 0) Console.WriteLine("No topics/subscriptions");
                else
                {
                    foreach (var kv in busHandlers)
                    {
                        Console.WriteLine($"Topic '{kv.Key}': subscribers: {string.Join(", ", kv.Value.Keys)}");
                    }
                }
            }
            else if (cmd == "emit")
            {
                Console.Write("Topic: "); var topic = Console.ReadLine();
                Console.Write("Payload: "); var payload = Console.ReadLine();
                bus.Emit(topic, payload);
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
    else
    {
        Console.WriteLine("Unknown demo choice");
    }
}


// Push-style Observer (subject pushes payload)
public class SubjectPush<T>
{
    private readonly List<Action<T>> _handlers = new();
    public void Subscribe(Action<T> handler) => _handlers.Add(handler);
    public void Unsubscribe(Action<T> handler) => _handlers.Remove(handler);
    public void Notify(T payload)
    {
        foreach (var h in _handlers) h(payload);
    }
}

// Pull-style Observer (subject signals change; observers pull state)
public class SubjectPull
{
    private readonly List<Action> _observers = new();
    private int _count = 0;
    public void Subscribe(Action observer) => _observers.Add(observer);
    public void Unsubscribe(Action observer) => _observers.Remove(observer);
    public void Increment()
    {
        _count++;
        Notify();
    }
    public int GetState() => _count;
    private void Notify()
    {
        foreach (var o in _observers) o();
    }
}

// Event Bus / Aggregator (topic-based)
public class EventBus
{
    private readonly Dictionary<string, List<Action<object>>> _topics = new();
    public void On(string topic, Action<object> cb)
    {
        if (!_topics.ContainsKey(topic)) _topics[topic] = new List<Action<object>>();
        _topics[topic].Add(cb);
    }
    public void Off(string topic, Action<object> cb)
    {
        if (_topics.ContainsKey(topic)) _topics[topic].Remove(cb);
    }
    public void Emit(string topic, object payload = null)
    {
        if (_topics.ContainsKey(topic))
            foreach (var cb in _topics[topic]) cb(payload);
    }
}

