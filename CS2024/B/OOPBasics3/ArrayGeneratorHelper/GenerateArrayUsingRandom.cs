using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO : As an assignment, students can build their own random name generator

//use this class to generate strings and ints and whatever else you are trying to create to reduce amount of typing

namespace ArrayGeneratorHelper
{
    public class GenerateArrayUsingRandom
    {
        private string[] ListOfPowers = ["Ice", "Fire", "Flight", "Invisible", "Volcano", "Flower"];

        public string[] GenerateSuperHeroes(int numberOfHeroes)
        {
            var ArrayOfHeroes = new string[numberOfHeroes];

            for(int i = 0;i <numberOfHeroes;i++)
            {
                Random rnd = new Random();
                var prefix = "Man";
                var randomnumber = rnd.Next(ListOfPowers.Length);
                var randomgender = rnd.Next(4);
                if(randomgender == 0)
                {
                    prefix = "Woman";
                    var SuperHeroName = ListOfPowers[randomnumber]+prefix;
                    ArrayOfHeroes[i] = SuperHeroName;
                }
                else if (randomgender == 1)
                {
                    prefix = "Guy";
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    ArrayOfHeroes[i] = SuperHeroName;
                }
                else if (randomgender == 2)
                {
                    prefix = "Girl";
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    ArrayOfHeroes[i] = SuperHeroName;
                }
                else 
                {
                    var SuperHeroName = ListOfPowers[randomnumber] + prefix;
                    ArrayOfHeroes[i] = SuperHeroName;
                }

            }

            return ArrayOfHeroes;
        }

        public int[] GenerateRandomNumbers(int numberOfRandomNumbers)
        {
            int[] ArrayOfRandomNumbers = new int[numberOfRandomNumbers];
            int SomeRandomUpperLimit = 69;

            for(int i =0; i < numberOfRandomNumbers; i++)
            {
                Random rnd = new Random();
                var randomnumber = rnd.Next(SomeRandomUpperLimit);
                ArrayOfRandomNumbers[i] = randomnumber;
            }

            return ArrayOfRandomNumbers;
        }
    }
}
