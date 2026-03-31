//int value;
//if (int.TryParse("123", out value))
//{
//    Console.WriteLine(value); // value is assigned if TryParse returned true
//}

//example 

int movieruntime=50;
bool continuelooping = true;

while(continuelooping)
{
    Console.WriteLine("Enter the movie runtime please: ");

    string? userinput = Console.ReadLine();

    if (int.TryParse(userinput, out movieruntime))
    {
        Console.WriteLine(movieruntime);
    }
    else
    {
        Console.WriteLine("There was a problem parsing the movie runtime");
    }

    Console.WriteLine("Do you want to keep going? (y/n)");
    string? usercontinueinput = Console.ReadLine();

    if(usercontinueinput == "y")
    {
        continuelooping = true;
    }
    else
    {
        continuelooping = false;
    }
}

//another example

bool TryNumberIsTwenty(int somenumber,out string result)
{
    if (somenumber == 20)
    {
        result = "The number you entered is definitely twenty";
        return true;
    }

    result = "The number you entered is not twenty:";
    return false;
}

Console.WriteLine("Please enter a number: ");
string? userinput = Console.ReadLine();

int.TryParse(userinput, out int number);

bool isTwenty = TryNumberIsTwenty(number, out string message);

Console.WriteLine(message);


