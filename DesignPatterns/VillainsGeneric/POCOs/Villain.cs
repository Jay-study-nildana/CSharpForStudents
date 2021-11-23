using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillainsGeneric
{
    public class Villain : CoreCharacter
    {
        //as of now, we only need the core character properties.
        public void DisplayVillain()
        {
            Console.WriteLine("Full Name : " + this.FirstName + " " + this.LastName);
            Console.WriteLine("Alter Ego : " + this.AlterEgo);
        }
    }
}
