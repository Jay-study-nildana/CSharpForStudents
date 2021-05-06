# Dependency Injection Demo

Collection of some basic Dependency Injection Demo.

# The Problem and Solution

Dependency injection provides the following solutions:

1. The use of an interface or base class to abstract the dependency implementation.
1. Registration of the dependency in a service container. .NET provides a built-in service container, IServiceProvider. Services are typically registered at the app's start-up, and appended to an IServiceCollection. Once all services are added, you use BuildServiceProvider to create the service container.
1. Injection of the service into the constructor of the class where it's used. The framework takes on the responsibility of creating an instance of the dependency and disposing of it when it's no longer needed

# Projects and Solutions

1. [DependencyInjectionHelloWorld](DependencyInjectionHelloWorld)
1. [DependencyInjectionHelloWorld2](DependencyInjectionHelloWorld2) - This is where you should start if you are learning DI for the first time.

# Getting Started Notes

Check [GettingStartedNotes](GettingStartedNotes.md)

# Essential Commands

Check [EssentialCommands.md](EssentialCommands.md)

# References

Check [References.md](References.md).

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)