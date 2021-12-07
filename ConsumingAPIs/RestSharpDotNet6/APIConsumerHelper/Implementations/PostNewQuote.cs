using AllTheInterfaces.Interfaces;
using RandomStuffGenerator;
using RandomStuffGenerator.General;
using RandomStuffGenerator.Request;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIConsumerHelper.Implementations
{
    public class PostNewQuote : IDoPost
    {
        public async Task<IResponse> PerformPostAsync(IRequest Request)
        {
            var tempResponse = new GeneralAPIResponse();
            var tempRequest = (RequestPOST)Request;

            var client = new RestClient(tempRequest.baseURI);
            var request = new RestRequest(tempRequest.endPoint, tempRequest.Method);
            request.AddParameter("accept: application/json", ParameterType.HttpHeader);
            request.AddHeader("authorization", "Bearer " + tempRequest.token);
            request.AddParameter("application/json; charset=utf-8", tempRequest.JSONString, ParameterType.RequestBody);
            request.RequestFormat = tempRequest.DataFormat;

            try
            {
                var tempResponseJSON = await client.ExecuteAsync(request);
                tempResponse = GeneralAPIResponse.FromJson(tempResponseJSON.Content);

                Console.WriteLine("POST is done");
            }
            catch (Exception error)
            {
                // Log
                Console.WriteLine("POST is done, but errors" + error.ToString());
            }

            return tempResponse;
        }
    }
}
