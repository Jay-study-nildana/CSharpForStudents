namespace SuperHeroClassLibrary
{
    public class Hero
    {
        public string Name { get; set; }
        public string PrimaryPower { get; set; }
        public string SecondaryPower { get; set;}

        //static fields. 
        public static string BrandOfSuperHeroes { get; set; }

        //default constructor. usually, you don't need to write this
        //but since we are overloading constructors, we must have the default one here.
        public Hero() { 
        }

        //instance constructor

        public Hero(string Name, string PrimaryPower, string SecondaryPower) { 

            this.Name = Name;
            this.PrimaryPower = PrimaryPower;
            this.SecondaryPower = SecondaryPower;
        }

        public Hero(string Name)
        {
            this.Name=Name;
            this.PrimaryPower = "Power of Flight";
            this.SecondaryPower = "Power of Invisibility";
        }

        //for updating values of static fields.
        static Hero()
        {
            BrandOfSuperHeroes = "DC Comics";
        }
    }



}
