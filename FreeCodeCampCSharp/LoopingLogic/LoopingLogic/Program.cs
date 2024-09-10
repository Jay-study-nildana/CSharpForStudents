// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//LoopingLogic

//do and while iteration loops

Random random = new Random();
int current = random.Next(1, 11);

do
{
    current = random.Next(1, 11);

    if (current >= 8) continue;

    Console.WriteLine(current);
} while (current != 7);

/*
while (current >= 3)
{
    Console.WriteLine(current);
    current = random.Next(1, 11);
}
Console.WriteLine($"Last number: {current}");
*/

//example

int hero = 10;
int monster = 10;

Random dice = new Random();

do
{
    int roll = dice.Next(1, 11);
    monster -= roll;
    Console.WriteLine($"Monster was damaged and lost {roll} health and now has {monster} health.");

    if (monster <= 0) continue;

    roll = dice.Next(1, 11);
    hero -= roll;
    Console.WriteLine($"Hero was damaged and lost {roll} health and now has {hero} health.");

} while (hero > 0 && monster > 0);

Console.WriteLine(hero > monster ? "Hero wins!" : "Monster wins!");

//differentiate between do and while iteration statements

//string? readResult;
//Console.WriteLine("Enter a string:");
//do
//{
//    readResult = Console.ReadLine();
//} while (readResult == null);

//string? readResult;
//bool validEntry = false;
//Console.WriteLine("Enter a string containing at least three characters:");
//do
//{
//    readResult = Console.ReadLine();
//    if (readResult != null)
//    {
//        if (readResult.Length >= 3)
//        {
//            validEntry = true;
//        }
//        else
//        {
//            Console.WriteLine("Your input is invalid, please try again.");
//        }
//    }
//} while (validEntry == false);

// capture user input in a string variable named readResult

//int numericValue = 0;
//bool validNumber = false;

//validNumber = int.TryParse(readResult, out numericValue);

//example one

//string? readResult;
//string valueEntered = "";
//int numValue = 0;
//bool validNumber = false;

//Console.WriteLine("Enter an integer value between 5 and 10");

//do
//{
//    readResult = Console.ReadLine();
//    if (readResult != null)
//    {
//        valueEntered = readResult;
//    }

//    validNumber = int.TryParse(valueEntered, out numValue);

//    if (validNumber == true)
//    {
//        if (numValue <= 5 || numValue >= 10)
//        {
//            validNumber = false;
//            Console.WriteLine($"You entered {numValue}. Please enter a number between 5 and 10.");
//        }
//    }
//    else
//    {
//        Console.WriteLine("Sorry, you entered an invalid number, please try again");
//    }
//} while (validNumber == false);

//Console.WriteLine($"Your input value ({numValue}) has been accepted.");

//readResult = Console.ReadLine();

//example two

//string? readResult;
//string roleName = "";
//bool validEntry = false;

//do
//{
//    Console.WriteLine("Enter your role name (Administrator, Manager, or User)");
//    readResult = Console.ReadLine();
//    if (readResult != null)
//    {
//        roleName = readResult.Trim();
//    }

//    if (roleName.ToLower() == "administrator" || roleName.ToLower() == "manager" || roleName.ToLower() == "user")
//    {
//        validEntry = true;
//    }
//    else
//    {
//        Console.Write($"The role name that you entered, \"{roleName}\" is not valid. ");
//    }

//} while (validEntry == false);

//Console.WriteLine($"Your input value ({roleName}) has been accepted.");
//readResult = Console.ReadLine();

//example three

string[] myStrings = new string[2] { "I like pizza. I like roast chicken. I like salad", "I like all three of the menu choices" };
int stringsCount = myStrings.Length;

string myString = "";
int periodLocation = 0;

for (int i = 0; i < stringsCount; i++)
{
    myString = myStrings[i];
    periodLocation = myString.IndexOf(".");

    string mySentence;

    // extract sentences from each string and display them one at a time
    while (periodLocation != -1)
    {

        // first sentence is the string value to the left of the period location
        mySentence = myString.Remove(periodLocation);

        // the remainder of myString is the string value to the right of the location
        myString = myString.Substring(periodLocation + 1);

        // remove any leading white-space from myString
        myString = myString.TrimStart();

        // update the comma location and increment the counter
        periodLocation = myString.IndexOf(".");

        Console.WriteLine(mySentence);
    }

    mySentence = myString.Trim();
    Console.WriteLine(mySentence);
}
