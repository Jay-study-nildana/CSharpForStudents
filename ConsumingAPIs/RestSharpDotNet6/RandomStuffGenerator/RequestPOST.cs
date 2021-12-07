using APIConsumerHelper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStuffGenerator
{
    public class RequestPOST : RequestBase
    {
        public string JSONString { set; get; }

        public RequestPOST()
        {
            ConstantHelper constantHelper = new ConstantHelper();
            baseURI = constantHelper.returnbaseURI();
            Method = Method.POST;
            DataFormat = DataFormat.Json;
        }
    }
}
