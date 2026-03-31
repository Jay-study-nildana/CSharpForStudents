# C# Memory: Heap, Stack, and Related Concepts

## Short answer
The heap is the runtime-managed area of memory where reference-type objects (classes, arrays, strings, boxed values, etc.) are allocated. The .NET runtime's Garbage Collector (GC) manages the heap: it allocates objects, finds unreachable objects, reclaims their memory, and (usually) compacts memory to reduce fragmentation and improve locality.

---

## Stack vs Heap (high level)
- Stack
  - Per-thread region.
  - Stores value-type local variables, return addresses, and small temporaries.
  - LIFO allocation/deallocation (fast, deterministic when method returns).
  - Typical storage for small value types and local method frames.
- Heap
  - Shared, global region for the process (managed by GC).
  - Stores reference-type objects and boxed value types.
  - Allocation is fast but managed by the runtime; lifetime is determined by reachability, not scope.

---

## How allocation works (overview)
- new MyClass(): runtime allocates space on the managed heap, sets up an object header (type pointer, sync block/index), zeroes the memory, and returns a reference.
- Value types (structs) typically live on the stack or inline inside other objects. When boxed or stored inside a reference-type container, a copy is allocated on the heap and a reference returned.
- The runtime uses optimized allocation paths (e.g., bump-pointer fast path) for short-lived allocations.

---

## Garbage Collector (GC) basics
- Generational GC: objects are grouped into generations to optimize the common case where most objects die young.
  - Gen0: newly allocated objects (collected frequently).
  - Gen1: survivors of Gen0 (for intermediate lifetimes).
  - Gen2: long-lived objects (collected rarely).
- Mark-and-sweep (or marking then compacting) is used to find live objects and reclaim unreachable ones. Many .NET GCs compact live objects to reduce fragmentation.
- Large Object Heap (LOH): used for large allocations (commonly > ~85 KB). LOH historically had special rules (less frequent GCs, limited compaction) though newer runtimes offer LOH compaction options — LOH fragmentation can still be a concern.

---

## Important object and runtime details
- Object header
  - Per-object metadata including a pointer to the type/MethodTable and a sync/frozen block index used for locking/hash.
  - Even an "empty" object has overhead because of these headers.
- Boxing and unboxing
  - Boxing: copying a value type into a heap-allocated object (allocation).
  - Unboxing: extracting the value back, usually copies the data back to a value-type variable.
  - Boxing causes allocations and GC pressure — avoid in hot paths.
- Finalizers vs IDisposable
  - Finalizer (~ClassName) runs non-deterministically on the finalizer thread before memory is reclaimed. Expensive and unpredictable.
  - IDisposable + Dispose (and `using` / `using` declaration) enable deterministic cleanup; prefer this for unmanaged resources.
  - If you implement a finalizer, call GC.SuppressFinalize(this) from Dispose to avoid double cleanup.
- Pinning
  - Pinning an object (fixed, GCHandle.Alloc(pin)) stops the GC from moving it, providing a stable address for interop/unsafe code.
  - Pinning can lead to fragmentation and reduced GC efficiency; keep pin regions short.
- Unsafe pointers
  - Allowed in `unsafe` contexts. When pointing into managed objects, pinning is required to avoid moved references by the GC.
- Span<T>, Memory<T>, stackalloc, ref struct
  - Span<T> (ref struct) allows safe, allocation-free access to contiguous memory (stack or heap segments) but cannot be captured by async/await or stored on the heap.
  - Memory<T>/ReadOnlyMemory<T> can be used across async boundaries and can wrap heap-owned buffers.
  - stackalloc allocates on the stack — extremely fast but limited lifetime and size.
- ArrayPool<T>
  - Enables reusing large arrays to reduce LOH allocations and GC pressure from repeated large buffers.

---

## Performance & practical tips
- Use structs for small immutable value types only. Avoid large, mutable structs because copying costs grow with size.
- Avoid boxing in hot code — use generics or `Span<T>`/`Memory<T>` where possible.
- Use Span<T>/stackalloc for short-lived transient buffers to avoid heap allocations.
- Use ArrayPool<T>.Shared to reuse large arrays and reduce LOH churn.
- Minimize long-lived pinning; prefer copying data for short-lived interop calls.
- Prefer IDisposable + using for unmanaged resources; implement a finalizer only as a safety net and suppress it in Dispose.
- Profile with a memory/GC profiler (PerfView, dotnet-gcdump + dotnet-dump, dotnet-trace, JetBrains dotMemory) before optimizing — measure to find real bottlenecks.

---

## When to worry (diagnostics)
- Frequent Gen0 collections: usually OK; expected for many short-lived allocations.
- Frequent Gen2 collections or steady memory growth: indicates many long-lived objects or a memory leak (static roots, event handlers, caches).
- LOH growth or fragmentation: look for large repeated allocations; consider ArrayPool<T> or reducing allocation sizes below LOH threshold.
- Excessive pinning: leads to heap fragmentation and longer GC pauses.

Tools to use
- dotnet-gcdump / dotnet-dump (collect memory dumps)
- dotnet-trace (trace allocations and events)
- PerfView (GC/alloc analysis, heap snapshots)
- JetBrains dotMemory, Visual Studio Diagnostic Tools

---

## Object layout (brief)
- [MethodTable pointer] — runtime type information pointer (vtable-like)
- [Sync block / object header] — used for locking, hash code, GC flags
- [Fields data] — instance fields (value-type fields are inlined; reference-type fields are pointers to other heap objects)
- Alignment/padding may add unused space after fields

---

## Examples

```csharp
// ExampleHeapVsStack.cs
using System;

class MyClass { public int X; }
struct MyStruct { public int X; }

class Program
{
    static void Main()
    {
        // Reference type -> allocated on the heap
        var a = new MyClass { X = 1 };
        var b = a;      // b references same object
        b.X = 2;
        Console.WriteLine(a.X); // prints 2

        // Value type -> copied
        MyStruct s1 = new MyStruct { X = 1 };
        MyStruct s2 = s1; // copy
        s2.X = 2;
        Console.WriteLine(s1.X); // prints 1

        // Boxing: value copied into heap object
        object boxed = s1;       // allocation on heap
        MyStruct s3 = (MyStruct)boxed; // unboxing copies back
    }
}
```

```csharp
// PinningFixedExample.cs
using System;
unsafe class PinExample
{
    static void Main()
    {
        byte[] buffer = new byte[1024];
        fixed (byte* p = buffer) // pins buffer for the duration of the fixed block
        {
            // p is a stable pointer to the array contents
        } // buffer becomes movable again here
    }
}
```

```csharp
// SpanStackallocExample.cs
using System;

class SpanExample
{
    static void FastMethod()
    {
        Span<byte> temp = stackalloc byte[256]; // no heap allocation
        // use temp for a short-lived buffer
    }
}
```

---

## Common mistakes & anti-patterns
- Large mutable structs that get copied implicitly (high CPU cost).
- Boxing value types in tight loops and hot paths.
- Long-lived pinning of arrays for interop.
- Relying on finalizers for deterministic cleanup (use IDisposable).
- Creating many large objects that live long and accumulate in Gen2/LOH.
- Holding static references unintentionally (caches, event handlers) — these become roots that prevent GC.

---

## Quick checklist for memory-related performance investigations
1. Measure: capture a GC/heap snapshot and allocation traces.
2. Identify: find hot allocation sites and large object churn.
3. Confirm: check for pinned objects and long-lived roots.
4. Fix: reduce allocations, use pooling (ArrayPool), replace boxing with generics or spans, limit pinning, implement IDisposable correctly.
5. Verify: re-sample after fixes.

---

## References & further reading
- .NET GC documentation (Microsoft)
- PerfView documentation and tutorials
- Articles on Span<T>, Memory<T>, stackalloc, and ArrayPool<T>