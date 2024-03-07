using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupOfMug
{
    //compare and contrast this struct with class
    //TODO why do we bother with structs in dot net? we have classes, don't we?
    public struct MugOfStruct
    {
        public string MaterialUsed { get; set; }
        public int Weight { get; set; }

        public void SetMugDetails()
        {

            MaterialUsed = "Ceramic";
            Weight = 69;
        }

        public void DisplayMugDetails()
        {
            Console.WriteLine("Material : " + MaterialUsed + " Weight : " + Weight);
        }
    }
}
