using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupOfMug
{
    public class MugGeneric2<T1,T2,T3>
    {
        public T1 FirstDetail { get; set; }
        public T2 SecondDetail { get; set; }
        public T3 ThirdDetail { get; set; }

        public void DisplayTheDetails()
        {

            Console.WriteLine(FirstDetail + " " + SecondDetail + "  " + ThirdDetail);
        }
    
    }
}
