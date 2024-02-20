using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacetedBuilder.POCOs
{
    public class TutorPersonalDetailsBuilder : TutorBuilder
    {
        public TutorPersonalDetailsBuilder(TutorTeacher tutorTeacher)
        {
            this.tutorTeacher = tutorTeacher;
        }

        public TutorPersonalDetailsBuilder AddPersonalDetails(string TutorName, string TutorCity, string TutorCountry)
        {
            tutorTeacher.TutorName = TutorName;
            tutorTeacher.TutorCity = TutorCity;
            tutorTeacher.TutorCountry = TutorCountry;

            return this;
        }
    }
}
