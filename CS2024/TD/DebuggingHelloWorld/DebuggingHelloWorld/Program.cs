// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

int result = Fibonacci(5);
Console.WriteLine(result);

int result2 = FibonacciFinal(5);
Console.WriteLine(result2);

LoggingStuff();

int result3 = IntegerDivide(5,2);
int result4 = IntegerDivide(5, 0);

int result5 = FibonacciFinalWithDebugging(5);

//does not work correctly. fix it with debugging

static int Fibonacci(int n)
{
    int n1 = 0;
    int n2 = 1;
    int sum;

    for (int i = 2; i < n; i++)
    {
        sum = n1 + n2;
        n1 = n2;
        n2 = sum;
    }

    return n == 0 ? n1 : n2;
}

static int FibonacciFinal(int n)
{
    int n1 = 0;
    int n2 = 1;
    int sum;

    for (int i = 2; i <= n; i++)
    {
        sum = n1 + n2;
        n1 = n2;
        n2 = sum;
    }

    return n == 0 ? n1 : n2;
}

static void LoggingStuff()
{
    Console.WriteLine("This message is readable by the end user.");
    Trace.WriteLine("This is a trace message when tracing the app.");
    Debug.WriteLine("This is a debug message just for developers.");

    Debug.Write("Debug - ");
    Debug.WriteLine("This is a full line.");
    Debug.WriteLine("This is another full line.");

    int count = 0; 

    Debug.WriteLineIf(count == 0, "The count is 0 and this may cause an exception.");

    bool errorFlag = false;
    System.Diagnostics.Trace.WriteIf(errorFlag, "Error in AppendData procedure.");
    System.Diagnostics.Debug.WriteIf(errorFlag, "Transaction abandoned.");
    System.Diagnostics.Trace.Write("Invalid value for data request");
}

int IntegerDivide(int dividend, int divisor)
{
    Debug.Assert(divisor != 0, $"{nameof(divisor)} is 0 and will cause an exception.");

    if (divisor == 0)
        return 0;
    
    return dividend / divisor;
}

static int FibonacciFinalWithDebugging(int n)
{
    Debug.WriteLine($"Entering {nameof(Fibonacci)} method");
    Debug.WriteLine($"We are looking for the {n}th number");

    int n1 = 0;
    int n2 = 1;
    int sum;

    for (int i = 2; i <= n; i++)
    {
        sum = n1 + n2;
        n1 = n2;
        n2 = sum;

        Debug.WriteLineIf(sum == 1, $"sum is 1, n1 is {n1}, n2 is {n2}");
    }

    // If n2 is 5 continue, else break.
    Debug.Assert(n2 == 5, "The return value is not 5 and it should be.");
    return n == 0 ? n1 : n2;
}

