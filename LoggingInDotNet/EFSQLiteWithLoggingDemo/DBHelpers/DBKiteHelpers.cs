using System;
using System.Linq;
using Microsoft.Extensions.Logging;
public class DBKiteHelpers
{
    private string tempmessage;

    public DBKiteHelpers()
    {
        tempmessage = "This is a Log Message. ";
    }
    public void addSomeKites(KiteDBContext db)
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

    public void showSomeKites(KiteDBContext db)
    {
        var listOfKites = db.Kites.ToList();

        foreach (var kite in listOfKites)
        {
            Console.WriteLine("Kite Number: " + kite.KiteId);
            Console.WriteLine(kite.KiteColor);
            Console.WriteLine(kite.KiteDesigner);
        }
    }

    public void addSomeKites(KiteDBContext db, ILogger logger)
    {

        logger.LogInformation(tempmessage + "About to add 3 kites.");
        Kite demoKite1 = new Kite();
        demoKite1.KiteColor = "Purple";
        demoKite1.KiteDesigner = "Dee";

        Kite demoKite2 = new Kite();
        demoKite2.KiteColor = "Yellow";
        demoKite2.KiteDesigner = "Varun";

        Kite demoKite3 = new Kite();
        demoKite3.KiteColor = "Blue";
        demoKite3.KiteDesigner = "Sam";
        logger.LogInformation(tempmessage + "3 Kites are added.");

        try
        {

            db.Add(demoKite1);
            db.Add(demoKite2);
            db.Add(demoKite3);

            db.SaveChanges();

            //here, I am using an informational log. 
            //DB changes getting saved is a good thing. No need to panic.
            logger.LogInformation(tempmessage + "Changes to DB have been saved.");
        }
        catch (Exception e)
        {
            //the DB not saving is a problem. 
            //Hence, I use a critical level of log so it stands out in the output.
            logger.LogCritical(tempmessage + "Something went wrong with DB. Error Details: " + e.ToString());
        }


    }

    public void showSomeKites(KiteDBContext db, ILogger logger)
    {
        var listOfKites = db.Kites.ToList();

        int i = 0;
        foreach (var kite in listOfKites)
        {
            Console.WriteLine(i + ".Kite Number: " + kite.KiteId + " Color: " + kite.KiteColor + " Designer: " + kite.KiteDesigner);
            // Console.WriteLine(kite.KiteColor);
            // Console.WriteLine(kite.KiteDesigner);
            i++;
        }

        logger.LogInformation(tempmessage + "All kites from the table displayed.");
    }
}