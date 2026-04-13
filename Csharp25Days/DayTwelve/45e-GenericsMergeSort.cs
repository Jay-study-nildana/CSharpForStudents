using System;
using System.Collections.Generic;

var intA = new List<int> { 1, 3, 5 };
var intB = new List<int> { 2, 4, 6 };
var doubleA = new List<double> { 1.1, 3.3, 5.5 };
var doubleB = new List<double> { 2.2, 4.4, 6.6 };
var charA = new List<char> { 'a', 'c', 'e' };
var charB = new List<char> { 'b', 'd', 'f' };
var stringA = new List<string> { "apple", "orange" };
var stringB = new List<string> { "banana", "pear" };
var imagA = new List<ImaginaryNumber> { new ImaginaryNumber(1), new ImaginaryNumber(3) };
var imagB = new List<ImaginaryNumber> { new ImaginaryNumber(2), new ImaginaryNumber(4) };

//show the input list for integer

Console.WriteLine("Int list A: " + string.Join(", ", intA));
Console.WriteLine("Int list B: " + string.Join(", ", intB));

Console.WriteLine("Merged ints: " + string.Join(", ", MergeSorted(intA, intB, null)));

//show the input list for double
Console.WriteLine("Double list A: " + string.Join(", ", doubleA));
Console.WriteLine("Double list B: " + string.Join(", ", doubleB));

Console.WriteLine("Merged doubles: " + string.Join(", ", MergeSorted(doubleA, doubleB, null)));

//show the input list for char
Console.WriteLine("Char list A: " + string.Join(", ", charA));
Console.WriteLine("Char list B: " + string.Join(", ", charB));

Console.WriteLine("Merged chars: " + string.Join(", ", MergeSorted(charA, charB, null)));

//show the input list for string
Console.WriteLine("String list A: " + string.Join(", ", stringA));
Console.WriteLine("String list B: " + string.Join(", ", stringB));

Console.WriteLine("Merged strings: " + string.Join(", ", MergeSorted(stringA, stringB, null)));

//show the input list for imaginary numbers
Console.WriteLine("Imaginary list A: " + string.Join(", ", imagA));
Console.WriteLine("Imaginary list B: " + string.Join(", ", imagB));

Console.WriteLine("Merged imaginary numbers: " + string.Join(", ", MergeSorted(imagA, imagB, new ImaginaryComparer())));

List<T> MergeSorted<T>(IList<T> a, IList<T> b, IComparer<T> comparer)
{
    comparer ??= Comparer<T>.Default;
    int i = 0, j = 0;
    var outList = new List<T>();
    while (i < a.Count && j < b.Count)
    {
        if (comparer.Compare(a[i], b[j]) <= 0) outList.Add(a[i++]);
        else outList.Add(b[j++]);
    }
    while (i < a.Count) outList.Add(a[i++]);
    while (j < b.Count) outList.Add(b[j++]);
    return outList;
}

public struct ImaginaryNumber
{
    public double Imaginary { get; }
    public ImaginaryNumber(double imaginary) => Imaginary = imaginary;
    public override string ToString() => $"{Imaginary}i";
}

public class ImaginaryComparer : IComparer<ImaginaryNumber>
{
    public int Compare(ImaginaryNumber x, ImaginaryNumber y) => x.Imaginary.CompareTo(y.Imaginary);
}



