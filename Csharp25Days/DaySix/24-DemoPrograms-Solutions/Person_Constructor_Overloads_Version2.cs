using System;

class Person_Constructor_Overloads
{
    // Person demonstrates overloaded constructors and constructor chaining.
    public class Person
    {
        public string Name { get; }
        public int Age { get; }
        public DateTime DateOfBirth { get; }

        public Person(string name) : this(name, 0, DateTime.MinValue) { }

        public Person(string name, int age) : this(name, age, DateTime.MinValue) { }

        public Person(string name, int age, DateTime dateOfBirth)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (age < 0) throw new ArgumentOutOfRangeException(nameof(age));
            Age = age;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString() => $"{Name}, Age={Age}, DOB={DateOfBirth:d}";
    }

    static void Main()
    {
        var p1 = new Person("Sam");
        var p2 = new Person("Jo", 29);
        var p3 = new Person("Lee", 40, new DateTime(1986, 5, 1));
        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
        // Constructor chaining reduces duplication and centralizes validation.
    }
}