using System;
using System.Collections.Generic;
using System.Text;

namespace Consuming3rdPartyAPI.Helpers
{
    public class DisplayHelper
    {
        internal void ShowRandomQuote(RandomQuote randomQuote2)
        {
            ShowTheThing("Operation Status", randomQuote2.OperationSuccessful);
            ShowTheThing("Author", randomQuote2.QuoteAuthor);
            ShowTheThing("Quote", randomQuote2.QuoteContent);
        }

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
