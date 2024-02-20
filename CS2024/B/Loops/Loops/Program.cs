// See https://aka.ms/new-console-template for more information

//1.Loops
//   1.While
//   1.Do While
//   1. For Loop
//   1. Break
//   1. Continue
//   1. Nested Loops with For Loop
//   1. Goto

Console.WriteLine("Hello, World!");

var counter = 5;

while (counter > 0)
{
    Console.WriteLine("Value of counter is " + counter);
    counter--;
}

counter = 5;  //resetting the counter
Console.WriteLine("Counter has been reset for new loop");

do
{
    Console.WriteLine("Value of counter is " + counter);
    counter--;

} while (counter > 0);

counter = 5;
Console.WriteLine("Counter has been reset for new loop");

while (counter > 0)
{
    if (counter == 1)
    {
        Console.WriteLine("break activated. skipped at counter " + counter);
        break;  //output will not have 1 as the loop has been broken
    }    

    Console.WriteLine("Value of counter is " + counter);
    counter--;
}

counter = 5;
Console.WriteLine("Counter has been reset for new loop");
while (counter > 0)
{
    counter--;
    if (counter == 1)
    {
        Console.WriteLine("Continue activated. current iteration restarts");
        continue;  //TODO. let's dig deeper into it
    }
    Console.WriteLine("Value of counter is " + counter);

}

for(var i=0;i<5;i++)
{
    Console.WriteLine("value of i " + i);
}

for (var i = 0; i < 5; i++)
{
    for(var j=0;j<5;j++)
    {
        Console.WriteLine("value of i " + i + "value of j is " + j);
    }

}

goto labelforgoto;

Console.WriteLine("Just some random sentence which will be skipped");  //not running because of goto jump

labelforgoto:

Console.WriteLine("Goto is working");

