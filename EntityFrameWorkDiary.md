# Entity Framework Work Diary

Let's look into this.

# April 22nd 2021

https://docs.microsoft.com/en-us/ef/

https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli

https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/create

First, SQLite project

EFSQLiteDemo

Second, Mongo DB project

EFMongoDBDemo

https://cloud.mongodb.com/v2/60291242e5320302b42dcbbb#security/database/users

database user

efmongouser

oPxooRzxzF66RjoV

Download Mongo GUI software 

https://downloads.mongodb.com/compass/mongodb-compass-1.26.1-win32-x64.exe

mongodb+srv://efmongouser:oPxooRzxzF66RjoV@cluster0.dgpm5.mongodb.net/test

Oh my god. I just found out that mongo DB does not support EF Core. 

Third, but actually, Second project

We need to switch to SQL Server

Okay, switching to SQL Server.

Getting credentials from AzureSQLStudio forlder, loginstuff.md

Server=tcp:practiceofqueries1.database.windows.net,1433;Initial Catalog=AdventureWorksDW2014;Persist Security Info=False;User ID=namidodb;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

practiceofqueries1

practiceofqueries1.database.windows.net

admin

namidodb

password

Nadipusswrd$320

EFMSSQLServerDemo

https://docs.microsoft.com/en-in/ef/core/providers/sql-server/?tabs=dotnet-core-cli

Okay, the console apps are ready. 

Next

Let's add SQL Server and SQLite to 

WebApiDotNetCore

projects.

Okay, let's add mongo DB via Azure Cosmos

efmongouser

That will also not work with migrations.

Now, continuing on WebAPIWorkDiary.md