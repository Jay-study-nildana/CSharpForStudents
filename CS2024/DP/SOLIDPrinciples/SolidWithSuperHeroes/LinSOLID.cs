using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWithSuperHeroes
{
    internal class LinSOLID
    {
    }

    // Liskov substitution principle - It is based on the concept of "substitutability" – a principle in object-oriented programming stating that an object (such as a class) and a sub-object (such as a class that extends the first class) must be interchangeable without breaking the program. 

    //https://en.wikipedia.org/wiki/Liskov_substitution_principle

    //Here, we are looking at Parent classes being able to replace child classes.
    //Here, we understand that Parent class can easily replace wherever the child classes are being used.
    //A child can have extra features, of course. That was the whole point of creating a child class in the first place. Add extra things that parent does not have.
    //So, when a parent tries to replace a child, these extra features will go away. 
    //But, the existing features should still be working and available.

    //Here, Batman is obviously the family head. The parent.
    //You have Robin, who is almost like a son. I think, one of the Batman son's actually became Batman.
    //So, Batman should be able to replace Robin, in case, Robin is not available.
    // If Batman is unable to replace Robin, then, that would be a wrong thing as per Liskov.

    public class BatmanParent
    {
        //virtual means, allow derived class to override.
        public virtual void RespondToTheBatSignal()
        {
            Console.WriteLine("Meet Commisioner. Don't Forget to pull off Stealth Hi Bye!");
        }

        public virtual void MeetCatWomanInTheBatCave()
        {
            Console.WriteLine("Spend time with Catwoman.");
        }
    }

    public class RobinChild : BatmanParent
    {
        public override void RespondToTheBatSignal()
        {
            Console.WriteLine("Inform Commisioner that Batman is not in town. Find out how Robin can help");
        }

        public override void MeetCatWomanInTheBatCave()
        {
            //dude, it's your dad's girlfriend. dont' go ahead with it.
            throw new Exception("This is wrong on so many levels. Stop right now");
        }
    }

    public class BatmanClone : BatmanParent
    {
        public override void RespondToTheBatSignal()
        {
            Console.WriteLine("Meet Commisioner. Fail horribly when you try to pull off Stealth Hi Bye! Only the real Batman can do this stealth thing. ");
        }
    }

    //test out the Liskov thing. This is the WRONG way of doing things, of course.
    public class BatmanLiskovSoBad
    {
        public void PerformBatmanDuties()
        {
            //we have three heroes who are related to the main Batman Parent
            var batfamilyheroes = new List<BatmanParent>
            {
                new BatmanParent(),
                new RobinChild(),
                new BatmanClone()
            };

            //now, lets try to use them.
            foreach(var hero in batfamilyheroes)
            {
                hero.RespondToTheBatSignal(); //this wont cause any problems.

                //this will cause problems with the Robin object
                //the important thing to note here something
                //you will notice that, you won't get any compile errors or visual studio red lines.
                //That means, from a code perspective, there is nothing wrong with this line
                //Unfortunately, it is more of breaking the Liskov substitution principle.
                //So, a good design stops this from happenign at the code level itself.
                hero.MeetCatWomanInTheBatCave(); 
            }
        }
    }



    //Same Batman Family Stuff but with proper Liskov Implementation.

    //the plan is to implement the batman parent behavior via interfaces
    public interface IRespondToTheBatSignal
    {
        void RespondToTheBatSignal();
    }
    public interface IMeetCatWomanInTheBatCave
    {
        void MeetCatWomanInTheBatCave();
    }

    //let's design the Batman Parent class
    public class ProperBatmanParent : IRespondToTheBatSignal, IMeetCatWomanInTheBatCave
    {
        //we dont want children of Batman dating catwoman.
        //So, we ensure that this behavior is not virtual
        public void MeetCatWomanInTheBatCave()
        {
            Console.WriteLine("Spend time with Catwoman.");
        }

        //we definitely want children of Batman responding and dealing with bat signal
        public virtual void RespondToTheBatSignal()
        {
            Console.WriteLine("Meet Commisioner. Don't Forget to pull off Stealth Hi Bye!");
        }
    }

    public class ProperRobin : ProperBatmanParent
    {
        public override void RespondToTheBatSignal()
        {
            Console.WriteLine("Inform Commisioner that Batman is not in town. Find out how Robin can help");
        }

        //now, Robin cannot implement meeting catwoman. Visual studio will give you an error.
        //public override void MeetCatWomanInTheBatCave()
        //{
        //    //dude, it's your dad's girlfriend. dont' go ahead with it.
        //    throw new Exception("This is wrong on so many levels. Stop right now");
        //}
    }

    public class ProperBatmanClone : ProperBatmanParent
    {
        public override void RespondToTheBatSignal()
        {
            Console.WriteLine("Meet Commisioner. Fail horribly when you try to pull off Stealth Hi Bye! Only the real Batman can do this stealth thing. ");
        }
    }

    //lets test our properly written Liskov thing.
    //Here, you will notice something unusual with the story. 
    //Robin can in fact do the date catwoman behavior.
    //But the main thing is stopping Batman from overriding the catwoman behavior.

    //further, the main magic boils down to the usage of override and virtual keywords
    //That is what makes Liskov principle really work in C Sharp.
    public class BatmanLiskovSoGoodProper
    {
        public void PerformBatmanDuties()
        {
            //we have three heroes who are related to the main Batman Parent
            //because, we are passing a list of Batman, Batman can take over all the following roles, and 
            //do everything batman does.
            var batfamilyheroes = new List<ProperBatmanParent>
            {
                new ProperBatmanParent(),
                new ProperBatmanClone(),
                new ProperRobin(),
                //now this is the best part.
                //remember the Bad Robin from before?
                //we cannot use that here. 
                //new new RobinChild()
            };

            //now, lets try to use them.
            foreach (var hero in batfamilyheroes)
            {
                hero.RespondToTheBatSignal(); 

                hero.MeetCatWomanInTheBatCave();
            }
        }
    }
}
