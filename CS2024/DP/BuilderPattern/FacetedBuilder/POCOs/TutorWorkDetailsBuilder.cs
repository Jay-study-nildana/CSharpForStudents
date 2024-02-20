using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacetedBuilder.POCOs
{
    public class TutorWorkDetailsBuilder : TutorBuilder
    {
        public TutorWorkDetailsBuilder(TutorTeacher tutorTeacher)
        {
            this.tutorTeacher = tutorTeacher;
        }

        public TutorWorkDetailsBuilder AddWorkDetails(int YearsofExperience, string FavoriteGame)
        {
            tutorTeacher.YearsofExperience = YearsofExperience;
            tutorTeacher.FavoriteGame = FavoriteGame;   

            return this;
        }
    }
}
