using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOLIDDesignPrinciples
{
    class DependencyInversionPrinciple
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //So here, there are two ideas but ultimately they are both talking about the same thing
    //and they circle back to the original idea behind good developer practices - dont depend your software behavior 
    //on concrete classes. always plan for modifications and changes in the future which dont break things that are already working.

    //idea #1 - high level things should not depend on low level things. both, should depend on abstractions
    //here abstractions are essentially interfaces
    //idea #2 - abstractions should not depend on details 
    //this enforces the idea behind the idea that, abstractions will have to be interfaces.

    //lets have a high level and low level things that i want in my monkey software
    //high level - i want all the monkeys that are male
    //low level - first, i want to get all the monkeys which then will fed into high level components
    //for example, the above mentioned, finding all monkeys which are male.

    //note - this is already done in the file OpenClosedPrinciple.cs which in fact, follows OpenClosedPrinciple
    //but breaks DependencyInversionPrinciple as used in the method/function CheckOutOpenClosedPrinciple
    //this code here compare and contrasts with that implementation to illustrate the problem
    //but we can now fix it here.

    //both are abstractions, and now, lets have our details/implementations that depend on these abstractions
    //I have a class called ListOfAllMonkeysFromHelper to indicate that this list of monkeys is coming from 
    //the helper method which is in memory.
    public class ListOfAllMonkeysFromHelper : IListOfAllMonkeys
    {
        public List<MonkeyBaseClass> listOfAllMonkeys()
        {
            //here, I can use my helper which already generates monkeys. 
            //this is detail /implementation and the software does not depend on this
            //the software depends on the interface.
            HelperStuff helperStuff = new HelperStuff();

            var listOfMonkeys = helperStuff.GiveMeTenMonkeys();

            return listOfMonkeys;
        }
    }

    //tomorrow, I could have a list of monkeys coming from a database
    //or any other source
    //but it wont break my software as the software only depends on the abstraction
    //aka the interface IListOfAlMonkeys
    public class ListOfAllMonkeysFromDatabase : IListOfAllMonkeys
    {
        public List<MonkeyBaseClass> listOfAllMonkeys()
        {
            //get the monkeys from database
            throw new NotImplementedException();
        }
    }

    //now, the high level get all male monkeys
    //here, I added a title GetAllMaleMonkeys1 with a 1 to indicate that later
    //I can have other implementations and illustrate that
    //the software that wants to get all male monkeys does not depend on this class
    //but rather the abstraction aka the interface
    public class GetAllMaleMonkeys1 : IGetAllMaleMonkeys
    {
        public List<MonkeyBaseClass> GetAllMaleMonkeys(List<MonkeyBaseClass> listOfAllMonkeys)
        {
            //here, the good thing is, I already havea a monkey that is available to use from the class MonkeyFilter
            //I can use that here. 
            //I need two things to make the filtering work
            //1. I need list of monkeys which is already available when this function is called
            //2. I need a Gender criteria, which is male 

            //Once again, I already have a class that allows me to set criteria
            var monkeyMaleCriteria = new GenderCriteria(Gender.Male);

            //now, I can do the filtering.
            MonkeyFilter monkeyFilter = new MonkeyFilter();
            var responseMonkeys = monkeyFilter.Filter(listOfAllMonkeys, monkeyMaleCriteria);

            //I get an IEnumerable but the expected return is a list so we do the list conversion before returning
            return responseMonkeys.ToList();
        }
    }
}
