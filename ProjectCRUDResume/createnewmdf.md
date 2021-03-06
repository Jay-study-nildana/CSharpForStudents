# Creating a new MDF file

Use these steps to make the project work with the local database file.

# Connection String

First up, here is the connection string. 

<connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\studentresumedb1.mdf;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>

# Creat the MDF file.

MDF needs to be created using Server Explorer, inside visual studio. 

Server Explorer 
> Data Connections
> Add Connection 
> Microsoft SQL Server Database File (SqlClient)
> (insert file name)

and MDF file will be created anywhere you want. 

Two files get created. (give whatever file name you want to give)

studentresumedb1.mdf
studentresumedb1_log.ldf

Now, go inside App_Data folder. Here, App_Data is visible in visual studio. 

but, the folder may not physically exist. 

create the folder, App_Data in your project folder. 

then, put the above two files inside. Then, include them in the project (only .mdf will be visible in visual studio, log wont be visible)

now, just do Update-Database and everything should work just fine. 

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)