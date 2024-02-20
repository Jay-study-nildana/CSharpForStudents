using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacetedBuilder.POCOs
{
    public class TutorTeacher
    {
        //Tutor personal details
        //will get updated with one builder
        public string TutorName;
        public string TutorCity;
        public string TutorCountry;

        //will get updated with another builder.
        //work details
        public int YearsofExperience;
        public string FavoriteGame;

        public override string ToString()
        {
            return $"{nameof(TutorName)}: {TutorName}, " +
                $"{nameof(TutorCity)}: {TutorCity}, " +
                $"{nameof(TutorCountry)}: {TutorCountry}, " +
                $"{nameof(YearsofExperience)}: {YearsofExperience}, " +
                $"{nameof(FavoriteGame)} : {FavoriteGame}";
        }
    }
}
