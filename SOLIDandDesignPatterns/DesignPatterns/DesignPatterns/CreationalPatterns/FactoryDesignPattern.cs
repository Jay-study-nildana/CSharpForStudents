using SOLIDDesignPrinciples;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.CreationalPatterns
{
    class FactoryDesignPattern
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //in my understanding factory design pattern is concerned with overcoming inherent 
    //limitations of the standard facilities available from constructors
    //for instance, you cannot have two constructors that take the same type parameters
    //like, constructor(string a, string b) and constructor(string x,string y)
    //as c# does type matching, not name matching. 
    //there are other reasons to use factory but this is what i felt was the biggest reason.

    //here, I am putting the Factory stuff inside the class
    //its possible to put the Factory as a separate class

    //TODO - add a factory that is a separate class

    public class MonkeyCreatorWithInterestingNames
    {
        private string monkeyFirstName { set; get; }
        private string monkeyLastName { set; get; }

        //TODO add a detailed explanation why this must be private
        private MonkeyCreatorWithInterestingNames(string firstName,string lastName)
        {
            monkeyFirstName = firstName;
            monkeyLastName = lastName;
        }

        public MonkeyCreatorWithInterestingNames()
        {
        }

        //factory method that creates a monkey with the name as it is sent. 
        public static MonkeyCreatorWithInterestingNames MonkeyNameAsItIs(string firstName, string lastName)
        {
            MonkeyCreatorWithInterestingNames monkeyCreatorWithInterestingNames = new MonkeyCreatorWithInterestingNames(firstName, lastName);

            return monkeyCreatorWithInterestingNames;
        }

        //factory method that creates a monkey after adding some extra letters
        public static MonkeyCreatorWithInterestingNames MonkeyNameWithChanges(string firstName, string lastName)
        {
            //this could any additional operations
            firstName = firstName + "addsomething1";
            lastName = lastName + "addsomething2";

            MonkeyCreatorWithInterestingNames monkeyCreatorWithInterestingNames = new MonkeyCreatorWithInterestingNames(firstName, lastName);

            return monkeyCreatorWithInterestingNames;
        }

        public void DisplayTheMonkey()
        {
            HelperStuff helperStuff = new HelperStuff();
            helperStuff.DisplayALine();
            helperStuff.DisplayText("Monkey Details below");
            helperStuff.DisplayText("First Name - " + this.monkeyFirstName);
            helperStuff.DisplayText("Last Name - " + this.monkeyLastName);
            helperStuff.DisplayALine();
        }
    }
}
