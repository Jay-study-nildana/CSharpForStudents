using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacetedBuilder.POCOs
{
    //so, this is not the actual builder as we have seen in other examples
    //this is simply a facade
    //now, facade means, a face that pretends to be the real thing. 
    //like in movies where the good guy is actually the most terrible. for example, your parents. 

    public class TutorBuilder
    {
        protected TutorTeacher tutorTeacher = new TutorTeacher();

        //here are the two actual builders that will build the Tutor object
        public TutorPersonalDetailsBuilder PersonalDetails => new TutorPersonalDetailsBuilder(tutorTeacher);
        public TutorWorkDetailsBuilder WorkDetails => new TutorWorkDetailsBuilder(tutorTeacher);

        //here we return the actual tutor after the building is done. 
        public static implicit operator TutorTeacher(TutorBuilder builder)
        {
            return builder.tutorTeacher;
        }
    }
}
