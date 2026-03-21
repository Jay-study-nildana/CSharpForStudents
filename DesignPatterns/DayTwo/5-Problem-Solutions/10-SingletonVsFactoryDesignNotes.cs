// 10-SingletonVsFactoryDesignNotes.cs
// Problem: Design notes summarizing when to prefer DI-managed Singleton vs Factory Method vs Factory Delegate.
// This file contains a concise checklist and guidance.

using System;

public static class DesignNotes
{
    /*
    Decision checklist (use when choosing between Singleton / Factory Method / Factory Delegate):

    1) Is there truly a single instance required for the whole app?
       - Yes → consider DI-registered Singleton, but ensure the service is stateless or thread-safe.
       - No  → avoid singleton; prefer scoped/transient/ factory.

    2) Does the object hold mutable per-request or per-user state?
       - Yes → avoid singleton; use Scoped or Transient lifetimes.

    3) Do you need runtime selection of concrete implementations (based on config or user choice)?
       - Yes → consider Factory Delegate (Func<...>) for simple selection or Factory Method / Configurable Creator if creation logic belongs to a class hierarchy.

    4) Does the creation logic need subclass control or extension via inheritance?
       - Yes → Factory Method pattern is appropriate.

    5) Is testability a priority (need to replace concrete products in unit tests)?
       - Prefer DI-managed services and factory delegates; Factory Method can be mocked via test creators.

    6) Are disposable resources involved that require per-use cleanup?
       - Use factories that return disposable instances and ensure consumers dispose (using / await using). Avoid caching such resources in singletons.

    7) Do you want the DI container to manage dependencies and lifetimes?
       - Prefer delegating construction to the DI container; register factories or use factory delegates to obtain needed instances.

    Summary Guidance:
    - Prefer DI + explicit lifetimes for production code: it centralizes lifecycle, improves testability, and allows structured composition.
    - Use Factory Method when creation is a responsibility of a family of classes, or when you want to defer product instantiation to subclasses.
    - Use factory delegates for simple runtime selection and when you want to leverage the DI container's resolution capabilities.
    - Use singletons sparingly; when used, register them with the DI container and make them stateless or thread-safe.
    */
}