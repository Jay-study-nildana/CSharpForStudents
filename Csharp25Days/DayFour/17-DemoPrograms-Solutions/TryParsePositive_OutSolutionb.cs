var Random = new Random();
var listofmarks = new List<int> {
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                    Random.Next(100),
                                };

static bool TryPassorFaile(int marksnumbertocheck, out string passmessage)
{
    passmessage = string.Empty;
    if (marksnumbertocheck >= 35)
    {
        passmessage = "Passed the Exam";
        return true;
    }
    else
    {
        passmessage = "Failed the Exam";
        return false;
    }
}

foreach(var mark in listofmarks)
{
    Console.WriteLine("Marks is " + mark);
    if(TryPassorFaile(mark, out string message))
    {

        Console.WriteLine(message);
    }
    else
    {
        Console.WriteLine(message);
    }
}