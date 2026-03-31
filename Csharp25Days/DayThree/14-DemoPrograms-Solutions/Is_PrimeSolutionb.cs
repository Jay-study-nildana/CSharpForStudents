// a list of 20 numbers in a array generated randomly between 1 and 100

Random random = new Random();
int[] numbers = new int[20];
for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = random.Next(1, 101); // Generates a random number between 1 and 100
}
Console.WriteLine("Generated numbers:");
foreach (int number in numbers)
{
    Console.WriteLine(number);
}

var listofoddnumbers = numbers.Where(n => n % 2 != 0).ToList();

// Print the odd numbers

Console.WriteLine("\nOdd numbers:");
Console.WriteLine(
    string.Join(", ", listofoddnumbers)
);

//let's get a list of prime numbers from the generated numbers

var listofprimenumbers = numbers.Where(n => IsPrime(n)).ToList();

// Print the prime numbers

Console.WriteLine("\nPrime numbers:");

Console.WriteLine(
    string.Join(", ", listofprimenumbers)
);

//implementing the IsPrime method

static bool IsPrime(int number)
    {
    if (number <= 1) return false;
    if (number == 2) return true;
    if (number % 2 == 0) return false;
    for (int i = 3; i <= Math.Sqrt(number); i += 2)
    {
        if (number % i == 0) return false;
    }
    return true;
}