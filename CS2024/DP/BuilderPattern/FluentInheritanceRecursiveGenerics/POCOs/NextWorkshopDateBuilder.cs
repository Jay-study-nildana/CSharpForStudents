using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentInheritanceRecursiveGenerics.POCOs
{
    public class NextWorkshopDateBuilder<SELF>
        : TopicBeingTaughtBuilder<NextWorkshopDateBuilder<SELF>>
        where SELF : NextWorkshopDateBuilder<SELF>
    {
        public SELF UpdateNextDateOfWorkshop(DateTime dateOfWorkshopComingUp)
        {
            tutor.DateOfNextWorkshop = dateOfWorkshopComingUp;
            return (SELF)this;
        }
    }
}
