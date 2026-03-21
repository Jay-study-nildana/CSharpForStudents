// 07_UnitTests_For_Strategy_Template.cs
// Lightweight console "unit tests" for strategies and template method behavior.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08.Tests07
{
    static class Assert
    {
        public static void True(bool cond, string msg = "") { if (!cond) throw new Exception("Assert.True failed: " + msg); }
        public static void AreEqual<T>(T expected, T actual, string msg = "") { if (!EqualityComparer<T>.Default.Equals(expected, actual)) throw new Exception($"Assert.AreEqual failed. Expected={expected}, Actual={actual}. {msg}"); }
    }

    // Minimal strategy and template to test
    public interface IPriceStrategy { decimal ApplyDiscount(decimal p); }
    public class TenPercent : IPriceStrategy { public decimal ApplyDiscount(decimal p) => Math.Round(p * 0.9m, 2); }

    public abstract class BaseProcessor
    {
        public List<string> Log = new();
        public void Process() { Log.Add("Validate"); Validate(); Log.Add("Calc"); Calc(); Log.Add("Finalize"); FinalizeStep(); }
        protected abstract void Validate();
        protected virtual void Calc() => Log.Add("CalcDefault");
        protected virtual void FinalizeStep() => Log.Add("FinalizeDefault");
    }
    public class ConcreteProcessor : BaseProcessor
    {
        protected override void Validate() => Log.Add("ValidateConcrete");
        protected override void Calc() { Log.Add("CalcConcrete"); }
    }

    class Program
    {
        static void Main()
        {
            // Strategy test
            var s = new TenPercent();
            var r = s.ApplyDiscount(100m);
            Assert.AreEqual(90m, r, "TenPercent should reduce 100 to 90");

            // Template test
            var p = new ConcreteProcessor();
            p.Process();
            Assert.True(p.Log.SequenceEqual(new[] { "Validate", "ValidateConcrete", "Calc", "CalcConcrete", "Finalize", "FinalizeDefault" }), "Template method steps mismatch");

            Console.WriteLine("All lightweight tests passed.");
        }
    }
}