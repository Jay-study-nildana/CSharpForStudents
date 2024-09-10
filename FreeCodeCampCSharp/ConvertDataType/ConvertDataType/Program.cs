// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//data type casting and conversion

//int first = 2;
//string second = "4"; //error
//int result = first + second; //error
//Console.WriteLine(result);

decimal myDecimal = 3.14m;
Console.WriteLine($"decimal: {myDecimal}");

int myInt = (int)myDecimal;
Console.WriteLine($"int: {myInt}");

//"widening conversion" or a "narrowing conversion"

decimal myDecimal2 = 1.23456789m;
float myFloat = (float)myDecimal2;

Console.WriteLine($"Decimal: {myDecimal2}");
Console.WriteLine($"Float  : {myFloat}");

///Use ToString() to convert a number to a string

int first = 5;
int second = 7;
string message = first.ToString() + second.ToString();
Console.WriteLine(message);

//Convert a string to an int using the Parse() helper method

string first3 = "5";
string second3 = "7";
int sum = int.Parse(first3) + int.Parse(second3);
Console.WriteLine(sum);

//Convert a string to a int using the Convert class

string value1 = "5";
string value2 = "7";
int result = Convert.ToInt32(value1) * Convert.ToInt32(value2);
Console.WriteLine(result);

//casting and converting a decimal into an int

int value4 = (int)1.5m; // casting truncates
Console.WriteLine(value4);

int value5 = Convert.ToInt32(1.5m); // converting rounds up
Console.WriteLine(value5);

//TryParse() method

string name = "Bob";
//Console.WriteLine(int.Parse(name)); //error

string value6 = "102";
int result6 = 0;
if (int.TryParse(value6, out result6))
{
    Console.WriteLine($"Measurement: {result}");
}
else
{
    Console.WriteLine("Unable to report the measurement.");
}

string value7 = "bad";
int result7 = 0;
if (int.TryParse(value7, out result7))
{
    Console.WriteLine($"Measurement: {result7}");
}
else
{
    Console.WriteLine("Unable to report the measurement.");
}

if (result7 > 0)
    Console.WriteLine($"Measurement (w/ offset): {50 + result7}");

//an example

string[] values = { "12.3", "45", "ABC", "11", "DEF" };

decimal total = 0m;
string message8 = "";

foreach (var value in values)
{
    decimal number; // stores the TryParse "out" value
    if (decimal.TryParse(value, out number))
    {
        total += number;
    }
    else
    {
        message8 += value;
    }
}

Console.WriteLine($"Message: {message8}");
Console.WriteLine($"Total: {total}");

//another example

int value9 = 11;
decimal value10 = 6.2m;
float value11 = 4.3f;

// The Convert class is best for converting the fractional decimal numbers into whole integer numbers
// Convert.ToInt32() rounds up the way you would expect.
int result9 = Convert.ToInt32(value9 / value10);
Console.WriteLine($"Divide value1 by value2, display the result as an int: {result9}");

decimal result10 = value10 / (decimal)value11;
Console.WriteLine($"Divide value2 by value3, display the result as a decimal: {result10}");

float result11 = value11 / value9;
Console.WriteLine($"Divide value3 by value1, display the result as a float: {result11}");



