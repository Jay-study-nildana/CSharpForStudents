using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectcrudresume.PostmanClasses
{
    #region notes

    //as the name suggests this is the view model of UserProfile

    #endregion
    public class UserProfileViewModel
    {
        public string UserUniqueKey { set; get; }
        public string UserEmail { set; get; }
        public bool UserActiveStatus { set; get; }

    }
}