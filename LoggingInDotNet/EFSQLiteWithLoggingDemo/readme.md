# EFSQLiteDemo

A quick and simple EF Core demo with SQLite.

This one focusses on Logging.

# Important Note

1. Recent changes on how EF Core works. 
1. You need to install 'Install-Package Microsoft.EntityFrameworkCore.Tools'. Previously, this was not there or was not required. 
1. Set Working Directory manually. Look here for more details - https://entityframeworkcore.com/knowledge-base/33455041/asp-net-5--ef-7-and-sqlite---sqlite-error-1---no-such-table--blog-

    ```

    In Solution Explorer, right click the project and then select Properties.
    Select the Debug tab in the left pane.
    Set Working directory to the project directory.
    Save the changes.

    ```
1. If you still get error, try 
    ```

    Add-Migration InitialCreate
    Update-Database

    ```

# References

1. https://docs.microsoft.com/en-in/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0
1. https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
1. https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging?view=dotnet-plat-ext-5.0
1. https://www.nuget.org/packages/Microsoft.Extensions.Logging.Console/
1. https://stackoverflow.com/questions/58259520/iloggerfactory-does-not-contain-a-definition-for-addconsole
1. https://stackoverflow.com/questions/67396315/how-or-where-do-i-see-logtrace-messages-in-dot-net-core

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)