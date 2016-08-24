using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.Configuration;
using QuizApp.Core.Authentication;
using QuizApp.Core.Database;
using BCrypt.Net;

namespace QuizApp
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthUser"] != null)
            {
                if (Session["AuthUser"] != null && Session["UserName"].ToString() != "")
                {
                    usernameLabel.Text = "Welcome, " + Session["UserName"].ToString();
                }
            }
            else
            {
                //Response.Redirect("Login.aspx");
            }
            
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }
    }
}