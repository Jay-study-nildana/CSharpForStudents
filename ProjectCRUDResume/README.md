# project crud - API server

This is a demo api server.

Note : This is an old school .NET 4.X pre-dot net core project. It was working when I tested a few years ago, but I am no longer maintain it.

# working with an online server

If you have an online SQL server (for example, on Azure, may be)

check file [sqlserver.md](sqlserver.md)

# working with local database MDF file

If you want to run this project with a local database.

check file [createnewmdf.md](createnewmdf.md)

# some points of note

- built using .NET 4.6
- has a simple username and password token system built in
- has swagger documentation bundled into the api server
- EF enabled database linked to an SQL server
- There is a [PostmanCollection](PostmanCollection) folder, ready to use. But I suggest you use the built in swagger for testing.

# what does this api server do

- it allows each user to login and store their education details. simple.

# deployment

- Update the connection string in web.config
- deploy as you would normally deploy a standard asp.net project on a web server that can run a dot net app.
- note - this is .NET standard project. Specifically 4.6.1
- run migrations with update-database before deploy or the tables wont be created.

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy).

# important note

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)
