using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Web.Configuration;
using System.Web.Security;
using QuizApp.Core.Database;
using BCrypt.Net;

namespace QuizApp.Core.Authentication
{
    public class AuthUser
    {
        public enum AuthUserUserType : int
        {
            None = -1,
            Any = 0,
            User = 10,
            Admin = 100,
            Super_Admin = 255
        }

        public enum AccessRightType : int
        {
            Any = 0,
            Administration = 1
        }

        public String DisplayName;
        public Guid UserID = Guid.Empty;
        public String UserEmailAddress = "";
        public AuthUserUserType UserType = AuthUserUserType.None;

        public AuthUser()
        {
            DisplayName = "";
        }

        public static AuthUserLoginResponse DoLogin(String Login, String Password, HttpSessionState Session, bool RememberMe)
        {
            bool passwordMatch = false;
            AuthUserLoginResponse retVal = new AuthUserLoginResponse();

            AuthUser au = new AuthUser();

            CheckBCryptVersion cbv = new CheckBCryptVersion();

            using (PortalDataContext db = new PortalDataContext())
            {
                User u = null;

                try
                {
                    u = db.Users.Where(x => x.UserName == Login).SingleOrDefault();
                    passwordMatch = BCrypt.Net.BCrypt.Verify(Password, u.Password);
                }
                catch 
                {
                    retVal.ErrorText = "Username not found";
                }

                if (u != null && passwordMatch)
                {
                    au.UserType = AuthUserUserType.None;
                    try
                    {
                        au.UserType = (AuthUser.AuthUserUserType)Enum.Parse(typeof(AuthUser.AuthUserUserType), u.UserAccessLevel.ToString());
                        Session["UserType"] = au.UserType;
                        Session["UserID"] = u.UserID;
                        au.DisplayName = u.UserFirstName;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    au = null;
                }
            }

            if (au != null)
            {
                if (Session != null)
                {
                    Session["AuthUser"] = au;
                    Session["UserName"] = au.DisplayName;
                    retVal.IsSuccess = true;
                }
            }
            else
            {
                retVal.IsSuccess = false;
                retVal.ErrorText += "Incorrect Login, Please try again";
            }

            return retVal;

        }



        
        public static AuthUser GetAuthUser(HttpSessionState Session)
        {
            AuthUser retVal = null;

            if(Session != null)
            {
                if(Session["AuthUser"] != null)
                {
                    AuthUser au = (AuthUser)Session["AuthUser"];
                    retVal = au;
                }
            }

            return retVal;
        }

        public static void CheckAuthentication(HttpSessionState Session, HttpResponse Response, HttpRequest Request, Control Parent)
        {
            //
            // make sure on https...
            //
            if (!Request.IsSecureConnection)
            {
                if (!Request.Url.ToString().Contains("/localhost:"))
                    Response.Redirect(Request.Url.ToString().Replace("http:", "https:"));
            }
            //
            // do some cool Active Dir per grabbing.
            //
            if (!IsAuthenticated(Session))
            {
#if DEBUG
                Response.Redirect("http://localhost:61642/Login.aspx", true);
#else
                Response.Redirect("http://", true);
#endif
            }
        }

        private static bool IsAuthenticated(HttpSessionState Session)
        {
            bool retVal = false;
            AuthUser au = (AuthUser)Session["AuthUser"];
            if (au != null)
                retVal = true;
            return (retVal);
        }


       

    }
}