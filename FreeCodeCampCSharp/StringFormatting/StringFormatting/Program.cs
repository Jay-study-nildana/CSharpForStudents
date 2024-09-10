// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Format literal strings in C#

Console.WriteLine("Hello\nWorld!");
Console.WriteLine("Hello\tWorld!");

// Console.WriteLine("Hello "World"!");  //won't work

Console.WriteLine("Hello \"World\"!");

Console.WriteLine("c:\\source\\repos");

//Format output using character escape sequences

Console.WriteLine("Generating invoices for customer \"Contoso Corp\" ... \n");
Console.WriteLine("Invoice: 1021\t\tComplete!");
Console.WriteLine("Invoice: 1022\t\tComplete!");
Console.WriteLine("\nOutput Directory:\t");

//Verbatim string literal

Console.WriteLine(@"    c:\source\repos    
        (this is where your code goes)");

//Format output using verbatim string literals

Console.Write(@"c:\invoices");

//Unicode escape characters

// Kon'nichiwa World
Console.WriteLine("\u3053\u3093\u306B\u3061\u306F World!");

//Format output using unicode escape characters

// To generate Japanese invoices:
// Nihon no seikyū-sho o seisei suru ni wa:
Console.Write("\n\n\u65e5\u672c\u306e\u8acb\u6c42\u66f8\u3092\u751f\u6210\u3059\u308b\u306b\u306f\uff1a\n\t");
// User command to run an application
Console.WriteLine(@"c:\invoices\app.exe -j");

//Combine strings using string concatenation

string firstName = "Bob";
string message = "Hello " + firstName;
Console.WriteLine(message);

string firstName2 = "Bob";
string greeting = "Hello";
string message2 = greeting + " " + firstName2 + "!";
Console.WriteLine(message2);

//Avoiding intermediate variables

string firstName3 = "Bob";
string greeting3 = "Hello";
Console.WriteLine(greeting3 + " " + firstName3 + "!");

//Combine strings using string interpolation

string message4 = $"{greeting} {firstName}!";

string firstName5 = "Bob";
string message5 = $"Hello {firstName}!";
Console.WriteLine(message5);

int version = 11;
string updateText = "Update to Windows";
string message6 = $"{updateText} {version}";
Console.WriteLine(message6);

//Combine verbatim literals and string interpolation
string projectName = "First-Project";
Console.WriteLine($@"C:\Output\{projectName}\Data");

string projectName2 = "ACME";
string englishLocation = $@"c:\Exercise\{projectName2}\data.txt";
Console.WriteLine($"View English output:\n\t\t{englishLocation}\n");

string russianMessage = "\u041f\u043e\u0441\u043c\u043e\u0442\u0440\u0435\u0442\u044c \u0440\u0443\u0441\u0441\u043a\u0438\u0439 \u0432\u044b\u0432\u043e\u0434";
string russianLocation = $@"c:\Exercise\{projectName}\ru-RU\data.txt";
Console.WriteLine($"{russianMessage}:\n\t\t{russianLocation}\n");


