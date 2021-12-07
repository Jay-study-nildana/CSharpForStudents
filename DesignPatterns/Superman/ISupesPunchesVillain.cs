using HeroesGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superman
{
    //this is superman only local interface.
    //as of now, suppose, this interface can punch only superman villains
    //this is the local interface
    // The Target defines the domain-specific interface used by the client code.
    public interface ISupesPunchesVillain<T> : IPunchVillain<T>
    {
        //any additional functionality.
    }
}
