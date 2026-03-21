using System;
using System.Collections.Generic;
using System.Linq;

//
// Problem: Characterization Tests & Incremental Refactor
// Test plan: run assertions to capture legacy behavior, then refactor and rerun assertions (still pass).
// Demonstrates: characterization tests to guard refactor.
//

namespace Day11.RefactorLab
{
    // Legacy function: calculates discount in a single method (to be refactored)
    public static class LegacyPricing
    {
        public static decimal ComputeTotal(decimal unitPrice, int qty)
        {
            var subtotal = unitPrice * qty;
            if (qty >= 10) subtotal *= 0.9m; // 10% bulk discount
            // apply admin fee for low totals
            if (subtotal < 50) subtotal += 2.5m;
            return subtotal;
        }
    }

    // Simple assert helper
    public static class Assert
    {
        public static void AreEqual<T>(T expected, T actual, string message = "")
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                throw new InvalidOperationException($"Assert Failed: {message} Expected={expected} Actual={actual}");
        }
    }

    // Characterization tests capturing legacy behavior
    public static class CharacterizationTests
    {
        public static void Run()
        {
            Assert.AreEqual(20m, LegacyPricing.ComputeTotal(10m, 2), "2 items no discount");
            Assert.AreEqual(90m, LegacyPricing.ComputeTotal(10m, 10), "10 items with discount");
            Assert.AreEqual(27.5m, LegacyPricing.ComputeTotal(10m, 3), "3 items with admin fee");
            Console.WriteLine("Characterization tests passed (legacy behavior captured).");
        }
    }

    // After refactor: introduce PricingStrategy but keep behavior same
    public interface IPricingCalculator { decimal ComputeTotal(decimal unitPrice, int qty); }
    public class RefactoredPricing : IPricingCalculator
    {
        public decimal ComputeTotal(decimal unitPrice, int qty)
        {
            var subtotal = unitPrice * qty;
            if (qty >= 10) subtotal *= 0.9m;
            if (subtotal < 50) subtotal += 2.5m;
            return subtotal;
        }
    }

    class Program
    {
        static void Main()
        {
            // Run characterization tests before refactor (simulated here)
            CharacterizationTests.Run();

            // Use refactored calculator and ensure same results
            var calc = new RefactoredPricing();
            Assert.AreEqual(20m, calc.ComputeTotal(10m, 2));
            Assert.AreEqual(90m, calc.ComputeTotal(10m, 10));
            Assert.AreEqual(27.5m, calc.ComputeTotal(10m, 3));
            Console.WriteLine("Refactored implementation passes characterization tests.");
        }
    }
}