using System;
using System.Linq;

namespace SOLIDDesignPrinciples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("This code uses a group of monkeys working together as an example throughout!");

            //CheckOutSingleResponsibilityPrinciple();

            //CheckOutOpenClosedPrinciple();

            //CheckOutLiskovSubstitutionPrinciple();

            //CheckOutDependencyInversionPrinciple();

        }

        private static void CheckOutDependencyInversionPrinciple()
        {
            IListOfAllMonkeys listOfAllMonkeys = new ListOfAllMonkeysFromHelper();

            IGetAllMaleMonkeys getAllMaleMonkeys = new GetAllMaleMonkeys1();

            var resultingMonkeys = getAllMaleMonkeys.GetAllMaleMonkeys(listOfAllMonkeys.listOfAllMonkeys());

            HelperStuff helperStuff = new HelperStuff();

            helperStuff.DisplayALine();

            helperStuff.DisplayText("This is DependencyInversionPrinciple");
            helperStuff.DisplayCollectionOfMonkeys(listOfAllMonkeys.listOfAllMonkeys(), "These are all the monkeys in the collection");
            helperStuff.DisplayCollectionOfMonkeys(resultingMonkeys, "These are all the male monkeys");

            helperStuff.DisplayALine();

        }

        private static void CheckOutLiskovSubstitutionPrinciple()
        {
            HelperStuff helperStuff = new HelperStuff();

            //get two randome monkeys to test this. 

            var randomMonkey1 = helperStuff.RandomMonkeyGenerator();
            var randomMonkey2 = helperStuff.RandomMonkeyGenerator();

            ParentMonkeyNames giveMeTheMonkeyNames = new ParentMonkeyNames();

            var output1 = giveMeTheMonkeyNames.HereAreTheNames(randomMonkey1, randomMonkey2);

            helperStuff.DisplayALine();
            helperStuff.DisplayText("This is LiskovSubstitutionPrinciple");

            helperStuff.DisplayText("This is Parent Class");
            helperStuff.DisplayText(output1);

            //this is the child that breaks the expected output.
            Child1MonkeyNames giveMeTheMonkeyNames2 = new Child1MonkeyNames();
            var output2 = giveMeTheMonkeyNames2.HereAreTheNames(randomMonkey1, randomMonkey2);
            helperStuff.DisplayText("This is First Child - Introducing Breaking Changes and Violating Liskov");
            helperStuff.DisplayText(output2);

            //this child retains the original functionality
            //and adds a new behavior
            Child2MonkeyNames giveMeTheMonkeyNames3 = new Child2MonkeyNames();
            var output3 = giveMeTheMonkeyNames3.HereAreTheNames(randomMonkey1, randomMonkey2);
            var output4 = giveMeTheMonkeyNames3.ReturnRandomNameSetence(randomMonkey1, randomMonkey2);
            helperStuff.DisplayText("This is Second Child - Introducing new Behavior WithOut Breaking Changes and Not Violating Liskov");
            helperStuff.DisplayText(output3);
            helperStuff.DisplayText(output4);

            helperStuff.DisplayALine();
        }

        private static void CheckOutOpenClosedPrinciple()
        {
            HelperStuff helperStuff = new HelperStuff();

            //lets do OpenClosedPrinciple

            var listOfMonkeys = helperStuff.GiveMeTenMonkeys();

            //lets filter based on the gender criteria

            //get the filter
            IMonkeyFiltering<MonkeyBaseClass> filterForMonkeys = new MonkeyFilter();
            //get the critria
            ICriteriaForFiltering<MonkeyBaseClass> criteriaForFilteringMale = new GenderCriteria(Gender.Male);
            ICriteriaForFiltering<MonkeyBaseClass> criteriaForFilteringFemale = new GenderCriteria(Gender.Female);

            //now, lets get all male monkeys
            var allMaleMonkeys = filterForMonkeys.Filter(listOfMonkeys, criteriaForFilteringMale).ToList<MonkeyBaseClass>();

            //now, lets get all female monkeys
            var allFemaleMonkeys = filterForMonkeys.Filter(listOfMonkeys, criteriaForFilteringFemale).ToList<MonkeyBaseClass>();

            helperStuff.DisplayALine();
            helperStuff.DisplayText("This is OpenClosedPrinciple");

            helperStuff.DisplayCollectionOfMonkeys(listOfMonkeys,"Showing all the randomly generated monkeys");
            helperStuff.DisplayCollectionOfMonkeys(allMaleMonkeys,"After filtering, showing all the male monkeys");
            helperStuff.DisplayCollectionOfMonkeys(allFemaleMonkeys, "After filtering, showing all the female monkeys");

            helperStuff.DisplayALine();

            //TODO 
            //lets filter based on the lifestage criteria

            //this is here to help me with putting breakpoints.
            // var temp123 = 0;
        }

        static void CheckOutSingleResponsibilityPrinciple()
        {
            HelperStuff helperStuff = new HelperStuff();

            helperStuff.DisplayALine();
            helperStuff.DisplayText("This is SingleResponsibilityPrinciple");
            //lets do SingleResponsibilityPrinciple

            TurnOnOffLights turnOnOffLights = new TurnOnOffLights();
            turnOnOffLights.TurnOnTheLightsMonkey();
            helperStuff.DisplayText(turnOnOffLights.MessageAboutLightStatus());
            turnOnOffLights.TurnOffTheLightsMonkey();
            helperStuff.DisplayText(turnOnOffLights.MessageAboutLightStatus());

            TurnOnOffLightsBad turnOnOffLightsBad = new TurnOnOffLightsBad();
            turnOnOffLightsBad.TurnOnTheLightsMonkey();
            helperStuff.DisplayText(turnOnOffLightsBad.MessageAboutLightStatus());
            helperStuff.DisplayText(turnOnOffLightsBad.MessageAboutLightStatus());
            turnOnOffLights.TurnOffTheLightsMonkey();
            //this breaks the SingleResponsibilityPrinciple
            //ideally, this should be a separate class.
            turnOnOffLightsBad.ChangeTheBulbsMonkey();

            helperStuff.DisplayALine();
        }
    }
}
