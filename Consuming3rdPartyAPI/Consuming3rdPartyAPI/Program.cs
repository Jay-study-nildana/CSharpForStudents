using Consuming3rdPartyAPI.Helpers;
using Consuming3rdPartyAPI.POCOs;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Consuming3rdPartyAPI
{
    class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {

            //await CallContoso();
            //await CallRandomStuff();
            //await CallRandomStuffWithTypes();
            //await CallRandomStuffWithTypesRestSharpExceptionsIncluded();
            await CallRandomStuffWithTypesRestSharpExceptionsIncludedPostDemo();
        }

        //calling contoso.com
        static async Task CallContoso()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling Contoso..........");
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            displayHelper.ShowALine();
        }

        //using the API server of my random stuff project
        //https://jay-study-nildana.github.io/RandomStuffDocs/
        static async Task CallRandomStuff()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling CallRandomStuff..........");
            //https://randomstuffapizeropoint4.azurewebsites.net/api/UserNotLoggedIn/GetHoldOfthem
            string baseURI = "https://randomstuffapizeropoint4.azurewebsites.net/";
            string endPoint = "api/UserNotLoggedIn/GetHoldOfthem";
            string fullURI = baseURI + endPoint;
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync(fullURI);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            displayHelper.ShowALine();
        }

        static async Task CallRandomStuffWithTypes()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling CallRandomStuffWithTypes..........");
            string baseURI = "https://randomstuffapizeropoint4.azurewebsites.net/";
            string endPoint = "api/UserNotLoggedIn/GetHoldOfthem";
            string fullURI = baseURI + endPoint;
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync(fullURI);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                //the MS provided JsonSerializer is not working.
                RandomQuote randomQuote = JsonSerializer.Deserialize<RandomQuote>(responseBody);
                //here, I am switching to Newton Soft
                var randomQuote2 = RandomQuote.FromJson(responseBody);
                displayHelper.ShowRandomQuote(randomQuote2);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            displayHelper.ShowALine();
        }

        static async Task CallRandomStuffWithTypesRestSharpExceptionsIncluded()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling CallRandomStuffWithTypesRestSharp..........");
            string baseURI = "https://randomstuffapizeropoint4.azurewebsites.net/";
            string endPoint = "api/UserNotLoggedIn/GetHoldOfthem";
            string fullURI = baseURI + endPoint;

            var client = new RestClient("https://api.twitter.com/1.1");
            var request = new RestRequest(fullURI, DataFormat.Json);
            var randomQuote2 = await client.GetAsync<RandomQuote>(request);
            displayHelper.ShowRandomQuote(randomQuote2);
            displayHelper.ShowALine();
        }

        static async Task CallRandomStuffWithTypesRestSharpExceptionsIncludedPostDemo()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling CallRandomStuffWithTypesRestSharpExceptionsIncludedPostDemo..........");

            string baseURI = "https://randomstuffapizeropoint4.azurewebsites.net/";
            string endPoint = "api/Moderator/AddNewQuote";

            //our post object
            var tempPostObject = new NewQuote();
            var number = 1;
            tempPostObject.QuoteAuthor = "Nov28thAuthor";
            tempPostObject.QuoteContent = "Something Something Hello " + number;
            tempPostObject.OptionalAdditionalNotes = "Additional Notes " + number;

            //// Json to post.
            string jsonToPost = tempPostObject.ToJson();


            var client = new RestClient(baseURI);
            var request = new RestRequest(endPoint, Method.POST);
            var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Im5VU3l1ZDJBc201NzFtYzVKWmpwYyJ9.eyJpc3MiOiJodHRwczovL3JhbmRvbXF1b3RlZXhwZXJpbWVudGFsLnVzLmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1Zjc5NzNmYjYzMDAyNDAwNzE5NDNiNzIiLCJhdWQiOlsiaHR0cHM6Ly90aGVjaGFsYWthcy5jb20iLCJodHRwczovL3JhbmRvbXF1b3RlZXhwZXJpbWVudGFsLnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE2MzgwODAwOTQsImV4cCI6MTYzODE2NjQ5NCwiYXpwIjoiYVo3b3piQVYySGk3V1drR3NOd1ZSWm9ZYjgyeGdORjYiLCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIHJlYWQ6cHJvZmlsZWRldGFpbHMgcmVhZDpzZWVhbGxxdW90ZXMgcmVhZDpzaXRlc3RhdHMifQ.mMLdcWzcETv22ki8_1JyT-0Ak5CY2lR5r_VLKOTMFOhPX_AkNGBpi-MNTEkJOAU3SYZx661agHomNDSDi18rxtqeqhtqLd1v3aHJg7LWRXHLh4vg26I1OBzbux5-0EKOzAviyN_xbxGWEi44vfs9jrIH14LMGL1oMWguaMRvxx1aqIWEzmxu6MlnxjBFaAxLVRJVg3Xxg1Z2KYq3HnlFUz8qM2VOCw7FKOnfFoFfEAQ5alYEhp8FblwMswHTIheccRJFpqwd9nNG63IhGBaNvrp6M81l4zSGzfxXGUQdfnKLdve3okjM-6lmBxatcqKoOhSbcrmWTs01UchULl_SSA";
            request.AddParameter("accept: application/json", ParameterType.HttpHeader);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddParameter("application/json; charset=utf-8", jsonToPost, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            try
            {
                var tempResponse = await client.ExecuteAsync<NewQuoteResponse>(request);
                var responseObject = NewQuoteResponse.FromJson(tempResponse.Content);
                Console.WriteLine("POST is done");
            }
            catch (Exception error)
            {
                // Log
                Console.WriteLine("POST is done, but errors" + error.ToString());
            }
        }
    }
}
