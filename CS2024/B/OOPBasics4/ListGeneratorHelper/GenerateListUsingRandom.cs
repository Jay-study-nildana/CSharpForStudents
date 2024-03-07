namespace ListGeneratorHelper
{
    //note, I am reusing the Class Library Helper from the VS solution, OOPBasics3
    //that one was about randomly generation arrays
    //this one is about randomly generation list
    public class GenerateListUsingRandom
    {
        private string[] ListOfPowers = ["Ice", "Fire", "Flight", "Invisible", "Volcano", "Flower"];

        public List<string> GenerateSuperHeroes(int numberOfHeroes)
        {
            var ListOfHeroes = new List<string>();

            for (int i = 0; i < numberOfHeroes; i++)
            {
                Random rnd = new Random();
                var prefix = "Man";
                var randomnumber = rnd.Next(ListOfPowers.Length);
                var randomgender = rnd.Next(4);
                if (randomgender == 0)
                {
                    prefix = "Woman";
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    //ListOfHeroes[i] = SuperHeroName;
                    ListOfHeroes.Add(SuperHeroName);
                }
                else if (randomgender == 1)
                {
                    prefix = "Guy";
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    //ListOfHeroes[i] = SuperHeroName;
                    ListOfHeroes.Add(SuperHeroName);
                }
                else if (randomgender == 2)
                {
                    prefix = "Girl";
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    //ListOfHeroes[i] = SuperHeroName;
                    ListOfHeroes.Add(SuperHeroName);
                }
                else
                {
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    //ListOfHeroes[i] = SuperHeroName;
                    ListOfHeroes.Add(SuperHeroName);
                }

            }

            return ListOfHeroes;
        }

        public List<int> GenerateRandomNumbers(int numberOfRandomNumbers)
        {
            var ListOfRandomNumbers = new List<int>();
            int SomeRandomUpperLimit = 69;

            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                Random rnd = new Random();
                var randomnumber = rnd.Next(SomeRandomUpperLimit);
                //ListOfRandomNumbers[i] = randomnumber;
                ListOfRandomNumbers.Add(randomnumber);
            }

            return ListOfRandomNumbers;
        }
    }
}
