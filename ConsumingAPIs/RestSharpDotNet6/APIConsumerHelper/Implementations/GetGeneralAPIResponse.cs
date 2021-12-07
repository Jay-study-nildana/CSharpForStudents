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

//NOTE : Check TestGetAdmin for additional comments and notes
//This file is a more cleaner version of an existing code.

//NOTE : The random stuff generator API has many endpoints which return the type
//GeneralAPIResponse. All of them can be called with this one.

namespace APIConsumerHelper.Implementations
{
    public class GetGeneralAPIResponse : IDoGet
    {

        public async Task<IResponse> PerformGetAsync(IRequest Request)
        {
            var tempResponse = new GeneralAPIResponse();
            var tempRequest = (RequestGet)Request;

            var client = new RestClient(tempRequest.baseURI);
            var request = new RestRequest(tempRequest.endPoint, tempRequest.Method);
            request.AddParameter("accept: application/json", ParameterType.HttpHeader);
            request.AddHeader("authorization", "Bearer " + tempRequest.token);
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
