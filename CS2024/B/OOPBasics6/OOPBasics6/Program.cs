// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

//SomeStringStuff();
//SomeStringStuff2();
//DateTimeStuff();
MathStuffOne();

void SomeStringStuff()
{
    string HelloBatman = "I am Batman";
    string HelloBatmanEmo = "I am Vengeance";
    string HelloBatmanExtraSpaces = "         I am Batman      ";

    Console.WriteLine(HelloBatman);
    Console.WriteLine( HelloBatmanEmo);

    var lengthOfHelloBatman = HelloBatman.Length;

    var specificCharacter1 = HelloBatman[0];
    var specificCharacter2 = HelloBatman[1];

    //TODO. obviously, dig deeper into the object nature of string. plenty of theory I suppose.

    var UpperCaseBatman = HelloBatman.ToUpper();
    var LowerCaseBatman = HelloBatman.ToLower();
    var SubStringBatman = HelloBatman.Substring(4); //TODO substring has many overloads. check them out.
    var ReplacedBatman = HelloBatman.Replace('b', 'r');
    var BatmanSplitCollection = HelloBatman.Split(' ');
    var BatmanRemoveAllSpaces = HelloBatmanExtraSpaces.Trim();
    var BatmanAllTheCharacters = HelloBatman.ToCharArray();
    var JoinedBatman = string.Join(HelloBatman, HelloBatmanEmo);
    var BatmanStitchedFromCharacters = new string(BatmanAllTheCharacters);

    var tempbreakpoint1 = ""; //put a breakpoint here to see all the string outputs above

    //TODO, of course, you can write outputs for all the above things if you want.

    string DukeNukem = "I go where I please";
    string DukeNukem2 = "I please where I go";

    if (DukeNukem.Equals(DukeNukem2)) //you can also use the == operator
    {
        Console.WriteLine("The two strings are same");
    }
    else
    {
        Console.WriteLine("They are not same.");
    }

    if(DukeNukem.StartsWith('I'))
    {
        Console.WriteLine("Yes, the string starts with I");
    }
    if(DukeNukem.EndsWith('e'))
    {
        Console.WriteLine("Yes, the string ends with e");
    }
    if(DukeNukem.Contains("p"))
    {
        Console.WriteLine("Yes, the sentence contains p");
    }

    var locationOne = DukeNukem.IndexOf("p");
    var locationTwo = DukeNukem.LastIndexOf("p");

    var NullCheckOne = string.IsNullOrEmpty(DukeNukem); //try using a null or an empty string here.
    var WhiteSpaceCheckOne = string.IsNullOrWhiteSpace(DukeNukem); //try using a null or whitespace only string here.

    //TODO. add outputs as usual. 



    var FormattedStringOutput1 = $" The first string is {DukeNukem}";
    var FormattedStringOutput2 = $" the second string is {DukeNukem2}";  //instead of $, you can also use string.Format

    var InsertStringOne = DukeNukem.Insert(DukeNukem.Length, DukeNukem2);
    var RemoveStringOne = DukeNukem.Remove(DukeNukem.Length-5, 5);

    var tempbreakpoint2 = ""; //put a breakpoint here to see all the string outputs above
}

void SomeStringStuff2()
{
    var BadBoysOne = "Everybody wants to Mike Lowry";

    foreach(var x in BadBoysOne)
    {
        Console.WriteLine($"Something {x}");
    }

    //TODO, you can also use a for loop above

    StringBuilder BadBoysStringOne = new StringBuilder();
    BadBoysStringOne.Append("We").Append("Were").Append("Wondering");
    BadBoysStringOne.Append("If We Can").Append("Borrow Some");
    BadBoysStringOne.Append("Brown Sugar");

    Console.WriteLine(BadBoysStringOne);

    //TODO go into detail as to when and why we might use a String Builder and contrast it with just using strings

    var GetTheFullString = BadBoysStringOne.ToString();

    var StringMaxCapacity = BadBoysStringOne.MaxCapacity;

    var InsertString = BadBoysStringOne.Insert(10, "Added This Thing");

    var RemovedString = BadBoysStringOne.Remove(10, 5);

    var ReplacedString = BadBoysStringOne.Replace('e', 'f');

    var breakpointone = ""; //put a break point here

}

void DateTimeStuff()
{
    var SomeRandomDate = DateTime.Now;  //get current date and time.
    var SomeRandomDate2 = DateTime.Parse("2024-03-07 02:27:59.999 pm");

    var Day = SomeRandomDate.Day;
    var Month = SomeRandomDate.Month;

    //also, try out other Date functions. 
    //look here for all the properties that you can extract
    //https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-8.0

    var DateString = SomeRandomDate.ToString();
    var DateShortString = SomeRandomDate.ToShortDateString();

    //also, look at other similar date methods. 

    var DateAfterTenDaysAdded = SomeRandomDate.AddDays(10);
    var DateBeforeTwentyDaysSubtracted = SomeRandomDate2.AddDays(-20);

    //TODO. Add some more date related examples. 

    var breakpoint1 = "";
}

void MathStuffOne()
{
    var ExampleOfPower = Math.Pow(10, 2);
    var SmallerOfTwoNumbers = Math.Min(69, 79);
    var SquareRootExample = Math.Sqrt(25);

    //TODO check out the Math documentation for math things
    //https://learn.microsoft.com/en-us/dotnet/api/system.math?view=net-8.0

    Regex expressionone = new Regex("ae");
    bool CheckForexpressionone = expressionone.IsMatch("gray");

    Regex expressiontwo = new Regex("[A-Z]");
    bool CheckForExpressionTwo = expressiontwo.IsMatch("AB123");

    //TODO plenty of regex examples. 
    //also, TODO some notes about regular expressions, the computer science degree theory
    //https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference

    var breakpointone = "";
}