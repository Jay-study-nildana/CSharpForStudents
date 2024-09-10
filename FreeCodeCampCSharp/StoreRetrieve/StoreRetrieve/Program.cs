// See https://aka.ms/new-console-template for more information
using System.Runtime.Intrinsics.X86;

Console.WriteLine("Hello, World!");

//Print literal values

//The term char is short for character. In C#, this data type is officially named "char", but frequently referred to as a "character".
//Use character literals

Console.WriteLine('b');

//Use integer literals

Console.WriteLine(123);

// Use floating-point literals

Console.WriteLine(0.25F); // float literal, append the letter F after the number.

Console.WriteLine(12.39816m); //decimal literal, append the letter m after the number

//Boolean literals

Console.WriteLine(true);
Console.WriteLine(false);

Console.WriteLine("123"); //string
Console.WriteLine(123); //number
 
Console.WriteLine("true"); //string
Console.WriteLine(true); //boolean

//Here's a few important considerations about variable names:

//Variable names can contain alphanumeric characters and the underscore character. Special characters like the hash symbol # (also known as the number symbol or pound symbol) or dollar symbol $ are not allowed.
//Variable names must begin with an alphabetical letter or an underscore, not a number.
//Variable names are case-sensitive, meaning that string Value; and string value; are two different variables.
//Variable names must not be a C# keyword. For example, you cannot use the following variable declarations: decimal decimal; or string string;.

//string firstName;

char userOption;

int gameScore;

decimal particlesPerMillion;

bool processedCustomer;

//Setting and getting values from variables

string firstName2;

//Initialize the variable
firstName2 = "Jay";
//Retrieve a value you stored in the variable
Console.WriteLine(firstName2);

//Reassign the value of a variable

string firstName;
firstName = "Bob";
Console.WriteLine(firstName);
firstName = "Liem";
Console.WriteLine(firstName);
firstName = "Isabella";
Console.WriteLine(firstName);
firstName = "Yasmin";
Console.WriteLine(firstName);


//An implicitly typed local variable is created by using the var keyword followed by a variable initialization. For example:

var message = "Hello world!";

//Why use the var keyword?

string name = "Bob";
int messages = 3;
decimal temperature = 34.4m;

Console.Write("Hello, ");
Console.Write(name);
Console.Write("! You have ");
Console.Write(messages);
Console.Write(" messages in your inbox. The temperature is ");
Console.Write(temperature);
Console.Write(" celsius.");



