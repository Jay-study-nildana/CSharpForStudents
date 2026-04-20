using System;

namespace SolidDc.Principles
{
    // Interface Segregation Principle (ISP)
    // Example: Avoid a monolithic IHero interface; split into focused interfaces.
    public static class InterfaceSegregationExample
    {
        // Bad: one big interface forcing all heroes to implement everything
        public interface IHeroBad
        {
            void Fly();
            void Fight();
            void HackComputer();
        }

        // Better: small focused interfaces
        public interface IFly { void Fly(); }
        public interface ICombat { void Fight(); }
        public interface ITech { void HackComputer(); }

        // Batman can fight and hack, but not fly — he only implements the interfaces he needs
        public class Batman : ICombat, ITech
        {
            public void Fight() => Console.WriteLine("Batman fights with gadgets and skill.");
            public void HackComputer() => Console.WriteLine("Batman hacks the system using the Batcomputer.");
        }

        // Superman can fly and fight
        public class Superman : IFly, ICombat
        {
            public void Fly() => Console.WriteLine("Superman flies across Metropolis!");
            public void Fight() => Console.WriteLine("Superman uses super-strength.");
        }

        public static void Run()
        {
            Console.WriteLine("--- Interface Segregation Principle (ISP) — DC Example ---\n");
            Console.WriteLine("We split big interfaces into small, role-focused interfaces.");

            var batman = new Batman();
            batman.Fight();
            batman.HackComputer();

            var superman = new Superman();
            superman.Fly();
            superman.Fight();

            Console.WriteLine("\nExplanation: Consumers depend only on the interfaces they use.");
        }
    }
}
