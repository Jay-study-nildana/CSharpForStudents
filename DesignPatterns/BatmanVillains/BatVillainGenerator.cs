using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillainsGeneric;
using VillainsGeneric.Interfaces;

//Sources of Batman Villains
//1. https://dc.fandom.com/wiki/Batman_Villains
//1. https://en.wikipedia.org/wiki/List_of_Batman_family_enemies

namespace BatmanVillains
{
    public class BatVillainGenerator : IVillainGenerator
    {
        private Villain tempVillain {set;get;}
        private List<Villain> listOfVillains { set; get; }

        public BatVillainGenerator()
        {
            tempVillain = new Villain();
            listOfVillains = new List<Villain>();
            //add some batman villains.
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Black Mask",
                FirstName = "Roman",
                LastName = "Sionis"
                
            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Catwoman",
                FirstName = "Selina",
                LastName = "Kyle"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Clayface",
                FirstName = "Basil",
                LastName = "Karlo"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Lady Clay",
                FirstName = "Sondra",
                LastName = "Fuller"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Deadshot",
                FirstName = "Floyd",
                LastName = "Lawton"

            });
            listOfVillains.Add(new Villain
            {
                AlterEgo = "Firefly",
                FirstName = "Garfield",
                LastName = "Lynnss"

            });
        }

        public Villain getRandomVillain()
        {
            //tempVillain.FirstName = "VFirstName";
            //tempVillain.LastName = "VLastName";
            //tempVillain.AlterEgo = "VEgo";

            //get a random number that is less than the total number of villains available.
            //return that villain.
            var tempRandomVillainNumber = new Random().Next() % this.listOfVillains.Count;
            tempVillain = listOfVillains.ElementAt(tempRandomVillainNumber);

            return tempVillain;
        }
    }
}
