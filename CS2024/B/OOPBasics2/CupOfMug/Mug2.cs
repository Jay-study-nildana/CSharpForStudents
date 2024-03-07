using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupOfMug
{
    public partial class Mug
    {
        public void DisplayMugDetails()
        {
            Console.WriteLine("Material : " + MaterialUsed + " Weight : " + Weight);
        }

        //definition of the partial method
        public partial void MugDisplayPartTwo()
        {
            Console.WriteLine("Material : " + MaterialUsed + " Weight : " + Weight);
        }
    }
}
