# Interlocked and static fields — guide for students

This guide explains static field initialization and thread-safety in .NET, how race conditions can occur, and how to use `System.Threading.Interlocked` (and related tools) to safely update static fields without locks when appropriate.

---

## TL;DR / Recommendation

- Static constructors (.cctor) guarantee one-time, thread-safe initialization for a type — prefer them for simple static initialization.
- For shared mutable static state accessed concurrently, use synchronization. `lock` is easiest for complex invariants.
- Use `Interlocked` for small atomic updates or lock-free publish patterns (increment counters, swap references, or compare-and-swap based initialization).
- Prefer `Lazy<T>` or static constructor initialization for lazy singletons rather than hand-rolled double-checked locking.
- Understand memory ordering: `Interlocked` methods provide strong memory-fence semantics; `Volatile.Read/Write` provide weaker acquire/release semantics.

---

## Static fields & initialization basics

- A static field belongs to the type, not an instance. Multiple threads can read and write it concurrently.
- A static constructor (type initializer) executes at most once per AppDomain/type and is automatically thread-safe — the runtime serializes it. Example:

```csharp
class C {
    public static readonly List<int> Shared;
    static C() {
        Shared = new List<int> { 1, 2, 3 };
    }
}
```

- If you don't provide a static constructor, the runtime may mark the type with `beforefieldinit`, meaning the type may be initialized at any time before first access. This usually doesn't cause problems, but for deterministic lazy initialization prefer an explicit static constructor or `Lazy<T>`.

---

## Common concurrency problems with static fields

- Race conditions: two threads write to a static field simultaneously and the final state is unexpected.
- Torn reads/writes: on some platforms, a 64-bit write to a `long` or `double` may not be atomic without `volatile` or `Interlocked`.
- Visibility: one thread writes a fully-constructed object to a static field but another thread sees a partially-constructed view because of instruction reordering or lack of memory barriers.

Example problematic pattern:

```csharp
static MyType instance; // not volatile

void Init() {
    if (instance == null) {
        instance = new MyType(); // another thread may see a stale or partial value
    }
}
```

---

## Interlocked — what it provides

`System.Threading.Interlocked` supplies atomic operations and full memory fences. Key methods:

- `Interlocked.Increment(ref int location)` / `Interlocked.Decrement`
- `Interlocked.Add(ref int location, int value)`
- `Interlocked.Exchange(ref T location, T value)` — atomic replace (works for reference types and primitive value types)
- `Interlocked.CompareExchange(ref T location, T value, T comparand)` — CAS: set `location` to `value` only if current value equals `comparand`, returns the original value

Interlocked operations are fast and do not require locks. They are ideal for counters, atomic swaps, and lock-free initialization routines.

---

## Examples

Atomic counter:

```csharp
static int _counter = 0;

void Increment() {
    Interlocked.Increment(ref _counter);
}
```

Atomic reference swap:

```csharp
static object _current;

void Replace(object newValue) {
    Interlocked.Exchange(ref _current, newValue);
}
```

Lock-free lazy initialization using CompareExchange:

```csharp
static MyService _service;

static MyService GetService() {
    var tmp = _service;
    if (tmp != null) return tmp;

    var created = new MyService(); // create outside CAS to avoid repeated work
    var original = Interlocked.CompareExchange(ref _service, created, null);
    return original ?? created; // if original was null we installed created; otherwise discard created
}
```

Notes:
- The CAS loop avoids locks, but if `MyService` construction is expensive and multiple threads may create instances concurrently, you may waste work. Consider `Lazy<T>` if you want to ensure only one construction.
- For reference types `CompareExchange<T>` is generic and works well. For `long` use `Interlocked.CompareExchange(ref long, long, long)` to get atomicity on all platforms.

---

## Memory ordering and visibility

- `Interlocked` methods include full fences: they ensure that reads/writes before the operation are not reordered after it and vice versa. This prevents visibility issues for initialization/publish patterns.
- `Volatile.Read` and `Volatile.Write` provide acquire/release semantics and are lighter-weight if you only need ordering without a full fence.
- `volatile` field modifier also provides volatile reads/writes and prevents certain optimizations that reorder accesses, but it doesn't provide atomic compound operations.

Example safe publish:

```csharp
// During construction:
var obj = new BigObject();
obj.Initialize();
Interlocked.Exchange(ref _shared, obj); // ensure other threads see fully initialized object
```

---

## When to use locks vs Interlocked

- Use `lock` (monitor) when you need to protect complex invariants, multiple fields, or sequences of operations that must be atomic together.
- Use `Interlocked` for single-word atomic updates: counters, swapping a pointer, or CAS-based flags.
- `lock` yields simpler, easier-to-reason code for many cases; Interlocked-based code can be more performant but harder to maintain.

---

## Practical recommendations

- Prefer static constructor or `Lazy<T>` for lazy initialization of singletons:
  - `static readonly MyType Instance = new MyType();` (eager)
  - `static readonly Lazy<MyType> Instance = new Lazy<MyType>(() => new MyType());` (lazy, thread-safe)
- Use `Interlocked.Increment` for simple counters (avoid `++` on shared ints).
- Use `Interlocked.CompareExchange` to publish references or implement lock-free singletons only after weighing the complexity.
- Avoid hand-rolled double-checked locking unless you fully understand `volatile`/memory ordering. Instead use `Lazy<T>` or static ctor.
- Test on target platforms — atomicity guarantees differ historically between 32-bit and 64-bit architectures; .NET guarantees atomicity for reads/writes of `int` and references, but prefer `Interlocked` for explicit atomic operations and memory barriers.

---

## Pitfalls & gotchas

- ABA problem: if you rely on CAS for complex coordination, be aware that a value can change A→B→A and CAS may succeed though intermediate changes occurred. Solutions include version tags or stronger synchronization.
- Construction work duplication: CAS-based lazy init can lead to multiple constructions in racey scenarios; prefer `Lazy<T>` when construction must happen exactly once.
- Overuse of Interlocked for complex logic can lead to unreadable, bug-prone code. Use locks for multi-field invariants.

---

## Summary

`Interlocked` is a powerful, low-level set of atomic primitives that, together with proper static initialization (static constructors or `Lazy<T>`), helps you build thread-safe static fields and counters without locks when appropriate. Use the simplest safe approach for your scenario: static constructors/Lazy for initialization; `lock` for complex consistency; `Interlocked` for small, hot, atomic operations.
