// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Declare a new array

string[] fraudulentOrderIDs = new string[3];

fraudulentOrderIDs[0] = "A123";
fraudulentOrderIDs[1] = "B456";
fraudulentOrderIDs[2] = "C789";

//index that is out of bounds of the array
//fraudulentOrderIDs[3] = "D000"; //gives error

//Retrieve values from elements of an array

Console.WriteLine($"First: {fraudulentOrderIDs[0]}");
Console.WriteLine($"Second: {fraudulentOrderIDs[1]}");
Console.WriteLine($"Third: {fraudulentOrderIDs[2]}");

fraudulentOrderIDs[0] = "F000";

Console.WriteLine($"Reassign First: {fraudulentOrderIDs[0]}");

// Initialize an array

string[] fraudulentOrderIDs2 = ["A123", "B456", "C789"];

string[] fraudulentOrderIDs3 = { "A123", "B456", "C789" };

//Length property of an array

Console.WriteLine($"There are {fraudulentOrderIDs.Length} fraudulent orders to process.");

//Implement the foreach statement

string[] names = { "Rowena", "Robin", "Bao" };
foreach (string name in names)
{
    Console.WriteLine(name);
}

//int[] inventory = { 200, 450, 700, 175, 250 };
//int sum = 0;
//foreach (int items in inventory)
//{
//    sum += items;
//}

//Console.WriteLine($"We have {sum} items in inventory.");


int[] inventory = { 200, 450, 700, 175, 250 };
int sum = 0;
int bin = 0;
foreach (int items in inventory)
{
    sum += items;
    bin++;
    Console.WriteLine($"Bin {bin} = {items} items (Running total: {sum})");
}
Console.WriteLine($"We have {sum} items in inventory.");

//challenge code

string[] orderIDs = { "B123", "C234", "A345", "C15", "B177", "G3003", "C235", "B179" };

foreach (string orderID in orderIDs)
{
    if (orderID.StartsWith("B"))
    {
        Console.WriteLine(orderID);
    }
}





