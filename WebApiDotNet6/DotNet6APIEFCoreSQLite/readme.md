# Dot NET 6 WEB API EF CORE SQLITE

.NET 6 is here. 

Might as well, have a demo api with EF CORE WITH SQLITE plus tips on deployment to azure web app.

# Other Notes

1. https://localhost:7004/swagger/index.html - Swagger Documentation
1. https://localhost:7004/ - Vanilla JavaScript Web App that consumes the web api
1. https://localhost:7004/index.html - will load the default welcome page. TODO - for some reason, as of now, visiting, the root, https://localhost:7004, does not load the index.html. dont know why.

# Notes Before Deploying

1. EF commands - the app is designed to automatically create and seed the database with some sample data. If not, use the following commands.
    ```
        Add-Migration InitialCreate
        Update-Database
    ```
1. Static Filies - this is separate from the swagger UI, which is generated because of Swagger being injected elsewhere. This is about the landing page. Please understand that, the order is important here. UseStaticFiles must come after UseHttpsRedirection

    ```
        app.UseHttpsRedirection();

        app.UseStaticFiles(); //this is what enables the index.html and the landing page.

        app.UseAuthorization();
    ```
1. The following code is what triggers the a) database creation b) seeding it with sample data. Check this function for details about what actually happens inside.

    ```
        tempOtherStuff.CreateDbIfNotExists(app);
    ```
1. My way of manual, non CICD deployment is something like this. 
1. Step One : Create a web app directly on the azure portal. If possible, create a free web app. 
1. Step Two : Then, open the project in VS Code. Now, one point of note. I created this project in visual studio. So, if you also used Visual Studio, make sure you open the project folder, and not the solution folder. Then, perform deployment using the Azure extension in VS Code. Far more easier than deploying in visual studio.      

# Packages installed

Install these packages before you begin.

1. Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
1. Microsoft.Data.Sqlite.Core
1. Microsoft.EntityFrameworkCore
1. Microsoft.EntityFrameworkCore.SqlServer
1. Swashbuckle.AspNetCore
1. Microsoft.EntityFrameworkCore.Sqlite
1. Microsoft.EntityFrameworkCore.Tools

# References

I used a combination of the following links to get this project working.

1. https://docs.microsoft.com/en-us/ef/core/
1. https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-6.0&tabs=visual-studio
1. https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-6.0
1. https://stackoverflow.com/questions/69722872/asp-net-core-6-how-to-access-configuration-during-setup
1. https://entityframeworkcore.com/providers-sqlite
1. https://github.com/dotnet/efcore#microsoftdatasqlite
1. https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs
1. https://sqlitebrowser.org/dl/
1. https://codepen.io/jay-pancodu/pen/WNOMNrx 

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)