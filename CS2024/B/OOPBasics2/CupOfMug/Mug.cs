using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupOfMug
{
    //class definition continues in file Mug2.cs
    //because this is partial
    public partial class Mug
    {
        public string MaterialUsed { get; set; }
        public int Weight { get; set; }

        public Mug() {

            MaterialUsed = "Ceramic";
            Weight = 69;
        }

        //partial method. definition has been added in Mug2.cs
        public partial void MugDisplayPartTwo();

        //Black = 1,
        //Blue = 2, 
        //Green = 3, 
        //BlueGreen = 4, 
        //BlueYellow = 5        
        
        //display all the enums

        public void DisplayAllEnums()
        {
            var EnumStringThing = "Black is " + MugColorEnum.Black + " Blue is " + MugColorEnum.Blue + " Green is " + MugColorEnum.Green;
            Console.WriteLine(EnumStringThing);

            int BlackValue = (int)MugColorEnum.Black;

            Console.WriteLine("The integer value of the Black enum is : "+BlackValue);
        }

        //override the equals method
        //useful when comparing two mug objects

        public override bool Equals(object? obj)
        {
            Mug objMug = obj as Mug;
            if(objMug.MaterialUsed == this.MaterialUsed &&  objMug.Weight == this.Weight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //override the string method
        //useful when building custom string options

        public override string ToString()
        {
            var TempString = "Material : " + MaterialUsed + " Weight : " + Weight;
            return TempString;
        }
    }
}
