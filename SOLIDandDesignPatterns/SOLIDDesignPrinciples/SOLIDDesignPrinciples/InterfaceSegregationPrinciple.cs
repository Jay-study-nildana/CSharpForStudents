using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SOLIDDesignPrinciples
{
    class InterfaceSegregationPrinciple
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //InterfaceSegregationPrinciple is similar to SingleResponsibilityPrinciple, me thinks.
    //SingleResponsibilityPrinciple focuses on classes where as InterfaceSegregationPrinciple
    //focuses on interfaces

    //this is doing it wrong.
    //The interface designer might think, it is cool, that a monkey should have all these behaviors.
    //So, every monkey implementation should have all these behaviors.

    public interface IDoMonkeyStuff
    {
        public Object MakeMonkeyDance(MonkeyBaseClass monkey);
        public Object MakeMonkeyRun(MonkeyBaseClass monkey);
        public Object MakeMonkeyLaugh(MonkeyBaseClass monkey);
        public Object MakeMonkeyEat(MonkeyBaseClass monkey);
        public Object MakeMonkeySleep(MonkeyBaseClass monkey);
    }

    //BUT
    //a developer using this interface, might only want a monkey that can dance and run.
    //but, he ends up with a class like this.
    public class MonkeyDoingThings1 : IDoMonkeyStuff
    {
        public object MakeMonkeyDance(MonkeyBaseClass monkey)
        {
            //I want this.
            return null;
        }

        public object MakeMonkeyEat(MonkeyBaseClass monkey)
        {
            //I want this.
            return null;
        }

        //I dont care if the monkey laughs but I am forced to have this here
        //because the interface designer thinks this is neccessary

        public object MakeMonkeyLaugh(MonkeyBaseClass monkey)
        {
            throw new NotImplementedException();
        }

        //I dont care if the monkey laughs but I am forced to have this here
        //because the interface designer thinks this is neccessary
        public object MakeMonkeyRun(MonkeyBaseClass monkey)
        {
            throw new NotImplementedException();
        }

        //I dont care if the monkey laughs but I am forced to have this here
        //because the interface designer thinks this is neccessary
        public object MakeMonkeySleep(MonkeyBaseClass monkey)
        {
            throw new NotImplementedException();
        }
    }

    //here is a better appraoch which follows InterfaceSegregationPrinciple
    public interface IMakeMonkeyDance
    {
        public Object MakeMonkeyDance(MonkeyBaseClass monkey);

    }

    public interface IMakeMonkeyRun
    {
        public Object MakeMonkeyRun(MonkeyBaseClass monkey);
    }

    public interface IMakeMonkeyLaugh
    {
        public Object MakeMonkeyLaugh(MonkeyBaseClass monkey);
    }

    public interface IMakeMonkeyEat
    {
        public Object MakeMonkeyEat(MonkeyBaseClass monkey);
    }

    public interface IMakeMonkeySleep
    {
        public Object MakeMonkeySleep(MonkeyBaseClass monkey);
    }

    //now, the developer, can build the monkey he wants.
    //for example, to build a monkey similar to MonkeyDoingThings1
    //where the developer only wants a monkey that can dance and eat
    public class MonkeyDoingThings2 : IMakeMonkeyDance, IMakeMonkeyEat
    {
        public object MakeMonkeyDance(MonkeyBaseClass monkey)
        {
            //I want this.
            return null;
        }

        public object MakeMonkeyEat(MonkeyBaseClass monkey)
        {
            //I want this.
            return null;
        }
    }
}
