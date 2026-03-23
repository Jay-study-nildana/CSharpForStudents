//Suppose we want to do a recursive summation.

int summationtillnumber = 10;

int RecursiveSummation(int n)
{
    if (n<=0)
    {
        return 0;
    }
    else
    {
        return n + RecursiveSummation(n - 1);
    }
}

int resultofsummation = RecursiveSummation(summationtillnumber);
Console.WriteLine("The result of summation : " + resultofsummation);