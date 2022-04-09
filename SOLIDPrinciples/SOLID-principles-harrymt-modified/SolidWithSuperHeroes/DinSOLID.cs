using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//BY FAR THE MOST COMPLEX SOLID PATTERN
//THIS HAS LONG TERM RAMIFICATIONS FOR YOU C SHARP LEARNING. OR ANY LEARNING ANY OBJECT ORIENTED CODING
//BY FAR THE MOST COMPLEX SOLID PATTERN
//THIS HAS LONG TERM RAMIFICATIONS FOR YOU C SHARP LEARNING. OR ANY LEARNING ANY OBJECT ORIENTED CODING
//BY FAR THE MOST COMPLEX SOLID PATTERN
//THIS HAS LONG TERM RAMIFICATIONS FOR YOU C SHARP LEARNING. OR ANY LEARNING ANY OBJECT ORIENTED CODING
//BY FAR THE MOST COMPLEX SOLID PATTERN
//THIS HAS LONG TERM RAMIFICATIONS FOR YOU C SHARP LEARNING. OR ANY LEARNING ANY OBJECT ORIENTED CODING
//BY FAR THE MOST COMPLEX SOLID PATTERN
//THIS HAS LONG TERM RAMIFICATIONS FOR YOU C SHARP LEARNING. OR ANY LEARNING ANY OBJECT ORIENTED CODING
//BY FAR THE MOST COMPLEX SOLID PATTERN
//THIS HAS LONG TERM RAMIFICATIONS FOR YOU C SHARP LEARNING. OR ANY LEARNING ANY OBJECT ORIENTED CODING

namespace SolidWithSuperHeroes
{
    internal class DinSOLID
    {
    }

    //Dependency inversion principle - 

    //    specific methodology for loosely coupling software modules.When following this principle, the conventional dependency relationships established from high-level, policy-setting modules to low-level, dependency modules are reversed, thus rendering high-level modules independent of the low-level module implementation details. The principle states:[1]

    //    High-level modules should not import anything from low-level modules. Both should depend on abstractions (e.g., interfaces).

    //   Abstractions should not depend on details. Details (concrete implementations) should depend on abstractions.

    //By dictating that both high-level and low-level objects must depend on the same abstraction, this design principle inverts the way some people may think about object-oriented programming.[2]

    //The idea behind points A and B of this principle is that when designing the interaction between a high-level module and a low-level one, the interaction should be thought of as an abstract interaction between them.This not only has implications on the design of the high-level module, but also on the low-level one: the low-level one should be designed with the interaction in mind and it may be necessary to change its usage interface.

    //In many cases, thinking about the interaction in itself as an abstract concept allows the coupling of the components to be reduced without introducing additional coding patterns, allowing only a lighter and less implementation-dependent interaction schema.

    //When the discovered abstract interaction schema(s) between two modules is/are generic and generalization makes sense, this design principle also leads to the following dependency inversion coding pattern.

    //https://en.wikipedia.org/wiki/Dependency_inversion_principle

    //As usual, let's use Batman
    //When Batman is out crime fighting, he always has someone in the batcave helping him.
    //Most of the time it is Alfred, the super awesome butler. 
    //Unfortunatley, sometimes Alfred is not at home. Or may be, he is taking a break or on a vacation in Paris, France.

    //Here is how it looks when Batman is 'hard coded' to only use Alfred and no one else.
    public class AlfredIsHelpingWITHOUTDEPENDENCYINVERSION
    {
        public void TalkingOverRadio()
        {
            Console.WriteLine("Master Wayne, I am totally just being sarcasting and insulting to you, although, I love you like a son.");
        }

        public void ArrangingFood()
        {
            Console.WriteLine("I have prepared breakfast for you, Master Wayne. ");
        }
    }
    
    public class BatmanBeingBatmanWITHOUTDEPENDENCYINVERSION
    {
        //here, we are hardcoding Alfred
        //That means, if Alfred is out of the picture, Batman, simply cannot fight crime.
        AlfredIsHelpingWITHOUTDEPENDENCYINVERSION helpFromBatCave = new AlfredIsHelpingWITHOUTDEPENDENCYINVERSION();

        public void FightCrime()
        {
            helpFromBatCave.ArrangingFood();
            helpFromBatCave.TalkingOverRadio();
        }
    }

    //So, let's make life for Batman, a little bit more flexible. 
    //For the sake of Gotham City
    public interface IisHelping
    {
        public void TalkingOverRadio();
        public void ArrangingFood();
    }

    //So, now Alfred is ready to help
    public class AlfredProper : IisHelping
    {
        public void TalkingOverRadio()
        {
            Console.WriteLine("Master Wayne, I am totally just being sarcasting and insulting to you, although, I love you like a son.");
        }

        public void ArrangingFood()
        {
            Console.WriteLine("I have prepared breakfast for you, Master Wayne. ");
        }
    }

    //but other people from batfamily can take over
    public class CatWomanProper : IisHelping
    {
        public void TalkingOverRadio()
        {
            Console.WriteLine("Bruce, once you are done with Joker, we should spend some time in the cave.");
        }

        public void ArrangingFood()
        {
            Console.WriteLine("I have cat food. Hope that's okay with you, Bruce. ");
        }
    }

    public class RobinProper : IisHelping
    {
        public void TalkingOverRadio()
        {
            Console.WriteLine("If you need me to come and join the fight, let me know.");
        }

        public void ArrangingFood()
        {
            Console.WriteLine("I have some noodles to boil here.");
        }
    }

    //now depending on who is available, Batman can take corresponding help
    //notice how, the only thing that changes is the person aka implementation class providing the actual help to batman
    //but the interface and object name remains the same.
    //IisHelping helpFromBatCave remains the same, in all the Batman scenarios
    public class BatmanBeingBatmanWithAlfred
    {
        IisHelping helpFromBatCave = new AlfredProper();

        public void FightCrime()
        {
            helpFromBatCave.ArrangingFood();
            helpFromBatCave.TalkingOverRadio();
        }
    }

    public class BatmanBeingBatmanWithCatWoman
    {
        IisHelping helpFromBatCave = new CatWomanProper();

        public void FightCrime()
        {
            helpFromBatCave.ArrangingFood();
            helpFromBatCave.TalkingOverRadio();
        }
    }

    public class BatmanBeingBatmanWithRobin
    {
        IisHelping helpFromBatCave = new RobinProper();

        public void FightCrime()
        {
            helpFromBatCave.ArrangingFood();
            helpFromBatCave.TalkingOverRadio();
        }
    }

    //here's another way of doing it.
    public class BatmanBeingBatman
    {
        //just like above, an object using the interface
        //we will supply the actual helper person aka implementation class later.
        private IisHelping helpFromBatCave;

        //we supply the implementation class during object creation
        public BatmanBeingBatman(IisHelping helping)
        {
            this.helpFromBatCave=helping;
        }

        public void FightCrime()
        {
            helpFromBatCave.ArrangingFood();
            helpFromBatCave.TalkingOverRadio();
        }
    }

    //now, let's try them out.
    //notice how, we simply supply the implementation class
    //while the syntax, object name and interface name remain consistent.
    //In other words, at the point of fighting crime, we supply Batman with information
    //on how who is going to help him during the night
    //this way, if one of them is not available, another person is ready to take over
    public class TryingOutFightinCrime
    {
        public void FightingWithHelpFromAlfred()
        {
            var batmanfightscrime = new BatmanBeingBatman(new AlfredProper());
            batmanfightscrime.FightCrime();
        }

        public void FightingWithHelpFromCatWoman()
        {
            var batmanfightscrime = new BatmanBeingBatman(new CatWomanProper());
            batmanfightscrime.FightCrime();
        }

        public void FightingWithHelpFromRobin()
        {
            var batmanfightscrime = new BatmanBeingBatman(new RobinProper());
            batmanfightscrime.FightCrime();
        }
    }

}
