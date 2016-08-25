using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuizApp.Core.Authentication;
using QuizApp.Core.Database;
using System.Threading;

namespace QuizApp
{
    public partial class QuizSetup : System.Web.UI.Page
    {
        string tableName;
        string action;
        string quizid;

        Guid quizID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Remove after final implementation
            Session["UserID"] = "62839730-9bf4-4724-b1ae-1759e0f21b20";

            InitializeQuizSetup();

            testLabel.Text = tableName + "____" + action + "____"+quizid;


        }

        protected void InitializeQuizSetup()
        {
            var parameterTableName = Session["TableName"];
            var parameterAction = Session["Action"];
            var parameterQuizID = Session["QuizID"];

            if(parameterAction !=null && parameterTableName != null)
            {
                string decoded_parameterTableName = SecurityClass.DecryptString(parameterTableName.ToString(), "TableNamePhrase");
                string decoded_action = SecurityClass.DecryptString(parameterAction.ToString(), "ActionPhrase");

                tableName = decoded_parameterTableName;
                action = decoded_action;

                if (decoded_action == "Create")
                {
                    ChooseAction(decoded_action, decoded_parameterTableName);
                }

                if (parameterQuizID != null)
                {
                    
                    quizid = decoded_quizid;

                    


                }

                else
                {

                    testLabel.Text = "Error, Select a table";
                    tableName = "";
                    action = "";
                    Response.Redirect("Dashboard.aspx");

                    //Set Error Message and redirect to the dashboard.aspx page.
                }
            }
        }

        protected void ChooseAction(string Action, string TableName)
        {
            switch (Action)
            {
                case "Create":
                    CreateTable(TableName); break;

                case "Edit": break;

                case "Delete":
                    DeleteTable(tableName); break;

                default:
                    Response.Redirect("Dashboard.aspx"); break;
            }
        }

        /// <summary>
        /// creates table with the specified table name in the Quiz Database and adds a entry into the QuizList Table
        /// </summary>
        /// <param name="TableName"></param>
        protected void CreateTable(string TableName)
        {
            string ConnectionString = QuizDBContext.QuizConnectionString();

            if (TableName != "")
            {
                string QuizListQueryString = "INSERT INTO QuizList VALUES (@QuizID, @QuizName, @QuizCreatedByUserID, @QuizCreatedOn, @QuizModifiedOn) ";
                string QuizNameQueryString = "CREATE TABLE " + TableName + "(QuizTableQuizID uniqueidentifier NOT NULL,QuizTableQuizName varchar(50) NOT NULL, QuizTableQuizQuestion text NOT NULL, QuizTableQuizAnswer1 text NULL, QuizTableQuizAnswer2 text NULL, QuizTableQuizAnswer3 text NULL,QuizTableQuizAnswer4 text NULL, QuizTableQuizAnswer5 text NULL, QuizTableQuizAnswer6 text NULL, PRIMARY KEY (QuizTableQuizID))";
                try
                {
                    NonQueryExecute(QuizListQueryString, ConnectionString, "CreateList");
                    NonQueryExecute(QuizNameQueryString, ConnectionString, "CreateTable");
                }
                catch
                {
                    testLabel.Text = "Error";
                }
            }
            else
            {
                testLabel.Text = "No Table selected";
            }

        }

        protected void DeleteTable(string TableName)
        {
            string ConnectionString = QuizDBContext.QuizConnectionString();

            if(TableName != "")
            {
                string RemoveQuizListQueryString = "DELETE FROM QuizList WHERE QuizID='"+quizid+"'";

                string DropQueryString = "IF OBJECT_ID('"+TableName+"', 'U') IS NOT NULL ";
                DropQueryString += "BEGIN ";
                DropQueryString += "DROP TABLE "+TableName;
                DropQueryString += " END";
                try
                {
                    NonQueryExecute(DropQueryString, ConnectionString, "DeleteTable");
                    NonQueryExecute(RemoveQuizListQueryString, ConnectionString, "DeleteTable");
                }
                catch
                {
                    string s = "";
                }
            }
        }

        /// <summary>
        /// executes the nonquery
        /// </summary>
        /// <param name="QueryString"></param>
        /// <param name="Connection"></param>
        /// <param name="QueryType">Defines whether the query is for the quiztable or the quizlist</param>
        public void NonQueryExecute(string QueryString, string Connection, string QueryType)
        {
            using (SqlConnection SqlConnection1 = new SqlConnection(Connection))
            {
                switch(QueryType)
                {
                    case "CreateTable":
                        {
                            try
                            {
                                SqlCommand Command = new SqlCommand(QueryString, SqlConnection1);
                                Command.Connection.Open();
                                Command.ExecuteNonQuery();
                                Command.Connection.Close();
                            }
                            catch
                            {
                                string s = "";
                            }
                        }
                        break;

                    case "CreateList":
                        {
                            try
                            {
                                quizID = Guid.NewGuid();
                                SqlCommand Command = new SqlCommand(QueryString, SqlConnection1);
                                Command.Parameters.AddWithValue("QuizID", quizID);
                                Session["QuizID"] = quizID;
                                Command.Parameters.AddWithValue("QuizName", tableName);
                                Command.Parameters.AddWithValue("QuizCreatedByUserID", Session["UserID"].ToString());
                                Command.Parameters.AddWithValue("QuizCreatedOn", DateTime.Now);
                                Command.Parameters.AddWithValue("QuizModifiedOn", DateTime.Now);
                                Command.Connection.Open();
                                Command.ExecuteNonQuery();
                                Command.Connection.Close();
                            }
                            catch
                            {
                                string s = "";
                            }
                        }

                        break;

                    case "DeleteTable":
                        {
                            try
                            {
                                SqlCommand Command = new SqlCommand(QueryString, SqlConnection1);
                                Command.Connection.Open();
                                Command.ExecuteNonQuery();
                                Command.Connection.Close();
                            }
                            catch
                            {
                                string s = "";
                            }
                        }break;
                    default:break;
                }

            }

        }

    }
}