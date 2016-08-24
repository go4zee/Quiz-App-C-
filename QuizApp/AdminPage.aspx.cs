using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.Configuration;
using QuizApp.Core.Authentication;
using QuizApp.Core.Database;
using QuizApp.Core.DataHandlers;
using BCrypt.Net;

namespace QuizApp
{
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthUser"] != null)
            {
                if (Session["UserType"] != null)
                    if ((int)Session["UserType"] < 100)
                        Response.Redirect("Dashboard.aspx");
            }


        }

        protected bool validateAddUserForm()
        {
            bool retVal = false;

            if (userNameTextBox.Value.ToString() != "" && passwordTextBox.Value.ToString() == confirmpasswordTextBox.Value.ToString() && passwordTextBox.Value.ToString() != ""
                && userFirstNameTextBox.Value.ToString() != "" && userLastNameTextBox.Value.ToString() != "")
            {
                AuthUser au = AuthUser.GetAuthUser(Session);
                User u = UserHandler.CreateNew(au);

                
                u.UserName = userNameTextBox.Value.ToString();
                u.Password = passwordTextBox.Value.ToString();
                u.UserFirstName = userFirstNameTextBox.Value.ToString();
                u.UserLastName = userLastNameTextBox.Value.ToString();
                
            }
            else
            {

            }

            return retVal;
        }

        protected void AddNewUserButton_Click(object sender, EventArgs e)
        {
            AuthUser au = AuthUser.GetAuthUser(Session);

        }
    }
}