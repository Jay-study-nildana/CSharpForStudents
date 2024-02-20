using System;
using System.Linq;

namespace EFMSSQLServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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

                addSomeKites(db);

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
