// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;

Console.WriteLine("Hello, World!");

//Variable name rules
//There are some variable naming rules that are enforced by the C# compiler.

//Variable names can contain alphanumeric characters and the underscore (_) character. Special characters like the pound #, the dash -, and the dollar sign $ are not allowed.
//Variable names must begin with an alphabetical letter or an underscore, not a number. Using an underscore character to start a variable name is typically reserved for private instance fields.A link to further reading can be found in the module summary.
//Variable names must NOT be a C# keyword. For example, these variable name declarations won't be allowed: float float; or string string;.
//Variable names are case-sensitive, meaning that string MyValue; and string myValue; are two different variables.


//Variable name conventions
//Conventions are suggestions that are agreed upon by the software development community. While you're free to decide not to follow these conventions, they're so popular that it might make it difficult for other developers to understand your code. You should practice adopting these conventions and make them part of your own coding habits.

//Variable names should use camel case, which is a style of writing that uses a lower-case letter at the beginning of the first word and an upper-case letter at the beginning of each subsequent word. For example: string thisIsCamelCase;.
//Variable names should be descriptive and meaningful in your application. You should choose a name for your variable that represents the kind of data it will hold (not the data type). For example: bool orderComplete;, NOT bool isComplete;.
//Variable names should be one or more entire words appended together. Don't use contractions because the name of the variable may be unclear to others who are reading your code. For example: decimal orderAmount;, NOT decimal odrAmt;.
//Variable names shouldn't include the data type of the variable. You might see some advice to use a style like string strMyValue;. It was a popular style years ago. However, most developers don't follow this advice anymore and there are good reasons not to use it.
//The example string firstName; follows all of these rules and conventions, assuming I want to use this variable to store data that represents someone's first name.

//What is a code comment ?

//This may not seem useful at first, however it's useful in three situations:

//When you want to leave a note about the intent of a passage of code. It can be helpful to include code comments that describe the purpose or the thought process when you're writing a particularly challenging set of coding instructions. Your future self will thank you.
//When you want to temporarily remove code from your application to try a different approach, but you're not yet convinced your new idea will work. You can comment out the code, write the new code, and once you're convinced the new code will work the way you want it to, you can safely delete the old (commented code).
//Adding a message like TODO to remind you to look at a given passage of code later. While you should use this judiciously, it's a useful approach. You may be working on another feature when you read a line of code that sparks a concern. Rather than ignoring the new concern, you can mark it for investigation later.

Random random = new Random();
string[] orderIDs = new string[5];
// Loop through each blank orderID
for (int i = 0; i < orderIDs.Length; i++)
{
    // Get a random value that equates to ASCII letters A through E
    int prefixValue = random.Next(65, 70);
    // Convert the random value into a char, then a string
    string prefix = Convert.ToChar(prefixValue).ToString();
    // Create a random number, pad with zeroes
    string suffix = random.Next(1, 1000).ToString("000");
    // Combine the prefix and suffix together, then assign to current OrderID
    orderIDs[i] = prefix + suffix;
}
// Print out each orderID
foreach (var orderID in orderIDs)
{
    Console.WriteLine(orderID);
}

//Use whitespace to make your code easier to read

Random dice = new Random();

int roll1 = dice.Next(1, 7);
int roll2 = dice.Next(1, 7);
int roll3 = dice.Next(1, 7);

int total = roll1 + roll2 + roll3;
Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");

if ((roll1 == roll2) || (roll2 == roll3) || (roll1 == roll3))
{
    if ((roll1 == roll2) && (roll2 == roll3))
    {
        Console.WriteLine("You rolled triples!  +6 bonus to total!");
        total += 6;
    }
    else
    {
        Console.WriteLine("You rolled doubles!  +2 bonus to total!");
        total += 2;
    }
}

/*
   This code reverses a message, counts the number of times 
   a particular character appears, then prints the results
   to the console window.
 */

string originalMessage = "The quick brown fox jumps over the lazy dog.";

char[] message = originalMessage.ToCharArray();
Array.Reverse(message);

int letterCount = 0;

foreach (char letter in message)
{
    if (letter == 'o')
    {
        letterCount++;
    }
}

string newMessage = new String(message);

Console.WriteLine(newMessage);
Console.WriteLine($"'o' appears {letterCount} times.");