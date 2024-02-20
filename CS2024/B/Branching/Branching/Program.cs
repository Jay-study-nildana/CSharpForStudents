// See https://aka.ms/new-console-template for more information

//1.Branching
//   1.If
//   1.If Else
//   1. Else If
//   1. Nested If Else Else If
//   1. Switch Case

Console.WriteLine("Hello, World!");

var SomeNumber = 10;

if (SomeNumber == 10)
{
    Console.WriteLine("The nubmer is 10");
}


if (SomeNumber == 10)
{
    Console.WriteLine("The nubmer is 10");
}
else
{
    Console.WriteLine("Number is not 10");
}

if (SomeNumber < 10)
{
    Console.WriteLine("The nubmer is less than 10");
}
else if(SomeNumber == 10)
{
    Console.WriteLine("The nubmer is 10");
}
else
{
    Console.WriteLine("something else happened");
    var SomeNextNumber = 30;
    if(SomeNextNumber == 30)
    {
        Console.WriteLine("the value is 30");
    }
    else
    {
        Console.WriteLine("the value is NOT 30");
    }
}

var SomeOtherNumber = 20;

switch(SomeOtherNumber)
{
    case 10: Console.WriteLine("The number has become 10");
        break;
    case 20:
        Console.WriteLine("The number has become 10");
        break;
    case 30:
        Console.WriteLine("The number has become 10");
        break;
    default: Console.WriteLine("something did not work");
        break;
}