// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;

Console.WriteLine("Hello, World!");

//Code blocks and variable scope
bool flag = true;
if (flag)
{
    int value = 10;
    Console.WriteLine($"Inside the code block: {value}");
}
// Console.WriteLine($"Outside the code block: {value}"); //error. value has no scope here

//readability exercise

//first way

//string name = "steve";
//if (name == "bob") Console.WriteLine("Found Bob");
//else if (name == "steve") Console.WriteLine("Found Steve");
//else Console.WriteLine("Found Chuck");

//second way

//string name = "steve";

//if (name == "bob")
//    Console.WriteLine("Found Bob");
//else if (name == "steve")
//    Console.WriteLine("Found Steve");
//else
//    Console.WriteLine("Found Chuck");

//example

int[] numbers = { 4, 8, 15, 16, 23, 42 };
int total = 0;
bool found = false;

foreach (int number in numbers)
{
    total += number;
    if (number == 42)
        found = true;
}

if (found)
    Console.WriteLine("Set contains 42");

Console.WriteLine($"Total: {total}");