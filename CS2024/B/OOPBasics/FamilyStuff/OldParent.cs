using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    sealed public class OldParent
    {
        public void DisplayOldParentDetails()
        {
            Console.WriteLine("Old Parents are so old. It's just the passage of time and it is what it is. Also, they don't want children.");
        }
    }


    //this will not work. base class is sealed.
    //public class ChildOfOldParent : OldParent { 
    
    //}
}
