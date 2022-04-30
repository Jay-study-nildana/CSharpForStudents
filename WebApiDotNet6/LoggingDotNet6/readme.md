# Logging in Dot Net 6.0 with NLog.

In this project, I look at some logging stuff.

As of now, using NLog.

# Some things to remember.

1. Remember Microsoft already has the excellent ILogger interface. The whole point of using a provider such as NLog, is so that, you can do extra things, like writing to files. Microsoft does not provide any providers that write to file.
1. After configuring NLog, you might see that the files are getting created, but, some logs are missing. If so, confirm that NLog is actually being injected in your startup code in Program.cs

   ```

   builder.Logging.ClearProviders();
   builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
   builder.Host.UseNLog();

   ```

1. Controllers will automatically get the injected loggers. However, if you have additional classes, for instance, in this code, GenericHelper.cs, need logger access, those should also be included in the dependency injection flow.

   ```

   builder.Services.AddTransient<GenericHelper>();

   ```

1. Look at nlog.config. It has the settings about the logging.
1. Specifically, where the logs are being stored.

   ```

   	<!-- the targets to write to -->
   <targets>
   	<!-- File Target for all log messages with basic details -->
   	<target xsi:type="File" name="allfile" fileName="c:\temp\nlog-AspNetCore-all-${shortdate}.log"
   			layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}" />

   	<!-- File Target for own log messages with extra web details using some ASP.NET core renderers -->
   	<target xsi:type="File" name="ownFile-web" fileName="c:\temp\nlog-AspNetCore-own-${shortdate}.log"
   			layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}|${callsite}" />

   	<!--Console Target for hosting lifetime messages to improve Docker / Visual Studio startup detection -->
   	<target xsi:type="Console" name="lifetimeConsole" layout="${MicrosoftConsoleLayout}" />
   </targets>

   ```

1. The logs would look something like this. With the current code, the logs are available in C:/temp folder. I usually open the entire folder, and browse through the logs in VS Code.

   ```

   2022-04-30 14:05:09.5755|1|INFO|LoggingDotNet6.Controllers.SherlockHolmesController|SherlockHolmesController has been constructed
   2022-04-30 14:05:09.5755|0|INFO|LoggingDotNet6.Helpers.GenericHelper|JustADumbFunctionCall has been called
   2022-04-30 14:05:10.6407|1|INFO|LoggingDotNet6.Helpers.GenericHelper|GenericHelper has been constructed
   2022-04-30 14:05:10.6407|1|INFO|LoggingDotNet6.Controllers.SherlockHolmesController|SherlockHolmesController has been constructed
   2022-04-30 14:05:10.6407|0|INFO|LoggingDotNet6.Helpers.GenericHelper|JustADumbFunctionCall has been called
   2022-04-30 14:07:02.1437|1|INFO|LoggingDotNet6.Helpers.GenericHelper|GenericHelper has been constructed
   2022-04-30 14:07:02.1449|1|INFO|LoggingDotNet6.Controllers.SherlockHolmesController|SherlockHolmesController has been constructed
   2022-04-30 14:07:02.1449|0|INFO|LoggingDotNet6.Helpers.GenericHelper|JustADumbFunctionCall has been called
   2022-04-30 14:07:03.0285|1|INFO|LoggingDotNet6.Helpers.GenericHelper|GenericHelper has been constructed
   2022-04-30 14:07:03.0285|1|INFO|LoggingDotNet6.Controllers.SherlockHolmesController|SherlockHolmesController has been constructed
   2022-04-30 14:07:03.0285|0|INFO|LoggingDotNet6.Helpers.GenericHelper|JustADumbFunctionCall has been called

   ```

# References

1. https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6
1. https://github.com/NLog/NLog.Web/tree/master/examples/ASP.NET%20Core%206/ASP.NET%20Core%206%20NLog%20Example
1. https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0
1. https://messagetemplates.org/

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy).

# Hobbies

I try to maintain a few hobbies.

1. Podcasting. You can listen to my [podcast here](https://stories.thechalakas.com/listen-to-podcast/).
1. Photography. You can see my photography on [Unsplash here](https://unsplash.com/@jay_neeruhaaku).
1. Digital Photorealism 3D Art and Arch Viz. You can see my work on this on [Adobe Behance](https://www.behance.net/vijayasimhabr).
1. Writing and Blogging. You can read my blogs. I have many medium Publications. [Read them here](https://medium.com/@vijayasimhabr).

# important note

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)
