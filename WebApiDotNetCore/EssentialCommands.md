# Essential Commands

If you are using an IDE - Visual Studio Windows or Visual Studio Mac - the IDE takes care of most of the things.

However, you can also use Visual Studio for developing .NET projects. 

These are some common commands.

Also Check [GettingStartedNotes](GettingStartedNotes.md).
Also Check [EssentialCommands.md](EssentialCommands.md).
Also Check [References.md](References.md).

# Common Commands

Creating a new project.

    dotnet new webapi -o TodoApi

Run the Project

    dotnet run

Add Entity Framework, the InMemory version.

    dotnet add package Microsoft.EntityFrameworkCore.InMemory

install a local https certificate.

    dotnet dev-certs https --trust

code generation and entity framework.

    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer

tool that will generate scaffold controllers.
    
    dotnet tool install -g dotnet-aspnet-codegenerator

command to scaffold a new controller (with all the tools from above installed)

    dotnet-aspnet-codegenerator controller -name TodoItemsController -async -api -m TodoItem -dc TodoContext -outDir Controllers

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)