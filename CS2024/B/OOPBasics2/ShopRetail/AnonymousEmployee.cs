using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRetail
{
    //delegate and anonymous methods

    public delegate void SomeDelegateForAnonymousMethod(int somenumber);
    public delegate int SomeDelegateForAnonymousMethodTwo(int numberone, int numbertwo);

    public class AnonymousEmployee
    {
        //anonymous method using delegate
        public SomeDelegateForAnonymousMethod MethodOne = delegate (int employeenumber)
        {
            Console.WriteLine("Employee Number is : " + employeenumber);
        };

        //anonymous method using delegate - one more.
        public SomeDelegateForAnonymousMethod MethodTwo = delegate (int sortingnumber)
        {
            for (var i = 0; i < sortingnumber; i++)
            {
                Console.WriteLine("This is printing with the value of i : " + i);
            }
        };

        //anonymous method without using delegate but using lambda expression
        public SomeDelegateForAnonymousMethod MethodThree = (countingnumber) =>
        {
            Console.WriteLine("The number being counted is : " + countingnumber);
        };

        //same as lambda expression but with inline expressions
        public SomeDelegateForAnonymousMethodTwo MethodFour = (firstnumber,secondnumber) => firstnumber + secondnumber;

        //using Func to skip having to create a new delegate and creating a method for it.
        
        Func<int, int> somenumberwithten = somenumber => somenumber * 10; 

        public void MultiplyWithTenFunc(int somenumber)
        {
            Console.WriteLine("The number has been multiplied by 10 using Func is here : " + somenumberwithten(somenumber));
        }

        Action<int> somenumbertodisplay = somenumber => Console.WriteLine("The number to display with Action is : " + somenumber);

        public void DisplayTheNumberAction(int somenumber)
        {
            somenumbertodisplay(somenumber);
        }

        //TODO we need to look at this Predicate<T>
        //TODO also need to look at EventHandler
        //TODO also expression tree
        //TODO also Expression Bodied Members
        //TODO also switch expression 

    }
}
