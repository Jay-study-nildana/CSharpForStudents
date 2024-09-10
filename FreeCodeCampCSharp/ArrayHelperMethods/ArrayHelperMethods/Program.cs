// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Discover Sort() and Reverse()

string[] pallets = { "B14", "A11", "B12", "A13" };

Console.WriteLine("Sorted...");
Array.Sort(pallets);
foreach (var pallet in pallets)
{
    Console.WriteLine($"-- {pallet}");
}

Console.WriteLine("");
Console.WriteLine("Reversed...");
Array.Reverse(pallets);
foreach (var pallet in pallets)
{
    Console.WriteLine($"-- {pallet}");
}

//Explore Clear() and Resize()

string[] pallets2 = { "B14", "A11", "B12", "A13" };
Console.WriteLine("");

Array.Clear(pallets, 0, 2);
Console.WriteLine($"Clearing 2 ... count: {pallets2.Length}");
foreach (var pallet in pallets2)
{
    Console.WriteLine($"-- {pallet}");
}

Console.WriteLine("");
Array.Resize(ref pallets2, 6);
Console.WriteLine($"Resizing 6 ... count: {pallets2.Length}");

// pallets[4] = "C01"; // error
// pallets[5] = "C02"; // error

foreach (var pallet in pallets2)
{
    Console.WriteLine($"-- {pallet}");
}

Console.WriteLine("");
Array.Resize(ref pallets2, 3);
Console.WriteLine($"Resizing 3 ... count: {pallets2.Length}");

foreach (var pallet in pallets2)
{
    Console.WriteLine($"-- {pallet}");
}

//Discover Split() and Join()

string value = "abc123";
char[] valueArray = value.ToCharArray();
Array.Reverse(valueArray);
// string result = new string(valueArray);
string result = String.Join(",", valueArray);
Console.WriteLine(result);

string[] items = result.Split(',');
foreach (string item in items)
{
    Console.WriteLine(item);
}

//example

string pangram = "The quick brown fox jumps over the lazy dog";

// Step 1
string[] message = pangram.Split(' ');

//Step 2
string[] newMessage = new string[message.Length];

// Step 3
for (int i = 0; i < message.Length; i++)
{
    char[] letters = message[i].ToCharArray();
    Array.Reverse(letters);
    newMessage[i] = new string(letters);
}

//Step 4
string result3 = String.Join(" ", newMessage);
Console.WriteLine(result3);

//another example

string orderStream = "B123,C234,A345,C15,B177,G3003,C235,B179";
string[] items2 = orderStream.Split(',');
Array.Sort(items2);

foreach (var item in items2)
{
    if (item.Length == 4)
    {
        Console.WriteLine(item);
    }
    else
    {
        Console.WriteLine(item + "\t- Error");
    }
}



