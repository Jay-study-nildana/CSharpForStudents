using System;

class Equality_Record_vs_Class
{
    public class PersonClass
    {
        public string Name { get; }
        public int Age { get; }
        public PersonClass(string name, int age) { Name = name; Age = age; }
    }

    public record PersonRecord(string Name, int Age);

    static void Main()
    {
        var pc1 = new PersonClass("Sam", 30);
        var pc2 = new PersonClass("Sam", 30);
        Console.WriteLine($"PersonClass reference equals: {pc1.Equals(pc2)}"); // default: reference equals -> false

        var pr1 = new PersonRecord("Sam", 30);
        var pr2 = new PersonRecord("Sam", 30);
        Console.WriteLine($"PersonRecord value equals: {pr1 == pr2}"); // true (value equality)

        Console.WriteLine("Use records when you want value-based equality (DTOs, value objects). Use class when identity matters.");
    }
}