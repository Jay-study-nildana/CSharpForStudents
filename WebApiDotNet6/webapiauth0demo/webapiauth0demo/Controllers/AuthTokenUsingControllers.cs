using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Note the usage of [Authorize]
//Understand the scope of this applies to everything below
//I have applied to each endpoint. 
//If you wish, you can move it outside and apply it to the entire controller


namespace webapiauth0demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTokenUsingControllers : ControllerBase
    {
        //here, I dont apply any specific policy or roles
        //I want every authenticated user to be able to access this endpoint
        //after all, this is where you confirm all the scopes or permissions
        //the current token aka user has
        [HttpGet("claims")]
        [Authorize]
        public IActionResult Claims()
        {
            return Ok(User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                }));
        }

        //remember to match the policy with the exact lettering of the policy
        [HttpGet]
        [Route("HelloFromRoleThatOnlyReads")]
        [Authorize(Policy = "RoleThatOnlyReads")]
        public ActionResult<GeneralAPIResponse> HelloOne()
        {
            var generalAPIResponse = new GeneralAPIResponse();

            generalAPIResponse.OperationSuccessful = true;
            generalAPIResponse.DetailsAboutOperation = "This is the HelloFrom RoleThatOnlyReads";

            return generalAPIResponse;
        }

        [HttpGet]
        [Route("HelloFromRoleThatDeletesUpdates")]
        [Authorize(Policy = "RoleThatDeletesUpdates")]
        public ActionResult<GeneralAPIResponse> HelloTwo()
        {
            var generalAPIResponse = new GeneralAPIResponse();

            generalAPIResponse.OperationSuccessful = true;
            generalAPIResponse.DetailsAboutOperation = "This is the HelloFromRoleThatDeletesUpdates";

            return generalAPIResponse;
        }
    }

    public class GeneralAPIResponse
    {
        //a note about setters and getters
        //if you don't put this getters and setters
        //the response won't appear in your API endpoint
        //nor will it show up on swagger documentation
        public bool OperationSuccessful { set; get; }
        public string DetailsAboutOperation { set; get; }

        //I want at least one message to be in the response.
        public GeneralAPIResponse()
        {
            OperationSuccessful = true;
            DetailsAboutOperation = "Clean as a whistle";
        }
    }
}
