// MulticastDelegateOrder.cs
// Solution: demonstrate multicast delegates, invocation order, and removal.

using System;

namespace Day19.Solutions
{
    public static class MulticastDelegateOrder
    {
        public static void Run()
        {
            Action handlers = null;

            Action a = () => Console.WriteLine("Handler A");
            Action b = () => Console.WriteLine("Handler B");
            Action c = () => Console.WriteLine("Handler C");

            handlers += a;
            handlers += b;
            handlers += c;

            Console.WriteLine("Invoke all handlers (A,B,C):");
            handlers?.Invoke();

            // Remove B
            handlers -= b;
            Console.WriteLine("After removing B (A,C):");
            handlers?.Invoke();

            // Note: for delegates returning values only the last return value is observed
        }
    }
}