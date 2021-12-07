using GenericsHelloWorld.GTypes;
using System;

namespace GenericsHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            //Let's test out Generics. 
            //Generic<string> g = new Generic<string>();

            //WorkWithInt();
            //WorkWithStrings();
            //WorkWithBoolean();

            //lets look at our PracticeG set of generic classes
            WorkWithIntPracticeG();
            WorkWithStringPracticeG();

            Console.WriteLine("Hello World!");
        }

        private static void WorkWithStringPracticeG()
        {
            PracticeGTwo<string> practiceGTwo = new PracticeGTwo<string>();
            PracticeG<string> practiceG = new PracticeG<string>();
            practiceG.Something = "Dave Bosmans";

            practiceGTwo.ShowPracticeGType(practiceG);
            practiceGTwo.DoSomethingWithPracticeGType(practiceG);
        }

        private static void WorkWithIntPracticeG()
        {
            PracticeGTwo<int> practiceGTwo = new PracticeGTwo<int>();
            PracticeG<int> practiceG = new PracticeG<int>();
            practiceG.Something = 100;

            practiceGTwo.ShowPracticeGType(practiceG);
            practiceGTwo.DoSomethingWithPracticeGType(practiceG);
        }

        static void WorkWithStrings()
        {
            HelloWorld2<string> tempHelloWorld2 = new HelloWorld2<string>();
            HelloWorld<string> inputOne = new HelloWorld<string>();
            HelloWorld<string> inputTwo = new HelloWorld<string>();

            inputOne.Thing = "Batman";
            inputTwo.Thing = "Robin";
            tempHelloWorld2.DisplayTheType(inputOne, inputTwo);
            tempHelloWorld2.DisplayTheDetails(inputOne, inputTwo);
            tempHelloWorld2.DoSomething(inputOne, inputTwo);
        }

        static void WorkWithInt()
        {
            HelloWorld2<int> tempHelloWorld2 = new HelloWorld2<int>();
            HelloWorld<int> inputOne = new HelloWorld<int>();
            HelloWorld<int> inputTwo = new HelloWorld<int>();

            inputOne.Thing = 10;
            inputTwo.Thing = 20;
            tempHelloWorld2.DisplayTheType(inputOne, inputTwo);
            tempHelloWorld2.DisplayTheDetails(inputOne, inputTwo);
            tempHelloWorld2.DoSomething(inputOne, inputTwo);
        }

        static void WorkWithBoolean()
        {
            HelloWorld2<bool> tempHelloWorld2 = new HelloWorld2<bool>();
            HelloWorld<bool> inputOne = new HelloWorld<bool>();
            HelloWorld<bool> inputTwo = new HelloWorld<bool>();

            inputOne.Thing = true;
            inputTwo.Thing = false;
            tempHelloWorld2.DisplayTheType(inputOne, inputTwo);
            tempHelloWorld2.DisplayTheDetails(inputOne, inputTwo);
            tempHelloWorld2.DoSomething(inputOne, inputTwo);
        }
    }
}
