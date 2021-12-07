using Microsoft.AspNet.Identity;
using projectcrudresume.DatabaseClasses;
using projectcrudresume.Helpers;
using projectcrudresume.Models;
using projectcrudresume.PostmanClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace projectcrudresume.Controllers.APIControllers
{
    #region notes

    //this is the primary controller which will do everything
    //use code from public class TestModelAPIController : ApiController

    #endregion
    //[EnableCors(origins: "*", headers: "*", methods: "*")]

    public class UserandResumeAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region user profile stuff

        //disabled this. making it in line with the dot core api server.

        ////first lets get our User Profile
        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetUserProfile")]
        //public UserProfileViewModel GetUserProfile()
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    //ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
        //    //var Name = ClaimsPrincipal.Current.Identity.Name;
        //    //var Email = User.Identity.Name;
        //    var UserId= User.Identity.GetUserId();
        //    tempUserProfileViewModel = tempUserHelpers.GetUserProfileViewModel(UserId);
        //    return tempUserProfileViewModel;
        //}

        //lets add a new entry into the database.
        //this will create a new resume
        //but if one already exists, it will update with details provided
        //that are not stored in the user resume table
        //This only updates the direct values (not the bool things. only the string things)
        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdateUserName")]
        //public GeneralResponseModel UpdateUserName(UserResumeUpdateNameViewModel userResumeUpdateNameViewModel)
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateUserandResume(userResumeUpdateNameViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetResume")]
        //public UserResumeFullDetailsViewModel GetResume()
        //{
        //    var tempUserResumeFullDetailsViewModel = new UserResumeFullDetailsViewModel();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();

        //    //Resume here
        //    tempUserResumeFullDetailsViewModel = tempResumeHelpers.GetUserResumeFullDetailsViewModel(UserId);

        //    return tempUserResumeFullDetailsViewModel;
        //}


        //This endpoint only sends a quick summary. does not contain the actual resume.
        //just the highlights
        //for a full full summary, look at GetResume
        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetResumeSummary")]
        //public UserResumeViewModel GetResumeSummary()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempUserResumeViewModel = new UserResumeViewModel();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempUserResumeViewModel = tempResumeHelpers.GetResumeSummary(UserId);

        //    return tempUserResumeViewModel;
        //}

        #endregion

        #region AddressTable related endpoints

        //right now, there is nothing to differentiate different addresses. include a 
        //unique identifier to each address so we can update specific address
        //and add multipler addresses
        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdateAddress")]
        //public GeneralResponseModel UpdateAddress(AddressViewModel addressViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    //TODO14 - check that the user id of the address matches the unique GUID
        //    //if not, some other user is trying to modify some other users address

        //    if(String.IsNullOrEmpty(addressViewModel.UniqueGuid) == true)
        //    {
        //        //guid is null or empty.
        //        var message3 = "Guid cannot be null or empty";
        //        tempGeneralResponseModel.ListOfResponses.Add(message3);
        //        return tempGeneralResponseModel;
        //    }

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateItem(addressViewModel,UserId);

        //    return tempGeneralResponseModel;
        //}

        ////that is not required during add. So a separate view model is required here.
        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/AddAddress")]
        //public GeneralResponseModel AddAddress(AddressViewModel addressViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.AddItem(addressViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetAddress")]
        //public List<AddressViewModel> GetAddress()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempItemViewModel = new List<AddressViewModel>();

        //    tempItemViewModel = tempResumeHelpers.GetItem(UserId, tempItemViewModel);

        //    return tempItemViewModel;
        //}

        #endregion

        #region PhoneNumberTable related endpoints

        ////PhoneNumber
        ////phoneNumberViewModel

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdatePhoneNumber")]
        //public GeneralResponseModel UpdatePhoneNumber(PhoneNumberViewModel phoneNumberViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    //TODO14  - check that the user id of the address matches the unique GUID
        //    //if not, some other user is trying to modify some other users address

        //    if (String.IsNullOrEmpty(phoneNumberViewModel.UniqueGuid) == true)
        //    {
        //        //guid is null or empty.
        //        var message3 = "Guid cannot be null or empty";
        //        tempGeneralResponseModel.ListOfResponses.Add(message3);
        //        return tempGeneralResponseModel;
        //    }

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateItem(phoneNumberViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/AddPhoneNumber")]
        //public GeneralResponseModel AddPhoneNumber(PhoneNumberViewModel phoneNumberViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.AddItem(phoneNumberViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetPhoneNumber")]
        //public List<PhoneNumberViewModel> GetPhoneNumber()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempItemViewModel = new List<PhoneNumberViewModel>();

        //    tempItemViewModel = tempResumeHelpers.GetItem(UserId,tempItemViewModel);

        //    return tempItemViewModel;
        //}

        #endregion

        #region SkillsTable endpoints

        ////skillsTable

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdateSkillsTable")]
        //public GeneralResponseModel UpdateSkillsTable(SkillsTableViewModel skillsTableViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    //TODO14  - check that the user id of the address matches the unique GUID
        //    //if not, some other user is trying to modify some other users address

        //    if (String.IsNullOrEmpty(skillsTableViewModel.UniqueGuid) == true)
        //    {
        //        //guid is null or empty.
        //        var message3 = "Guid cannot be null or empty";
        //        tempGeneralResponseModel.ListOfResponses.Add(message3);
        //        return tempGeneralResponseModel;
        //    }

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateItem(skillsTableViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/AddSkillsTable")]
        //public GeneralResponseModel AddSkillsTable(SkillsTableViewModel skillsTableViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.AddItem(skillsTableViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetSkillsTable")]
        //public List<SkillsTableViewModel> GetSkillsTable()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempItemViewModel = new List<SkillsTableViewModel>();

        //    tempItemViewModel = tempResumeHelpers.GetItem(UserId, tempItemViewModel);

        //    return tempItemViewModel;
        //}

        #endregion

        #region EducationalDetails endpoints

        [Authorize]
        [HttpPost]
        [Route("api/UserandResume/UpdateEducationalDetails")]
        public GeneralResponseModel UpdateEducationalDetails(EducationalDetailViewModel educationalDetailViewModel)
        {
            var tempUserProfileViewModel = new UserProfileViewModel();
            var tempUserHelpers = new UserHelpers();
            var tempResumeHelpers = new ResumeHelpers();
            var UserId = User.Identity.GetUserId();
            var tempGeneralResponseModel = new GeneralResponseModel();

            //TODO14  - check that the user id of the address matches the unique GUID
            //if not, some other user is trying to modify some other users address

            if (String.IsNullOrEmpty(educationalDetailViewModel.UniqueGuid) == true)
            {
                //guid is null or empty.
                var message3 = "Guid cannot be null or empty";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            tempGeneralResponseModel = tempResumeHelpers.UpdateItem(educationalDetailViewModel, UserId);

            return tempGeneralResponseModel;
        }

        [Authorize]
        [HttpPost]
        [Route("api/UserandResume/DeleteEducationalDetails")]
        public GeneralResponseModel DeleteEducationalDetails(DeleteEducationalDetailViewModel deleteEducationalDetailViewModel)
        {
            var tempUserProfileViewModel = new UserProfileViewModel();
            var tempUserHelpers = new UserHelpers();
            var tempResumeHelpers = new ResumeHelpers();
            var UserId = User.Identity.GetUserId();
            var tempGeneralResponseModel = new GeneralResponseModel();

            //TODO14  - check that the user id of the address matches the unique GUID
            //if not, some other user is trying to modify some other users address

            if (String.IsNullOrEmpty(deleteEducationalDetailViewModel.UniqueGuid) == true)
            {
                //guid is null or empty.
                var message3 = "Guid cannot be null or empty";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //tempGeneralResponseModel = tempResumeHelpers.UpdateItem(educationalDetailViewModel, UserId);
            tempGeneralResponseModel = tempResumeHelpers.DeleteItem(deleteEducationalDetailViewModel, UserId);

            return tempGeneralResponseModel;
        }

        [Authorize]
        [HttpPost]
        [Route("api/UserandResume/AddEducationalDetails")]
        public GeneralResponseModel AddEducationalDetails(EducationalDetailViewModel educationalDetailViewModel)
        {
            var tempUserProfileViewModel = new UserProfileViewModel();
            var tempUserHelpers = new UserHelpers();
            var tempResumeHelpers = new ResumeHelpers();
            var UserId = User.Identity.GetUserId();
            var tempGeneralResponseModel = new GeneralResponseModel();

            tempGeneralResponseModel = tempResumeHelpers.AddItem(educationalDetailViewModel, UserId);

            return tempGeneralResponseModel;
        }

        [Authorize]
        [HttpGet]
        [Route("api/UserandResume/GetEducationalDetails")]
        public List<EducationalDetailViewModel> GetEducationalDetails()
        {
            var tempResumeHelpers = new ResumeHelpers();
            var UserId = User.Identity.GetUserId();
            var tempItemViewModel = new List<EducationalDetailViewModel>();

            tempItemViewModel = tempResumeHelpers.GetItem(UserId, tempItemViewModel);

            return tempItemViewModel;
        }



        #endregion

        #region ExtraCurricular endpoints


        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdateExtraCurricular")]
        //public GeneralResponseModel UpdateExtraCurricular(ExtraCurricularViewModel extraCurricularViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    //TODO14  - check that the user id of the address matches the unique GUID
        //    //if not, some other user is trying to modify some other users address

        //    if (String.IsNullOrEmpty(extraCurricularViewModel.UniqueGuid) == true)
        //    {
        //        //guid is null or empty.
        //        var message3 = "Guid cannot be null or empty";
        //        tempGeneralResponseModel.ListOfResponses.Add(message3);
        //        return tempGeneralResponseModel;
        //    }

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateItem(extraCurricularViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/AddExtraCurricular")]
        //public GeneralResponseModel AddExtraCurricular(ExtraCurricularViewModel extraCurricularViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.AddItem(extraCurricularViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetExtraCurricular")]
        //public List<ExtraCurricularViewModel> GetExtraCurricular()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempItemViewModel = new List<ExtraCurricularViewModel>();

        //    tempItemViewModel = tempResumeHelpers.GetItem(UserId, tempItemViewModel);

        //    return tempItemViewModel;
        //}

        #endregion

        #region OtherStuff endpoints

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdateOtherStuff")]
        //public GeneralResponseModel UpdateOtherStuff(OtherStuffViewModel otherStuffViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    //TODO14  - check that the user id of the address matches the unique GUID
        //    //if not, some other user is trying to modify some other users address

        //    if (String.IsNullOrEmpty(otherStuffViewModel.UniqueGuid) == true)
        //    {
        //        //guid is null or empty.
        //        var message3 = "Guid cannot be null or empty";
        //        tempGeneralResponseModel.ListOfResponses.Add(message3);
        //        return tempGeneralResponseModel;
        //    }

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateItem(otherStuffViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/AddOtherStuff")]
        //public GeneralResponseModel AddOtherStuff(OtherStuffViewModel otherStuffViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.AddItem(otherStuffViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetOtherStuff")]
        //public List<OtherStuffViewModel> GetOtherStuff()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempItemViewModel = new List<OtherStuffViewModel>();

        //    tempItemViewModel = tempResumeHelpers.GetItem(UserId, tempItemViewModel);

        //    return tempItemViewModel;
        //}

        #endregion

        #region ProjectDetails endpoints

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/UpdateProjectDetails")]
        //public GeneralResponseModel UpdateProjectDetails(ProjectDetailViewModel projectDetailViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    //TODO14 - check that the user id of the address matches the unique GUID
        //    //if not, some other user is trying to modify some other users address

        //    if (String.IsNullOrEmpty(projectDetailViewModel.UniqueGuid) == true)
        //    {
        //        //guid is null or empty.
        //        var message3 = "Guid cannot be null or empty";
        //        tempGeneralResponseModel.ListOfResponses.Add(message3);
        //        return tempGeneralResponseModel;
        //    }

        //    tempGeneralResponseModel = tempResumeHelpers.UpdateItem(projectDetailViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("api/UserandResume/AddProjectDetails")]
        //public GeneralResponseModel AddProjectDetails(ProjectDetailViewModel projectDetailViewModel)
        //{
        //    var tempUserProfileViewModel = new UserProfileViewModel();
        //    var tempUserHelpers = new UserHelpers();
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempGeneralResponseModel = new GeneralResponseModel();

        //    tempGeneralResponseModel = tempResumeHelpers.AddItem(projectDetailViewModel, UserId);

        //    return tempGeneralResponseModel;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("api/UserandResume/GetProjectDetails")]
        //public List<ProjectDetailViewModel> GetProjectDetails()
        //{
        //    var tempResumeHelpers = new ResumeHelpers();
        //    var UserId = User.Identity.GetUserId();
        //    var tempItemViewModel = new List<ProjectDetailViewModel>();

        //    tempItemViewModel = tempResumeHelpers.GetItem(UserId, tempItemViewModel);

        //    return tempItemViewModel;
        //}

        #endregion

    }
}
