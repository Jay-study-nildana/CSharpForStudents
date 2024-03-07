using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsProvider
{
    public class SuperHero2 : SuperHero
    {
        public string Brand {  get; set; }

        //hiding the base class display function
        public new void DisplayHeroDetails()
        {
            base.DisplayHeroDetails();
            Console.WriteLine(" Brand : " + Brand);
        }
    }
}
