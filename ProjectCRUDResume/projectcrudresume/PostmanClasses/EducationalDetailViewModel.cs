using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class EducationalDetailViewModel
    {
        public string EducationTitle { set; get; }
        public string InstituationName { set; get; }
        public int YearOfGraduation { set; get; }
        public string PassGrade { set; get; }
        public string EducationOtherNotes1 { set; get; }
        public string EducationOtherNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }
}