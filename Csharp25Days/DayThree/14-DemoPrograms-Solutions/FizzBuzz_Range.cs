using System;

class FizzBuzz_Range
{
    // Classic FizzBuzz from 1..N
    // Control flow: for with ordered if/else if (checks multiples)
    // Time: O(N), Space: O(1)
    static void FizzBuzz(int N)
    {
        for (int i = 1; i <= N; i++)
        {
            if (i % 15 == 0) Console.WriteLine("FizzBuzz");
            else if (i % 3 == 0) Console.WriteLine("Fizz");
            else if (i % 5 == 0) Console.WriteLine("Buzz");
            else Console.WriteLine(i);
        }
    }

    static void Main()
    {
        FizzBuzz(16);
    }
}