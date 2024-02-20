using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentInheritanceRecursiveGenerics.POCOs
{
    public class TutorInfoBuilder<SELF> : TutorBuilder
      where SELF : TutorInfoBuilder<SELF>
    {
        public SELF UpdateTutorName(string nameOfTutor)
        {
            tutor.TutorName = nameOfTutor;
            return (SELF)this;
        }
    }

}

