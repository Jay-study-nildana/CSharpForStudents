using DesignPatterns.CreationalPatterns;
using SOLIDDesignPrinciples;
using System;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //TestTheHelperFromSOLIDDesignPrinciples();

            //TestMonkeyFamilyBuilder();

            //TestMonkeyFactory();

            TestTheCopierMonkey();
        }

        private static void TestTheCopierMonkey()
        {
            HelperStuff helperStuff = new HelperStuff();

            var monkeyRandom = helperStuff.RandomMonkeyGenerator();
            helperStuff.DisplaySingleMonkey(monkeyRandom);
            //now, lets create a Copier Monkey
            var monkeyOriginal = new TheCopierMonkey();
            monkeyOriginal.firstNameOfMonkey = monkeyRandom.firstNameOfMonkey;
            monkeyOriginal.gender = monkeyRandom.gender;
            monkeyOriginal.lastNameOfMonkey = monkeyRandom.lastNameOfMonkey;
            monkeyOriginal.lifeStage = monkeyRandom.lifeStage;

            helperStuff.DisplaySingleMonkey(monkeyOriginal,"The Original Monkey");

            var monkeyCopied1 = monkeyOriginal.DeepCopyTheObject();

            helperStuff.DisplaySingleMonkey(monkeyCopied1,"The Copied Monkey");

            //now, lets take advantage of the copy action. 
            //Suppose the original monkey is the eldest sibling of the family
            //Now, there are two more siblings.
            var monkeyCopied2 = monkeyOriginal.DeepCopyTheObject();
            monkeyCopied1.firstNameOfMonkey = "Brazen";
            monkeyCopied2.firstNameOfMonkey = "Mandren";
            helperStuff.DisplayText("---------------Here are all the siblings-------------");
            helperStuff.DisplaySingleMonkey(monkeyOriginal);
            helperStuff.DisplaySingleMonkey(monkeyCopied1);
            helperStuff.DisplaySingleMonkey(monkeyCopied2);
        }

        private static void TestMonkeyFactory()
        {
            var monkeyWithName1 = MonkeyCreatorWithInterestingNames.MonkeyNameAsItIs("John", "Wayne");
            var monkeyWithName2 = MonkeyCreatorWithInterestingNames.MonkeyNameWithChanges("John", "Wayne");

            monkeyWithName1.DisplayTheMonkey();
            monkeyWithName2.DisplayTheMonkey();
        }

        //testing MonkeyFamilyBuilder
        private static void TestMonkeyFamilyBuilder()
        {
            MonkeyFamilyBuilder familyWayne = new MonkeyFamilyBuilder("Wayne");
            familyWayne.AddMonkeyBaby();
            familyWayne.AddMonkeyBaby();
            //adding using the fluent function
            familyWayne.AddMonkeyBabyFluent().AddMonkeyBabyFluent().AddMonkeyBabyFluent();

            familyWayne.DisplayFamilyOnScreen();
        }

        //I am using an external project to get some components
        //SOLIDDesignPrinciples
        //this function tests that.
        private static void TestTheHelperFromSOLIDDesignPrinciples()
        {
            HelperStuff helperStuff = new HelperStuff();

            helperStuff.DisplayALine();
            helperStuff.DisplayText("This is a Test to see if I am able to use stuff from the separate project SOLIDDesignPrinciples");
            helperStuff.DisplayCollectionOfMonkeys(helperStuff.GiveMeTenMonkeys(),"Showing Some Randomly Generated Monkeys");
            helperStuff.DisplayALine();
        }
    }
}
