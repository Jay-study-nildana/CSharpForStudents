using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHelloWorld.GTypes
{
    public class HelloWorld2<T>
    {
        public void DisplayTheType(HelloWorld<T> input1, HelloWorld<T> input2)
        {
            Console.WriteLine("input1 is " + input1.Thing.GetType());
            Console.WriteLine("input2 is " + input2.Thing.GetType());
        }

        public void DisplayTheDetails(HelloWorld<T> input1, HelloWorld<T> input2)
        {
            Console.WriteLine("input1 is " + input1.Thing);
            Console.WriteLine("input2 is " + input2.Thing);
        }

        public void DoSomething(HelloWorld<T> input1, HelloWorld<T> input2)
        {
            Console.WriteLine("Type is " + input1.Thing.GetType());
            Console.WriteLine("string type is " + typeof(string));
            Console.WriteLine("int type is " + typeof(int));
            Console.WriteLine("bool type is " + typeof(bool));
            //typeof(T);
            if (input1.Thing.GetType() == typeof(string))
            {
                Console.WriteLine("We have a string. So concatenating ");
                var temp1 = String.Concat(input1.Thing, input2.Thing);
                Console.WriteLine(temp1);
            }
            if (input1.Thing.GetType() == typeof(int))
            {
                Console.WriteLine("We have a int. So math operation ");
                var temp1 = Convert.ToInt32(input1.Thing) + Convert.ToInt32(input2.Thing);
                Console.WriteLine(temp1);
            }
            if (input1.Thing.GetType() == typeof(bool))
            {
                Console.WriteLine("We have a bool. So boolean operation ");
                var temp1 = Convert.ToBoolean(input1.Thing) && Convert.ToBoolean(input2.Thing);
                Console.WriteLine(temp1);
            }
        }
    }
}
