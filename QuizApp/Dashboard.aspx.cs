using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using QuizApp.Core.Authentication;
using QuizApp.Core.Database;

namespace QuizApp
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeQuizList();
        }


        public void InitializeQuizList()
        {

            string QueryString = @"Select QuizName, QuizID FROM QuizList";

            DataTable datatable = new DataTable();
            datatable = QueryExecute(QueryString, QuizDBContext.QuizConnectionString() );

            if (datatable != null)
            {
                if (datatable.Rows.Count > 0)
                {
                    foreach (DataRow dr in datatable.Rows)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.InnerText = dr["QuizName"].ToString();
                        li.Attributes["class"] = "list-group-item";

                        HtmlButton editButton = new HtmlButton();
                        editButton.InnerText = "Edit";
                        editButton.Attributes["class"] = "btn btn-primary";
                        editButton.Attributes["value"] = SecurityClass.EncryptString(dr["QuizName"].ToString(), "TableNamePhrase");
                        editButton.Attributes["quizid"] = SecurityClass.EncryptString(dr["QuizID"].ToString(), "QuizID");
                        editButton.Attributes["action"] = SecurityClass.EncryptString("Edit", "ActionPhrase");
                        editButton.Attributes["runat"] = "server";
                        editButton.ServerClick += EditQuiz;
                        editButton.Attributes["id"] = "editButton";
                        

                        HtmlButton DeleteButton = new HtmlButton();
                        DeleteButton.InnerText = "Delete";
                        DeleteButton.Attributes["class"] = "btn btn-danger";
                        DeleteButton.Attributes["value"] = SecurityClass.EncryptString(dr["QuizName"].ToString(), "TableNamePhrase");
                        DeleteButton.Attributes["action"] = SecurityClass.EncryptString("Delete", "ActionPhrase");
                        DeleteButton.Attributes["quizid"] = SecurityClass.EncryptString(dr["QuizID"].ToString(), "QuizID");
                        DeleteButton.Attributes["runat"] = "server";
                        DeleteButton.ServerClick += DeleteQuiz;
                        DeleteButton.Attributes["id"] = "deleteButton";

                        HtmlButton SendToButton = new HtmlButton();
                        SendToButton.InnerText = "Send To Email";
                        SendToButton.Attributes["class"] = "btn btn-success";
                        SendToButton.Attributes["value"] = SecurityClass.EncryptString(dr["QuizName"].ToString(), "TableNamePhrase");
                        SendToButton.Attributes["action"] = SecurityClass.EncryptString("Delete", "ActionPhrase");
                        SendToButton.Attributes["quizid"] = SecurityClass.EncryptString(dr["QuizID"].ToString(), "QuizID");
                        SendToButton.Attributes["runat"] = "server";
                        SendToButton.ServerClick += DeleteQuiz;
                        SendToButton.Attributes["id"] = "sendToEmailButton";

                        QuizListDiv.Controls.Add(li);
                        li.Controls.Add(editButton);
                        li.Controls.Add(DeleteButton);
                        li.Controls.Add(SendToButton);
                    }
                }
            }
            



        }

        protected void CreateNewQuiz(object sender, EventArgs e)
        {
            if(QuizNameTextBox.Text != "")
            {
                Session["TableName"] = SecurityClass.EncryptString(QuizNameTextBox.Text, "TableNamePhrase");
                Session["Action"] = SecurityClass.EncryptString("Create", "ActionPhrase");

                Response.Redirect("QuizSetup.aspx");
            }
        }

        private void DeleteQuiz(object sender, EventArgs e)
        {
            HtmlButton button = (HtmlButton)sender;
            string parameterTableName = button.Attributes["value"];
            string parameterAction = button.Attributes["action"];
            string parameterQuizID = button.Attributes["quizid"];

            Session["TableName"] = parameterTableName;
            Session["Action"] = parameterAction;
            Session["QuizID"] = parameterQuizID;

            Response.Redirect("QuizSetup.aspx");
        }

        public DataTable QueryExecute(string QueryString, string Connection)
        {
            DataTable retVal = new DataTable();
            using (SqlConnection SqlConnection1 = new SqlConnection(Connection))
            {
                try
                {
                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(QueryString, SqlConnection1);
                    adapter.Fill(dataset, "QuizList");
                    retVal = dataset.Tables["QuizList"];
                }
                catch
                {
                    retVal = null;
                }
            }

            return retVal;
        }

        protected void EditQuiz(object sender, EventArgs e)
        {
            HtmlButton button = (HtmlButton)sender;
            string parameterTableName = button.Attributes["value"];
            string parameterAction = button.Attributes["action"];
            string parameterQuizID = button.Attributes["quizid"];

            Session["TableName"] = parameterTableName;
            Session["Action"] = parameterAction;
            Session["QuizID"] = parameterQuizID;

            Response.Redirect("QuizSetup.aspx");
        }

    }
}