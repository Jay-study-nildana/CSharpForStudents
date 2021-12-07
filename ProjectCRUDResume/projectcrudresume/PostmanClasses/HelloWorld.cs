using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class HelloWorld
    {
        public string Message1 { set; get; }
        public string Message2 { set; get; }

        public int Number1 { set; get; }

        public double Number2 { set; get; }
    }

    //class similar to HelloWorld but to be used with authorization
    public class HelloWorld2
    {
        public string Message1 { set; get; }
        public string Message2 { set; get; }

        public int Number1 { set; get; }

        public double Number2 { set; get; }
    }

    public class HelloWorldUserDetails
    {
        public string MessageDescription { set; get; }

        public string UserName { set; get; }
    }
}