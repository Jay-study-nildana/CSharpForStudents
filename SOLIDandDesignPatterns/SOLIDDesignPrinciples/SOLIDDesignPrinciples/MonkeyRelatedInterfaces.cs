using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDDesignPrinciples
{

    //first we build a interface that will tell us if a certain criteria was met
    //this is required during filtering because, we are filtering based on certain criteria
    //being true or false. 
    //this criteria, as an interface, is generic enough that the implementations can decide
    //whatever criteria that can be applied, currently and in the future, 
    //without affecting existing code. 

    class MonkeyRelatedInterfaces
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //low level - list of all monkeys.
    public interface IListOfAllMonkeys
    {
        //contrast this with how monkeys are obtained in CheckOutOpenClosedPrinciple method/function
        //and you will see how that one breaks DependencyInversionPrinciple
        public List<MonkeyBaseClass> listOfAllMonkeys();
    }

    //high level - get all male monkeys
    public interface IGetAllMaleMonkeys
    {
        //compare with CheckOutOpenClosedPrinciple method. There, the technique to obtain all monkeys
        //is dependent on the class HelperStuff's one specific method of getting monkeys
        //that means, later, if we suddenly change the way monkeys are provided by HelperStuff
        //we are forced to modify every function and class that depends on it
        //thereby breaking DependencyInversionPrinciple

        public List<MonkeyBaseClass> GetAllMaleMonkeys(List<MonkeyBaseClass> listOfAllMonkeys);
    }

    public interface ICriteriaForFiltering<T>
    {
        //this is what we might call a predicate in programming terms.
        bool CriteriaIsSatisfied(T item);
    }

    //now comes the filtering functionality 
    //this will use the criteria as provided by ICriteriaForFiltering
    public interface IMonkeyFiltering<T>
    {
        //this has a single function that will take a collection of any type of Monkey or anything else
        //and it also takes a criteria filter of any type so we can criteria of whatever we want
        //and it returns a collection of any type of Monkey or anything else.
        IEnumerable<T> Filter(IEnumerable<T> listOfMonkeys, ICriteriaForFiltering<T> criteriaForFiltering);
    }
}
