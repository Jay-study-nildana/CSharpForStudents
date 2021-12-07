# EFSQLiteDemo

A quick and simple EF Core demo with SQLite.

Note : check the notes below if you keep getting 'table not found' exceptions. This happens because, the project cannot locate the database.

# Important Note about SQLite Path Issues - Part One

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

# Important Note about SQLite Path Issues - Part Two

You could also do something like this. 

```
    // Reference: https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
    public class BloggingContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public string DbPath { get; private set; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}blogging.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

```

# Important Note about SQLite Path Issues - Part Three

Or, you could manually provide a path to any location on your computer. Dont let the computer create the path for yourself and make the sqlite db file go where you want it to go.

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)