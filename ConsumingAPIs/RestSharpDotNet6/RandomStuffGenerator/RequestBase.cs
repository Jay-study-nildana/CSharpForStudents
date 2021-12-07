using AllTheInterfaces.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStuffGenerator
{
    public class RequestBase : IRequest
    {
        public string baseURI { get; set; }
        public string endPoint { get; set; }
        public Method Method { get; set; }
        public string token { get; set; }
        public DataFormat DataFormat { get; set; }
    }
}
