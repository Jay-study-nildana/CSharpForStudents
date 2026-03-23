int numberone = 42;
int numbertwo = 52;

void SwapTheseTwoNumbersUsingReference(ref int numberone,ref int numbertwo)
{
    int temp = numberone;
    numberone = numbertwo;
    numbertwo = temp;
}

Console.WriteLine($" Before Swap : {numberone} and {numbertwo}");

SwapTheseTwoNumbersUsingReference(ref numberone, ref numbertwo);

Console.WriteLine($" After Swap : {numberone} and {numbertwo}");