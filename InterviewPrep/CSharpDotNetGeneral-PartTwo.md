# C# / .NET — Assemblies & GAC, Regex, Reflection & Attributes, Multithreading, TPL & Async, Serialization  
Interview & Developer Reference with Code Samples

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Assemblies & GAC](#assemblies--gac)  
   - Assemblies overview  
   - Public vs Private assemblies  
   - Class library example  
   - Shared assemblies & Global Assembly Cache (GAC) — notes for .NET Framework vs .NET (Core/.NET 5+)  
3. [Regular Expressions (Regex)](#regular-expressions-regex)  
   - Regex class and common methods  
   - Pattern examples and options  
   - Named groups, captures, replace and performance tips  
4. [Reflection & Attributes](#reflection--attributes)  
   - Reflection basics (Assembly/Type/Member discovery)  
   - Pre-defined attributes (Obsolete, Serializable, CLSCompliant, etc.)  
   - Custom attributes example  
   - Invoking members (MethodInfo.Invoke, Type.InvokeMember, BindingFlags)  
5. [Multithreading (System.Threading)](#multithreading-systemthreading)  
   - Thread basics, priority, background vs foreground, Interrupt  
   - Why Suspend/Resume are obsolete and safer alternatives  
   - Cross-thread UI access and synchronization context notes  
   - ThreadPool usage  
   - Synchronization primitives: Monitor, lock, Mutex, Semaphore(Slim), AutoResetEvent / ManualResetEventSlim  
6. [Task Parallel Library (TPL) & async/await](#task-parallel-library-tpl--asyncawait)  
   - Creating and working with Tasks, Task vs Thread  
   - async/await patterns and ConfigureAwait  
   - Exceptions in TPL (AggregateException) and handling patterns  
   - Cancellation with CancellationToken and cooperative cancellation examples  
7. [Serialization & Deserialization](#serialization--deserialization)  
   - XML serialization (XmlSerializer, DataContractSerializer)  
   - Binary serialization (warning about BinaryFormatter; alternatives)  
   - JSON serialization (System.Text.Json and Newtonsoft.Json examples)  
   - Controlling serialization with attributes and custom converters  
8. [Best Practices & Pitfalls](#best-practices--pitfalls)  
9. [Short Q&A — Common Interview Questions & Answers](#short-qa---common-interview-questions--answers)  
10. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document is a focused reference for core C#/.NET topics: assemblies and the GAC, regular expressions, reflection and attributes, multithreading primitives, Task Parallel Library and async/await, and serialization patterns. It uses .NET idioms and includes practical code examples. Where APIs are deprecated or differ between .NET Framework and .NET (Core/.NET 5+), the guidance calls that out.

---

## 2. Assemblies & GAC

Assemblies
- An assembly is the building block of .NET deployment: a compiled unit (.dll or .exe) containing metadata, IL, and resources.
- Assemblies have identity: AssemblyName (name, version, culture, public key token).
- Types live in assemblies; an assembly can contain multiple namespaces.

Public vs Private Assemblies
- Private assemblies: placed in the application folder and loaded by the app — simple deployment (xcopy).
- Shared / Public assemblies: installed to a machine-wide location so multiple apps can share a single version — historically the GAC.

Class library example
- Create a class library that exposes a public type.

```csharp
// MyLib.cs (Class Library project)
namespace MyCompany.MyLib
{
    public class Greeter
    {
        public string Greet(string name) => $"Hello, {name}";
    }
}
```

Consuming the class library:
```csharp
// Program.cs (Console app)
using MyCompany.MyLib;

class Program
{
    static void Main()
    {
        var g = new Greeter();
        Console.WriteLine(g.Greet("Alice"));
    }
}
```

Shared assemblies & GAC (Global Assembly Cache)
- .NET Framework: GAC stores strongly-named assemblies for sharing. Use `gacutil` or installers to install. Assemblies must be signed with a strong name (public/private key).
- .NET (Core/.NET 5+): no GAC concept. .NET Core uses NuGet packages and shared framework components. Don't rely on GAC for modern .NET.
- Strong-name example (generation of key and signing) — use for CLR binding and versioning.

Strong-name signing (example in .NET Framework):
```bash
sn -k MyKey.snk
```
In project (.csproj):
```xml
<PropertyGroup>
  <SignAssembly>true</SignAssembly>
  <AssemblyOriginatorKeyFile>MyKey.snk</AssemblyOriginatorKeyFile>
</PropertyGroup>
```

Reflection to inspect an assembly:
```csharp
using System;
using System.Reflection;

var asm = Assembly.LoadFrom("MyLib.dll");
Console.WriteLine(asm.FullName);
foreach (var t in asm.GetTypes())
    Console.WriteLine(t.FullName);
```

Notes:
- For shared dependencies across apps, prefer NuGet packages and proper versioning over GAC for modern .NET.
- Use binding redirects in legacy .NET Framework apps to resolve assembly version conflicts.

---

## 3. Regular Expressions (Regex)

Namespace: System.Text.RegularExpressions

Key classes:
- Regex
- Match, MatchCollection, Group, GroupCollection, Capture

Basic usage (Match/IsMatch/Matches/Replace/Split):
```csharp
using System.Text.RegularExpressions;

var pattern = @"\b(?<word>[A-Za-z]+)\b";
var input = "Hello world 2025!";
var m = Regex.Match(input, pattern);
if (m.Success)
    Console.WriteLine($"First word: {m.Groups["word"].Value}");

// Find all words
foreach (Match match in Regex.Matches(input, pattern))
    Console.WriteLine(match.Groups["word"].Value);

// Replace digits
var clean = Regex.Replace(input, @"\d+", "[num]");
Console.WriteLine(clean);
```

Regex with compiled option (useful when pattern runs many times):
```csharp
var r = new Regex(@"\d{3}-\d{2}-\d{4}", RegexOptions.Compiled);
var match = r.Match("SSN 123-45-6789");
```

Named groups and captures:
```csharp
var datePattern = @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})";
var dateInput = "2025-12-02";
var match = Regex.Match(dateInput, datePattern);
if (match.Success) {
    Console.WriteLine(match.Groups["year"].Value);
}
```

Performance tips:
- Prefer compiled Regex for hot paths, but compiling increases startup cost.
- For simple checks prefer `String` methods (IndexOf, StartsWith) when possible — they are faster and simpler.
- Avoid catastrophic backtracking: prefer atomic groups, possessive quantifiers (not in .NET), or rewrite regex patterns.

Common use cases:
- Validation (email — but beware of perfect coverage), tokenization, extraction, bulk text transformations.

---

## 4. Reflection & Attributes

Reflection Basics
- Reflection allows inspecting types, members, attributes, and creating/invoking members at runtime.
- Useful for plugin systems, serialization, ORMs, DI containers, and runtime type discovery.

Simple reflection example:
```csharp
using System;
using System.Reflection;

var asm = Assembly.GetExecutingAssembly();
var type = asm.GetType("MyCompany.MyLib.Greeter");
var ctor = type.GetConstructor(Type.EmptyTypes);
var greeter = ctor.Invoke(null);
var method = type.GetMethod("Greet");
var result = method.Invoke(greeter, new object[] { "Bob" });
Console.WriteLine(result); // Hello, Bob
```

Pre-defined attributes (examples)
- [Obsolete]: mark members as obsolete and optionally provide compile-time errors.
- [Serializable]: mark a type as binary-serializable (binary formatter legacy).
- [CLSCompliant(true)]: indicate CLS compliance.
- [DebuggerDisplay], [DebuggerStepThrough], [Conditional], [AssemblyVersion], etc.

Custom attributes
- Define and read custom attributes:

```csharp
using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class DocumentationAttribute : Attribute
{
    public string Description { get; }
    public DocumentationAttribute(string description) => Description = description;
}

// Usage
[Documentation("This class greets people.")]
public class Greeter { public string Greet(string n) => "Hi " + n; }

// Reading attributes via reflection
var t = typeof(Greeter);
var attr = t.GetCustomAttribute<DocumentationAttribute>();
Console.WriteLine(attr?.Description);
```

Invoking members with binding:
- You can use MethodInfo.Invoke or Type.InvokeMember.

MethodInfo.Invoke example:
```csharp
var type = typeof(Greeter);
var mi = type.GetMethod("Greet");
var instance = Activator.CreateInstance(type);
var output = mi.Invoke(instance, new object[] { "Sam" });
Console.WriteLine(output);
```

Type.InvokeMember example (older, uses BindingFlags):
```csharp
var t = Type.GetType("MyCompany.MyLib.Greeter");
var obj = Activator.CreateInstance(t);
var result = t.InvokeMember("Greet",
    BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
    null, obj, new object[] { "Ann" });
```

Reflection caveats:
- Reflection is slower than direct calls; cache MethodInfo/PropertyInfo where used repeatedly.
- Reflection can bypass accessibility (if using BindingFlags.NonPublic) but that's brittle and may break security constraints.
- Use expression trees or compiled delegates for better performance (e.g., create Func<T,object> wrappers).

---

## 5. Multithreading (System.Threading)

Thread basics:
- System.Threading.Thread represents an OS-managed thread.
- ThreadStart / ParameterizedThreadStart are used to start a new thread.
- Thread priority influences scheduling (ThreadPriority enum), but should be used sparingly.

Example: starting a thread
```csharp
using System;
using System.Threading;

var t = new Thread(() => {
    Console.WriteLine($"Thread running: {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);
});
t.IsBackground = false; // foreground by default
t.Start();
t.Join(); // wait for completion
```

Background vs Foreground threads:
- Foreground threads keep the process alive; background threads do not (process exits when only background threads remain).

Thread interruption:
```csharp
var t = new Thread(() => {
    try {
        for (int i = 0; i < 10; i++) {
            Thread.Sleep(1000);
            Console.WriteLine(i);
        }
    } catch (ThreadInterruptedException) {
        Console.WriteLine("Interrupted");
    }
});
t.Start();
Thread.Sleep(1500);
t.Interrupt(); // interrupts Sleep/Wait/Join causing ThreadInterruptedException
```

Suspend / Resume
- Thread.Suspend/Resume are obsolete and unsafe (deadlock-prone). Don't use them. Use cooperative signals (ManualResetEvent, CancellationToken) to pause/resume safely.

Cross-thread UI access
- UI frameworks (WinForms/WPF) require marshaling to UI thread (Control.Invoke/Dispatcher.Invoke). Use synchronization context or proper dispatchers rather than direct cross-thread calls.

ThreadPool
- ThreadPool threads are managed and reused; use ThreadPool.QueueUserWorkItem or Task.Run for short-lived work.
```csharp
ThreadPool.QueueUserWorkItem(_ => {
    Console.WriteLine($"Pool thread {Thread.CurrentThread.ManagedThreadId}");
});
```

Synchronization primitives

lock / Monitor
- `lock(obj)` is syntactic sugar for Monitor.Enter/Monitor.Exit.
```csharp
private readonly object _sync = new object();

void Increment()
{
    lock(_sync) // Monitor.Enter + try/finally Monitor.Exit
    {
        // critical section
    }
}
```

Monitor.Wait / Pulse
- Used for producer/consumer coordination.

```csharp
lock(_sync)
{
    while (!condition) Monitor.Wait(_sync);
    // proceed
}
```

Mutex (cross-process)
```csharp
using (var mutex = new Mutex(false, "Global\\MyApp_Mutex"))
{
    if (mutex.WaitOne(TimeSpan.FromSeconds(5)))
    {
        try { /* protected region */ }
        finally { mutex.ReleaseMutex(); }
    }
}
```

Semaphore / SemaphoreSlim
- SemaphoreSlim for intra-process async-friendly waits; Semaphore for cross-process.

```csharp
var sem = new SemaphoreSlim(3); // allow 3 concurrent
await sem.WaitAsync();
try { /* protected */ }
finally { sem.Release(); }
```

AutoResetEvent / ManualResetEventSlim
```csharp
var evt = new AutoResetEvent(false);

var t = new Thread(() => {
    Console.WriteLine("Worker waiting");
    evt.WaitOne();
    Console.WriteLine("Worker resumed");
});
t.Start();

Thread.Sleep(500);
evt.Set(); // releases one waiter
```

Best practices
- Prefer higher-level abstractions (Tasks, async/await, Parallel.For/ForEach, dataflow) over raw threads.
- Avoid locking on `this` or publicly accessible objects; use private readonly lock objects.
- Keep critical sections small to avoid deadlocks and contention.

---

## 6. Task Parallel Library (TPL) & async/await

Task vs Thread
- Task represents an asynchronous operation that may use the thread-pool; threads are low-level OS threads.
- Use Task for I/O-bound and CPU-bound work (for CPU-bound consider Task.Run or Parallel APIs).

Creating and working with Tasks
```csharp
// fire-and-forget (not always recommended)
Task.Run(() => DoWork());

// return a value
var t = Task.Run(() => {
    Thread.Sleep(500);
    return 42;
});
int result = await t; // in async method

// Task.Factory.StartNew has options for long-running tasks, but prefer Task.Run
```

Async/await
```csharp
public async Task<int> GetDataAsync()
{
    using var http = new HttpClient();
    var s = await http.GetStringAsync("https://example.com");
    return s.Length;
}

// caller
int len = await GetDataAsync();
```

ConfigureAwait
- In libraries, use `ConfigureAwait(false)` to avoid capturing synchronization context:
```csharp
await Task.Delay(1000).ConfigureAwait(false);
```

Exception handling in TPL
- When awaiting a Task, exceptions are propagated as the original exception.
- When using Task.Wait or Task.Result, exceptions are wrapped in AggregateException.
```csharp
try {
    await TaskThatThrows();
} catch (Exception ex) {
    // handle
}

// non-await usage
var t = Task.Run(() => throw new InvalidOperationException("bad"));
try {
    t.Wait();
} catch (AggregateException ae) {
    foreach(var e in ae.InnerExceptions) Console.WriteLine(e);
}
```

Handling multiple tasks
```csharp
var tasks = new[] { Task1(), Task2(), Task3() };
var all = await Task.WhenAll(tasks); // throws on first failure, aggregated later
// or
var results = await Task.WhenAll(tasks.Select(t => t.ContinueWith(tt => tt.Status == TaskStatus.RanToCompletion ? tt.Result : default)));
```

Cancellation (CancellationToken)
```csharp
public async Task DoWorkAsync(CancellationToken ct)
{
    for (int i=0;i<100;i++)
    {
        ct.ThrowIfCancellationRequested();
        await Task.Delay(100, ct);
    }
}

// usage
var cts = new CancellationTokenSource();
var task = DoWorkAsync(cts.Token);
cts.CancelAfter(TimeSpan.FromSeconds(2));
try { await task; }
catch (OperationCanceledException) { Console.WriteLine("Canceled"); }
```

Long-running tasks
- Use TaskCreationOptions.LongRunning to hint a dedicated thread:
```csharp
Task.Factory.StartNew(() => DoCpuWork(), TaskCreationOptions.LongRunning);
```

When to use TPL over threads
- Prefer Task APIs: easier exception handling, composition, cancellation, and integration with async/await.

---

## 7. Serialization & Deserialization

Important note: BinaryFormatter is obsolete and insecure. Avoid using it. Use System.Text.Json, DataContractSerializer, XmlSerializer, protobuf-net, MessagePack, or other vetted serializers.

XML Serialization (XmlSerializer)
```csharp
using System.Xml.Serialization;
using System.IO;

[XmlRoot("person")]
public class Person
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlIgnore] // exclude
    public int TempValue { get; set; }
}

var p = new Person { Name = "Alice", TempValue = 99 };
var ser = new XmlSerializer(typeof(Person));
using var sw = new StringWriter();
ser.Serialize(sw, p);
var xml = sw.ToString();

// Deserialize
using var sr = new StringReader(xml);
var p2 = (Person)ser.Deserialize(sr);
```

DataContractSerializer (good for WCF-like scenarios)
```csharp
using System.Runtime.Serialization;

[DataContract]
public class Product
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [IgnoreDataMember]
    public string InternalNote { get; set; }
}
```

JSON Serialization (System.Text.Json — recommended for modern .NET)
```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string Name { get; set; }

    [JsonIgnore]
    public string Internal { get; set; }
}

var user = new User { Id = 1, Name = "Bob" };
var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
var back = JsonSerializer.Deserialize<User>(json);
```

Newtonsoft.Json (Json.NET) example — still widely used for features not in System.Text.Json:
```csharp
using Newtonsoft.Json;

var json = JsonConvert.SerializeObject(user);
var obj = JsonConvert.DeserializeObject<User>(json);
```

Binary serialization alternative (do not use BinaryFormatter)
- Prefer System.Runtime.Serialization.Formatters.Binary is obsolete (security).
- Alternatives: protobuf-net, MessagePack-CSharp, custom binary via BinaryWriter/BinaryReader.

Example using protobuf-net (conceptual):
```csharp
[ProtoContract]
public class Item { [ProtoMember(1)] public int Id {get;set;} [ProtoMember(2)] public string Name {get;set;} }
```

Custom serialization control
- Implement ISerializable (legacy) for fine control.
- Use attributes: [JsonIgnore], [XmlIgnore], [NonSerialized] (for fields).
- System.Text.Json supports custom converters (JsonConverter<T>).

Example: custom converter for DateOnly
```csharp
public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateOnly.Parse(reader.GetString());

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}
```

Versioning tips
- Include defaults for new properties and be tolerant of missing fields to support backward/forward compatibility.
- Avoid depending on property order in JSON/XML.

---

## 8. Best Practices & Pitfalls

- Prefer high-level concurrency primitives: Tasks, async/await, SemaphoreSlim, concurrent collections (ConcurrentQueue/Dictionary).
- Avoid thread suspension APIs; use cooperative cancellation and signaling.
- Avoid BinaryFormatter; use safe, explicit serializers.
- Minimize reflection hot-path usage; consider compiled expressions or source generators.
- In multi-threaded code, prefer immutable data where possible.
- Keep critical sections small and avoid nested locks to prevent deadlock.
- For long-running data or CPU-bound work, use dedicated thread or TaskCreationOptions.LongRunning.
- When serializing secrets, ensure encryption or secure storage; do not serialize sensitive data in plaintext.
- Use ConfigureAwait(false) in library code to avoid deadlock and context capturing issues.

---

## 9. Short Q&A — Common Interview Questions & Answers

Q: What is the difference between Task and Thread?  
A: Thread is an OS-level thread. Task is a logical representation of work — it may run on the thread pool or use a dedicated thread, and integrates with the TPL for composition, cancellation, and exception handling.

Q: Why avoid BinaryFormatter?  
A: BinaryFormatter is insecure and can lead to remote code execution when deserializing untrusted data. Microsoft recommends alternatives: System.Text.Json, DataContractSerializer, protobuf, MessagePack, or custom serializers.

Q: How do you stop a thread safely?  
A: Use cooperative cancellation: set a CancellationToken or a volatile flag that the worker checks and exits gracefully. Avoid Thread.Abort, Suspend/Resume.

Q: What is a deadlock and how to avoid it?  
A: A deadlock occurs when two or more threads wait on resources held by each other. Avoid by ordering lock acquisition consistently, minimizing lock scope, and preferring lock timeouts or lock-free/immutable patterns.

Q: When to use Monitor vs Semaphore vs Mutex?  
A: Monitor/lock: intra-process mutual exclusion. SemaphoreSlim: limit concurrent access within process (async-friendly). Mutex: cross-process mutual exclusion.

---

## 10. References & Further Reading

- .NET docs — Assemblies: https://learn.microsoft.com/dotnet/standard/assembly/  
- System.Text.RegularExpressions: https://learn.microsoft.com/dotnet/standard/base-types/regular-expressions  
- Reflection (System.Reflection): https://learn.microsoft.com/dotnet/standard/reflection  
- Threading (System.Threading): https://learn.microsoft.com/dotnet/standard/threading/  
- Task Parallel Library / async: https://learn.microsoft.com/dotnet/standard/parallel-programming/  
- Serialization guidance: https://learn.microsoft.com/dotnet/standard/serialization/ and https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-security-guide (BinaryFormatter obsoletion)  
- Concurrency in C#: Stephen Toub and Microsoft docs; "Concurrency in C# Cookbook" (O'Reilly)

---

Prepared as a developer-friendly reference for C#.NET topics covering assemblies, regex, reflection & attributes, threading/TPL, and serialization with working examples. Adapt examples to your framework version (.NET Framework vs .NET Core/.NET 5+) where noted.  