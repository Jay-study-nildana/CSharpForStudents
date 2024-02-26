using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    public class RealGrandPa : AbstractGrandPa
    {
        //implements the abstract method
        public override void AbstractMethodFromAbstractGrandPa()
        {
            Console.WriteLine("Abstract Method of abstract grandpa has been implemented in the Real GrandPa");
        }
    }
}
