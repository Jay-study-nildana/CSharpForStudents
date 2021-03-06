using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class AddressViewModel
    {

        public string AddressLineOne { set; get; }
        public string AddressLineTwo { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Pincode { set; get; }
        public string Landmark { set; get; }
        public string AddressExtraNotes { set; get; }
        //Set this bool to true to indicate that this is a primary address
        //each person can have multiple addresses
        public bool PrimaryAddress { set; get; }
        public string UniqueGuid { set; get; }
    }
}