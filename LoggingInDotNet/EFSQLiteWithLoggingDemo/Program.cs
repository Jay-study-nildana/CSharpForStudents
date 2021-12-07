using System;
using System.Linq;
//for logging
using Microsoft.Extensions.Logging;
//for logging and console logging
using Microsoft.Extensions.Logging.Console;

namespace EFSQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var tempmessage = "";

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();

            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            tempmessage = "Program has now started. This is a 'Trace' level log";
            logger.LogTrace(tempmessage);
            tempmessage = "Program has now started. This is a 'Debug' level log";
            logger.LogDebug(tempmessage);
            tempmessage = "Program has now started. This is a 'Information' level log";
            logger.LogInformation(tempmessage);
            tempmessage = "Program has now started. This is a 'Warning' level log";
            logger.LogWarning(tempmessage);
            tempmessage = "Program has now started. This is a 'Error' level log";
            logger.LogError(tempmessage);
            tempmessage = "Program has now started. This is a 'Critical' level log";
            logger.LogCritical(tempmessage);
            tempmessage = "Program has now started. This is a 'None' level log";
            //no need to log this.

            #region essential db code with comments            

            // using (var db = new BloggingContext())
            // {
            //     // Note: This sample requires the database to be created before running.

            //     // Create
            //     Console.WriteLine("Inserting a new blog");
            //     db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            //     db.SaveChanges();

            //     // Read
            //     Console.WriteLine("Querying for a blog");
            //     var blog = db.Blogs
            //         .OrderBy(b => b.BlogId)
            //         .First();

            //     // Update
            //     Console.WriteLine("Updating the blog and adding a post");
            //     blog.Url = "https://devblogs.microsoft.com/dotnet";
            //     blog.Posts.Add(
            //         new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
            //     db.SaveChanges();

            //     // Delete
            //     Console.WriteLine("Delete the blog");
            //     db.Remove(blog);
            //     db.SaveChanges();
            // }

            #endregion


            using (var db = new KiteDBContext())
            {
                DBKiteHelpers tempDBKiteHelpers = new DBKiteHelpers();
                //uncommment the next line to add some kites.
                // tempDBKiteHelpers.addSomeKites(db);

                //uncomment the next line to show some kites.
                //tempDBKiteHelpers.showSomeKites(db);

                //uncommment the next line to add some kites. with logging
                tempDBKiteHelpers.addSomeKites(db, logger);

                //uncomment the next line to show some kites. with logging
                tempDBKiteHelpers.showSomeKites(db, logger);
            }

        }

        static void addSomeKites(KiteDBContext db)
        {

            Kite demoKite1 = new Kite();
            demoKite1.KiteColor = "Purple";
            demoKite1.KiteDesigner = "Dee";

            Kite demoKite2 = new Kite();
            demoKite2.KiteColor = "Yellow";
            demoKite2.KiteDesigner = "Varun";

            Kite demoKite3 = new Kite();
            demoKite3.KiteColor = "Blue";
            demoKite3.KiteDesigner = "Sam";

            db.Add(demoKite1);
            db.Add(demoKite2);
            db.Add(demoKite3);

            db.SaveChanges();
        }

        static void showSomeKites(KiteDBContext db)
        {
            var listOfKites = db.Kites.ToList();

            foreach (var kite in listOfKites)
            {
                Console.WriteLine("Kite Number: " + kite.KiteId);
                Console.WriteLine(kite.KiteColor);
                Console.WriteLine(kite.KiteDesigner);
            }
        }
    }
}
