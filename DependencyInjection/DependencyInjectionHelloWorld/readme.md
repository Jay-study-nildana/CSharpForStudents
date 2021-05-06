# Dependency Injection Hello World

A quick and simple hello world dependency injection.

# Concepts Included.

1. shows how to use dependency injection (DI) in .NET. With Microsoft Extensions, DI is a first-class citizen where services are added and configured in an IServiceCollection. 
1. The IHost interface exposes the IServiceProvider instance, which acts as a container of all the registered services.
1. Creates an IHostBuilder instance with the default binder settings.
1. Configures services and adds them with their corresponding service lifetime.
1. Calls Build() and assigns an instance of IHost.
1. Calls ExemplifyScoping, passing in the IHost.Services.

# Output Analysis

1. Transient operations are always different, a new instance is created with every retrieval of the service.
1. Scoped operations change only with a new scope, but are the same instance within a scope.
1. Singleton operations are always the same, a new instance is only created once.

# References

1. https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)