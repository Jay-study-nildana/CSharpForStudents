using System;
using System.Linq;

namespace EFSQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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

            using (var db = new KiteDBContext())
            {
                //uncomment the following to add new items

                // Kite demoKite1 = new Kite();
                // demoKite1.KiteColor = "Red";
                // demoKite1.KiteDesigner = "Jay";

                // Kite demoKite2 = new Kite();
                // demoKite2.KiteColor = "Blue";
                // demoKite2.KiteDesigner = "Bay";

                // Kite demoKite3 = new Kite();
                // demoKite3.KiteColor = "Green";
                // demoKite3.KiteDesigner = "May";

                // db.Add(demoKite1);
                // db.Add(demoKite2);
                // db.Add(demoKite3);

                // db.SaveChanges();

                // addSomeKites(db);

                var listOfKites = db.Kites.ToList();

                foreach (var kite in listOfKites)
                {
                    Console.WriteLine("Kite Number: " + kite.KiteId);
                    Console.WriteLine(kite.KiteColor);
                    Console.WriteLine(kite.KiteDesigner);
                }
            }

        }

        static void addSomeKites(KiteDBContext db)
        {
            Kite demoKite1 = new Kite();
            demoKite1.KiteColor = "Red";
            demoKite1.KiteDesigner = "Jay";

            Kite demoKite2 = new Kite();
            demoKite2.KiteColor = "Blue";
            demoKite2.KiteDesigner = "Bay";

            Kite demoKite3 = new Kite();
            demoKite3.KiteColor = "Green";
            demoKite3.KiteDesigner = "May";

            db.Add(demoKite1);
            db.Add(demoKite2);
            db.Add(demoKite3);

            db.SaveChanges();
        }
    }
}
