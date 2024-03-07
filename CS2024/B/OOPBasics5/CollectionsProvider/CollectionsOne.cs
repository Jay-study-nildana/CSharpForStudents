namespace CollectionsProvider
{
    public class CollectionsOne
    {
        public List<SuperHero> SuperHeroSetOne()
        {
            var ListOfSuperHeroes = new List<SuperHero>();

            //we need at least 5 super heroes
            var SuperHero1 = new SuperHero();
            var SuperHero2 = new SuperHero();
            var SuperHero3 = new SuperHero();
            var SuperHero4 = new SuperHero();
            var SuperHero5 = new SuperHero();

            SuperHero1.Name = "Batman";
            SuperHero1.AlterEgo = "Bruce Wayne";
            SuperHero1.NumberHero = 1;

            SuperHero2.Name = "Superman";
            SuperHero2.AlterEgo = "Clark Kent";
            SuperHero2.NumberHero = 2;

            SuperHero3.Name = "Wonder Woman";
            SuperHero3.AlterEgo = "Diana";
            SuperHero3.NumberHero = 3;

            SuperHero4.Name = "The Flash";
            SuperHero4.AlterEgo = "Barry Allen";
            SuperHero4.NumberHero = 4;

            SuperHero5.Name = "Green Lantern";
            SuperHero5.AlterEgo = "Hal Jordan";
            SuperHero5.NumberHero = 5;

            ListOfSuperHeroes.Add(SuperHero1);
            ListOfSuperHeroes.Add(SuperHero2);
            ListOfSuperHeroes.Add(SuperHero3);
            ListOfSuperHeroes.Add(SuperHero4);
            ListOfSuperHeroes.Add(SuperHero5);

            return ListOfSuperHeroes;
        }

        public List<SuperHero2> SuperHeroSetTwo()
        {
            var ListOfSuperHeroes = new List<SuperHero2>();

            //we need at least 5 super heroes
            var SuperHero1 = new SuperHero2();
            var SuperHero2 = new SuperHero2();
            var SuperHero3 = new SuperHero2();
            var SuperHero4 = new SuperHero2();
            var SuperHero5 = new SuperHero2();

            //we need some heroes from marvel as well

            var SuperHero6 = new SuperHero2();
            var SuperHero7 = new SuperHero2();
            var SuperHero8 = new SuperHero2();

            SuperHero1.Name = "Batman";
            SuperHero1.AlterEgo = "Bruce Wayne";
            SuperHero1.NumberHero = 1;
            SuperHero1.Brand = "DC";

            SuperHero2.Name = "Superman";
            SuperHero2.AlterEgo = "Clark Kent";
            SuperHero2.NumberHero = 2;
            SuperHero2.Brand = "DC";

            SuperHero3.Name = "Wonder Woman";
            SuperHero3.AlterEgo = "Diana";
            SuperHero3.NumberHero = 3;
            SuperHero3.Brand = "DC";

            SuperHero4.Name = "The Flash";
            SuperHero4.AlterEgo = "Barry Allen";
            SuperHero4.NumberHero = 4;
            SuperHero4.Brand = "DC";

            SuperHero5.Name = "Green Lantern";
            SuperHero5.AlterEgo = "Hal Jordan";
            SuperHero5.NumberHero = 5;
            SuperHero5.Brand = "DC";

            SuperHero6.Name = "Iron Man";
            SuperHero6.AlterEgo = "Tony Stark";
            SuperHero6.NumberHero = 6;
            SuperHero6.Brand = "Marvel";

            SuperHero7.Name = "Captain America";
            SuperHero7.AlterEgo = "Steve Rogers";
            SuperHero7.NumberHero = 7;
            SuperHero7.Brand = "Marvel";

            SuperHero8.Name = "Thor";
            SuperHero8.AlterEgo = "Odinson";
            SuperHero8.NumberHero = 8;
            SuperHero8.Brand = "Marvel";

            ListOfSuperHeroes.Add(SuperHero1);
            ListOfSuperHeroes.Add(SuperHero2);
            ListOfSuperHeroes.Add(SuperHero3);
            ListOfSuperHeroes.Add(SuperHero4);
            ListOfSuperHeroes.Add(SuperHero5);
            ListOfSuperHeroes.Add(SuperHero6);
            ListOfSuperHeroes.Add(SuperHero7);
            ListOfSuperHeroes.Add(SuperHero8);

            return ListOfSuperHeroes;
        }
    }
}
