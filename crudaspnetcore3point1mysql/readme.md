# CRUD ASP.NET CORE 3.1 with MySQL

Simple CRUD demo. 

* Uses Identity UI for login/register/logout
* Uses MySQL Server with Entity Framework

In this demo, you can.

* Register, Login and Logout with a local email address account
* CRUD a list of movies
* Add movies to watchlist, and see that watchlist of your own, in a separate view.

# Connection String

* "DefaultConnection": "server=localhost;userid=root;password=YourPassWord;port=3306;database=YourSchemaDBName;SslMode=None"

You need to update this as per your MySQL server in appsettings.json

# Other Notes

* This was built using visual studio code. So, it should run fine on both Windows and Mac.
* DONT, EVER, create the project first and then, try and add authentication. Especially with MySQL. 
* always create a new project with authentication from the first step. This ensures that you only have one database context to deal with. 

# References and Links

* https://docs.microsoft.com/en-in/aspnet/core/tutorials/first-mvc-app/?view=aspnetcore-3.1
* https://docs.microsoft.com/en-in/aspnet/core/tutorials/first-mvc-app/adding-controller?view=aspnetcore-3.1&tabs=visual-studio
* https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=netcore-cli
* https://github.com/jasonsturges/mysql-dotnet-core
* https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)