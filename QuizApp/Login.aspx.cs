using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI.WebControls;
using App.Web.Security;

using QuizApp.Core.Authentication;
using QuizApp.Core.Notification;

namespace QuizApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                txtLogin.Text = "";
                txtPassword.Value = "";
                txtStatus.Text = "";

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtLogin.Text.Length > 0 && txtLogin.Text.Length > 0)
            {
                AuthUserLoginResponse aulr = AuthUser.DoLogin((String)txtLogin.Text, (String)txtPassword.Value, Session, checkRememberMe.Checked);
                if(!aulr.IsSuccess)
                {
                    txtStatus.Text = aulr.ErrorText;
                    Core.Notification.NotificationHandler.AddWebNotification(aulr.ErrorText, this, Core.Notification.NotificationHandler.NotificationTypes.Error);
                }
                else
                {
                    /**Code for Remember me cookie. Need to work on the remember me function
                    if (checkRememberMe.Checked == true)
                    {
                        Response.Cookies.Clear();

                        // Set the new expiry date - to thirty days from now
                        DateTime expiryDate = DateTime.Now.AddDays(30);

                        // Create a new forms auth ticket
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, "UserName", DateTime.Now, expiryDate, true, Session["UserName"].ToString());

                        // Encrypt the ticket
                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                        // Create a new authentication cookie - and set its expiration date
                        HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        authenticationCookie.Expires = ticket.Expiration;

                        // Add the cookie to the response.
                        Response.Cookies.Add(authenticationCookie);
                    }
    **/
                    string redirectTo = "~/Dashboard.aspx";
                    
                    Response.Redirect(redirectTo);
                }
            }
            else
            {
                txtStatus.Text = "Incorret Login, Please try again!////";
                Core.Notification.NotificationHandler.AddWebNotification("Incorrect Login, Please try again.////", this, Core.Notification.NotificationHandler.NotificationTypes.Error);
            }
        }

        public static void SetCookie(object sender, EventArgs e)
        {
                // Clear any other tickets that are already in the response
               
        }

        protected override void OnPreRender(EventArgs e)
        {
            AuthUser au = AuthUser.GetAuthUser(Session);

            if(au != null)
            {
                Response.Redirect("~/Dashboard.aspx");
            }
            else
            {
                ClientScript.GetPostBackEventReference(this, string.Empty);
                txtPassword.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + btnLogin.UniqueID + "','')");
                base.OnPreRender(e);
            }
        }
       
    }
}