using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    //multipel inheritance with interfaces
    public class ParentPartTwo : IGrandParentActivities, IParentActivities
    {
        public void TellChildrenToStudy()
        {
            Console.WriteLine(" Kids, go and study");
        }

        public void TellKidsToHaveFun()
        {
            Console.WriteLine("Kids, forget studying. just go and have fun");
        }

        void IGrandParentActivities.PrayToGodForChildWelfare()
        {
            throw new NotImplementedException();
        }

        void IParentActivities.PrayToGodForChildWelfare()
        {
            throw new NotImplementedException();
        }
    }
}
