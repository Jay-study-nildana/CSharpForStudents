using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesGeneric
{
    // The Adaptee contains some useful behavior, but its interface is
    // incompatible with the existing client code. The Adaptee needs some
    // adaptation before the client code can use it.
    //This is a behavior that wont work with Superman's 
    //for the sake of argument. Just to illustrate Adapter Pattern
    public class PunchAnyVillain<T>
    {
        public T OneSolidPunch()
        {
            Console.WriteLine("PunchAnyVillain - Some Hero Punched Some Villain");
            return default(T);
        }
    }
}
