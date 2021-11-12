# TODO API - minimal API.

I spun this project up to 

1. test drive Visual Studio 2022
1. test drive .NET 6 - which runs in visual studio 2022 and not in 2019
1. test drive the minimal API system.

# Other Notes

1. https://localhost:7004/swagger/index.html - Swagger Documentation
1. https://localhost:7004/ - Vanilla JavaScript Web App that consumes the web api
1. Unlike previous tutorials, this one uses Program.cs to launch the entire api. for example, you have this
    ```
    var builder = WebApplication.CreateBuilder(args);    
    ```
    This seems to be the replacement for Startup.cs that I am normally used to seeing.
1. I have purposefully avoided using any DTOs. Have done that enough in the old projects. 
1. Note how everything is cramed into a single Program.cs file. Interesting.

# References

1. https://docs.microsoft.com/en-gb/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio
1. https://docs.microsoft.com/en-gb/aspnet/core/tutorials/web-api-javascript?view=aspnetcore-6.0
1. https://docs.microsoft.com/en-gb/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
1. https://docs.microsoft.com/en-gb/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)