using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestSharpUsageDOTNET452
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            RestSharpSimple();
        }

        private static void RestSharpSimple()
        {
            try
            {
                var client = new RestClient("https://randomstuffgeneratorsep23.azurewebsites.net");

                var request = new RestRequest("api/ProjectDetails/Hi", DataFormat.Json);

                var response = client.Get(request);

                var something = 0;
            }

            catch(Exception e)
            {
                var response = e.ToString();
            }



        }
    }
}
