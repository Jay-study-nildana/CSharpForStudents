using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillainsGeneric;
using VillainsGeneric.Interfaces;

namespace SupermanVillains
{
    public class SupesVillainGenerator : IVillainGenerator
    {
        private Villain tempVillain { set; get; }
        private List<Villain> listOfVillains { set; get; }

        public SupesVillainGenerator()
        {
            tempVillain = new Villain();
            listOfVillains = new List<Villain>();
            //add some batman villains.
            listOfVillains.Add(new Villain
            {
                AlterEgo = "None",
                FirstName = "Lex",
                LastName = "Luthor"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "None",
                FirstName = "General",
                LastName = "Zod"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Bizarro",
                FirstName = "None",
                LastName = "None"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Darkseid",
                FirstName = "Uxas",
                LastName = "Khan"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Maxima",
                FirstName = "None",
                LastName = "None"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Plastique",
                FirstName = "Bette",
                LastName = "Souci"

            });
        }

        public Villain getRandomVillain()
        {
            var tempRandomVillainNumber = new Random().Next() % this.listOfVillains.Count;
            tempVillain = listOfVillains.ElementAt(tempRandomVillainNumber);

            return tempVillain;
        }
    }
}
