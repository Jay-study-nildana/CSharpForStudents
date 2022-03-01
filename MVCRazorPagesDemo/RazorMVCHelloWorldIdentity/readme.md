# MVC Razor Pages Demo With Individual Accounts

Quick and dirty .NET 6 MVC with Razor Pages Demo with a quick EF Core with SQL Server Express usage.

Also, this has the Individual Accounts Enabled. You can Register and Login for the basic user authentication experience.

# Note About Identity and Authentication

Although I have put this repository for my students to see, please note, I personally dont use User Identity in any of my projects. This is strictly for your school or university projects and a general curiosity.

If you want a proper authentication system, I would recommend you use, Auth0. 

Look at my demo project [Random Stuff Generator](https://github.com/Jay-study-nildana/RandomStuffDocs), which uses, Auth0.

# Notes General

1. To ensure Individual Accounts is enabled, select the option during project creation in Visual Studio
1. Dont forget to run the following command to get the database up and running. It has to be in the package manager console.
    ```
        Update-Database
    ```
1. Use a password and account, that looks, something like, a@a.com and Password$321
1. Remember that, unlike the old days, we no longer get the 'AccountController' with the current Identity system. Still though, you can access the current user details, with the following code.
    ```
        var tempUserDetails = User.Identity;
    ```
    Remember to use this code inside a controller.
1. Notes end here.

# References

1. https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-6.0&tabs=visual-studio
1. https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-6.0&tabs=visual-studio -- use this if you want control over how identity is added to your project. 
1. https://stackoverflow.com/questions/36641338/how-to-get-current-user-in-asp-net-core

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)