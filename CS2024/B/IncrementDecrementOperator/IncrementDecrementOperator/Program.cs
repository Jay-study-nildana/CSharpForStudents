// See https://aka.ms/new-console-template for more information

//1.Increment and Decrement Operators
//      1. Increment Post a++
//      1. Increment Pre ++a
//      1. Decrement Post a--
//      1. Decrement Pre --a

Console.WriteLine("Hello, World!");

var FirstNumber = 10;
var SecondNumber = 0;

SecondNumber = FirstNumber++;
Console.WriteLine(FirstNumber);
Console.WriteLine(SecondNumber);
SecondNumber = ++FirstNumber;
Console.WriteLine(FirstNumber);
Console.WriteLine(SecondNumber);
SecondNumber = FirstNumber--;
Console.WriteLine(FirstNumber);
Console.WriteLine(SecondNumber);
SecondNumber =--FirstNumber;
Console.WriteLine(FirstNumber);
Console.WriteLine(SecondNumber);
