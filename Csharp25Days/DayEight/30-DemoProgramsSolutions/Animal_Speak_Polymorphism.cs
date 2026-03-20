using System;
using System.Collections.Generic;

class Animal_Speak_Polymorphism
{
    public class Animal
    {
        public virtual string Speak() => "(silence)";
    }

    public class Dog : Animal
    {
        public override string Speak() => "Woof!";
    }

    public class Cat : Animal
    {
        public override string Speak() => "Meow!";
    }

    public class Bird : Animal
    {
        public override string Speak() => "Tweet!";
    }

    static void Main()
    {
        var animals = new List<Animal> { new Dog(), new Cat(), new Bird(), new Animal() };
        foreach (var a in animals)
        {
            Console.WriteLine($"{a.GetType().Name} says: {a.Speak()}");
        }

        // Polymorphism lets us treat all animals uniformly while dispatching to derived behavior.
    }
}