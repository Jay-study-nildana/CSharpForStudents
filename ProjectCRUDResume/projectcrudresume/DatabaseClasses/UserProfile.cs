using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.DatabaseClasses
{
    #region notes
    //this is the class that will store unique information about the user. 
    //related to the account. 
    //this is the database related class. dont use them in Postman
    #endregion
    public class UserProfile
    {
        public int ID { set; get; }
        public string AspNetUsersUniqueIdentifier { set; get; } //this is the id from AspNetUsersTable
        public string UserUniqueKey { set; get; }  //not used anymore.
        public string UserEmail { set; get; }
        public bool UserActiveStatus { set; get; }
    }
}