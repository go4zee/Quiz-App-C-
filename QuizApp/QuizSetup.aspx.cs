using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuizApp.Core.Authentication;
namespace QuizApp
{
    public partial class QuizSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string test = "1O5S8OjVXEJum4is9H%2B06hg%3D%3D";
            string testdecode = Server.UrlDecode(test);

            /*
            string parameterTableName = HttpUtility.UrlDecode(Request.QueryString["parameter"].ToString());
            string action = Server.UrlDecode(Request.QueryString["action"].ToString());
            string decoded_parameterTableName = SecurityClass.DecryptString(parameterTableName, "ButtonType");
            string decoded_action = SecurityClass.DecryptString(action, "Action");
            testLabel.Text = decoded_parameterTableName;
            */

            string parameterTableName = WebUtility.UrlDecode(Request.QueryString["parameter"].ToString());
            string action = HttpUtility.UrlDecode(Request.QueryString["action"].ToString());
            string s = "iop";
            string decoded_parameterTableName = SecurityClass.DecryptString(parameterTableName, "ButtonType");
            string decoded_action = SecurityClass.DecryptString(action, "Action");

            testLabel.Text = decoded_parameterTableName;
            try
            {
               
            }
            catch
            {
                //Response.Redirect("Dashboard.aspx");
            }
            
        }
    }
}