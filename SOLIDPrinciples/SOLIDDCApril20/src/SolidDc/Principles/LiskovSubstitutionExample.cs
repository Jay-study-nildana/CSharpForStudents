using System;

namespace SolidDc.Principles
{
    // Liskov Substitution Principle (LSP)
    // We present a small example where a subclass violates LSP and then
    // how to fix it so that derived classes remain substitutable.
    public static class LiskovSubstitutionExample
    {
        // Base class: GadgetUser expects to be able to UseGadget safely
        public class GadgetUser
        {
            public virtual void UseGadget(string gadget)
            {
                if (string.IsNullOrEmpty(gadget)) throw new ArgumentException("gadget required");
                Console.WriteLine($"Using gadget: {gadget}");
            }
        }

        // Bad subclass: throws for some inputs — violates LSP
        public class AnxiousRobin : GadgetUser
        {
            public override void UseGadget(string gadget)
            {
                // Violates the contract of the base class by adding a stricter precondition
                if (gadget.Contains("bat")) throw new InvalidOperationException("Robin won't use bat-things!");
                base.UseGadget(gadget);
            }
        }

        // Fixed approach: use composition or ensure subclass honors the base contract
        public class BraveRobin : GadgetUser
        {
            public override void UseGadget(string gadget)
            {
                // Honours base contract: throws only when gadget is null/empty
                base.UseGadget(gadget);
            }
        }

        public static void Run()
        {
            Console.WriteLine("--- Liskov Substitution Principle (LSP) — DC Example ---\n");
            Console.WriteLine("Demonstration of violation: a subclass adds stricter rules, breaking substitutability.");

            GadgetUser robin = new AnxiousRobin();
            try
            {
                Console.WriteLine("Trying AnxiousRobin with 'batarang'");
                robin.UseGadget("batarang");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error (LSP violation): {ex.Message}");
            }

            Console.WriteLine("\nFixed: use BraveRobin which respects base contract");
            robin = new BraveRobin();
            robin.UseGadget("batarang");

            Console.WriteLine("\nExplanation: Subtypes must be usable wherever base types are used.");
        }
    }
}
