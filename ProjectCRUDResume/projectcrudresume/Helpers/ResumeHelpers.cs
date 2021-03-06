using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using projectcrudresume.DatabaseClasses;
using projectcrudresume.Models;
using projectcrudresume.PostmanClasses;

namespace projectcrudresume.Helpers
{
    //TODO update UpdateResumeAsync
    public class ResumeHelpers
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        #region notes

        //this will add a new entry to the database
        //or it will udpate an existing entry

        #endregion
        public async Task<UpdateUserandResumeResponseViewModel> UpdateResumeAsync(UserResumeViewModel userResumeViewModel, string email)
        {
            var tempUpdateUserandResumeResponseViewModel = new UpdateUserandResumeResponseViewModel();

            //note that the email of the account is linked to unique key
            var tempResume = db.UserResumes.Select(x => x).Where(x => x.UserUniqueKey == email).FirstOrDefault();

            #region add new entry

            if(tempResume == null)
            {
                var tempUserResume = new UserResume();

                //tempUserResume.Address = userResumeViewModel.Address;
                //tempUserResume.EducationalDetailsSummary = userResumeViewModel.EducationalDetailsSummary;
                //tempUserResume.Email = userResumeViewModel.Email;
                //tempUserResume.ExtraCurricularActivitiesSummary = userResumeViewModel.ExtraCurricularActivitiesSummary;
                //tempUserResume.FirstName = userResumeViewModel.FirstName;
                //tempUserResume.LastName = userResumeViewModel.LastName;
                //tempUserResume.MiddleName = userResumeViewModel.MiddleName;
                //tempUserResume.PhoneNumber = userResumeViewModel.PhoneNumber;
                //tempUserResume.SkillsSummary = userResumeViewModel.SkillsSummary;
                tempUserResume.UserEmail = email;
                tempUserResume.UserUniqueKey = email;
                
                try
                {
                    db.UserResumes.Add(tempUserResume);
                    db.SaveChanges();

                    tempUpdateUserandResumeResponseViewModel.ResponseMessage =
                        "Resume Did Not Exist. New Resume was created and added to the database";
                }
                catch
                {
                    tempUpdateUserandResumeResponseViewModel.ResponseMessage =
                        "Resume Did Not Exist. Problem. We could not add this to the database";
                }
            }

            #endregion

            #region update existing entry
            else
            {
                //if we are that means, resume already exists and we are updating it.
                //UserProfile userProfile
                //db.Entry(userProfile).State = EntityState.Modified;

                #region build our model object for resume
                var tempUserResume = tempResume;

                //tempUserResume.Address = userResumeViewModel.Address;
                //tempUserResume.EducationalDetailsSummary = userResumeViewModel.EducationalDetailsSummary;
                //tempUserResume.Email = userResumeViewModel.Email;
                //tempUserResume.ExtraCurricularActivitiesSummary = userResumeViewModel.ExtraCurricularActivitiesSummary;
                //tempUserResume.FirstName = userResumeViewModel.FirstName;
                //tempUserResume.LastName = userResumeViewModel.LastName;
                //tempUserResume.MiddleName = userResumeViewModel.MiddleName;
                //tempUserResume.PhoneNumber = userResumeViewModel.PhoneNumber;
                //tempUserResume.SkillsSummary = userResumeViewModel.SkillsSummary;
                tempUserResume.UserEmail = email;
                tempUserResume.UserUniqueKey = email;

                #endregion

                try
                {
                    db.Entry(tempUserResume).State = EntityState.Modified;
                    //db.SaveChanges();
                    await db.SaveChangesAsync();
                    tempUpdateUserandResumeResponseViewModel.ResponseMessage =
                        "Resume already in database. Updated with new details";
                }
                catch(Exception e)
                {
                    var response = e.ToString();
                    tempUpdateUserandResumeResponseViewModel.ResponseMessage =
                        "Resume already in database. Unable to update details in the database";
                }
            }
            #endregion

            return tempUpdateUserandResumeResponseViewModel;
        }

        internal UserResumeFullDetailsViewModel GetUserResumeFullDetailsViewModel(string userId)
        {
            var tempUserResumeFullDetailsViewModel = new UserResumeFullDetailsViewModel();

            //first get the essential items. the user summary
            var tempUserResumeViewModel = GetResumeSummary(userId);

            tempUserResumeFullDetailsViewModel.AspNetUsersUniqueIdentifier = userId;
            tempUserResumeFullDetailsViewModel.FirstName = tempUserResumeViewModel.FirstName;
            tempUserResumeFullDetailsViewModel.MiddleName = tempUserResumeViewModel.MiddleName;
            tempUserResumeFullDetailsViewModel.LastName = tempUserResumeViewModel.LastName;
            tempUserResumeFullDetailsViewModel.Email = tempUserResumeViewModel.Email;
            tempUserResumeFullDetailsViewModel.UserEmail = tempUserResumeViewModel.UserEmail;

            #region bug fix

            //The way I built my code, when a new account is created, the 'add items' to resume in the edit resume
            //page of the web app, does not show up. it was built on the (wrong) assumption that adding only 
            //comes after adding at least one item. 
            //I dont want to touch the web app right now, so I will update the api so a dummy entry is 
            //always added.

            #endregion

            //update individual items based on the bool flag.
            tempUserResumeFullDetailsViewModel.Address = tempUserResumeViewModel.Address;
            if(tempUserResumeFullDetailsViewModel.Address == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.addressViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.addressViewModels);
            }

            tempUserResumeFullDetailsViewModel.EducationalDetailsSummary = tempUserResumeViewModel.EducationalDetailsSummary;
            if (tempUserResumeFullDetailsViewModel.EducationalDetailsSummary == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.educationalDetailViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.educationalDetailViewModels);
            }
            tempUserResumeFullDetailsViewModel.ExtraCurricularActivitiesSummary = tempUserResumeViewModel.ExtraCurricularActivitiesSummary;
            if (tempUserResumeFullDetailsViewModel.ExtraCurricularActivitiesSummary == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.extraCurricularViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.extraCurricularViewModels);
            }
            tempUserResumeFullDetailsViewModel.GetOtherStuff = tempUserResumeViewModel.GetOtherStuff;
            if (tempUserResumeFullDetailsViewModel.GetOtherStuff == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.otherStuffViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.otherStuffViewModels);
            }

            tempUserResumeFullDetailsViewModel.PhoneNumber = tempUserResumeViewModel.PhoneNumber;
            if (tempUserResumeFullDetailsViewModel.PhoneNumber == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.phoneNumberViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.phoneNumberViewModels);
            }

            tempUserResumeFullDetailsViewModel.ProjectDetails = tempUserResumeViewModel.ProjectDetails;
            if (tempUserResumeFullDetailsViewModel.ProjectDetails == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.projectDetailViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.projectDetailViewModels);
            }
            tempUserResumeFullDetailsViewModel.SkillsSummary = tempUserResumeViewModel.SkillsSummary;
            if (tempUserResumeFullDetailsViewModel.SkillsSummary == true)
            {
                //lets get the collection since the corresponding flag is set to true
                tempUserResumeFullDetailsViewModel.skillsTableViewModels = GetItem(userId, tempUserResumeFullDetailsViewModel.skillsTableViewModels);
            }

            return tempUserResumeFullDetailsViewModel;
        }

        internal GeneralResponseModel UpdateUserandResume(UserResumeUpdateNameViewModel userResumeUpdateNameViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();

            if (tempUserResume == null)
            {
                //if it is null, no entries are made yet. lets create a new one.
                tempUserResume = new UserResume();
                tempUserResume.FirstName = userResumeUpdateNameViewModel.FirstName;
                tempUserResume.MiddleName = userResumeUpdateNameViewModel.MiddleName;
                tempUserResume.LastName = userResumeUpdateNameViewModel.LastName;
                tempUserResume.AspNetUsersUniqueIdentifier = userId;
                db.UserResumes.Add(tempUserResume);
                db.SaveChanges();

                var message2 = "New User was created and also updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message2);

                return tempGeneralResponseModel;
            }

            //TODO dont update if the values are null

            tempUserResume.FirstName = userResumeUpdateNameViewModel.FirstName;
            tempUserResume.MiddleName = userResumeUpdateNameViewModel.MiddleName;
            tempUserResume.LastName = userResumeUpdateNameViewModel.LastName;

            db.UserResumes.AddOrUpdate(tempUserResume);
            db.SaveChanges();

            var message = "name was updated successfully.";
            tempGeneralResponseModel.ListOfResponses.Add(message);

            return tempGeneralResponseModel;
        }

        internal UserResumeViewModel GetResumeSummary(string userId)
        {
            var tempUserResumeViewModel = new UserResumeViewModel();

            var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();

            if(tempUserResume == null)
            {
                //if it is null, no entries are made yet. lets create a new one.
                tempUserResume = new UserResume();
                tempUserResume.FirstName = "NA";
                tempUserResume.AspNetUsersUniqueIdentifier = userId;
                db.UserResumes.Add(tempUserResume);
                db.SaveChanges();
            }

            tempUserResumeViewModel.Address = tempUserResume.Address;
            tempUserResumeViewModel.AspNetUsersUniqueIdentifier = tempUserResume.AspNetUsersUniqueIdentifier;
            tempUserResumeViewModel.EducationalDetailsSummary = tempUserResume.EducationalDetailsSummary;
            tempUserResumeViewModel.Email = tempUserResume.Email;
            tempUserResumeViewModel.ExtraCurricularActivitiesSummary = tempUserResume.ExtraCurricularActivitiesSummary;
            tempUserResumeViewModel.FirstName = tempUserResume.FirstName;
            tempUserResumeViewModel.GetOtherStuff = tempUserResume.GetOtherStuff;
            tempUserResumeViewModel.LastName = tempUserResume.LastName;
            tempUserResumeViewModel.MiddleName = tempUserResume.MiddleName;
            tempUserResumeViewModel.PhoneNumber = tempUserResume.PhoneNumber;
            tempUserResumeViewModel.ProjectDetails = tempUserResume.ProjectDetails;
            tempUserResumeViewModel.SkillsSummary = tempUserResume.SkillsSummary;
            tempUserResumeViewModel.UserEmail = tempUserResume.UserEmail;


            return tempUserResumeViewModel;
        }

        public UserResumeViewModel GetResume(string email)
        {
            var tempUserResumeViewModel = new UserResumeViewModel();
            //note that the unique key is the email attached to the users login account
            //the resume itself can have a different email from the email that is attached to the account
            var tempResume = db.UserResumes.Select(x => x).Where(x => x.UserUniqueKey == email).FirstOrDefault();

            if (tempResume == null)
            {
                tempUserResumeViewModel.FirstName = "Resume Not Found";
            }
            else
            {
                //tempUserResumeViewModel.Address = tempResume.Address;
                //tempUserResumeViewModel.EducationalDetailsSummary = tempResume.EducationalDetailsSummary;
                //tempUserResumeViewModel.Email = tempResume.Email;
                //tempUserResumeViewModel.ExtraCurricularActivitiesSummary = tempResume.ExtraCurricularActivitiesSummary;
                //tempUserResumeViewModel.FirstName = tempResume.FirstName;
                //tempUserResumeViewModel.LastName = tempResume.LastName;
                //tempUserResumeViewModel.MiddleName = tempResume.MiddleName;
                //tempUserResumeViewModel.PhoneNumber = tempResume.PhoneNumber;
                //tempUserResumeViewModel.SkillsSummary = tempResume.SkillsSummary;
            }
            return tempUserResumeViewModel;
        }



        internal List<PhoneNumberViewModel> GetItem(string userId, List<PhoneNumberViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<PhoneNumberViewModel>();
            var listOfAllItems = db.PhoneNumberTables.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new PhoneNumberViewModel();
                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModelx.CountryCode = tempItem.CountryCode;
                tempItemViewModelx.PhoneNumber = tempItem.PhoneNumber;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal List<AddressViewModel> GetItem(string userId, List<AddressViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<AddressViewModel>();
            var listOfAllItems = db.AddressTables.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new AddressViewModel();
                tempItemViewModelx.AddressExtraNotes = tempItem.AddressExtraNotes;
                tempItemViewModelx.AddressLineOne = tempItem.AddressLineOne;
                tempItemViewModelx.AddressLineTwo = tempItem.AddressLineTwo;
                tempItemViewModelx.City = tempItem.City;
                tempItemViewModelx.Landmark = tempItem.Landmark;
                tempItemViewModelx.Pincode = tempItem.Pincode;
                tempItemViewModelx.PrimaryAddress = tempItem.PrimaryAddress;
                tempItemViewModelx.State = tempItem.State;
                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal List<EducationalDetailViewModel> GetItem(string userId, List<EducationalDetailViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<EducationalDetailViewModel>();
            var listOfAllItems = db.EducationalDetails.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new EducationalDetailViewModel();
                tempItemViewModelx.EducationOtherNotes1 = tempItem.EducationOtherNotes1;
                tempItemViewModelx.EducationOtherNotes2 = tempItem.EducationOtherNotes2;
                tempItemViewModelx.EducationTitle = tempItem.EducationTitle;
                tempItemViewModelx.InstituationName = tempItem.InstituationName;
                tempItemViewModelx.PassGrade = tempItem.PassGrade;
                tempItemViewModelx.YearOfGraduation = tempItem.YearOfGraduation;
                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal List<SkillsTableViewModel> GetItem(string userId, List<SkillsTableViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<SkillsTableViewModel>();
            var listOfAllItems = db.SkillsTables.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new SkillsTableViewModel();
                tempItemViewModelx.SkillDescription = tempItem.SkillDescription;
                tempItemViewModelx.SkillExperience = tempItem.SkillExperience;
                tempItemViewModelx.SkillOtherNotes1 = tempItem.SkillOtherNotes1;
                tempItemViewModelx.SkillOtherNotes2 = tempItem.SkillOtherNotes2;
                tempItemViewModelx.SkillTitle = tempItem.SkillTitle;
                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal List<ExtraCurricularViewModel> GetItem(string userId, List<ExtraCurricularViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<ExtraCurricularViewModel>();
            var listOfAllItems = db.ExtraCurriculars.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new ExtraCurricularViewModel();

                tempItemViewModelx.ExtraCurricularNotes2 = tempItem.ExtraCurricularNotes2;
                tempItemViewModelx.ExtraCurricularOtherNotes1 = tempItem.ExtraCurricularOtherNotes1;

                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal List<OtherStuffViewModel> GetItem(string userId, List<OtherStuffViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<OtherStuffViewModel>();
            var listOfAllItems = db.OtherStuffs.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new OtherStuffViewModel();

                tempItemViewModelx.OtherStuffNotes1 = tempItem.OtherStuffNotes1;
                tempItemViewModelx.OtherStuffNotes2 = tempItem.OtherStuffNotes2;
                tempItemViewModelx.OtherStuffNotes3 = tempItem.OtherStuffNotes3;
                tempItemViewModelx.OtherStuffNotes4 = tempItem.OtherStuffNotes4;
                tempItemViewModelx.OtherStuffNotes5 = tempItem.OtherStuffNotes5;

                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal List<ProjectDetailViewModel> GetItem(string userId, List<ProjectDetailViewModel> tempItemViewModel)
        {
            tempItemViewModel = new List<ProjectDetailViewModel>();
            var listOfAllItems = db.ProjectDetails.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).ToList();

            foreach (var tempItem in listOfAllItems)
            {
                var tempItemViewModelx = new ProjectDetailViewModel();

                tempItemViewModelx.ProjectDescription = tempItem.ProjectDescription;
                tempItemViewModelx.ProjectNotes1 = tempItem.ProjectNotes1;
                tempItemViewModelx.ProjectNotes2 = tempItem.ProjectNotes2;
                tempItemViewModelx.ProjectTitle = tempItem.ProjectTitle;
                tempItemViewModelx.YearOfProject = tempItem.YearOfProject;

                tempItemViewModelx.UniqueGuid = tempItem.UniqueGuid;

                tempItemViewModel.Add(tempItemViewModelx);
            }

            return tempItemViewModel;
        }

        internal GeneralResponseModel AddItem(EducationalDetailViewModel educationalDetailViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new EducationalDetail();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();
            tempItem.EducationOtherNotes1 = educationalDetailViewModel.EducationOtherNotes1;
            tempItem.EducationOtherNotes2 = educationalDetailViewModel.EducationOtherNotes2;
            tempItem.EducationTitle = educationalDetailViewModel.EducationTitle;
            tempItem.InstituationName = educationalDetailViewModel.InstituationName;
            tempItem.PassGrade = educationalDetailViewModel.PassGrade;
            tempItem.YearOfGraduation = educationalDetailViewModel.YearOfGraduation;

            try
            {
                //update education of the user's resume
                db.EducationalDetails.Add(tempItem);
                db.SaveChanges();

                var message = "education was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the education flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
                //only update if the flag is not already set.
                if (tempUserResume.EducationalDetailsSummary == false)
                {
                    tempUserResume.EducationalDetailsSummary = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "education flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel AddItem(SkillsTableViewModel skillsTableViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new SkillsTable();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();
            tempItem.SkillDescription = skillsTableViewModel.SkillDescription;
            tempItem.SkillExperience = skillsTableViewModel.SkillExperience;
            tempItem.SkillOtherNotes1 = skillsTableViewModel.SkillOtherNotes1;
            tempItem.SkillOtherNotes2 = skillsTableViewModel.SkillOtherNotes2;
            tempItem.SkillTitle = skillsTableViewModel.SkillTitle;

            try
            {
                //update skill of the user's resume
                db.SkillsTables.Add(tempItem);
                db.SaveChanges();

                var message = "skill was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
                //only update if the flag is not already set.
                if (tempUserResume.SkillsSummary == false)
                {
                    tempUserResume.SkillsSummary = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "skill flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel AddItem(PhoneNumberViewModel phoneNumberViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new PhoneNumberTable();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();
            tempItem.CountryCode = phoneNumberViewModel.CountryCode;
            tempItem.PhoneNumber = phoneNumberViewModel.PhoneNumber;

            try
            {
                //update address of the user's resume
                db.PhoneNumberTables.Add(tempItem);
                db.SaveChanges();

                var message = "phone number was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the phone number flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
                //only update if the flag is not already set.
                if (tempUserResume.PhoneNumber == false)
                {
                    tempUserResume.PhoneNumber = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "phone number on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel AddItem(ExtraCurricularViewModel extraCurricularViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new ExtraCurricular();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();
            tempItem.ExtraCurricularNotes2 = extraCurricularViewModel.ExtraCurricularNotes2;
            tempItem.ExtraCurricularOtherNotes1 = extraCurricularViewModel.ExtraCurricularOtherNotes1;

            try
            {
                //update extra curricular of the user's resume
                db.ExtraCurriculars.Add(tempItem);
                db.SaveChanges();

                var message = "extra curricular was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the extra curricular flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
                //only update if the flag is not already set.
                if (tempUserResume.ExtraCurricularActivitiesSummary == false)
                {
                    tempUserResume.ExtraCurricularActivitiesSummary = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "extra curricular flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel AddItem(ProjectDetailViewModel projectDetailViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new ProjectDetail();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();

            tempItem.ProjectDescription = projectDetailViewModel.ProjectDescription;
            tempItem.ProjectNotes1 = projectDetailViewModel.ProjectNotes1;
            tempItem.ProjectNotes2 = projectDetailViewModel.ProjectNotes2;
            tempItem.ProjectTitle = projectDetailViewModel.ProjectTitle;
            tempItem.YearOfProject = projectDetailViewModel.YearOfProject;


            try
            {
                //update project detail of the user's resume
                db.ProjectDetails.Add(tempItem);
                db.SaveChanges();

                var message = "project detail was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the project detail of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
                //only update if the flag is not already set.
                if (tempUserResume.ProjectDetails == false)
                {
                    tempUserResume.ProjectDetails = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "project detail flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel AddItem(OtherStuffViewModel otherStuffViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new OtherStuff();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();

            tempItem.OtherStuffNotes1 = otherStuffViewModel.OtherStuffNotes1;
            tempItem.OtherStuffNotes2 = otherStuffViewModel.OtherStuffNotes2;
            tempItem.OtherStuffNotes3 = otherStuffViewModel.OtherStuffNotes3;
            tempItem.OtherStuffNotes4 = otherStuffViewModel.OtherStuffNotes4;
            tempItem.OtherStuffNotes5 = otherStuffViewModel.OtherStuffNotes5;

            try
            {
                //update other stuff of the user's resume
                db.OtherStuffs.Add(tempItem);
                db.SaveChanges();

                var message = "other stuff was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the other stuff flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

                //TODO - this flag is missing in the UserResume class. add and update this.

                //only update if the flag is not already set.
                if (tempUserResume.GetOtherStuff == false)
                {
                    tempUserResume.GetOtherStuff = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "other stuff flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel AddItem(AddressViewModel addressViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            //we are updating so table should already exist. but still.
            var tempItem = new AddressTable();
            tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.UniqueGuid = Guid.NewGuid().ToString();
            tempItem.AddressExtraNotes = addressViewModel.AddressExtraNotes;
            tempItem.AddressLineOne = addressViewModel.AddressLineOne;
            tempItem.AddressLineTwo = addressViewModel.AddressLineTwo;
            tempItem.City = addressViewModel.City;
            tempItem.Landmark = addressViewModel.Landmark;
            tempItem.Pincode = addressViewModel.Pincode;
            //TODO - we will have to switch off primary address that may have been set to other addresses on the resume
            tempItem.PrimaryAddress = addressViewModel.PrimaryAddress;
            tempItem.State = addressViewModel.State;

            try
            {
                //update address of the user's resume
                db.AddressTables.Add(tempItem);
                db.SaveChanges();

                var message = "Address was added successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;

                    var message2 = "Brand new Resume was created";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
                //only update if the flag is not already set.
                if (tempUserResume.Address == false)
                {
                    tempUserResume.Address = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "Address flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel UpdateItem(PhoneNumberViewModel phoneNumberViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.PhoneNumberTables.Select(x => x).Where(x => x.UniqueGuid == phoneNumberViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them

            tempItem.CountryCode = phoneNumberViewModel.CountryCode;
            tempItem.PhoneNumber = phoneNumberViewModel.PhoneNumber;


            try
            {
                //update phone number of the user's resume
                db.PhoneNumberTables.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "phone number was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }
                //only update if the flag is not already set.
                if (tempUserResume.PhoneNumber == false)
                {
                    tempUserResume.PhoneNumber = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "phone number flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        //TODO - merge common functionality in AddAddress and UpdateAddress - code duplication
        internal GeneralResponseModel UpdateItem(AddressViewModel addressViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.AddressTables.Select(x => x).Where(x => x.UniqueGuid == addressViewModel.UniqueGuid).FirstOrDefault();
            if(tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them
            tempItem.AddressExtraNotes = addressViewModel.AddressExtraNotes;
            tempItem.AddressLineOne = addressViewModel.AddressLineOne;
            tempItem.AddressLineTwo = addressViewModel.AddressLineTwo;
            //we are not updating user ID and guid as they are suppose to be unupdatable
            //tempItem.AspNetUsersUniqueIdentifier = userId;
            tempItem.City = addressViewModel.City;
            tempItem.Landmark = addressViewModel.Landmark;
            tempItem.Pincode = addressViewModel.Pincode;
            //TODO - we will have to switch off primary address that may have been set to other addresses on the resume
            tempItem.PrimaryAddress = addressViewModel.PrimaryAddress;
            tempItem.State = addressViewModel.State;

            try
            {
                //update address of the user's resume
                db.AddressTables.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "Address was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if(tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }
                //only update if the flag is not already set.
                if (tempUserResume.Address == false)
                {
                    tempUserResume.Address = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "Address flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel UpdateItem(SkillsTableViewModel skillsTableViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.SkillsTables.Select(x => x).Where(x => x.UniqueGuid == skillsTableViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them

            tempItem.SkillDescription = skillsTableViewModel.SkillDescription;
            tempItem.SkillExperience = skillsTableViewModel.SkillExperience;
            tempItem.SkillOtherNotes1 = skillsTableViewModel.SkillOtherNotes1;
            tempItem.SkillOtherNotes2 = skillsTableViewModel.SkillOtherNotes2;
            tempItem.SkillTitle = skillsTableViewModel.SkillTitle;


            try
            {
                //update skill of the user's resume
                db.SkillsTables.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "skill was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }
                //only update if the flag is not already set.
                if (tempUserResume.PhoneNumber == false)
                {
                    tempUserResume.PhoneNumber = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "skill flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel DeleteItem(DeleteEducationalDetailViewModel deleteEducationalDetailViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.EducationalDetails.Select(x => x).Where(x => x.UniqueGuid == deleteEducationalDetailViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //at this point, we know, we have an item

            try
            {
                //delete the element

                db.EducationalDetails.Remove(tempItem);
                db.SaveChanges();

                var message = "education entry was deleted successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel UpdateItem(EducationalDetailViewModel educationalDetailViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.EducationalDetails.Select(x => x).Where(x => x.UniqueGuid == educationalDetailViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them

            tempItem.EducationOtherNotes1 = educationalDetailViewModel.EducationOtherNotes1;
            tempItem.EducationOtherNotes2 = educationalDetailViewModel.EducationOtherNotes2;
            tempItem.EducationTitle = educationalDetailViewModel.EducationTitle;
            tempItem.InstituationName = educationalDetailViewModel.InstituationName;
            tempItem.PassGrade = educationalDetailViewModel.PassGrade;
            tempItem.YearOfGraduation = educationalDetailViewModel.YearOfGraduation;

            try
            {
                //update education of the user's resume
                db.EducationalDetails.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "education was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }
                //only update if the flag is not already set.
                if (tempUserResume.EducationalDetailsSummary == false)
                {
                    tempUserResume.EducationalDetailsSummary = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "education flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel UpdateItem(ExtraCurricularViewModel extraCurricularViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.ExtraCurriculars.Select(x => x).Where(x => x.UniqueGuid == extraCurricularViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them

            tempItem.ExtraCurricularNotes2 = extraCurricularViewModel.ExtraCurricularNotes2;
            tempItem.ExtraCurricularOtherNotes1 = extraCurricularViewModel.ExtraCurricularOtherNotes1;


            try
            {
                //update extra curricural of the user's resume
                db.ExtraCurriculars.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "extra curricural was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }
                //only update if the flag is not already set.
                if (tempUserResume.ExtraCurricularActivitiesSummary == false)
                {
                    tempUserResume.ExtraCurricularActivitiesSummary = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "extra curricural flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel UpdateItem(OtherStuffViewModel otherStuffViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.OtherStuffs.Select(x => x).Where(x => x.UniqueGuid == otherStuffViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them

            tempItem.OtherStuffNotes1 = otherStuffViewModel.OtherStuffNotes1;
            tempItem.OtherStuffNotes2 = otherStuffViewModel.OtherStuffNotes2;
            tempItem.OtherStuffNotes3 = otherStuffViewModel.OtherStuffNotes3;
            tempItem.OtherStuffNotes4 = otherStuffViewModel.OtherStuffNotes4;
            tempItem.OtherStuffNotes5 = otherStuffViewModel.OtherStuffNotes5;


            try
            {
                //update extra curricural of the user's resume
                db.OtherStuffs.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "extra curricural was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }

                //TODO - other stuff has no flag. please implement it.

                //only update if the flag is not already set.
                if (tempUserResume.GetOtherStuff == false)
                {
                    tempUserResume.GetOtherStuff = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "extra curricural flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

        internal GeneralResponseModel UpdateItem(ProjectDetailViewModel projectDetailViewModel, string userId)
        {
            var tempGeneralResponseModel = new GeneralResponseModel();

            var tempItem = db.ProjectDetails.Select(x => x).Where(x => x.UniqueGuid == projectDetailViewModel.UniqueGuid).FirstOrDefault();
            if (tempItem == null)
            {
                var message3 = "Pleaes check GUID is correct";
                tempGeneralResponseModel.ListOfResponses.Add(message3);
                return tempGeneralResponseModel;
            }

            //TODO - if the provided values are null dont update them

            tempItem.ProjectDescription = projectDetailViewModel.ProjectDescription;
            tempItem.ProjectNotes1 = projectDetailViewModel.ProjectNotes1;
            tempItem.ProjectNotes2 = projectDetailViewModel.ProjectNotes2;
            tempItem.ProjectTitle = projectDetailViewModel.ProjectTitle;
            tempItem.YearOfProject = projectDetailViewModel.YearOfProject;

            try
            {
                //update project detail of the user's resume
                db.ProjectDetails.AddOrUpdate(tempItem);
                db.SaveChanges();

                var message = "project detail was updated successfully.";
                tempGeneralResponseModel.ListOfResponses.Add(message);

                //update the address flag of the user.
                var tempUserResume = db.UserResumes.Select(x => x).Where(x => x.AspNetUsersUniqueIdentifier == userId).FirstOrDefault();
                if (tempUserResume == null)
                {
                    //a user resume has never been created. lets do that. 
                    tempUserResume = new UserResume();
                    tempUserResume.AspNetUsersUniqueIdentifier = userId;
                }
                //only update if the flag is not already set.
                if (tempUserResume.ProjectDetails == false)
                {
                    tempUserResume.ProjectDetails = true;
                    db.UserResumes.AddOrUpdate(tempUserResume);
                    db.SaveChanges();

                    var message2 = "project detail flag on User Resume was updated successfully.";
                    tempGeneralResponseModel.ListOfResponses.Add(message2);
                }
            }
            catch (Exception e)
            {
                var message = "Error - " + e.ToString();
                tempGeneralResponseModel.ListOfResponses.Add(message);
            }

            return tempGeneralResponseModel;
        }

    }
}