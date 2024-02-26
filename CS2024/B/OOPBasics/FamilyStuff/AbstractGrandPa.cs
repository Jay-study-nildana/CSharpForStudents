using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    public abstract class AbstractGrandPa
    {
        public void DisplayAbstractGrandPaDetails()
        {
            Console.WriteLine("This Grandpa is totally abstract. Cannot be used directly, but, available in derived classes");
        }

        //needs to be implemented in the derived class
        public abstract void AbstractMethodFromAbstractGrandPa();
    }
}
