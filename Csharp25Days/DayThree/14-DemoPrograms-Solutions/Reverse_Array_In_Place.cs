using System;

class Reverse_Array_In_Place
{
    // Reverse array in place using two-index for-loop
    // Control flow: for with i and j
    // Time: O(n), Space: O(1)
    static void Reverse(int[] numbers)
    {
        int i = 0, j = numbers.Length - 1;
        while (i < j)
        {
            int tmp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = tmp;
            i++; j--;
        }
    }

    static void Main()
    {
        int[] a = { 1, 2, 3, 4 };
        Reverse(a);
        Console.WriteLine(string.Join(", ", a)); // 4, 3, 2, 1
    }
}