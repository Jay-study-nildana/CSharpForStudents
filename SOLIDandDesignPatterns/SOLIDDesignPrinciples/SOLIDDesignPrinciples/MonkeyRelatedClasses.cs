using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDDesignPrinciples
{
    class MonkeyRelatedClasses
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    public enum LifeStage
    {
        Baby, YoungAdult, Adult, OldAge
    }

    public enum Gender
    {
        Male, Female
    }

    public class MonkeyBaseClass
    {
        public string firstNameOfMonkey { set; get; }
        public string lastNameOfMonkey { set; get; }
        public LifeStage lifeStage { set; get; }
        public Gender gender { set; get; }

        public MonkeyBaseClass(string firstNameOfMonkey, string lastNameOfMonkey, LifeStage lifeStage, Gender gender)
        {
            this.firstNameOfMonkey = firstNameOfMonkey;
            this.lastNameOfMonkey = lastNameOfMonkey;
            this.lifeStage = lifeStage;
            this.gender = gender;
        }

        public MonkeyBaseClass()
        {

        }
    }

    //This class is for family related operations. 
    public class MonkeyWithParentsInfo : MonkeyBaseClass
    {
        //TODO - majority of classes use public properties
        //switch it to private or protected please
        //we need a unique identifier.
        public string monkeyUniqueIdentifier { set; get; }
        //we need to set a father monkey
        public string fatherMonkeyIdentifier { set; get; }
        //we need to set a mother monkey
        public string motherMonkeyIdentifier { set; get; }

        //by default, a monkey's parents are unknown to us.
        //and a unique identifier is attached right away
        public MonkeyWithParentsInfo()
        {
            fatherMonkeyIdentifier = "Unknown";
            motherMonkeyIdentifier = "Unknown";
            monkeyUniqueIdentifier = monkeyUniqueIdentifierGenerator();
        }

        //this can be used to indicate if a specific monkey is the family creator
        //or a child of existing monkeys
        //very useful when building family trees and we want to when and where the family starts.
        public bool FamilyCreator()
        {
            if(fatherMonkeyIdentifier == "Unknown" && motherMonkeyIdentifier== "Unknown")
            {
                return true;
            }

            return false;
        }

        public string monkeyUniqueIdentifierGenerator()
        {
            string monkeyUniqueIdentifier = Guid.NewGuid().ToString();

            return monkeyUniqueIdentifier;
        }
    }

}
