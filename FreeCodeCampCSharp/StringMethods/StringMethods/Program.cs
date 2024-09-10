// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//IndexOf() and Substring() helper methods

string message = "Find what is (inside the parentheses)";

int openingPosition = message.IndexOf('(');
int closingPosition = message.IndexOf(')');

Console.WriteLine(openingPosition);
Console.WriteLine(closingPosition);

string message2 = "Find what is (inside the parentheses)";

int openingPosition2 = message2.IndexOf('(');
int closingPosition2 = message2.IndexOf(')');

// Console.WriteLine(openingPosition);
// Console.WriteLine(closingPosition);

int length = closingPosition2 - openingPosition2;
Console.WriteLine(message.Substring(openingPosition2, length));

string message3 = "Find what is (inside the parentheses)";

int openingPosition3 = message3.IndexOf('(');
int closingPosition3 = message3.IndexOf(')');

openingPosition3 += 1;

int length3 = closingPosition3 - openingPosition3;
Console.WriteLine(message3.Substring(openingPosition3, length3));

string message4 = "What is the value <span>between the tags</span>?";

int openingPosition4 = message4.IndexOf("<span>");
int closingPosition4 = message4.IndexOf("</span>");

openingPosition4 += 6;
int length4 = closingPosition4 - openingPosition4;
Console.WriteLine(message4.Substring(openingPosition4, length4));


string message5 = "What is the value <span>between the tags</span>?";

const string openSpan5 = "<span>";
const string closeSpan5 = "</span>";

int openingPosition5 = message5.IndexOf(openSpan5);
int closingPosition5 = message5.IndexOf(closeSpan5);

openingPosition5 += openSpan5.Length;
int length5 = closingPosition5 - openingPosition5;
Console.WriteLine(message5.Substring(openingPosition5, length5));

//IndexOf() and LastIndexOf() helper methods

//string message6 = "(What if) I have [different symbols] but every {open symbol} needs a [matching closing symbol]?";

//// The IndexOfAny() helper method requires a char array of characters. 
//// You want to look for:

//char[] openSymbols6 = { '[', '{', '(' };

//// You'll use a slightly different technique for iterating through 
//// the characters in the string. This time, use the closing 
//// position of the previous iteration as the starting index for the 
////next open symbol. So, you need to initialize the closingPosition 
//// variable to zero:

//int closingPosition6 = 0;

//while (true)
//{
//    int openingPosition6 = message6.IndexOfAny(openSymbols6, closingPosition6);

//    if (openingPosition6 == -1) break;

//    string currentSymbol6 = message6.Substring(openingPosition6, 1);

//    // Now  find the matching closing symbol
//    char matchingSymbol6 = ' ';

//    switch (currentSymbol6)
//    {
//        case "[":
//            matchingSymbol6 = ']';
//            break;
//        case "{":
//            matchingSymbol6 = '}';
//            break;
//        case "(":
//            matchingSymbol6 = ')';
//            break;
//    }

//    // To find the closingPosition, use an overload of the IndexOf method to specify 
//    // that the search for the matchingSymbol should start at the openingPosition in the string. 

//    openingPosition6 += 1;
//    closingPosition6 = message6.IndexOf(matchingSymbol6, openingPosition6);

//    // Finally, use the techniques you've already learned to display the sub-string:

//    int length6 = closingPosition6 - openingPosition6;
//    Console.WriteLine(message.Substring(openingPosition6, length6)); //crashes here, why? 
//}

// Remove() and Replace() methods

string data = "12345John Smith          5000  3  ";
string updatedData = data.Remove(5, 20);
Console.WriteLine(updatedData);

string message7 = "This--is--ex-amp-le--da-ta";
message = message7.Replace("--", " ");
message = message7.Replace("-", "");
Console.WriteLine(message7);

//another example

const string input = "<div><h2>Widgets &trade;</h2><span>5000</span></div>";

string quantity = "";
string output = "";

// Your work here

// Extract the "quantity"
const string openSpan = "<span>";
const string closeSpan = "</span>";

int quantityStart = input.IndexOf(openSpan) + openSpan.Length; // + length of <span> so index at end of <span> tag
int quantityEnd = input.IndexOf(closeSpan);
int quantityLength = quantityEnd - quantityStart;
quantity = input.Substring(quantityStart, quantityLength);
quantity = $"Quantity: {quantity}";

// Set output to input, replacing the trademark symbol with the registered trademark symbol
const string tradeSymbol = "&trade;";
const string regSymbol = "&reg;";
output = input.Replace(tradeSymbol, regSymbol);

// Remove the opening <div> tag
const string openDiv = "<div>";
int divStart = output.IndexOf(openDiv);
output = output.Remove(divStart, openDiv.Length);

// Remove the closing </div> tag and add "Output:" to the beginning
const string closeDiv = "</div>";
int divCloseStart = output.IndexOf(closeDiv);
output = "Output: " + output.Remove(divCloseStart, closeDiv.Length);

Console.WriteLine(quantity);
Console.WriteLine(output);




