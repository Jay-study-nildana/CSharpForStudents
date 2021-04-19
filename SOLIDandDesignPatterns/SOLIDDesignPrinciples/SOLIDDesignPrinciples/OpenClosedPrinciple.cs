using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDDesignPrinciples
{
    class OpenClosedPrinciple
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //we are trying to sort/filter and similar things with monkeys using monkey traits. 


    //when not applying OpenClosedPrinciple
    //we would end up building a class/methods that will do filtering 
    //based on LifeStage, Gender either alone or in combination
    //what if tomorrow we had more categories like, I dont know, CountryOfOrigin, 
    //from where the monkeys were brought in. 

    //if we are forced to write a new method everytime the criteria for 
    //filtering changes, that is just bad code and design.

    //So, we build something that is fixed and is also extensible. 
    
    //at this point we have an interface for filtering and an interface for criteria.
    //we can build our actual classes that follow OpenClosedPrinciple
    public class MonkeyFilter : IMonkeyFiltering<MonkeyBaseClass>
    {
        public IEnumerable<MonkeyBaseClass> Filter(IEnumerable<MonkeyBaseClass> listOfMonkeys, ICriteriaForFiltering<MonkeyBaseClass> criteriaForFiltering)
        {
            //throw new NotImplementedException();
            foreach (var monkey in listOfMonkeys)
            {
                if(criteriaForFiltering.CriteriaIsSatisfied(monkey))
                {
                    //TODO dig into this yield word a little more.
                    yield return monkey;
                }
            }
        }
    }

    //now, we have the filtering. lets setup some criteria. 
    //I want to filter based on Gender. 
    public class GenderCriteria : ICriteriaForFiltering<MonkeyBaseClass>
    {
        private Gender gender;

        //we set that gender that we are filtering for.
        public GenderCriteria(Gender gender)
        {
            this.gender = gender;
        }

        public bool CriteriaIsSatisfied(MonkeyBaseClass item)
        {
            //we have set the criteria/gender we are looking for in the constructor.
            //now, we check the set criteria/gender with the gender of the item sent in
            if(item.gender == gender)
            {
                return true;
            }
            else
            {
                return false;
            }

            //a more simple line of code would be
            //return item.gender == gender
            //but i prefer a more descriptive code, as this is meant for learners
        }
    }

    //now, I want to filter based on LifeStage
    public class LifeStageCriteria : ICriteriaForFiltering<MonkeyBaseClass>
    {
        private LifeStage lifeStage;

        //we set that age that we are filtering for.
        public LifeStageCriteria(LifeStage lifeStage)
        {
            this.lifeStage = lifeStage;
        }

        public bool CriteriaIsSatisfied(MonkeyBaseClass item)
        {
            //we have set the criteria/age we are looking for in the constructor.
            //now, we check the set criteria/age with the age of the item sent in
            if (item.lifeStage == lifeStage)
            {
                return true;
            }
            else
            {
                return false;
            }

            //a more simple line of code would be
            //return item.lifeStage == lifeStage
            //but i prefer a more descriptive code, as this is meant for learners
        }

        //TODO - that combines both Life Stage and Gender
        //build an interface that combines two or more criterias
    }
}
