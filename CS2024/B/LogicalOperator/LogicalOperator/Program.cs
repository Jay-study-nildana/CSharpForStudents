// See https://aka.ms/new-console-template for more information

//1.Logical Operators
//      1. Logical And &
//      1. Conditional And &&
//      1. Logical Or |
//      1. Conditional Or ||
//      1. Logical Exclusive OR ^
//      1. Negation Operator !

Console.WriteLine("Hello, World!");

var SomeBooleanOne = true;
var SomeBooleanTwo = true;

if(SomeBooleanOne & SomeBooleanTwo)
{
    Console.WriteLine("Both are true");
}

if (SomeBooleanOne && SomeBooleanTwo)
{
    Console.WriteLine("Both are true");
}

SomeBooleanTwo = false;
if (SomeBooleanOne | SomeBooleanTwo)
{
    Console.WriteLine("One or both of them is true");
}

if (SomeBooleanOne || SomeBooleanTwo)
{
    Console.WriteLine("One or both of them is true");
}

if (SomeBooleanOne ^ SomeBooleanTwo)
{
    Console.WriteLine("Exactly one of them is true");
}

SomeBooleanTwo = true;
if (SomeBooleanOne ^ SomeBooleanTwo)
{
    Console.WriteLine("Exactly one of them is true");
}
else
{
    Console.WriteLine("Looks like two of them are true");
}

var SomeBooleanThree = !SomeBooleanOne;

Console.WriteLine(SomeBooleanOne);
Console.WriteLine(SomeBooleanThree);

