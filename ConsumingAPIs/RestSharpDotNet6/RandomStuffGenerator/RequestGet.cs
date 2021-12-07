using AllTheInterfaces.Interfaces;
using APIConsumerHelper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStuffGenerator.Request
{
    public class RequestGet : RequestBase
    {
        public RequestGet()
        {
            ConstantHelper constantHelper = new ConstantHelper();
            baseURI = constantHelper.returnbaseURI();
            Method = Method.GET;
            DataFormat = DataFormat.Json;
        }
    }
}
