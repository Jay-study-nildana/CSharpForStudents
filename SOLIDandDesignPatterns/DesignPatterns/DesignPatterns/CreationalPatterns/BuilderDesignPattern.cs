using SOLIDDesignPrinciples;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.CreationalPatterns
{
    class BuilderDesignPattern
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //Build Pattern is another creation pattern
    //This is concerned with creating complicated objects
    //Classes that contain other classes and there is easy and direct way to initialize them
    //but more likely, there is a specific way these complex objects must be created.

    //TODO - this builder does not stop folks from using MonkeyWithParentsInfo directly
    //find a way to stop that.

    //TODO - we could have a society builder that is a collection of families
    //meaning, a society builder that uses multiple builders inside...so builders building 
    //using existing builders

    //as always, we are using our monkey classes example.

    public class MonkeyFamilyBuilder
    {
        //we need a family name for this.
        //also the last name of the monkeys to indicate that they are related.
        //TODO - add a note explaining why this is protected
        protected readonly string familyName;
        //when a family is started, we assume, there is a male monkey and a female monkey
        protected MonkeyWithParentsInfo fatherMonkey;
        protected MonkeyWithParentsInfo motherMonkey;
        //then we have a collection fo children.
        protected List<MonkeyWithParentsInfo> childMonkeyCollection;

        //the moment we start a family, the last name must be set right away.
        //in this case, we randomly pick two monkeys out of the blue and assert a family name
        //TODO - we could have a constructor that takes two monkeys who are ready to start a family
        public MonkeyFamilyBuilder(string familyName)
        {
            this.familyName = familyName;
            HelperStuff helperStuff = new HelperStuff();
            fatherMonkey = helperStuff.RandomMonkeyWithParentsInfoGenerator(familyName,Gender.Male);
            motherMonkey = helperStuff.RandomMonkeyWithParentsInfoGenerator(familyName, Gender.Female);
            childMonkeyCollection = new List<MonkeyWithParentsInfo>();
        }

        //now, we can provide an option to add children.
        //usually when you have a baby, you get to name it. 
        //but we will leave it random for now.
        //TOD - add an option to provide a baby name
        public void AddMonkeyBaby()
        {
            HelperStuff helperStuff = new HelperStuff();
            var babyMonkeyBorn = helperStuff.RandomMonkeyWithParentsInfoGenerator(this.familyName);
            babyMonkeyBorn.fatherMonkeyIdentifier = fatherMonkey.monkeyUniqueIdentifier;
            babyMonkeyBorn.motherMonkeyIdentifier = motherMonkey.monkeyUniqueIdentifier;
            childMonkeyCollection.Add(babyMonkeyBorn);
        }

        //use this for a fluent approach.
        //this makes inheriting from this class difficult
        //so, ensure that when using this option, you dont have any future inheritance plans
        //for the current class
        public MonkeyFamilyBuilder AddMonkeyBabyFluent()
        {
            HelperStuff helperStuff = new HelperStuff();
            var babyMonkeyBorn = helperStuff.RandomMonkeyWithParentsInfoGenerator(this.familyName);
            babyMonkeyBorn.fatherMonkeyIdentifier = fatherMonkey.monkeyUniqueIdentifier;
            babyMonkeyBorn.motherMonkeyIdentifier = motherMonkey.monkeyUniqueIdentifier;
            childMonkeyCollection.Add(babyMonkeyBorn);
            return this;
        }

        public void DisplayFamilyOnScreen()
        {
            HelperStuff helperStuff = new HelperStuff();
            helperStuff.DisplayALine();
            helperStuff.DisplayText("Family of -- " + this.familyName);
            helperStuff.DisplayALine();
            helperStuff.DisplayText("Parents Information");
            DisplaySingleMonkey(this.fatherMonkey);
            DisplaySingleMonkey(this.motherMonkey);
            helperStuff.DisplayALine();
            DisplayAllChildren();
            helperStuff.DisplayALine();
        }

        private void DisplayAllChildren()
        {
            HelperStuff helperStuff = new HelperStuff();
            helperStuff.DisplayText("All the children are shown below");
            foreach (var monkey in this.childMonkeyCollection)
            {
                DisplaySingleMonkey(monkey);
                helperStuff.DisplayALine();
            }
        }

        public void DisplaySingleMonkey(MonkeyWithParentsInfo monkeyWithParentsInfo)
        {
            HelperStuff helperStuff = new HelperStuff();
            helperStuff.DisplayMonkeyWithParentsInfo(monkeyWithParentsInfo);
        }
    }
}
