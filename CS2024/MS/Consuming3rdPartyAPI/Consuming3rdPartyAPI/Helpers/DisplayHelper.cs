using System;
using System.Collections.Generic;
using System.Text;

namespace Consuming3rdPartyAPI.Helpers
{
    public class DisplayHelper
    {

        public void ShowTheThing(string v, string quoteAuthor)
        {
            Console.WriteLine(v + " : " + quoteAuthor);
        }

        public void ShowTheThing(string v, bool operationSuccessful)
        {
            Console.WriteLine(v + " : " + operationSuccessful);
        }

        internal void ShowTheThing(string v)
        {
            Console.WriteLine(v);
        }

        internal void ShowALine()
        {
            Console.WriteLine("---------------------------------------------------------");
        }
    }
}
