using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuizApp.Core.Authentication;
using QuizApp.Core.Database;

namespace QuizApp.Core.DataHandlers
{
    public class UserHandler
    {
        public static User CreateNew(AuthUser au)
        {
            User u = new User();

            u.UserID = Guid.NewGuid();
            u.UserActive = true;
            u.UserCreatedByUserID = au.UserID;
            u.UserCreatedOn = DateTime.Now;
            u.UserModifiedOn = u.UserCreatedOn;
            
            return u;
        }
    }
}