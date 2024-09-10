// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Creating an instance of a class
Random dice = new Random();

//use methods in the class via the instance/object
//Return values and parameters of methods
int roll = dice.Next(1, 7);

Console.WriteLine(roll);

//Overloaded methods

int number = 7;
string text = "seven";

Console.WriteLine(number);
Console.WriteLine();
Console.WriteLine(text);

//Use IntelliSense

Random dice2 = new Random();
int roll1 = dice2.Next();
int roll2 = dice2.Next(101);
int roll3 = dice2.Next(50, 101);

Console.WriteLine($"First roll: {roll1}");
Console.WriteLine($"Second roll: {roll2}");
Console.WriteLine($"Third roll: {roll3}");

//Math usage

int firstValue = 500;
int secondValue = 600;
int largerValue;
largerValue = Math.Max(firstValue, secondValue);
Console.WriteLine(largerValue);




