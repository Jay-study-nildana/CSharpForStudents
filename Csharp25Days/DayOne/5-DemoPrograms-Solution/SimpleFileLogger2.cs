string logfilename = "logfile.txt";
var currentdirectory = Environment.CurrentDirectory;

var logfilepath = Path.Combine(currentdirectory, logfilename);

Console.WriteLine($" lot file path is : {logfilepath}");

if(File.Exists(logfilepath))
{
    Console.WriteLine("File exists");
}
else
{
    Console.WriteLine(" file does not exist :");
    try
    {
        File.Create(logfilepath).Dispose();
        Console.WriteLine("log file created. ready for logging");
    }
    catch(Exception ex)
    {
        Console.WriteLine("Error Creating File");
        Console.WriteLine(ex.Message);
    }


    if (File.Exists(logfilepath))
    {
        Console.WriteLine("After file creation, confirmed that File exists");
    }
}

var logentrynumberone = "This is a sample log entry";

for(int i=0;i<5;i++)
{
    AddLogEntry(logentrynumberone);
}


try
{
    var allthelogsfromfile = File.ReadAllLines(logfilepath);
    foreach(var x in allthelogsfromfile)
    {
        Console.WriteLine(x);
    }

}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

void AddLogEntry(string logentrynumberone)
{
    try
    {
        var logentrywithdate = DateTime.Now.ToString("g") + " " + logentrynumberone;
        File.AppendAllText(logfilepath, logentrywithdate);
        File.AppendAllText(logfilepath, Environment.NewLine);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

Console.WriteLine("Press Any Key...");