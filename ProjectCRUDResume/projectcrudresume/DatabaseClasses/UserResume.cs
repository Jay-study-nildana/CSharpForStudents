using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//note - these classes and a lot of functions were borrowed from my original dot net api server
//this api server is essentially a remake of the server that was already built so as much as possible code has been reused

//you may find references to items - for example, address or firstname and last name - littered through out the project
//that is becuase, originally, this was supposed to be a app that dynamically generates resumes
//but then, my company fell apart so now, it simply tracks education qualifications

namespace projectcrudresume.DatabaseClasses
{
    #region notes

    //Then, we have User Resume Details Table
    //here is where we keep the actual user resume stuff
    //like Name, Biography details and all that.
    //In the User Resume Details, single entry items,
    //you keep it in the User Resume Details
    //Collection items, you keep a reference to the collection

    #endregion

    //TODO - AspNetUsersUniqueIdentifier may be not expose such an ugly name outside the API


    //TODO - GetOtherStuff flag is missing in the UserResume class. add and update this.
    //update the endpoints, especially the flag in the resume which happens with either add or update
    public class UserResume
    {
        public int ID { set; get; }
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
        public string UserUniqueKey { set; get; }
        public string UserEmail { set; get; }
        public bool ProjectDetails { set; get; }
        public bool GetOtherStuff { set; get; }
    }


    #region additional user details

    //TODO - AddressTable 
    //TODO - PhoneNumberTable
    //TODO - SkillsTable
    //TODO - EducationalDetails
    //TODO - ExtraCurricular
    //TODO - OtherStuff
    //TODO - ProjectDetails
    //TODO - Remove blank space lines in class variable declarations
    //TODO - the classes here and their corresponding view model classes share some common elements
    //perhaps move those into a base class. 

    #endregion

    #region additional classes that contain UserResume

    public class AddressTable
    {
        public int ID { set; get; }

        //So, this is the foreign key kind of thing. I am not using an actual foreign key
        //this connects with the Id (which is a string / navchar of length 128) in the AspNetUsers table
        //It is this connection I want to exploit to build the user resume.
        public string AspNetUsersUniqueIdentifier { set; get; }

        public string AddressLineOne { set; get; }

        public string AddressLineTwo { set; get; }

        public string City { set; get; }

        public string State { set; get; }

        public string Pincode { set; get; }

        public string Landmark { set; get; }

        public string AddressExtraNotes { set; get; }

        //Set this bool to true to indicate that this is a primary address
        //each person can have multiple addresses
        public bool PrimaryAddress { set; get; }

        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }

    public class PhoneNumberTable
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; }
        public string PhoneNumber { set; get; }
        public bool CountryCode { set; get; }

        //TODO and note 
        //This was added by mistake. remove it later. until removal dont use this field.
        public bool PrimaryAddress { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }

    public class SkillsTable
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; }
        public string SkillTitle { set; get; }
        public string SkillDescription { set; get; }
        public int SkillExperience { set; get; }
        public string SkillOtherNotes1 { set; get; }
        public string SkillOtherNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }

    public class EducationalDetail
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; }
        public string EducationTitle { set; get; }
        public string InstituationName { set; get; }
        public int YearOfGraduation { set; get; }
        public string PassGrade { set; get; }
        public string EducationOtherNotes1 { set; get; }
        public string EducationOtherNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }

    public class ExtraCurricular
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; }
        public string ExtraCurricularOtherNotes1 { set; get; }
        public string ExtraCurricularNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }
    }

    public class OtherStuff
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; }
        public string OtherStuffNotes1 { set; get; }
        public string OtherStuffNotes2 { set; get; }
        public string OtherStuffNotes3 { set; get; }
        public string OtherStuffNotes4 { set; get; }
        public string OtherStuffNotes5 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }

    }

    public class ProjectDetail
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; }
        public string ProjectTitle { set; get; }
        public string ProjectDescription { set; get; }
        public int YearOfProject { set; get; }
        public string ProjectNotes1 { set; get; }
        public string ProjectNotes2 { set; get; }
        //we are allowing multiple items per student
        //for example multiple addresses and multiple phone numbers and so on
        //so we need each entry to be unique
        public string UniqueGuid { set; get; }

    }

    #endregion
}