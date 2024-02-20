using Consuming3rdPartyAPI.Helpers;
using Consuming3rdPartyAPI.POCOS.NASAAPOD;
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
        //NASA API endpoint
        //Students: use your own api key. don't use mine.
        const string NASAPODAPIURL = "https://api.nasa.gov/planetary/apod?api_key=JjP84CKefxzmg2fyAvN4zWsRyAAqg1nzrXvHdtc6";

        static async Task Main()
        {

            //await CallContoso();

            //call the NASA API

            //await CallNASAAPODAPI();
            //await CallNASAAPODAPIwithTypes();
            //await CallNASAAPODAPIwithTypesWithRestSharp();

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

        //calling the NASA API

        static async Task CallNASAAPODAPI()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling Contoso..........");
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync(NASAPODAPIURL);
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

        static async Task CallNASAAPODAPIwithTypes()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling Contoso..........");
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync(NASAPODAPIURL);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                var NASAAPODResponse = NasaapodResponseDto.FromJson(responseBody);

                //TODO do something with the NASA response object. 
                //may be display it in the command line
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            displayHelper.ShowALine();
        }

        static async Task CallNASAAPODAPIwithTypesWithRestSharp()
        {
            DisplayHelper displayHelper = new DisplayHelper();
            displayHelper.ShowTheThing("Now Calling Contoso..........");
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                var client = new RestClient();
                var request = new RestRequest(NASAPODAPIURL, Method.Get);
                var NASAAPODResponse = await client.GetAsync<NasaapodResponseDto>(request);

                //TODO do something with the NASA response object. 
                //may be display it in the command line
                displayHelper.ShowTheThing("TODO. who some stuff here.");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            displayHelper.ShowALine();
        }

    }
}
