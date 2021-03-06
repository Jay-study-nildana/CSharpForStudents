using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class ProjectDetailViewModel
    {
        public string ProjectTitle { set; get; }
        public string ProjectDescription { set; get; }
        public int YearOfProject { set; get; }
        public string ProjectNotes1 { set; get; }
        public string ProjectNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }
}