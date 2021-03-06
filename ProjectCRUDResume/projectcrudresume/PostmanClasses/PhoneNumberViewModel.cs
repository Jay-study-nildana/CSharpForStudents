using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class PhoneNumberViewModel
    {
        public string PhoneNumber { set; get; }
        public bool CountryCode { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }
}