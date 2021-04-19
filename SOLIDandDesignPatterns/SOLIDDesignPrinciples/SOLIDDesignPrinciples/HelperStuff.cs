using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDDesignPrinciples
{

    //i use this as a temporary holder of many methods
    //as I build new functionality and behavior, i begin by creating required methods here.
    //and if they are class specific and not globally required
    //they are moved to their corresponding class
    //so, think of this as a playground before we know which behavior belongs to which class

    //TODO - see all those MonkeyBaseClass returning functions. 
    //They should be moved to the class MonkeyBaseClass
    public class HelperStuff
    {
        public void DisplayText(string messagetoshow)
        {
            Console.WriteLine(messagetoshow);
        }

        public void DisplayALine()
        {
            Console.WriteLine("---------------------------------");
        }

        //I want a collection of randomly generated monkeys to use in this project.
        public List<MonkeyBaseClass> GiveMeTenMonkeys()
        {
            List<MonkeyBaseClass> monkeys = new List<MonkeyBaseClass>();

            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());
            monkeys.Add(RandomMonkeyGenerator());

            return monkeys;
        }

        //randomly generate a single monkey
        public MonkeyBaseClass RandomMonkeyGenerator()
        {
            //get a random life stage.
            LifeStage lifeStage = GetARandomLifeStage();
            //get a random gender.
            Gender gender = GetARandomGender();
            //get a random name.
            string firstNameOfMonkey = GetARandomMonkeyFirstName();
            string lastNameOfMonkey = GetARandomMonkeyLastName();
            MonkeyBaseClass monkeyBaseClass = new MonkeyBaseClass(firstNameOfMonkey, lastNameOfMonkey, lifeStage, gender);
            return monkeyBaseClass;
        }

        //generate a monkey for which we already have a last name in advance
        //primarily used for created children monkeys for existing parents.
        public MonkeyBaseClass RandomMonkeyGenerator(string lastNameOfMonkey)
        {
            //this is a baby, so we set it to baby.
            LifeStage lifeStage = LifeStage.Baby;
            //get a random gender.
            Gender gender = GetARandomGender();
            //get a random name.
            string firstNameOfMonkey = GetARandomMonkeyFirstName();
            MonkeyBaseClass monkeyBaseClass = new MonkeyBaseClass(firstNameOfMonkey, lastNameOfMonkey, lifeStage, gender);
            return monkeyBaseClass;
        }

        //generate a monkey for which we already have a last name in advance
        //further, we know the gender in advance. useful when we are starting a brand new family.
        //and we need to specific the gender for the sake of baby making
        public MonkeyBaseClass RandomMonkeyGenerator(string lastNameOfMonkey,Gender gender)
        {
            //this is specifically used when starting a new family
            //so the monkeys are definitely adults.
            LifeStage lifeStage = LifeStage.Adult;
            //get a random name.
            string firstNameOfMonkey = GetARandomMonkeyFirstName();
            MonkeyBaseClass monkeyBaseClass = new MonkeyBaseClass(firstNameOfMonkey, lastNameOfMonkey, lifeStage, gender);
            return monkeyBaseClass;
        }

        public void DisplayMonkeyWithParentsInfo(MonkeyWithParentsInfo monkeyWithParentsInfo)
        {
            DisplayText("Full Name - "+ monkeyWithParentsInfo.firstNameOfMonkey + " " + monkeyWithParentsInfo.lastNameOfMonkey);
            //TODO - use the IDs to pick up the actual father and mother name
            DisplayText("Father ID - "+ monkeyWithParentsInfo.fatherMonkeyIdentifier);
            DisplayText("Mother ID - "+ monkeyWithParentsInfo.motherMonkeyIdentifier);
            DisplayText("Gender - "+ monkeyWithParentsInfo.gender.ToString());
            DisplayText("Life Stage - "+ monkeyWithParentsInfo.lifeStage.ToString());
            DisplayText("Monkey ID - "+ monkeyWithParentsInfo.monkeyUniqueIdentifier);
        }

        //similar to RandomMonkeyGenerator
        //generate a random monkey but a family monkey must be provided.
        //a family name is provided to indicate part of being a family
        public MonkeyWithParentsInfo RandomMonkeyWithParentsInfoGenerator(string familyName)
        {
            MonkeyWithParentsInfo monkeyWithParentsInfo = new MonkeyWithParentsInfo();

            //first a basic monkey.
            var tempBasicMonkey = RandomMonkeyGenerator(familyName);
            monkeyWithParentsInfo.firstNameOfMonkey = tempBasicMonkey.firstNameOfMonkey;
            monkeyWithParentsInfo.gender = tempBasicMonkey.gender;
            monkeyWithParentsInfo.lastNameOfMonkey = tempBasicMonkey.lastNameOfMonkey;
            monkeyWithParentsInfo.lifeStage = tempBasicMonkey.lifeStage;

            return monkeyWithParentsInfo;
        }

        //further, we know the gender in advance. useful when we are starting a brand new family.
        //and we need to specific the gender for the sake of baby making
        public MonkeyWithParentsInfo RandomMonkeyWithParentsInfoGenerator(string familyName,Gender gender)
        {
            MonkeyWithParentsInfo monkeyWithParentsInfo = new MonkeyWithParentsInfo();

            //first a basic monkey.
            var tempBasicMonkey = RandomMonkeyGenerator(familyName,gender);

            monkeyWithParentsInfo.firstNameOfMonkey = tempBasicMonkey.firstNameOfMonkey;
            monkeyWithParentsInfo.gender = tempBasicMonkey.gender;
            monkeyWithParentsInfo.lastNameOfMonkey = tempBasicMonkey.lastNameOfMonkey;
            monkeyWithParentsInfo.lifeStage = tempBasicMonkey.lifeStage;

            return monkeyWithParentsInfo;
        }

        public string GetARandomMonkeyName()
        {
            string tempFirstName = GetARandomMonkeyFirstName();
            string tempLastName = GetARandomMonkeyLastName();
            string tempMonkeyName = tempFirstName + tempLastName;

            return tempMonkeyName;
        }

        public string GetARandomMonkeyFirstName()
        {
            List<string> monkeyFirstNames = GetListOfRandomFirstNames();
            Random randomFirstNames = new Random();
            string tempFirstName = monkeyFirstNames[randomFirstNames.Next(monkeyFirstNames.Count)];

            return tempFirstName;
        }

        public string GetARandomMonkeyLastName()
        {
            List<string> monkeyLastNames = GetListOfRandomSecondNames();
            Random randomLastNames = new Random();
            string tempLastName = monkeyLastNames[randomLastNames.Next(monkeyLastNames.Count)];

            return tempLastName;
        }

        public LifeStage GetARandomLifeStage()
        {
            //get a random life stage.
            Array valuesLifeStage = Enum.GetValues(typeof(LifeStage));
            Random randomLifeStage = new Random();
            LifeStage lifeStage = (LifeStage)valuesLifeStage.GetValue(randomLifeStage.Next(valuesLifeStage.Length));

            return lifeStage;
        }

        public Gender GetARandomGender()
        {
            //get a random gender.
            Array valuesGender = Enum.GetValues(typeof(Gender));
            Random randomGender = new Random();
            Gender gender = (Gender)valuesGender.GetValue(randomGender.Next(valuesGender.Length));

            return gender;
        }


        //get a list of random first names.
        public List<string> GetListOfRandomFirstNames()
        {
            List<string> monkeyFirstNames = new List<string>();

            monkeyFirstNames.Add("Sterling");
            monkeyFirstNames.Add("Lana");
            monkeyFirstNames.Add("Bruce");
            monkeyFirstNames.Add("Pam");
            monkeyFirstNames.Add("John");
            monkeyFirstNames.Add("Veronica");
            monkeyFirstNames.Add("Cyril");
            monkeyFirstNames.Add("Jay");
            monkeyFirstNames.Add("Slam");
            monkeyFirstNames.Add("Apple");

            return monkeyFirstNames;
        }

        //get a list of random second names.
        public List<string> GetListOfRandomSecondNames()
        {
            List<string> monkeyLastNames = new List<string>();

            monkeyLastNames.Add("Archer");
            monkeyLastNames.Add("Kane");
            monkeyLastNames.Add("Wayne");
            monkeyLastNames.Add("Poovie");
            monkeyLastNames.Add("Turok");
            monkeyLastNames.Add("Deane");
            monkeyLastNames.Add("Figgis");
            monkeyLastNames.Add("Dude");
            monkeyLastNames.Add("Dunk");
            monkeyLastNames.Add("Comp");

            return monkeyLastNames;
        }

        //display the monkey collection on the screen.
        public void DisplayCollectionOfMonkeys(List<MonkeyBaseClass> collectionOfMonkeys,string titleOfDisplay)
        {
            DisplayText("-------------------BEGINNING DISPLAY---------------------");
            DisplayText("************"+titleOfDisplay+ "************");
            var TotalMonkeys = "Total Number of Monkeys - " + collectionOfMonkeys.Count;
            DisplayText(TotalMonkeys);
            foreach(var monkey in collectionOfMonkeys)
            {
                //DisplayALine();
                //DisplayText("Name Of Monkey - " + monkey.firstNameOfMonkey + " " + monkey.lastNameOfMonkey );
                //DisplayText("Gender Of Monkey - " + monkey.gender);
                //DisplayText("Life Stage Of Monkey  - " + monkey.lifeStage);
                //DisplayALine();
                DisplaySingleMonkey(monkey);
            }
            DisplayText("-------------------All DONE---------------------");
        }

        public void DisplaySingleMonkey(MonkeyBaseClass monkey)
        {
            DisplayALine();
            DisplayText("Name Of Monkey - " + monkey.firstNameOfMonkey + " " + monkey.lastNameOfMonkey);
            DisplayText("Gender Of Monkey - " + monkey.gender);
            DisplayText("Life Stage Of Monkey  - " + monkey.lifeStage);
            DisplayALine();
        }

        public void DisplaySingleMonkey(MonkeyBaseClass monkey,string titleMessage)
        {
            DisplayALine();
            DisplayText(titleMessage);
            DisplayALine();
            DisplayText("Name Of Monkey - " + monkey.firstNameOfMonkey + " " + monkey.lastNameOfMonkey);
            DisplayText("Gender Of Monkey - " + monkey.gender);
            DisplayText("Life Stage Of Monkey  - " + monkey.lifeStage);
            DisplayALine();
        }

    }
}
