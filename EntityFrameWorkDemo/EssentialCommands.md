# Essential Commands

If you are using an IDE - Visual Studio Windows or Visual Studio Mac - the IDE takes care of most of the things.

However, you can also use Visual Studio for developing .NET projects. 

These are some common commands.

Also Check [GettingStartedNotes](GettingStartedNotes.md).
Also Check [EssentialCommands.md](EssentialCommands.md).
Also Check [References.md](References.md).

# Common Commands

Creating a new project.

    dotnet new console -o EFGetStarted

Run your app

    dotnet run    

Install Entity Framework Core

    dotnet add package Microsoft.EntityFrameworkCore.Sqlite

Installs dotnet ef and the design package which is required to run the command

    dotnet tool install --global dotnet-ef
    dotnet add package Microsoft.EntityFrameworkCore.Design

Migrations of EF

    dotnet ef migrations add InitialCreate
    dotnet ef database update

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)