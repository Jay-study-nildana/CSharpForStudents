// See https://aka.ms/new-console-template for more information
using ArrayGeneratorHelper;

Console.WriteLine("Hello, World!");

//JustSomeBasics();
//DisplayArrayUsingForLooop();
//SomeMultiDimensionalArrayStuff();
//SomeJaggedArrayStuff();
//SomeObjectArrayStuff();
//SomeArrayMethodsStuff();



void JustSomeBasics()
{

    int[] SomeIntegerArray = [69, 6, 9];
    int[] SomeIntegerArray2 = new int[3] { 69, 6, 9 };
    string[] SomeStringArray = ["Batman", "Superman", "Wonder Woman"];
    string[] SomeStringArray2 = new string[3] { "Batman", "Superman", "Wonder Woman" };

    //show the integers

    Console.WriteLine("Some Integer : " + SomeIntegerArray[0]);
    Console.WriteLine("Some Integer : " + SomeIntegerArray[1]);
    Console.WriteLine("Some Integer : " + SomeIntegerArray[2]);

    Console.WriteLine("Some Integer : " + SomeIntegerArray2[0]);
    Console.WriteLine("Some Integer : " + SomeIntegerArray2[1]);
    Console.WriteLine("Some Integer : " + SomeIntegerArray2[2]);


    //show all the strings

    Console.WriteLine("Some Array : " + SomeStringArray[0]);
    Console.WriteLine("Some Array : " + SomeStringArray[1]);
    Console.WriteLine("Some Array : " + SomeStringArray[2]);

    Console.WriteLine("Some Array : " + SomeStringArray2[0]);
    Console.WriteLine("Some Array : " + SomeStringArray2[1]);
    Console.WriteLine("Some Array : " + SomeStringArray2[2]);

}

//for loop and foreach
void DisplayArrayUsingForLooop()
{

    GenerateArrayUsingRandom tempGenerateArrayUsingRandom = new GenerateArrayUsingRandom();

    var tempStringArray = tempGenerateArrayUsingRandom.GenerateSuperHeroes(10);
    var tempIntegerArray = tempGenerateArrayUsingRandom.GenerateRandomNumbers(10);

    Console.WriteLine("Showing String Array using For Loop");

    for(int i=0; i<tempStringArray.Length; i++)
    {
        Console.WriteLine(tempStringArray[i]);
    }

    Console.WriteLine("Showing Integer Array using For Loop");

    for (int i=0;i<tempIntegerArray.Length; i++)
    {
        Console.WriteLine(tempIntegerArray[i]);
    }

    Console.WriteLine("Showing String Array using For Each Loop");

    foreach (var x in  tempStringArray)
    {
        Console.WriteLine(x);
    }

    Console.WriteLine("Showing Integer Array using For Each Loop");

    foreach (var x in tempIntegerArray)
    { 
        Console.WriteLine(x); 
    }
}

void SomeMultiDimensionalArrayStuff()
{
    int[,] SomeIntegerArray = new int[5, 5];
    int[,] SomeIntegerArray2 = new int[2, 2]
    {
        { 69,59 },
        { 19,29 }
    };
    string[,] SomeStringArray3 = new string[5, 5];
    string[,] SomeStringArray4 = new string[2, 2]
    {
        {"One","Two" },
        { "Three","Four"}
    };

    Console.WriteLine(" Here is a number from the multi dimensional array : " + SomeIntegerArray2[0,0]);
    Console.WriteLine(" Here is another number from the multi dimensional array : " + SomeIntegerArray2[1, 0]);

    Console.WriteLine("Here is a string from the multi dimensional array : " + SomeStringArray4[0, 0]);
    Console.WriteLine("Here is another string from the multi dimensional array : " + SomeStringArray4[1, 0]);


}

void SomeJaggedArrayStuff()
{
    int[][] someIntegerArray = new int[5][];
    someIntegerArray[0] = new int[2]; //this has a different size
    someIntegerArray[1] = new int[5];//this has a different size. 
    someIntegerArray[2] = new int[10];//this has a different size.

    //different sizes make it jagged. like jagged edges. 

    someIntegerArray[0][0] = 69;
    someIntegerArray[0][1] = 39;
    someIntegerArray[2][9] = 109;

    Console.WriteLine("a number from the jagged array : " + someIntegerArray[0][0]);
    Console.WriteLine("another number from the jagged array : " + someIntegerArray[0][1]);
    Console.WriteLine("another number from the jagged array : " + someIntegerArray[2][9]);

}


void SomeObjectArrayStuff()
{
    SomeClass[] SomeClassArray= new SomeClass[5];
    SomeClassArray[0] = new SomeClass { SomeInteger = 69, SomeString = "Did you Miss Me? " };
    SomeClassArray[4] = new SomeClass { SomeInteger = 9, SomeString = "Yes, you did " };
    
    for(int i = 0;i < SomeClassArray.Length;i++)
    {
        //SomeClassArray[i].DisplaySomeClass(); //here, it will crash because, some objects are not initialized. //uncomment this line to see the crash.
    }

    //initialize remaining objects in the array
    SomeClassArray[1] = new SomeClass();
    SomeClassArray[2] = new SomeClass();
    SomeClassArray[3] = new SomeClass();

    for (int i = 0; i < SomeClassArray.Length; i++)
    {
        SomeClassArray[i].DisplaySomeClass(); //here, it will not crash. all index items have been intialized.
    }

    //let's do some copying
    SomeClass[] AnotherClassArray = new SomeClass[5];
    SomeClassArray.CopyTo(AnotherClassArray,0);

    for (int i = 0; i < AnotherClassArray.Length; i++)
    {
        AnotherClassArray[i].DisplaySomeClass(); 
    }

    //lets do some cloning

    //var AttackOfTheClones = SomeClassArray.Clone(); //this is also fine, but it will be an Object Array Clone. you will have to do mapping later.
    var AttackOfTheClones = (SomeClass[])SomeClassArray.Clone(); //this maps into the target class right away

    for (int i = 0; i < AnotherClassArray.Length; i++)
    {
        AttackOfTheClones[i].DisplaySomeClass();
    }

    //TODO. dig deeper into this deep copy and shallow copy. 
    //TODO. difference between CopyTo and Clone as well. 
}

void SomeArrayMethodsStuff()
{
    int[] SomeIntegerArray2 = new int[3] { 69, 6, 9 };
    string[] SomeStringArray = ["Batman", "Superman", "Wonder Woman"];

    var checkFor69 = Array.IndexOf(SomeIntegerArray2, 69);
    Console.WriteLine("The index/location of the number 69 is " + checkFor69);

    var checkForBatman = Array.IndexOf(SomeStringArray, "Batman");
    Console.WriteLine("The index/location of Batman is " + checkForBatman);

    var checkFor9 = Array.IndexOf(SomeIntegerArray2, 9);
    Console.WriteLine("The index/location of the number 9 is " + checkFor9);

    var checkForSuperman = Array.IndexOf(SomeStringArray, "Superman");
    Console.WriteLine("The index/location of Superman is " + checkForSuperman);

    var locationusingBinarySearch = Array.BinarySearch(SomeIntegerArray2, 9);
    Console.WriteLine("Location of 9 found using Binary Search : " + locationusingBinarySearch);

    //lets use the Clear option

    GenerateArrayUsingRandom tempGenerateArrayUsingRandom = new GenerateArrayUsingRandom();

    var tempStringArray = tempGenerateArrayUsingRandom.GenerateSuperHeroes(10);
    var tempIntegerArray = tempGenerateArrayUsingRandom.GenerateRandomNumbers(10);

    Console.WriteLine("Before using Clear method");

    foreach( var i in tempStringArray )
    {
        Console.WriteLine(i);
    }

    foreach(var i in tempIntegerArray )
    { 
        Console.WriteLine(i);
    }

    //let's clear some parts of the array
    Array.Clear(tempStringArray, 2, 5); //clear 5 elements from the 2nd position
    Array.Clear(tempIntegerArray, 4, 4); //clear 4 elements from the 4th position

    Console.WriteLine("After using Clear method");

    foreach (var i in tempStringArray)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray)
    {
        Console.WriteLine(i);
    }

    //lets use Resize
    var tempStringArray2 = tempGenerateArrayUsingRandom.GenerateSuperHeroes(10);
    var tempIntegerArray2 = tempGenerateArrayUsingRandom.GenerateRandomNumbers(10);

    Console.WriteLine("Before using Resize method");

    foreach (var i in tempStringArray2)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray2)
    {
        Console.WriteLine(i);
    }

    Array.Resize(ref tempStringArray2, 2);  //resize till 2nd position
    Array.Resize(ref tempIntegerArray2, 4); //resize till 4th position

    Console.WriteLine("After using Resize method");

    foreach (var i in tempStringArray2)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray2)
    {
        Console.WriteLine(i);
    }

    //lets do some sorting

    var tempStringArray3 = tempGenerateArrayUsingRandom.GenerateSuperHeroes(10);
    var tempIntegerArray3 = tempGenerateArrayUsingRandom.GenerateRandomNumbers(10);

    Console.WriteLine("Before using Sort method");

    foreach (var i in tempStringArray3)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray3)
    {
        Console.WriteLine(i);
    }

    Array.Sort(tempStringArray3);
    Array.Sort(tempIntegerArray3);

    Console.WriteLine("After using Sort method");

    foreach (var i in tempStringArray3)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray3)
    {
        Console.WriteLine(i);
    }

    Console.WriteLine("Before using Reverse method");

    foreach (var i in tempStringArray3)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray3)
    {
        Console.WriteLine(i);
    }

    Array.Reverse(tempStringArray3);
    Array.Reverse(tempIntegerArray3);

    Console.WriteLine("After using Reverse method");

    foreach (var i in tempStringArray3)
    {
        Console.WriteLine(i);
    }

    foreach (var i in tempIntegerArray3)
    {
        Console.WriteLine(i);
    }



}

class SomeClass
{
    public int SomeInteger { set; get; }
    public string SomeString { set; get; }

    public void DisplaySomeClass()
    {
        Console.WriteLine(" Some Class Number : " + this.SomeInteger);
        Console.WriteLine(" Some Class String : " + this.SomeString);
    }
}