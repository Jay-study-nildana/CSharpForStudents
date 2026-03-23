//using System;

//class Max_and_Index
//{
//    // Find maximum value and its first index
//    // Control flow: for (need index)
//    // Time: O(n), Space: O(1)
//    static (int maxValue, int index) FindMaxAndIndex(int[] numbers)
//    {
//        if (numbers == null || numbers.Length == 0)
//            throw new ArgumentException("Array must be non-empty");

//        int maxVal = numbers[0];
//        int idx = 0;
//        for (int i = 1; i < numbers.Length; i++)
//        {
//            if (numbers[i] > maxVal)
//            {
//                maxVal = numbers[i];
//                idx = i;
//            }
//        }
//        return (maxVal, idx);
//    }

//    static void Main()
//    {
//        int[] a = { 3, 7, 2, 7, 5 };
//        var (max, i) = FindMaxAndIndex(a);
//        Console.WriteLine($"{max} at {i}"); // "7 at 1"
//    }
//}


int[] listofnumbers = { 1, 2, 3, 4, 5, 6, 7, 8 };

for (int i = 0; i < listofnumbers.Length; i++)
{
    Console.WriteLine($" Number : {listofnumbers[i]}  Location : {i}");
    
}
Console.WriteLine("-----");

var response = GetMax(listofnumbers);

Console.WriteLine($" Max Number : {response.maxvalue} Location : {response.location}");

Random random = new Random();
int[] listofnumbers2 = { 
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        };

for (int i = 0; i < listofnumbers2.Length; i++)
{
    Console.WriteLine($" Number : {listofnumbers2[i]}  Location : {i}");

}
Console.WriteLine("-----");

var response2 = GetMax(listofnumbers2);

Console.WriteLine($" Max Number : {response2.maxvalue} Location : {response2.location}");

static (int location,int maxvalue) GetMax(int[] listofnumbers)
{
    int location = 0;
    int maxvalue = 0;

    for(int i=0;i<listofnumbers.Length;i++)
    {
        if (listofnumbers[i] > maxvalue)
        {
            maxvalue = listofnumbers[i];
            location = i;
        }
    }

    return (location, maxvalue);
}