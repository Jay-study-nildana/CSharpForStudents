using System;

namespace IteratorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IteratorPattern Main Begins");

            var villainCollection = new VillainCollection();

            foreach(var x in villainCollection)
            {
                x.DisplayVillain();
            }

            Console.WriteLine("IteratorPattern Main Ends");
        }
    }
}
