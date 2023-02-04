// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//variables

int a = 10;
int b = 20;

//operators

int c = a + b;

//if else

if (a > b)
{
    Console.WriteLine("a is greater than b");
}
else
{
    Console.WriteLine("b is greater than a");
}

//for loop

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

//while loop

int i = 0;

while (i < 10)
{
    Console.WriteLine(i);
    i++;
}

//do while loop

int i = 0;

do
{
    Console.WriteLine(i);
    i++;
} while (i < 10);

//switch case

int i = 1;

switch (i)
{
    case 1:
        Console.WriteLine("i is 1");
        break;
    case 2:
        Console.WriteLine("i is 2");
        break;
    default:
        Console.WriteLine("i is not 1 or 2");
        break;
}

//arrays

int[] arr = new int[5];

arr[0] = 1;

arr[1] = 2;

arr[2] = 3;

arr[3] = 4;

arr[4] = 5;

//foreach loop

foreach (int i in arr)
{
    Console.WriteLine(i);
}

//methods

public static void MyMethod()
{
    Console.WriteLine("Hello World!");
}

//classes

public class MyClass
{
    public int x = 5;
}

//objects

MyClass myObj = new MyClass();

Console.WriteLine(myObj.x);

//inheritance

public class Vehicle
{
    public string brand = "Ford";
    public void honk()
    {
        Console.WriteLine("Tuut, tuut!");
    }
}

public class Car : Vehicle
{
    public string modelName = "Mustang";
}

//polymorphism

public class Animal
{
    public virtual void animalSound()
    {
        Console.WriteLine("The animal makes a sound");
    }
}

public class Pig : Animal
{
    public override void animalSound()
    {
        Console.WriteLine("The pig says: wee wee");
    }
}

//encapsulation

public class Car
{
    private string model = "Mustang";

    public string Model
    {
        get { return model; }
        set { model = value; }
    }
}

//abstraction

public abstract class Animal
{
    public abstract void animalSound();
}

public class Pig : Animal
{
    public override void animalSound()
    {
        Console.WriteLine("The pig says: wee wee");
    }
}

//interface


public interface IFirstInterface
{
    void myMethod();
}

public interface ISecondInterface
{
    void myOtherMethod();
}

public class DemoClass : IFirstInterface, ISecondInterface
{
    public void myMethod()
    {
        Console.WriteLine("Some text..");
    }
    public void myOtherMethod()
    {
        Console.WriteLine("Some other text...");
    }
}

//exception handling

try
{
    int[] myNumbers = { 1, 2, 3 };
    Console.WriteLine(myNumbers[10]);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

//file handling

using System.IO;

string text = "Hello World!";

File.WriteAllText("filename.txt", text);

string text = File.ReadAllText("filename.txt");

Console.WriteLine(text);

//generics

public class GenericList<T>
{
    public void Add(T input) { }
}

//delegates

public delegate void MyDelegate();

public class Program
{
    public static void Main()
    {
        MyDelegate del = new MyDelegate(Method1);
        del();
    }

    public static void Method1()
    {
        Console.WriteLine("Method1");
    }
}

//events

public delegate void MyEventHandler();

public class MyEvent
{
    public event MyEventHandler MyEventName;

    public void OnMyEvent()
    {
        if (MyEventName != null)
        {
            MyEventName();
        }
    }
}

public class Program
{
    public static void Main()
    {
        MyEvent evt = new MyEvent();
        evt.MyEventName += new MyEventHandler(Method1);
        evt.OnMyEvent();
    }

    public static void Method1()
    {
        Console.WriteLine("Method1");
    }
}

//lambda expressions

Func<int, int> square = x => x * x;

Console.WriteLine(square(5));

//linq

int[] numbers = { 5, 10, 8, 3, 6, 12 };

var result = numbers.Where(x => x > 5);

foreach (int x in result)
{
    Console.WriteLine(x);
}

//extension methods

public static class MyExtensions
{
    public static int WordCount(this string str)
    {
        return str.Split(new char[] { ' ', '.', '?' },
            StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

//multithreading

using System.Threading;

public class Program
{
    public static void Main()
    {
        ThreadStart threadStart = new ThreadStart(Method1);
        Thread thread = new Thread(threadStart);
        thread.Start();
    }

    public static void Method1()
    {
        Console.WriteLine("Method1");
    }
}

//asynchronous programming


using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        Method1();
        Console.WriteLine("Method1");
    }

    public static async void Method1()
    {
        await Task.Run(() => Method2());
        Console.WriteLine("Method2");
    }

    public static void Method2()
    {
        Console.WriteLine("Method2");
    }
}

//reflection


using System.Reflection;

public class Program
{
    public static void Main()
    {
        Type type = typeof(string);

        Console.WriteLine(type.FullName);
    }
}

//attributes


using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]

public class HelpAttribute : System.Attribute
{
    public readonly string Url;

    public string Topic
    {
        get { return topic; }
        set { topic = value; }
    }

    public HelpAttribute(string url)
    {
        this.Url = url;
    }

    private string topic;
}


[HelpAttribute("Information on the class MyClass")]

class MyClass
{
}

public class Program
{
    public static void Main()
    {
        Type type = typeof(MyClass);

        object[] attributes = type.GetCustomAttributes(true);

        for (int i = 0; i < attributes.Length; i++)
        {
            Console.WriteLine(attributes[i]);
        }
    }
}

//dynamic

dynamic name = "John";

Console.WriteLine(name);

//anonymous types

var myAnonymousType = new { Name = "John", Age = 18 };

Console.WriteLine(myAnonymousType.Name);

//nullable types

int? x = null;

Console.WriteLine(x.GetValueOrDefault());

//null-coalescing operator

int? x = null;

int y = x ?? -1;

Console.WriteLine(y);

//null-conditional operator

string name = null;

int? length = name?.Length;

Console.WriteLine(length);

//string interpolation

string name = "John";

Console.WriteLine($"Hello, {name}!");

//nameof operator

string name = "John";

Console.WriteLine(name);

//async/await

using System.Threading.Tasks;


public class Program
{
    public static void Main()
    {
        Method1();
        Console.WriteLine("Method1");
    }

    public static async void Method1()
    {
        await Task.Run(() => Method2());
        Console.WriteLine("Method2");
    }

    public static void Method2()
    {
        Console.WriteLine("Method2");
    }
}

//indexers

public class Person
{
    private string[] names = new string[size];
    static public int size = 10;

    public Person()
    {
        for (int i = 0; i < size; i++)
        {
            names[i] = "N. N.";
        }
    }

    public string this[int index]
    {
        get
        {
            string tmp;
            if (index >= 0 && index <= size - 1)
            {
                tmp = names[index];
            }
            else
            {
                tmp = "";
            }
            return tmp;
        }
        set
        {
            if (index >= 0 && index <= size - 1)
            {
                names[index] = value;
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        Person person = new Person();

        person[0] = "John";
        person[1] = "Bill";

        Console.WriteLine(person[0]);
        Console.WriteLine(person[1]);
    }
}

//iterators

public class Program
{
    public static void Main()
    {
        foreach (int i in Power(2, 8))
        {
            Console.WriteLine(i);
        }
    }

    public static IEnumerable<int> Power(int number, int exponent)
    {
        int result = 1;

        for (int i = 0; i < exponent; i++)
        {
            result = result * number;
            yield return result;
        }
    }
}

//generics

public class MyClass<T>
{
    public void Method1(T arg)
    {
        Console.WriteLine(arg);
    }
}

public class Program
{
    public static void Main()
    {
        MyClass<int> my = new MyClass<int>();
        my.Method1(5);
    }
}

//partial classes

public partial class MyClass
{
    public void Method1()
    {
        Console.WriteLine("Method1");
    }
}

public partial class MyClass
{
    public void Method2()
    {
        Console.WriteLine("Method2");
    }
}

public class Program
{
    public static void Main()
    {
        MyClass my = new MyClass();
        my.Method1();
        my.Method2();
    }
}

//partial methods

public partial class MyClass
{
    partial void Method1();

    public void Method2()
    {
        Method1();
    }
}

public partial class MyClass
{
    partial void Method1()
    {
        Console.WriteLine("Method1");
    }
}

public class Program
{
    public static void Main()
    {
        MyClass my = new MyClass();
        my.Method2();
    }
}

//extension methods

public static class MyExtensions
{
    public static int WordCount(this string str)
    {
        return str.Split(new char[] { ' ', '.', '?' },
            StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

public class Program
{
    public static void Main()
    {
        string str = "Hello, world!";

        Console.WriteLine(str.WordCount());
    }
}

//LINQ

using System;

using System.Linq;


public class Program
{
    public static void Main()
    {
        int[] numbers = { 2, 3, 4, 5 };

        var result = numbers.Select(x => x * x);

        foreach (int i in result)
        {
            Console.WriteLine(i);
        }
    }
}








