using HeroesGeneric;
using Superman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPattern
{
    //this adapter will bridge our behavior
    //PunchAnyVillain
    //to superman's local interface
    internal class PunchingAdapter<T> : ISupesPunchesVillain<T>
    {
        private readonly PunchAnyVillain<T> _adaptee;
        public T OneSolidPunch()
        {
            //Console.WriteLine("One Solid Punch Was Landed");
            //we use the adapter object to call the target method.
            //this target method wont work with Superman directly
            //but now superman can punch the villain via a proxy.
            _adaptee.OneSolidPunch();
            return default(T);            
        }

        public PunchingAdapter(PunchAnyVillain<T> punchAnyVillain)
        {
            this._adaptee = punchAnyVillain;
        }
    }
}
