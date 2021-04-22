# Work Diary C Sharp For Students

Okay man, lets get this in high gear.

Previously, the diary was maintained at 

dotnetworkdiary.md

# April 19th 2021

Okay, lets try to update this again.

https://github.com/Jay-study-nildana/CSharpForStudents

https://github.com/Jay-study-nildana/DotNetConceptsPrivate/tree/master/CSharpForStudents

Updating

ProjectCRUDResume

Okay, that is done. 

Updating

DesignPatterns

Okay, that is done.



# March 6th 2021

taking some old school api servers from this location

https://github.com/Jay-study-nildana/ProjectWT

also did a little bit of dot net code re-organization

that above repo still has all the old diaries. 

Next, look at all the CSharpForStudents folders and check and update them for the aske of training of blessed beyond Ife training.

Okay, first, lets get started working on the project 

projectcrudresume

First, need to shift from using the online SQL server to a local server.

  <connectionStrings>
    <add name="DefaultConnection" connectionString="Server=tcp:crudpunaserver.database.windows.net,1433;          Initial Catalog=crudpuna1;Persist Security Info=False;User ID=shuruaayithu;          Password=eegabande$543; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;          Connection Timeout=30;" providerName="System.Data.SqlClient" />
  </connectionStrings>
	<connectionStrings>
		<add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-WebApiHelloWorldNET461-20210303104127.mdf;Initial Catalog=aspnet-WebApiHelloWorldNET461-20210303104127;Integrated Security=True" providerName="System.Data.SqlClient" />
	</connectionStrings>
	<connectionStrings>
	    <add name="DefaultConnectiong" connectionString="Data Source=(LocalDb)\resumedb1.db" providerName="System.Data.SQLite.EF6"/>
    </connectionStrings>

Okay    

Finally solved it 

<connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\studentresumedb1.mdf;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>

Also, MDF needs to be created using Server Explorer. 

Server Explorer 
> Data Connections
> Add Connection 
> Microsoft SQL Server Database File (SqlClient)
> (inser file name)

and MDF file will be created anywhere you want. I just created it in the computer. 

Two files get created.

studentresumedb1.mdf
studentresumedb1_log.ldf

Now, go inside App_Data folder. Here, App_Data is visible in visual studio. 

but, the folder may not physically exist. 

create the folder, App_Data

then, put the above two files inside. Then, include them in the project (only .mdf will be visible in visual studio, log wont be visible)

now, just do Update-Database and everything should work just fine. 

Okay, its running. 

Okay, it is up and running. 

http://localhost:64674/

https://localhost:64674/

https://stackoverflow.com/questions/66508319/4-6-1-project-no-longer-runs-on-https-localhost64674

Okay, i think except for the HTTPS part, everything is good. 

Next, 

just keep updating the projects and keep adding them to the CSharpForStudents folder.


# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)