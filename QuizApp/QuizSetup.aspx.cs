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

        #region ADD, EDIT and DELETE Table

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
                catch(Exception e)
                {
                    testLabel.Text = e.ToString();
                }
            }
            else
            {
                testLabel.Text = "No Table selected";
            }

        }

        /// <summary>
        /// On Page load Queries the "TableName" table and populates the side bar with the list of questions.
        /// </summary>
        /// <param name="TableName"></param>
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
                        li.Attributes["qnumber"] = dr["QuizTableQuestionNumber"].ToString();
                        li.CommandArgument = dr["QuizTableQuestionNumber"].ToString();
                        li.Attributes["runat"] = "server";
                        li.Click += new System.EventHandler(QuizSideBarLinkButton_Click);

                        HtmlGenericControl questionDiv = new HtmlGenericControl("div");
                        
                        HtmlButton DeleteButton = new HtmlButton();
                        DeleteButton.InnerText = "Delete";
                        DeleteButton.Attributes["class"] = "btn btn-danger btn-xs";
                        DeleteButton.Attributes["runat"] = "server";
                        DeleteButton.ServerClick += DeleteQuestion;
                        DeleteButton.Attributes["qnumber"] = dr["QuizTableQuestionNumber"].ToString();
                        //li.Attributes["onclientclick"] = "LinkButton_Click()";


                        questionDiv.Controls.Add(li);
                        quizQuestionListBar.Controls.Add(questionDiv);
                        questionDiv.Controls.Add(DeleteButton);
                        
                    }
                }
            }


        }

        private void DeleteQuestion(object sender, EventArgs e)
        {
            HtmlButton DeleteButton = (HtmlButton)sender;
            
            if(DeleteButton.Attributes["qnumber"].ToString() != "")
            {
                int rowNumber = Convert.ToInt16(DeleteButton.Attributes["qnumber"].ToString());
                using (SqlConnection SqlConnection1 = new SqlConnection(QuizDBContext.QuizConnectionString()))
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        string DeleteQueryString = string.Format("DELETE FROM {0} WHERE QuizTableQuestionNumber = @qnumber", tableName);
                        SqlCommand UpdateCommand = new SqlCommand(DeleteQueryString, SqlConnection1);
                        UpdateCommand.Parameters.AddWithValue("@qnumber", rowNumber);
                        SqlConnection1.Open();
                        adapter.DeleteCommand = UpdateCommand;

                        UpdateCommand.ExecuteNonQuery();
                        InitializeQuizSetup();
                    }
                    catch(Exception error)
                    {
                        string errorString = error.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the Quiz and removes the list item from the QuizList table tableName=TableName
        /// </summary>
        /// <param name="TableName"></param>
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

        #endregion

        #region HTML Add and Load Elements
        protected void btnAddNewQuestion_Click(object sender, EventArgs e)
        {
            AddHTMLElements(true, null);
        }

        /// <summary>
        /// is called when an existing question is clicked and sets the check box to checked or unchecked based on the answer list
        /// </summary>
        /// <param name="Answer"></param>
        /// <returns></returns>
        protected void SetCheckBoxFromAnswerString(string Answer)
        {
            List<int> answerList = new List<int>();
            string[] seperators = { "," };
            string[] answerString = Answer.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in answerString)
            {
                answerList.Add(Convert.ToInt32(s));
            }
            foreach (int n in answerList)
            {
                switch (n)
                {
                    case 1: checkbox1.Checked = true; break;
                    case 2: checkbox2.Checked = true; break;
                    case 3: checkbox3.Checked = true; break;
                    case 4: checkbox4.Checked = true; break;
                    case 5: checkbox5.Checked = true; break;
                    case 6: checkbox6.Checked = true; break;
                }
            }

        }

        /// <summary>
        /// if isNewQuestion is false then the DB is called and popoultes the options, else just adds the default structure of DOM Elements
        /// 
        /// </summary>
        /// <param name="isNewQuestion"></param>
        private void AddHTMLElements(bool isNewQuestion, DataTable dataTable)
        {
            Session["display"] = "block";
            quizEditWindowContainer.Attributes.Add("style", "display:block");

            if (!isNewQuestion && dataTable.Rows.Count > 0)
            {
                SetCheckBoxFromAnswerString(dataTable.Rows[0].Field<string>("QuizTableAnswer"));

                txtQuestion.InnerText = dataTable.Rows[0].Field<string>("QuizTableQuizQuestion");
                txtanswer1.Value = dataTable.Rows[0].Field<string>("QuizTableQuizAnswer1");
                txtanswer2.Value = dataTable.Rows[0].Field<string>("QuizTableQuizAnswer2");
                txtanswer3.Value = dataTable.Rows[0].Field<string>("QuizTableQuizAnswer3");
                txtanswer4.Value = dataTable.Rows[0].Field<string>("QuizTableQuizAnswer4");
                txtanswer5.Value = dataTable.Rows[0].Field<string>("QuizTableQuizAnswer5");
                txtanswer6.Value = dataTable.Rows[0].Field<string>("QuizTableQuizAnswer6");

                btnAddQuestionToDB.Text = "Update";
                Session["update"] = "true";
            }
            else
            {
                Session["update"] = "false";
                Session["qnumber"] = "";

                checkbox1.Checked = false;
                checkbox2.Checked = false;
                checkbox3.Checked = false;
                checkbox4.Checked = false;
                checkbox5.Checked = false;
                checkbox6.Checked = false;

                txtQuestion.InnerText = "";
                txtanswer1.Value = "";
                txtanswer2.Value = "";
                txtanswer3.Value = "";
                txtanswer4.Value = "";
                txtanswer5.Value = "";
                txtanswer6.Value = "";
                btnAddQuestionToDB.Text = "Add";
            }
        }

        #endregion


        #region SideBar Handles the side bar click events
        /// <summary>
        /// Click event when any question is clicked in EditMode from the sidebar of the QuizSetup.aspx page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QuizSideBarLinkButton_Click(object sender, EventArgs e)
        {
            //Response.Redirect("testpage.aspx");
            LinkButton li = (LinkButton)sender;
            string parameterTableRowID = li.Attributes["qnumber"].ToString();

            if (parameterTableRowID != "")
            {
                DataSet dataset = new DataSet();
                using (SqlConnection SqlConnection1 = new SqlConnection(QuizDBContext.QuizConnectionString()))
                {
                    try
                    {
                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        string QueryString = string.Format("SELECT * FROM {0} WHERE QuizTableQuestionNumber = @qnumber", tableName);
                        SqlCommand Command = new SqlCommand(QueryString, SqlConnection1);
                        Command.Parameters.AddWithValue("@qnumber", parameterTableRowID);
                        SqlConnection1.Open();
                        adapter.SelectCommand = Command;

                        adapter.Fill(dataset, tableName);
                        dataTable = dataset.Tables[tableName];
                        SqlConnection1.Close();

                        if (dataTable.Rows.Count > 0)
                        {
                            AddHTMLElements(false, dataTable);
                            Session["qnumber"] = parameterTableRowID;


                        }

                    }
                    catch (Exception error)
                    {
                        string errorString = error.ToString();
                    }
                }

            }
            else
            {
                Response.Redirect("QuizSetup.aspx");
            }


        }
        #endregion

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

        #region Query and Non-Query Execute
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
                            catch (Exception error)
                            {
                                string errorString = error.ToString();
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
                            catch (Exception error)
                            {
                                string errorString = error.ToString();
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
                            catch (Exception error)
                            {
                                string errorString = error.ToString();
                            }
                        }
                        break;

                    default: break;
                }

            }

        }
        #endregion


        protected void btnQuestionCancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("QuizSetup.aspx");
        }

        protected void btnAddQuestionToDB_Click(object sender, EventArgs e)
        {
            //css value for bootstrap alert type danger
            string alertTypeDanger = "alert alert-danger";
            //Checks the set session variable for extra security if the display property of questionWindow is modified on runtime and prevents attacks
            if (Session["display"].ToString() == "block")
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
                        if (txtanswer1.Value != "")
                        {
                            checkCount++;
                            answerArray.Add(1);
                        }
                        else
                        {
                            SetMessage("Answer cannot be empty", alertTypeDanger);
                        }

                    }

                    if (checkbox2.Checked == true)
                    {
                        if (txtanswer2.Value != "")
                        {
                            checkCount++;
                            answerArray.Add(2);
                        }
                        else
                        {
                            SetMessage("Answer cannot be empty", alertTypeDanger);
                        }
                    }
                    if (checkbox3.Checked == true)
                    {
                        if (txtanswer3.Value != "")
                        {
                            checkCount++;
                            answerArray.Add(3);
                        }
                        else
                        {
                            SetMessage("Answer cannot be empty", alertTypeDanger);
                        }
                    }
                    if (checkbox4.Checked == true)
                    {
                        if (txtanswer4.Value != "")
                        {
                            checkCount++; 
                            answerArray.Add(4);
                        }
                        else
                        {
                            SetMessage("Answer cannot be empty", alertTypeDanger);
                        }
                    }
                    if (checkbox5.Checked == true)
                    {
                        if (txtanswer5.Value != "")
                        {
                            checkCount++;
                            answerArray.Add(5);
                        }
                        else
                        {
                            SetMessage("Answer cannot be empty", alertTypeDanger);
                        }
                    }
                    if (checkbox6.Checked == true)
                    {
                        if (txtanswer6.Value != "")
                        {
                            checkCount++;
                            answerArray.Add(6);
                        }
                        else
                        {
                            SetMessage("Answer cannot be empty", alertTypeDanger);
                        }
                    }

                    if (answerCount > 1)
                    {
                        if (checkCount > 0)
                        {
                            SetMessage("Success", "alert alert-success");
                            string answerList = string.Join(",", answerArray);

                            string InsertQueryString = string.Format("INSERT INTO {0} ", tableName);
                            InsertQueryString += "(QuizTableQuizID, QuizTableQuizName, QuizTableQuizQuestion, QuizTableQuizAnswer1, QuizTableQuizAnswer2, QuizTableQuizAnswer3, QuizTableQuizAnswer4, QuizTableQuizAnswer5, QuizTableQuizAnswer6, QuizTableAnswer) ";
                            InsertQueryString += "VALUES (@QuizID, @QuizName, @QuizQuestion, @Answer1, @Answer2, @Answer3, @Answer4, @Answer5, @Answer6,@Answer)";

                            string UpdateQueryString = string.Format("UPDATE {0} ", tableName);
                            UpdateQueryString += " SET QuizTableQuizID=@QuizID, QuizTableQuizName= @QuizName, QuizTableQuizQuestion=@QuizQuestion, QuizTableQuizAnswer1=@Answer1, QuizTableQuizAnswer2=@Answer2, QuizTableQuizAnswer3=@Answer3, QuizTableQuizAnswer4=@Answer4, QuizTableQuizAnswer5=@Answer5, QuizTableQuizAnswer6=@Answer6, QuizTableAnswer=@Answer ";
                            UpdateQueryString += " WHERE QuizTableQuestionNumber=@qnumber";


                            using (SqlConnection SqlConnection1 = new SqlConnection(QuizDBContext.QuizConnectionString()))
                            {
                                try
                                {
                                    SqlDataAdapter adapter = new SqlDataAdapter();

                                    SqlCommand InsertCommand = new SqlCommand(InsertQueryString, SqlConnection1);
                                    InsertCommand.Parameters.AddWithValue("QuizID", quizid);
                                    InsertCommand.Parameters.AddWithValue("QuizName", tableName);
                                    InsertCommand.Parameters.AddWithValue("QuizQuestion", txtQuestion.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer1", txtanswer1.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer2", txtanswer2.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer3", txtanswer3.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer4", txtanswer4.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer5", txtanswer5.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer6", txtanswer6.Value);
                                    InsertCommand.Parameters.AddWithValue("Answer", answerList);

                                    SqlCommand UpdateCommand = new SqlCommand(UpdateQueryString, SqlConnection1);
                                    UpdateCommand.Parameters.AddWithValue("QuizID", quizid);
                                    UpdateCommand.Parameters.AddWithValue("QuizName", tableName);
                                    UpdateCommand.Parameters.AddWithValue("QuizQuestion", txtQuestion.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer1", txtanswer1.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer2", txtanswer2.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer3", txtanswer3.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer4", txtanswer4.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer5", txtanswer5.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer6", txtanswer6.Value);
                                    UpdateCommand.Parameters.AddWithValue("Answer", answerList);
                                    UpdateCommand.Parameters.AddWithValue("qnumber", Session["qnumber"].ToString());


                                    SqlConnection1.Open();
                                    adapter.InsertCommand = InsertCommand;
                                    adapter.UpdateCommand = UpdateCommand;
                                    if (Session["update"].ToString() == "false")
                                    {
                                        InsertCommand.ExecuteNonQuery();

                                    }
                                    else if(Session["update"].ToString() == "true")
                                    {
                                        UpdateCommand.ExecuteNonQuery();
                                    }
                                    SqlConnection1.Close();

                                }
                                catch (Exception p)
                                {
                                    string s = p.ToString();
                                }

                                EditTable(tableName);
                                Response.Redirect("QuizSetup.aspx");

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