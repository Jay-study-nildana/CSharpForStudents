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
    public class TestGetAdmin : IDoGet
    {
        //working on
        //https://randomstuffapizeropoint4.azurewebsites.net/api/Admin/Hi
        public async Task<IResponse> PerformGetAsync(IRequest Request)
        {
            var tempResponse = new GeneralAPIResponse();
            var tempRequest = (RequestGet) Request;

            //string baseURI = "https://randomstuffapizeropoint4.azurewebsites.net/";
            //string endPoint = "api/Admin/Hi";


            var client = new RestClient(tempRequest.baseURI);
            var request = new RestRequest(tempRequest.endPoint, tempRequest.Method);
            //var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Im5VU3l1ZDJBc201NzFtYzVKWmpwYyJ9.eyJpc3MiOiJodHRwczovL3JhbmRvbXF1b3RlZXhwZXJpbWVudGFsLnVzLmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1Zjc5NzNmYjYzMDAyNDAwNzE5NDNiNzIiLCJhdWQiOlsiaHR0cHM6Ly90aGVjaGFsYWthcy5jb20iLCJodHRwczovL3JhbmRvbXF1b3RlZXhwZXJpbWVudGFsLnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE2Mzg2ODM2NzMsImV4cCI6MTYzODc3MDA3MywiYXpwIjoiYVo3b3piQVYySGk3V1drR3NOd1ZSWm9ZYjgyeGdORjYiLCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIHJlYWQ6cHJvZmlsZWRldGFpbHMgcmVhZDpzZWVhbGxxdW90ZXMgcmVhZDpzaXRlc3RhdHMifQ.q1lT60tUaWY6sIx8S-TMR_qhiuKQay4H-pXZMcCZuNflvF-WZiu2WJOJlchdEwX_1QOAIsAQlEAUsPJpd1Bt6un4-hgKgukguH1q9S55yfB_6r1nc_PKLvqRDgIlL2ZhzD69jQ1wqTKg8U5jXULzqjwXarRWjfYT-WOnCO8j9B13VsyniLt40aTDRBCLT9pfJ08qjipGRcXjxuWAW1_-ZgP69DDw82_FVyFJm_3qDnTGMOxoZ4z3tklMSqJDLGLRD1Fwk3pwyunuk4ISnMh9cBuHJmT88sNIjRyrDZlLz0pg_HDbEYqBe3MSzqoc6-1MhCbU78vUzDEH8ZHwjNjw1Q";
            request.AddParameter("accept: application/json", ParameterType.HttpHeader);
            request.AddHeader("authorization", "Bearer " + tempRequest.token);
            //request.RequestFormat = DataFormat.Json;
            request.RequestFormat = tempRequest.DataFormat;

            try
            {
                //tempResponse = (AdminResponse)await client.ExecuteAsync<AdminResponse>(request);
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
