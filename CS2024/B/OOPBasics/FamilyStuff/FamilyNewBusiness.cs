using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    public class FamilyNewBusiness : FamilyMainBusiness
    {
        public void ShowFamilyDetails()
        {
            base.ShowFamilyDetails(); //notice that the basic class function name and this function name is also the same.
            Console.WriteLine("Selling Ice Cream during Winter");

        }
    }
}
