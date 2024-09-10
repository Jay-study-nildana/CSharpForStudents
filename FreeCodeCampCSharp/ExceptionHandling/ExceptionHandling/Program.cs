// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");

try
{
    // try code block - code that may generate an exception
}
catch
{
    // catch code block - code to handle an exception
}
finally
{
    // finally code block - code to clean up resources
}


try
{
    // Step 1: code execution begins
    try
    {
        // Step 2: an exception occurs here
    }
    finally
    {
        // Step 4: the system executes the finally code block associated with the try statement where the exception occurred
    }

}
catch // Step 3: the system finds a catch clause that can handle the exception
{
    // Step 5: the system transfers control to the first line of the catch code block
}

//string[] names = { "Dog", "Cat", "Fish" };
//Object[] objs = (Object[])names;

//Object obj = (Object)13;
//objs[2] = obj; // ArrayTypeMismatchException occurs

//int number1 = 3000;
//int number2 = 0;
//Console.WriteLine(number1 / number2); // DivideByZeroException occurs

//int valueEntered;
//string userValue = "two";
//valueEntered = int.Parse(userValue); // FormatException occurs

//int[] values1 = { 3, 6, 9, 12, 15, 18, 21 };
//int[] values2 = new int[6];

//values2[values1.Length - 1] = values1[values1.Length - 1]; // IndexOutOfRangeException occurs

//object obj2 = "This is a string";
//int num = (int)obj2; //InvalidCastException

//int[] values = null;
//for (int i = 0; i <= 9; i++)
//    values[i] = i * 2; //NullReferenceException

//string? lowCaseString = null;
//Console.WriteLine(lowCaseString.ToUpper());//NullReferenceException

//decimal x = 400;
//byte i;

//i = (byte)x; // OverflowException occurs
//Console.WriteLine(i);

double float1 = 3000.0;
double float2 = 0.0;
int number1 = 3000;
int number2 = 0;

try
{
    Console.WriteLine(float1 / float2);
    Console.WriteLine(number1 / number2);
}
catch
{
    Console.WriteLine("An exception has been caught");
}

try
{
    Process1();
}
catch
{
    Console.WriteLine("An exception has occurred");
}

Console.WriteLine("Exit program");

static void Process1()
{
    WriteMessage();
}

static void WriteMessage()
{
    double float1 = 3000.0;
    double float2 = 0.0;
    int number1 = 3000;
    int number2 = 0;

    Console.WriteLine(float1 / float2);
    Console.WriteLine(number1 / number2);
}

//another example

try
{
    Process12();
}
catch
{
    Console.WriteLine("An exception has occurred");
}

Console.WriteLine("Exit program");

static void Process12()
{
    try
    {
        WriteMessage2();
    }
    //Catch a specific exception type
    catch (DivideByZeroException ex)
    {
        Console.WriteLine($"Exception caught in Process1: {ex.Message}");
    }
    catch //catch (Exception ex)
    {
        Console.WriteLine("Exception caught in Process1");
        //Access the properties of an exception object
        //Console.WriteLine($"Exception caught in Process1: {ex.Message}");
    }

}

static void WriteMessage2()
{
    double float1 = 3000.0;
    double float2 = 0.0;
    int number1 = 3000;
    int number2 = 0;

    Console.WriteLine(float1 / float2);
    Console.WriteLine(number1 / number2);
}

//Catch separate exception types in a code block

// inputValues is used to store numeric values entered by a user
string[] inputValues = new string[] { "three", "9999999999", "0", "2" };

foreach (string inputValue in inputValues)
{
    int numValue = 0;
    try
    {
        numValue = int.Parse(inputValue);
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid readResult. Please enter a valid number.");
    }
    catch (OverflowException)
    {
        Console.WriteLine("The number you entered is too large or too small.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//another example

checked
{
    try
    {
        int num1 = int.MaxValue;
        int num2 = int.MaxValue;
        int result = num1 + num2;
        Console.WriteLine("Result: " + result);
    }
    catch (OverflowException ex)
    {
        Console.WriteLine("Error: The number is too large to be represented as an integer. " + ex.Message);
    }
}

try
{
    string? str = null;
    int length = str.Length;
    Console.WriteLine("String Length: " + length);
}
catch (NullReferenceException ex)
{
    Console.WriteLine("Error: The reference is null. " + ex.Message);
}

try
{
    int[] numbers = new int[5];
    numbers[5] = 10;
    Console.WriteLine("Number at index 5: " + numbers[5]);
}
catch (IndexOutOfRangeException ex)
{
    Console.WriteLine("Error: Index out of range. " + ex.Message);
}

try
{
    int num3 = 10;
    int num4 = 0;
    int result2 = num3 / num4;
    Console.WriteLine("Result: " + result2);
}
catch (DivideByZeroException ex)
{
    Console.WriteLine("Error: Cannot divide by zero. " + ex.Message);
}

Console.WriteLine("Exiting program.");

