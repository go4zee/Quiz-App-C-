using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace QuizApp.Core.Notification
{
    public class NotificationHandler
    {
        public enum NotificationTypes : int
        {
            General = 1,
            WarningWithFade = 0,
            Warning = 6,
            ErrorWithFade = 9,
            Error = 10
        }

        public static void AddWebNotification(string message, System.Web.UI.Page Page, NotificationTypes NotificationType)
        {
            StringBuilder script = new StringBuilder();

            script.AppendLine("<script type=\"text/javascript\">");
            script.AppendLine("$(document).ready(function () {");
            script.AppendLine("$.jnotify('" + message.Replace("'", "\"").Replace(Environment.NewLine, "<br />") + "'");
            if (NotificationType == NotificationTypes.Error)
            {
                script.Append(", 'error', true");
            }
            else if (NotificationType == NotificationTypes.ErrorWithFade)
            {
                script.Append(", 'error', false");
            }
            else if (NotificationType == NotificationTypes.Warning)
            {
                script.Append(", 'warning', true");
            }
            else if (NotificationType == NotificationTypes.WarningWithFade)
            {
                script.Append(", 'warning', false");
            }
            script.Append(");");
            script.AppendLine("});");
            script.AppendLine("</script>");

            if(Page != null)
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "notificationScript_" + Guid.NewGuid().ToString().Replace("-", ""), script.ToString());
        }
    }
}