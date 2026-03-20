using System;

class Interface_Segregation_Refactor
{
    // Problem: refactor fat IMachine into smaller interfaces
    public interface IPrinter { void Print(string doc); }
    public interface IScanner { string Scan(); }
    public interface IFax { void Fax(string doc); }

    public class AllInOneMachine : IPrinter, IScanner, IFax
    {
        public void Print(string doc) => Console.WriteLine($"Printing: {doc}");
        public string Scan() { Console.WriteLine("Scanning..."); return "scanned-content"; }
        public void Fax(string doc) => Console.WriteLine($"Faxing: {doc}");
    }

    public class SimplePrinter : IPrinter
    {
        public void Print(string doc) => Console.WriteLine($"Simple printing: {doc}");
    }

    static void Main()
    {
        IPrinter p = new SimplePrinter();
        p.Print("Hello");

        var multi = new AllInOneMachine();
        ((IScanner)multi).Scan();

        // Interface segregation reduces required surface for implementers and improves decoupling/testability.
    }
}