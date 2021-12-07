using AllTheInterfaces.Interfaces;
using APIConsumerHelper.Implementations;
using RandomStuffGenerator;
using RandomStuffGenerator.NewQuote;
using RandomStuffGenerator.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIConsumerHelper
{
    public class APISinglePoint
    {
        public string token { get; private set; }

        public APISinglePoint(string token)
        {
            this.token = token;
        }

        public async Task<IResponse> DemoSimpleGet()
        {
            IDoGet testGetAdmin = new GetGeneralAPIResponse();
            var tempRequest = new RequestGet();
            tempRequest.endPoint = "api/Admin/Hi";
            tempRequest.token = token;
            var response1 = await testGetAdmin.PerformGetAsync(tempRequest);

            return response1;
        }

        public async Task<IResponse> DemoSimplePost()
        {
            //lets test the POST
            IDoPost testAddQuote = new PostNewQuote();
            //get the quote object and make it a json string
            //our post object
            var tempPostObject = new PostAddNewQuoteBody();
            var number = 1;
            tempPostObject.QuoteAuthor = "Nov28thAuthor";
            tempPostObject.QuoteContent = "Something Something Hello " + number;
            tempPostObject.OptionalAdditionalNotes = "Additional Notes " + number;

            //// Json to post.
            string jsonToPost = tempPostObject.ToJson();

            var tempRequest2 = new RequestPOST();
            tempRequest2.endPoint = "api/Moderator/AddNewQuote";
            tempRequest2.token = token;
            tempRequest2.JSONString = jsonToPost;

            var response2 = await testAddQuote.PerformPostAsync(tempRequest2);

            return response2;
        }
    }
}
