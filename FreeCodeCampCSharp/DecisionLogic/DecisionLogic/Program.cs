// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Random dice = new Random();

//int roll1 = dice.Next(1, 7);
//int roll2 = dice.Next(1, 7);
//int roll3 = dice.Next(1, 7);

//int total = roll1 + roll2 + roll3;

//Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");

//if statement to display different messages based on the value of the total variable

//if (total > 14)
//{
//    Console.WriteLine("You win!");
//}

//if (total < 15)
//{
//    Console.WriteLine("Sorry, you lose.");
//}

//Boolean expression?

string message = "The quick brown fox jumps over the lazy dog.";
bool result = message.Contains("dog");
Console.WriteLine(result);

if (message.Contains("fox"))
{
    Console.WriteLine("What does the fox say?");
}

//if statement to implement the doubles bonus

//if ((roll1 == roll2) || (roll2 == roll3) || (roll1 == roll3))
//{
//    Console.WriteLine("You rolled doubles! +2 bonus to total!");
//    total += 2;
//}

//final version

//Random dice = new Random();

//int roll1 = dice.Next(1, 7);
//int roll2 = dice.Next(1, 7);
//int roll3 = dice.Next(1, 7);

//int total = roll1 + roll2 + roll3;

//Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");

//if ((roll1 == roll2) || (roll2 == roll3) || (roll1 == roll3))
//{
//    Console.WriteLine("You rolled doubles! +2 bonus to total!");
//    total += 2;
//}

//if ((roll1 == roll2) && (roll2 == roll3))
//{
//    Console.WriteLine("You rolled triples! +6 bonus to total!");
//    total += 6;
//}

//if (total >= 15)
//{
//    Console.WriteLine("You win!");
//}

//if (total < 15)
//{
//    Console.WriteLine("Sorry, you lose.");
//}

//final version part 2

Random dice = new Random();

int roll1 = dice.Next(1, 7);
int roll2 = dice.Next(1, 7);
int roll3 = dice.Next(1, 7);

int total = roll1 + roll2 + roll3;

Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");

if ((roll1 == roll2) || (roll2 == roll3) || (roll1 == roll3))
{
    if ((roll1 == roll2) && (roll2 == roll3))
    {
        Console.WriteLine("You rolled triples!  +6 bonus to total!");
        total += 6;
    }
    else
    {
        Console.WriteLine("You rolled doubles!  +2 bonus to total!");
        total += 2;
    }

    Console.WriteLine($"Your total including the bonus: {total}");
}

if (total >= 16)
{
    Console.WriteLine("You win a new car!");
}
else if (total >= 10)
{
    Console.WriteLine("You win a new laptop!");
}
else if (total == 7)
{
    Console.WriteLine("You win a trip for two!");
}
else
{
    Console.WriteLine("You win a kitten!");
}

//challenge

Random random = new Random();
int daysUntilExpiration = random.Next(12);
int discountPercentage = 0;

if (daysUntilExpiration == 0)
{
    Console.WriteLine("Your subscription has expired.");
}
else if (daysUntilExpiration == 1)
{
    Console.WriteLine("Your subscription expires within a day!");
    discountPercentage = 20;
}
else if (daysUntilExpiration <= 5)
{
    Console.WriteLine($"Your subscription expires in {daysUntilExpiration} days.");
    discountPercentage = 10;
}
else if (daysUntilExpiration <= 10)
{
    Console.WriteLine("Your subscription will expire soon. Renew now!");
}

if (discountPercentage > 0)
{
    Console.WriteLine($"Renew now and save {discountPercentage}%.");
}