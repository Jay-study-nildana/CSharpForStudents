using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPBasics
{
    public class FictionalDetective
    {
        //these are the class fields. some people also use the word properties. 
        public string Name { get; set; }
        public string PrimaryPower { get; set; }
        public string SecondaryPower { get; set; }

        //private access modifier. cannot be accessed outside.
        private int SomeRandomNumber { get; set; }

        //set the private property
        public void SetSomeRandomNumber(int someRandomNumber)
        {
            this.SomeRandomNumber = someRandomNumber;
        }

        //get the private property
        public int GetSomeRandomNumber()
        {
            return this.SomeRandomNumber;
        }

        //this is a static field. available to be accessed directly from the class. 
        //shared across objects
        public static int SomeRandomYear = 1969;

        //constant field 
        public const string SomeConstantString = "Elementary Dear Watson";

        //readonly filed. 
        public readonly string ReadThisPlease = "This is a Sentence That Cannot Be Changed";

        //a simple method or function, that can be used to display things
        public void DisplayTheFictionalDetective()
        {
            Console.WriteLine("----------");
            Console.WriteLine("Name of Detective : " + Name);
            Console.WriteLine("Primary Power : " + PrimaryPower);
            Console.WriteLine("Secondary Power : " + SecondaryPower);
            Console.WriteLine("----------");

        }

        //static method
        public static void MouseColor()
        {
            Console.WriteLine("The Color of the Mouse is Pink");
        }
    }
}
