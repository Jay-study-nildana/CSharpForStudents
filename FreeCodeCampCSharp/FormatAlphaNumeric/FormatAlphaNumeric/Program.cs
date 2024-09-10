// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//string formatting basics

//Composite Formatting?

string first = "Hello";
string second = "World";
Console.WriteLine("{1} {0}!", first, second);
Console.WriteLine("{0} {0} {0}!", first, second);

//string interpolation?

string first2 = "Hello";
string second2 = "World";
Console.WriteLine($"{first2} {second2}!");
Console.WriteLine($"{second2} {first2}!");
Console.WriteLine($"{first2} {first2} {first2}!");

//Formatting currency

decimal price = 123.45m;
int discount = 50;
Console.WriteLine($"Price: {price:C} (Save {discount:C})");

//Formatting numbers

decimal measurement = 123456.78912m;
Console.WriteLine($"Measurement: {measurement:N} units");

//decimal measurement = 123456.78912m;
//Console.WriteLine($"Measurement: {measurement:N4} units"); //more precision

//Formatting percentages

decimal tax = .36785m;
Console.WriteLine($"Tax rate: {tax:P2}");

//Combining formatting approaches

decimal price2 = 67.55m;
decimal salePrice2 = 59.99m;

string yourDiscount = String.Format("You saved {0:C2} off the regular {1:C2} price. ", (price2 - salePrice2), price2);

yourDiscount += $"A discount of {((price2 - salePrice2) / price2):P2}!"; //inserted
Console.WriteLine(yourDiscount);

//example

int invoiceNumber = 1201;
decimal productShares = 25.4568m;
decimal subtotal = 2750.00m;
decimal taxPercentage = .15825m;
decimal total = 3185.19m;

Console.WriteLine($"Invoice Number: {invoiceNumber}");
Console.WriteLine($"   Shares: {productShares:N3} Product");
Console.WriteLine($"     Sub Total: {subtotal:C}");
Console.WriteLine($"           Tax: {taxPercentage:P2}");
Console.WriteLine($"     Total Billed: {total:C}");

//padding and alignment

string first3 = "Hello";
string second3 = "World";
string result3 = string.Format("{0} {1}!", first3, second3);
Console.WriteLine(result3);

string input = "Pad this";
Console.WriteLine(input.PadLeft(12));

Console.WriteLine(input.PadRight(12));

string paymentId = "769C";
string payeeName = "Mr. Stephen Ortega";
string paymentAmount = "$5,000.00";

var formattedLine = paymentId.PadRight(6);
formattedLine += payeeName.PadRight(24);
formattedLine += paymentAmount.PadLeft(10);

Console.WriteLine("1234567890123456789012345678901234567890");
Console.WriteLine(formattedLine);

//example

string customerName = "Ms. Barros";

string currentProduct = "Magic Yield";
int currentShares = 2975000;
decimal currentReturn = 0.1275m;
decimal currentProfit = 55000000.0m;

string newProduct = "Glorious Future";
decimal newReturn = 0.13125m;
decimal newProfit = 63000000.0m;

Console.WriteLine($"Dear {customerName},");
Console.WriteLine($"As a customer of our {currentProduct} offering we are excited to tell you about a new financial product that would dramatically increase your return.\n");
Console.WriteLine($"Currently, you own {currentShares:N} shares at a return of {currentReturn:P}.\n");
Console.WriteLine($"Our new product, {newProduct} offers a return of {newReturn:P}.  Given your current volume, your potential profit would be {newProfit:C}.\n");

Console.WriteLine("Here's a quick comparison:\n");

string comparisonMessage = "";

comparisonMessage = currentProduct.PadRight(20);
comparisonMessage += String.Format("{0:P}", currentReturn).PadRight(10);
comparisonMessage += String.Format("{0:C}", currentProfit).PadRight(20);

comparisonMessage += "\n";
comparisonMessage += newProduct.PadRight(20);
comparisonMessage += String.Format("{0:P}", newReturn).PadRight(10);
comparisonMessage += String.Format("{0:C}", newProfit).PadRight(20);

Console.WriteLine(comparisonMessage);
