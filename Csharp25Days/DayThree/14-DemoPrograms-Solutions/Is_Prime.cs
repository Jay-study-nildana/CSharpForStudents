using System;

class Is_Prime
{
    // Check primality using loop up to sqrt(n)
    // Control flow: for with early return
    // Time: O(sqrt(n)), Space: O(1)
    static bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0) return false;
        int i = 3;
        while (i * i <= n)
        {
            if (n % i == 0) return false;
            i += 2; // skip even divisors
        }
        return true;
    }

    static void Main()
    {
        Console.WriteLine(IsPrime(2));   // True
        Console.WriteLine(IsPrime(15));  // False
        Console.WriteLine(IsPrime(97));  // True
    }
}