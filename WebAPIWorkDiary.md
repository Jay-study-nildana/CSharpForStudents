# Web API Work Diary

lets put our API stuff here

# April 22nd 2021

Picking up from EntityFrameWorkDiary.md

Let's add a web api project with SQLite

WebApiDotNetCore5point1SQLite

Look at 

https://github.com/Jay-study-nildana/RandomStuffGenerator/blob/master/RandomStuffGeneratorPrivate/Startup.cs

    //database context
    services.AddDbContext<QuoteCMSContext>(options =>
                options.UseSqlite(Configuration["SqliteConnectionString"]));

https://github.com/Jay-study-nildana/RandomStuffGenerator/blob/master/RandomStuffGeneratorPrivate/appsettings.json    

    "SqliteConnectionString": "Data Source=blogging.db"     

https://github.com/Jay-study-nildana/RandomStuffGenerator/blob/master/RandomStuffGeneratorPrivate/DatabaseClasses/QuoteCMSContext.cs

    public QuoteCMSContext(DbContextOptions<QuoteCMSContext> options)
        : base(options)
    {

    }                       

Let's put that in our project.    

Okay, its ready. 

Generated a controller. Swagger was able to add things just fine. 

And checked SQLite DB directly. Looks good.

Okay, let's do the same for SQL Server.

SQL database

sqlserverdemo1

Getting credentials from AzureSQLStudio forlder, loginstuff.md

Server=tcp:practiceofqueries1.database.windows.net,1433;Initial Catalog=AdventureWorksDW2014;Persist Security Info=False;User ID=namidodb;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

practiceofqueries1

practiceofqueries1.database.windows.net

admin

namidodb

password

Nadipusswrd$320

Okay, the SQLserver demo is also working.

WebApiDotNetCore5point1SQLserver



# April 19th 2021

https://docs.microsoft.com/en-in/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio

https://docs.microsoft.com/en-in/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio

https://dotnet.microsoft.com/download/dotnet

https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new

Contains information about SDK switching from .NET Core 3.X to .NET 5.X

https://stackoverflow.com/questions/42077229/switch-between-dotnet-core-sdk-versions

Install the C# extension on visual studio code. This is required for debugging. 

On loading project, 'Debug Assets' will get added. Let it happen. 

The, just hit Run > Start Debugging and its good to go.

Remember to ensure that the global SDK version matches the project currently being debugged. Otherwise, Omnisharp will not run.

Later, Resuming.

I just realized that the web api word was missing in all the folders.

Okay, got the controller is working. I think we have enough for the beginning class.

Next

I think we need to look at 

SQLite
Unit Testing
Mongo DB
Azure DevOps deployment

