using System;
using System.Net;
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
            Session["display"] = "hidden";

            testLabel.Text = tableName + "____" + action + "____" + quizid;


        }

        protected void InitializeQuizSetup()
        {
            var parameterTableName = Session["TableName"];
            var parameterAction = Session["Action"];
            var parameterQuizID = Session["QuizID"];

            if (parameterAction != null && parameterTableName != null)
            {
                string decoded_parameterTableName = SecurityClass.DecryptString(parameterTableName.ToString(), "TableNamePhrase");
                string decoded_action = SecurityClass.DecryptString(parameterAction.ToString(), "ActionPhrase");

                tableName = decoded_parameterTableName;
                action = decoded_action;
                if (decoded_action == "Create")
                {
                    ChooseAction(decoded_action, decoded_parameterTableName);
                }
                else if ((decoded_action == "Edit" || decoded_action == "Delete") && parameterQuizID != null)
                {
                    quizid = SecurityClass.DecryptString(parameterQuizID.ToString(), "QuizID");

                    ChooseAction(decoded_action, decoded_parameterTableName);
                }
                else
                {
                    testLabel.Text = "Error, Select a table";
                    tableName = "";
                    action = "";
                    Response.Redirect("Dashboard.aspx");
                }
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
                string QuizNameQueryString = "CREATE TABLE " + TableName + "(QuizTableQuizID uniqueidentifier NOT NULL,QuizTableQuizName varchar(50) NOT NULL,QuizTableQuestionNumber int IDENTITY(1,1), QuizTableQuizQuestion text NOT NULL, QuizTableQuizAnswer1 text NULL, QuizTableQuizAnswer2 text NULL, QuizTableQuizAnswer3 text NULL,QuizTableQuizAnswer4 text NULL, QuizTableQuizAnswer5 text NULL, QuizTableQuizAnswer6 text NULL, QuizTableAnswer text NOT NULL, PRIMARY KEY (QuizTableQuestionNumber))";
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

        protected void EditTable(string TableName)
        {
            string QueryString = @"Select * FROM " + TableName;

            DataTable datatable = new DataTable();
            datatable = QueryExecute(QueryString, QuizDBContext.QuizConnectionString());

            if (datatable != null)
            {
                if (datatable.Rows.Count > 0)
                {
                    foreach (DataRow dr in datatable.Rows)
                    {
                        LinkButton li = new LinkButton();
                        li.Text = dr["QuizTableQuizQuestion"].ToString();
                        li.Attributes["class"] = "list-group-item";
                        li.Attributes["rowid"] = dr["QuizTableQuestionNumber"].ToString();



                        quizQuestionListBar.Controls.Add(li);
                    }
                }
            }


        }
        protected void DeleteTable(string TableName)
        {
            string ConnectionString = QuizDBContext.QuizConnectionString();

            if (TableName != "")
            {
                string RemoveQuizListQueryString = "DELETE FROM QuizList WHERE QuizID='" + quizid + "'";

                string DropQueryString = "IF OBJECT_ID('" + TableName + "', 'U') IS NOT NULL ";
                DropQueryString += "BEGIN ";
                DropQueryString += "DROP TABLE " + TableName;
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

        protected void btnAddNewQuestion_Click(object sender, EventArgs e)
        {
            AddQuestion("CreateQuestion");
        }

        protected void AddQuestion(string QuestionAction)
        {
            if (QuestionAction == "CreateQuestion")
            {
                AddHTMLElements(false);
            }
        }

        /// <summary>
        /// if iscurrent is true then the DB is called and popoultes the options, else just adds the default structure of DOM Elements
        /// </summary>
        /// <param name="isCurrent"></param>
        private void AddHTMLElements(bool isCurrent)
        {
            if (!isCurrent)
            {
                Session["display"] = "block";
                quizEditWindowContainer.Attributes.Add("style", "display:block");

            }
        }


        protected void ChooseAction(string Action, string TableName)
        {
            switch (Action)
            {
                case "Create":
                    CreateTable(TableName); break;

                case "Edit":
                    EditTable(TableName); break;

                case "Delete":
                    DeleteTable(TableName); break;

                default:
                    Response.Redirect("Dashboard.aspx"); break;
            }
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
                    adapter.Fill(dataset, tableName);
                    retVal = dataset.Tables[tableName];
                }
                catch
                {
                    retVal = null;
                }
            }

            return retVal;
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
                switch (QueryType)
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
                        }
                        break;

                    default: break;
                }

            }

        }

        protected void FillElementsFromDB(object sender, EventArgs e)
        {

        }

        protected void btnQuestionCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuizSetup.aspx");
        }

        protected void btnAddQuestionToDB_Click(object sender, EventArgs e)
        {
            //css value for bootstrap alert type danger
            string alertTypeDanger = "alert alert-danger";
            //Checks the set session variable for extra security if the display property of questionWindow is modified on runtime and prevents attacks
            if (Session["display"].ToString() != "block")
            {
                if (txtQuestion.InnerText == "")
                {
                    SetMessage("The Question Cannot be Empty", alertTypeDanger);
                }
                else
                {
                    int answerCount = 0;
                    int checkCount = 0;

                    List<int> answerArray = new List<int>();

                    if (txtanswer1.Value != "")
                        answerCount++;
                    if (txtanswer2.Value != "")
                        answerCount++;
                    if (txtanswer3.Value != "")
                        answerCount++;
                    if (txtanswer4.Value != "")
                        answerCount++;
                    if (txtanswer5.Value != "")
                        answerCount++;
                    if (txtanswer6.Value != "")
                        answerCount++;

                    if (checkbox1.Checked == true)
                    {
                        checkCount++;
                        answerArray.Add(1);
                    }

                    if (checkbox2.Checked == true)
                    {
                        checkCount++;
                        answerArray.Add(2);
                    }
                    if (checkbox3.Checked == true)
                    {
                        checkCount++;
                        answerArray.Add(3);
                    }
                    if (checkbox4.Checked == true)
                    {
                        checkCount++;
                        answerArray.Add(4);
                    }
                    if (checkbox5.Checked == true)
                    {
                        checkCount++;
                        answerArray.Add(5);
                    }
                    if (checkbox6.Checked == true)
                    {
                        checkCount++;
                        answerArray.Add(6);
                    }

                    if (answerCount > 1)
                    {
                        if (checkCount > 0)
                        {
                            SetMessage("Success", "alert alert-success");
                            string answerList = string.Join(",", answerArray);
                            //string QueryString = string.Format("SET IDENTITY_INSERT {0} ON;", tableName);
                            string QueryString = string.Format("INSERT INTO {0} ", tableName);
                            QueryString += "(QuizTableQuizID, QuizTableQuizName, QuizTableQuizQuestion, QuizTableQuizAnswer1, QuizTableQuizAnswer2, QuizTableQuizAnswer3, QuizTableQuizAnswer4, QuizTableQuizAnswer5, QuizTableQuizAnswer6, QuizTableAnswer) ";
                            QueryString += "VALUES (@QuizID, @QuizName, @QuizQuestion, @Answer1, @Answer2, @Answer3, @Answer4, @Answer5, @Answer6,@Answer)";
                            //QueryString += string.Format("SET IDENTITY_INSERT {0} OFF; ", tableName);
                            //NonQueryExecute(QueryString, QuizDBContext.QuizConnectionString(), "AddQuestion");
                            using (SqlConnection SqlConnection1 = new SqlConnection(QuizDBContext.QuizConnectionString()))
                            {
                                try
                                {
                                    SqlCommand Command = new SqlCommand(QueryString, SqlConnection1);
                                    Command.Parameters.AddWithValue("QuizID", quizid);
                                    Command.Parameters.AddWithValue("QuizName", tableName);
                                    Command.Parameters.AddWithValue("QuizQuestion", txtQuestion.Value);
                                    Command.Parameters.AddWithValue("Answer1", txtanswer1.Value);
                                    Command.Parameters.AddWithValue("Answer2", txtanswer2.Value);
                                    Command.Parameters.AddWithValue("Answer3", txtanswer3.Value);
                                    Command.Parameters.AddWithValue("Answer4", txtanswer4.Value);
                                    Command.Parameters.AddWithValue("Answer5", txtanswer5.Value);
                                    Command.Parameters.AddWithValue("Answer6", txtanswer6.Value);
                                    Command.Parameters.AddWithValue("Answer", answerList);
                                    Command.Connection.Open();
                                    Command.ExecuteNonQuery();
                                    Command.Connection.Close();

                                }
                                catch (Exception p)
                                {
                                    string s = p.ToString();
                                }

                                EditTable(tableName);

                            }
                        }
                        else
                        {
                            SetMessage("Select Atleast one answer", alertTypeDanger);
                        }
                    }
                    else
                    {
                        SetMessage("Atleast two answers are required", alertTypeDanger);
                    }

                }
            }
            else
            {
                Response.Redirect("QuizSetup.aspx");
            }
        }

        protected void SetMessage(string Message, string AlertType)
        {
            if (Message != "" && AlertType != "")
            {
                HtmlGenericControl alertDiv = new HtmlGenericControl("div");
                alertDiv.Attributes["class"] = AlertType;
                alertDiv.Attributes["id"] = "alertDiv";
                alertDiv.Attributes["role"] = "alert";
                alertDiv.Attributes["style"] = "width: 100%";
                alertDiv.InnerText = Message;

                quizEditWindowContainer.Controls.Add(alertDiv);

            }

        }
    }
}