using Consuming3rdPartyAPI.Helpers;
using RestSharp;
using System;
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
            //uncomment what you want to call

            //await CallContoso();
            await CallRandomStuff();
            await CallRandomStuffWithTypes();
            await CallRandomStuffWithTypesRestSharp();
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

        static async Task CallRandomStuffWithTypesRestSharp()
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
    }
}
