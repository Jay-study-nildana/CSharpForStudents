using EFSQLiteDemo.Enums;
using EFSQLiteDemo.Interfaces;
using EFSQLiteDemo.POCO;
using System;
using System.Linq;

namespace EFSQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Print the current directory to debug output
            // Note: Sometimes the SQLite DB gets created in the solution folder instead of the Current Directory.
            // This can happen due to the way the application is executed in different environments (e.g., Visual Studio, command line).
            // When running the application from Visual Studio, the working directory might be set to the solution folder.
            // As a result, the SQLite database file may be created in the solution folder instead of the intended current directory.
            // To ensure the database file is in the correct location, you may need to manually copy it from the solution folder to the current directory.
            // This behavior is due to the environment's working directory settings and is generally unavoidable.
            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);

            using (var db = new KiteDBContext())
            {

                if (PromptUserForConfirmation("Do you want to add some kites? (y/n)"))
                {
                    addSomeKites(db);
                }

                showAllKites(db);

            }

        }

        static void showAllKites(KiteDBContext db)
        {
            try
            {
                var listOfKites = db.KiteUpdateOctober4ths.ToList();

                foreach (var kite in listOfKites)
                {
                    Console.WriteLine("Kite Number: " + kite.KiteId);
                    Console.WriteLine(kite.KiteColor);
                    Console.WriteLine(kite.KiteDesigner);
                    Console.WriteLine(kite.KiteHeight);
                    Console.WriteLine(kite.KiteWeight);
                    Console.WriteLine(kite.KiteWidth);
                    Console.WriteLine("-----------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving kites from the database: " + ex.Message);
            }
        }

        static void addSomeKites(KiteDBContext db)
        {
            try
            {
                Kite demoKite1 = new Kite();
                demoKite1.KiteColor = ColorEnum.Blue;
                demoKite1.KiteDesigner = "Dee";

                Kite demoKite2 = new Kite();
                demoKite2.KiteColor = ColorEnum.Orange;
                demoKite2.KiteDesigner = "Varun";

                Kite demoKite3 = new Kite();
                demoKite3.KiteColor = ColorEnum.Violet;
                demoKite3.KiteDesigner = "Sam";

                db.Add(demoKite1);
                db.Add(demoKite2);
                db.Add(demoKite3);

                db.SaveChanges();

                IGetKite getKite = new GetKiteOne();
                var response = getKite.GiveMeAKite();
                var response2 = getKite.GiveMeAKite();
                var response3 = getKite.GiveMeAKite();

                db.Add(response);
                db.Add(response2);
                db.Add(response3);

                db.SaveChanges();

                Console.WriteLine("Some kites were added to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding kites to the database: " + ex.Message);
            }
        }


        static bool PromptUserForConfirmation(string message)
        {
            Console.WriteLine(message);
            var response = Console.ReadLine();
            return response?.ToLower() == "y";
        }
    }
}
