using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    public class FamilyMember
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual void FamilyMoto()
        {
            Console.WriteLine("Family Stays Together");
        }
    }
}
