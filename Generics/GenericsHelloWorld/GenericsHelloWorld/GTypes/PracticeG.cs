using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHelloWorld.GTypes
{
    //this is for property only.
    public class PracticeG<T>
    {
        public T Something;
    }

    //this is for behavior only
    public class PracticeGTwo<T>
    {
        //show the type
        public void ShowPracticeGType(PracticeG<T> practiceG)
        {
            Console.WriteLine("The type of PracticeG<T>: " + practiceG.Something.GetType());
        }

        //do something based on the type
        public void DoSomethingWithPracticeGType(PracticeG<T> practiceG)
        {
            Console.WriteLine("The type of PracticeG<T>: " + practiceG.Something.GetType());
            if (practiceG.Something.GetType() == typeof(int))
            {
                //we do some int stuff
                var result = Convert.ToInt32(practiceG.Something) + 50;
                Console.WriteLine("The type was int...so did a addition operation. Result is : " + result);
            }

            if (practiceG.Something.GetType() == typeof(string))
            {
                //we do some int stuff
                var result = practiceG.Something.ToString().Length;
                Console.WriteLine("The type was string...so showing the string length as an example of string operation: " + result);
            }

            
        }
    }
}
