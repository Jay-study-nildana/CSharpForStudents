using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    public class UserResumeViewModel
    {
        public string AspNetUsersUniqueIdentifier { set; get; }//this is the id from AspNetUsersTable
        public string FirstName { set; get; }
        public string MiddleName { set; get; }
        public string LastName { set; get; }
        public bool Address { set; get; }
        public bool PhoneNumber { set; get; }
        public string Email { set; get; }
        public bool SkillsSummary { set; get; }
        public bool EducationalDetailsSummary { set; get; }
        public bool ExtraCurricularActivitiesSummary { set; get; }
        //public string UserUniqueKey { set; get; }
        public string UserEmail { set; get; }
        public bool ProjectDetails { set; get; }
        public bool GetOtherStuff { set; get; }
    }

    //while udpating, we cannot allow user to update the bool values. 
    //So, need a separate view model for that. 
    public class UserResumeUpdateNameViewModel
    {
        public string FirstName { set; get; }
        public string MiddleName { set; get; }

        public string LastName { set; get; }
        //public string Email { set; get; }
        ////public string UserUniqueKey { set; get; }
        //public string UserEmail { set; get; }
    }

    //we need one more view model to return all the resume details in one go.
    public class UserResumeFullDetailsViewModel
    {
        public string AspNetUsersUniqueIdentifier { set; get; }//this is the id from AspNetUsersTable
        public string FirstName { set; get; }
        public string MiddleName { set; get; }
        public string LastName { set; get; }
        public string UserEmail { set; get; }
        public string Email { set; get; }

        public bool SkillsSummary { set; get; }
        public bool EducationalDetailsSummary { set; get; }
        public bool ExtraCurricularActivitiesSummary { set; get; }
        //public string UserUniqueKey { set; get; }
        public bool ProjectDetails { set; get; }
        public bool GetOtherStuff { set; get; }
        public bool Address { set; get; }
        public bool PhoneNumber { set; get; }

        //the collections and items
        public List<SkillsTableViewModel> skillsTableViewModels { set; get; }
        public List<EducationalDetailViewModel> educationalDetailViewModels { set; get; }
        public List<ExtraCurricularViewModel> extraCurricularViewModels { set; get; }
        public List<ProjectDetailViewModel> projectDetailViewModels { set; get; }
        public List<OtherStuffViewModel> otherStuffViewModels { set; get; }
        public List<AddressViewModel> addressViewModels { set; get; }
        public List<PhoneNumberViewModel> phoneNumberViewModels { set; get; }
    }
}