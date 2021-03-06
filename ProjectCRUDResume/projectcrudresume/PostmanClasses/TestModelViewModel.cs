using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    //this is the postman view model for TestModel class that links to the database.
    public class TestModelViewModel
    {
        public string Message1 { set; get; }
        public string Message2 { set; get; }
        public int Number1 { set; get; }
        public double Number2 { set; get; }

        //lets add user name and email 
        public string UserEmail { set; get; }

        //a status message. not to be sent by user but to be returned by the system
        public string StatusMessage { set; get; }
    }
}