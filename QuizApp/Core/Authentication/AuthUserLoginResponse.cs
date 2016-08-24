using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApp.Core.Authentication
{
    public class AuthUserLoginResponse
    {
        public bool IsSuccess = false;
        public string ErrorText = "";
        public Guid UserID = Guid.Empty;
    }
}