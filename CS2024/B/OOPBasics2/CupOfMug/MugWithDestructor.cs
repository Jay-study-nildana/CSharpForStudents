using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupOfMug
{
    public class MugWithDestructor
    {
        public MugWithDestructor() {
            Console.WriteLine("Object has been created");
        }

        //gets called when program is about to end.
        //TODO, but why is this not getting called though? 
        ~MugWithDestructor()
        {
            Console.WriteLine("Object has been destoryed");
        }
    }
}
