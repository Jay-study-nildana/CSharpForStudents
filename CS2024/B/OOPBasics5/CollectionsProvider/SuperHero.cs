using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsProvider
{
    public class SuperHero
    {
        public string Name { get; set; }
        public string AlterEgo { get; set; }
        public int NumberHero { get; set; }

        public void DisplayHeroDetails()
        {
            Console.WriteLine(" Name : " +  Name);
            Console.WriteLine(" Alter Ego : " +  AlterEgo);
            Console.WriteLine(" Number : " +  NumberHero);
        }
    }
}
