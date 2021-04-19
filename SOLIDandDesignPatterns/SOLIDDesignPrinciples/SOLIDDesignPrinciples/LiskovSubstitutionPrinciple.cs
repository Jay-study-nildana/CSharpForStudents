using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDDesignPrinciples
{
    class LiskovSubstitutionPrinciple
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //I am going to reuse the existing class MonkeyOpenClosedPrinciple

    //The idea with Liskov is that, Child implementations of Parent classes should not have breaking changes
    //lets have an interface that takes two monkeys and then returns their names together. 
    public interface IGiveMeTheMonkeyNames
    {
        public string HereAreTheNames(MonkeyBaseClass monkey1, MonkeyBaseClass monkey2);
    }

    //I have a class that does the implementation.
    public class ParentMonkeyNames : IGiveMeTheMonkeyNames
    {
        public string HereAreTheNames(MonkeyBaseClass monkey1, MonkeyBaseClass monkey2)
        {
            string combinedNames = monkey1.firstNameOfMonkey + " -- " + monkey2.firstNameOfMonkey;
            return combinedNames;
        }
    }

    //now, I will get a child class 
    public class Child1MonkeyNames : ParentMonkeyNames
    {
        //this method overrides the base method
        //but instead of combining the monkey names, it does its own thing
        //which gives an incorrect output. 
        //it just returns a sentence, breaking an earlier functionality that was working just fine. 
        public new string HereAreTheNames(MonkeyBaseClass monkey1, MonkeyBaseClass monkey2)
        {
            string combinedNames = "the monkey names are joined. There you go";
            return combinedNames;
        }
    }

    public class Child2MonkeyNames : ParentMonkeyNames
    {
        //now, this one, does not over ride and break the base functionality
        //it keeps the original behavior while adding new behavior that Child1MonkeyNames
        //is trying to provide
        //now, here, note that over riding itself by nature, is not a bad thing
        //there will always be occasions when over riding is required as the software gets more complex
        //and requirements change.
        //but overriding that breaks the original functionality (either accidentally or intentionally) is a problem.
        public string ReturnRandomNameSetence(MonkeyBaseClass monkey1, MonkeyBaseClass monkey2)
        {
            string combinedNames = "the monkey names are joined. There you go";
            return combinedNames;
        }
    }
}
