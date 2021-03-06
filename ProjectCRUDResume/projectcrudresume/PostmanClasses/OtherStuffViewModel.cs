﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class OtherStuffViewModel
    {
        public string OtherStuffNotes1 { set; get; }
        public string OtherStuffNotes2 { set; get; }
        public string OtherStuffNotes3 { set; get; }
        public string OtherStuffNotes4 { set; get; }
        public string OtherStuffNotes5 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }
}