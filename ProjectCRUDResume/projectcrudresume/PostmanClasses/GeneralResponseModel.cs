using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    //use this as a generic response to send to API calls
    //we can fill this up with errors, error codes, success codes and anything
    //an endless list of messages
    public class GeneralResponseModel
    {
        public List<string> ListOfResponses { set; get; }

        //I want at least one message to be in the response.
        public GeneralResponseModel()
        {
            ListOfResponses = new List<string>();
            var firstmessage = "Project WT Responses below";
            ListOfResponses.Add(firstmessage);
        }
    }
}