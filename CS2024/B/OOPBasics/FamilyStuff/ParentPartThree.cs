using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    //explicit implementation. useful when interfaces have similar function names
    public class ParentPartThree : IGrandParentActivities, IParentActivities
    {
        const string PreString = "ParentPartThree : ";
        void IGrandParentActivities.PrayToGodForChildWelfare()
        {
            Console.WriteLine(PreString+"Hope kids have lot of fun");
        }

        void IParentActivities.PrayToGodForChildWelfare()
        {
            Console.WriteLine(PreString+"Hope kids study well");
        }

        void IParentActivities.TellChildrenToStudy()
        {
            Console.WriteLine(PreString + "Go and study a lot");
        }

        void IGrandParentActivities.TellKidsToHaveFun()
        {
            Console.WriteLine(PreString + "kids, just go and have fun. forget studying");
        }

        public void ParentDisplayStuff()
        {
            ((IGrandParentActivities)this).PrayToGodForChildWelfare();
            ((IGrandParentActivities)this).TellKidsToHaveFun();
            ((IParentActivities)this).PrayToGodForChildWelfare();
            ((IParentActivities)this).TellChildrenToStudy();
        }
    }
}
