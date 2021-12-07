using projectcrudresume.PostmanClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

//These are the test service endpoints 
//This is where we will find all the Test endpoints 
//useful for testing the API running status. 
//returns some single strings to indicate if this is working just fine.

//TODO - we need to update this HelloWorldController with the simple CRUD that I have in the dot net core HelloWorldController
//Actually we need two. One that is open and one that authorized.

namespace projectcrudresume.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HelloWorldController : ApiController
    {
        //this is used to test if the service is alive or not. 
        //it does not need token
        [HttpGet]
        [Route("api/HelloWorld/TestService")]
        public HelloWorld TestService()
        {
            var tempHelloWorld = new HelloWorld();

            tempHelloWorld.Message1 = "we can either complain. Or, we can do something about it";
            tempHelloWorld.Message2 = "thats what she said";
            tempHelloWorld.Number1 = 69;
            tempHelloWorld.Number2 = 11.38;

            return tempHelloWorld;
        }

        //this one is similar to HelloWorld
        //but needs a token to be passed on or it will fail
        [Authorize]
        [HttpGet]
        [Route("api/HelloWorld/TestServiceAuthorized")]
        public HelloWorld2 TestServiceAuthorized()
        {
            var tempHelloWorld = new HelloWorld2();

            tempHelloWorld.Message1 = "we can either complain. Or, we can do something about it";
            tempHelloWorld.Message2 = "thats what she said";
            tempHelloWorld.Number1 = 69;
            tempHelloWorld.Number2 = 11.38;

            return tempHelloWorld;
        }

        //I am removing this user endpoint.
        //making it in line with the dot net core api server

        //[Authorize]
        //[HttpGet]
        //[Route("api/HelloWorld/TestServiceUserDetails")]
        //public HelloWorldUserDetails TestServiceUserDetails()
        //{
        //    var tempHelloWorldUserDetails = new HelloWorldUserDetails();
        //    var tempAuthenticationHelpers = new AuthenticationHelpers();

        //    ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

        //    var Name = ClaimsPrincipal.Current.Identity.Name;
        //    var Name1 = User.Identity.Name;
            

        //    tempHelloWorldUserDetails.MessageDescription = "This API should give you details about your own token";
        //    tempHelloWorldUserDetails.UserName = Name1;

        //    return tempHelloWorldUserDetails;
        //}

    }
}