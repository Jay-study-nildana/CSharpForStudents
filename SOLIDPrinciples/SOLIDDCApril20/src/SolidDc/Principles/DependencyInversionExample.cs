using System;

namespace SolidDc.Principles
{
    // Dependency Inversion Principle (DIP)
    // Example: High-level `Hero` classes depend on abstractions (`IWeapon`) not concrete weapons.
    public static class DependencyInversionExample
    {
        // Abstraction for weapons
        public interface IWeapon { void Use(); }

        // Concrete weapons
        public class Batarang : IWeapon { public void Use() => Console.WriteLine("Batarang thrown — target disabled."); }
        public class KryptoniteRock : IWeapon { public void Use() => Console.WriteLine("Kryptonite rock used — bad idea for Superman."); }

        // High-level module depends on IWeapon abstraction
        public class Hero
        {
            private readonly IWeapon _weapon;
            public string Name { get; }
            public Hero(string name, IWeapon weapon) { Name = name; _weapon = weapon; }

            public void Attack()
            {
                Console.WriteLine($"{Name} attacks:");
                _weapon.Use();
            }
        }

        public static void Run()
        {
            Console.WriteLine("--- Dependency Inversion Principle (DIP) — DC Example ---\n");

            // Inject different weapons (concrete implementations) via abstraction
            var batman = new Hero("Batman", new Batarang());
            batman.Attack();

            var (kryptonite, superman) = (new KryptoniteRock(), new Hero("Traitor", new KryptoniteRock()));
            superman.Attack();

            Console.WriteLine("\nExplanation: High-level modules depend on abstractions, not concrete implementations.");
        }
    }
}
