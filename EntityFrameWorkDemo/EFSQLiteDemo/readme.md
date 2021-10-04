# EFSQLiteDemo

A quick and simple EF Core demo with SQLite.

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

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)