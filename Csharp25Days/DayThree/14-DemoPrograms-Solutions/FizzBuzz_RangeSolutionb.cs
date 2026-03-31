Console.WriteLine("Give a number until which you want the Fizz Buzz");

var userinput = Console.ReadLine();

var propernumberrecieved = int.TryParse(userinput, out int fizzbuzzlimitnumber);

if(propernumberrecieved == false)
{
    Console.WriteLine("Number is not correct. Unable to proceed");
    return;
}

for(int i =0;i<=fizzbuzzlimitnumber;i++)
{
    if( i % 3 ==0 && i% 5 == 0)
    {
        Console.WriteLine(" You got a Fizzzzzz....Buzzzzzz" + "Number is ...." + i);
        

    }
    else if(i% 5 == 0)
    {
        Console.WriteLine(" You got a Buzzzzz...." + "Number is ...." + i);
        
    }
    else if(i%3==0)
    {
        Console.WriteLine(" You got a Fizz....." + "Number is ...." + i);
    }
}