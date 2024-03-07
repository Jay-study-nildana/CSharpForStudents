using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupOfMug
{
    public class MugWithDisposable : IDisposable
    {

        public MugWithDisposable() {

            Console.WriteLine("Object has been created");
        }

        public void JustSomethingToDoWithDatabase()
        { 
            Console.WriteLine("Something something that happened with something something database");
        }
        public void Dispose()
        {
            Console.WriteLine("Object has been disposed off");
        }
    }
}
