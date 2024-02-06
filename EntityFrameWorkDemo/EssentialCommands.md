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

Install Entity Framework Core - Sqlite

    dotnet add package Microsoft.EntityFrameworkCore.Sqlite    

Install Entity Framework Core - SqlServer

    dotnet add package Microsoft.EntityFrameworkCore.SqlServer

Installs dotnet ef and the design package which is required to run the command

    dotnet tool install --global dotnet-ef
    dotnet add package Microsoft.EntityFrameworkCore.Design

Migrations of EF

    dotnet ef migrations add InitialCreate
    dotnet ef database update

# hire and get to know me

find ways to hire me, follow me and stay in touch with me.

https://jay-study-nildana.github.io/developerprofile/