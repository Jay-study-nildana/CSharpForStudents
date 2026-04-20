using System;

namespace SolidDc.Principles
{
    // Open/Closed Principle (OCP)
    // Example: Gotham vehicles can gain new attack behaviors without
    // modifying existing code by using abstractions (strategy pattern).
    public static class OpenClosedExample
    {
        // Attack strategy abstraction
        public interface IAttack
        {
            void Execute();
        }

        // Concrete attack: Batarang
        public class BatarangAttack : IAttack { public void Execute() => Console.WriteLine("Batarang thrown!"); }

        // Concrete attack: Grapple hook strike (new behavior added without changing Batmobile)
        public class GrappleAttack : IAttack { public void Execute() => Console.WriteLine("Grapple hook strike!"); }

        // Batmobile depends on abstraction IAttack; adding new attacks extends behavior
        public class Batmobile
        {
            private readonly IAttack _attack;
            public Batmobile(IAttack attack) { _attack = attack; }
            public void PerformAttack() => _attack.Execute();
        }

        public static void Run()
        {
            Console.WriteLine("--- Open/Closed Principle (OCP) — DC Example ---\n");
            Console.WriteLine("Batmobile can use different attacks; add new attacks without changing Batmobile.");

            var batmobileWithBatarang = new Batmobile(new BatarangAttack());
            batmobileWithBatarang.PerformAttack();

            // Later we add GrappleAttack without modifying Batmobile — open for extension, closed for modification
            var batmobileWithGrapple = new Batmobile(new GrappleAttack());
            batmobileWithGrapple.PerformAttack();

            Console.WriteLine("\nExplanation: `Batmobile` depends on `IAttack`. New attacks implement `IAttack`.");
        }
    }
}
