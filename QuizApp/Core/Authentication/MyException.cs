using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuizApp.Core.Notification;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace QuizApp.Core.Authentication
{
    public class MyException : ApplicationException
    {
        public void NotFoundinDBException(String Message, Page page)
        {
            NotificationHandler.AddWebNotification(Message, page, NotificationHandler.NotificationTypes.Error);
            
        }
    }
}