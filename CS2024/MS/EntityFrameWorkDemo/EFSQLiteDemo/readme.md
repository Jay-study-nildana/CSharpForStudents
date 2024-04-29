# EFSQLiteDemo - Console App with EF Core using SQLite

A quick and simple EF Core demo with SQLite. Remember to use 'SQLITE DB Browser' to explore the database. 

1. Note - check the notes below if you keep getting 'table not found' exceptions. This happens because, the project cannot locate the database.
1. Tested on Visual Studio on Windows
1. Tested on Visual Studio Code on Windows
1. NOT currently working on Mac. I keep getting 'working directory' errors. TODO.

# Table 'Kites' not found error

This is explained in detail below. In short, Try the following (Visual Studio and Windows only)

1. Delete Migrations folder
1. Delete the SQLite file (.db)
1. Set the working directory manually to the current working folder. more details - https://entityframeworkcore.com/knowledge-base/33455041/asp-net-5--ef-7-and-sqlite---sqlite-error-1---no-such-table--blog-
    1. In Solution Explorer, right click the project and then select Properties.
    1. Select the Debug tab in the left pane.
    1. Set Working directory to the project directory.
    1. Save the changes.
1. If you still get error, try
    1. Add-Migration InitialCreate
    1. Update-Database
1. TODO. Add a try catch for db operations

# Additional Note

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

# hire and get to know me

find ways to hire me, follow me and stay in touch with me.

1. https://jay-study-nildana.github.io/developerprofile/
1. https://thechalakas.com
