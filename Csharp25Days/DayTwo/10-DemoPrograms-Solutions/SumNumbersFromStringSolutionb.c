var numbersenteredbyuser = Console.ReadLine() ?? String.Empty;
Console.WriteLine(numbersenteredbyuser);

if(numbersenteredbyuser.Length == 0)
{
    Console.WriteLine("No input to parse for numbers");
}

var listofNumbers = numbersenteredbyuser.Split(' ');
int numberssum = 0;

foreach(var number in listofNumbers)
{
    Console.WriteLine(number);
    bool resultofconversion = int.TryParse(number, out int numberinintform); 
    if(resultofconversion == true)
    {
        numberssum = numberssum + numberinintform;
    }
    else
    {
        Console.WriteLine(" Parsing issue with " + number + " Ignoring it");
    }
}

Console.WriteLine("Total Sum " + numberssum);