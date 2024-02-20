using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentInheritanceRecursiveGenerics.POCOs
{
    public abstract class TutorBuilder
    {
        protected TutorTeacher tutor = new TutorTeacher();

        public TutorTeacher Build()
        {
            return tutor;
        }
    }

}

