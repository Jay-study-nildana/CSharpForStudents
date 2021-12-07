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

            using (var db = new KiteDBContext())
            {

                #region the basic Kites class add and show

                addSomeKites(db);

                showAllKites(db);

                #endregion

                #region the KiteUpdateOctober4th related db stuff

                IGetKite getKite = new GetKiteOne();
                var response = getKite.GiveMeAKite();
                var response2 = getKite.GiveMeAKite();
                var response3 = getKite.GiveMeAKite();

                db.Add(response);
                db.Add(response2);
                db.Add(response3);

                db.SaveChanges();

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

                #endregion
            }

        }

        static void showAllKites(KiteDBContext db)
        {
            var listOfKites = db.Kites.ToList();

            foreach (var kite in listOfKites)
            {
                Console.WriteLine("Kite Number: " + kite.KiteId);
                Console.WriteLine(kite.KiteColor);
                Console.WriteLine(kite.KiteDesigner);
            }
        }

        static void addSomeKites(KiteDBContext db)
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
        }
    }
}
