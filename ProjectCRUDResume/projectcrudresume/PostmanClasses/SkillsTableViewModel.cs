using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class SkillsTableViewModel
    {
        public string SkillTitle { set; get; }
        public string SkillDescription { set; get; }
        public int SkillExperience { set; get; }
        public string SkillOtherNotes1 { set; get; }
        public string SkillOtherNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }
}