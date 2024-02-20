// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//1.Variables
//   1.Creating Variables / Declaring Variables
//   1. Default Values
//   1. Assigning and Changing Values
//   1. Naming Convention
//   1. Variable Name and (avoiding) C Sharp Keywords
//   1. Showing output with Variable Values

int NumberOfFileFolders = 3;  //creating and assigning values
string NameOfGirl = "Alex"; //creating and assigning values

string NameOfGirl2 = NameOfGirl; //assign one variable value from another variable

NameOfGirl = "Natalie"; //changing the original value with a new value

//By convention, C# programs use PascalCase for type names, namespaces, and all public members

Console.WriteLine(NumberOfFileFolders + NameOfGirl);
Console.WriteLine("Just combining the two variables : "+NameOfGirl2 + NameOfGirl);

Console.ReadKey();