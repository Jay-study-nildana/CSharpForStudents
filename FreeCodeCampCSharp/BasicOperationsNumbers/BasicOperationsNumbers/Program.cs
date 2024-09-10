// See https://aka.ms/new-console-template for more information
using System.ComponentModel;

Console.WriteLine("Hello, World!");

// Add two numeric values

int firstNumber = 12;
int secondNumber = 7;
Console.WriteLine(firstNumber + secondNumber);

// Mix data types to force implicit type conversions

string firstName = "Bob";
int widgetsSold = 7;
Console.WriteLine(firstName + " sold " + widgetsSold + " widgets.");

// advanced case of adding numbers and concatenating strings
string firstName2 = "Bob";
int widgetsSold2 = 7;
Console.WriteLine(firstName2 + " sold " + widgetsSold2 + 7 + " widgets.");

// parentheses to clarify your intention to the compiler

string firstName3 = "Bob";
int widgetsSold3 = 7;
Console.WriteLine(firstName3 + " sold " + (widgetsSold3 + 7) + " widgets.");

//perform addition, subtraction, multiplication, and division with integers

int sum = 7 + 5;
int difference = 7 - 5;
int product = 7 * 5;
int quotient = 7 / 5;

Console.WriteLine("Sum: " + sum);
Console.WriteLine("Difference: " + difference);
Console.WriteLine("Product: " + product);
Console.WriteLine("Quotient: " + quotient);

//perform division using literal decimal data

decimal decimalQuotient = 7.0m / 5;
Console.WriteLine($"Decimal quotient: {decimalQuotient}");

// code to cast results of integer division

int first = 7;
int second = 5;
decimal quotient2 = (decimal)first / (decimal)second;
Console.WriteLine(quotient2);

//determine the remainder after integer division

Console.WriteLine($"Modulus of 200 / 5 : {200 % 5}");
Console.WriteLine($"Modulus of 7 / 5 : {7 % 5}");

//In math, PEMDAS is an acronym that helps students remember the order of operations. The order is:

//Parentheses(whatever is inside the parenthesis is performed first)
//Exponents
//Multiplication and Division (from left to right)
//Addition and Subtraction (from left to right)

int value1 = 3 + 4 * 5;
int value2 = (3 + 4) * 5;
Console.WriteLine(value1);
Console.WriteLine(value2);

//Increment and decrement values

int value = 0;     // value is now 0.
value = value + 5; // value is now 5.
value += 5;        // value is now 10.

int value3 = 0;     // value is now 0.
value3 = value3 + 1; // value is now 1.
value3++;           // value is now 2.

int value4 = 1;

value4 = value4 + 1;
Console.WriteLine("First increment: " + value4);

value4 += 1;
Console.WriteLine("Second increment: " + value4);

value4++;
Console.WriteLine("Third increment: " + value4);

value4 = value4 - 1;
Console.WriteLine("First decrement: " + value4);

value4 -= 1;
Console.WriteLine("Second decrement: " + value4);

value4--;
Console.WriteLine("Third decrement: " + value4);

int value5 = 1;
value5++;
Console.WriteLine("First: " + value5);
Console.WriteLine($"Second: {value5++}");
Console.WriteLine("Third: " + value5);
Console.WriteLine("Fourth: " + (++value5));

// Calculate Celsius given the current temperature in Fahrenheit

int fahrenheit = 94;
decimal celsius = (fahrenheit - 32m) * (5m / 9m);
Console.WriteLine("The temperature is " + celsius + " Celsius.");




