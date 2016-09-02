using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using QuizApp.Core.Database;
using QuizApp.Core.Authentication;

namespace QuizApp
{
    public partial class TakeQuiz : System.Web.UI.Page
    {
        
        string tableName;

        //holds the current row being answered by the quiz taker
        int currentRow;

        //holds all the question numbers / primary key of the question numbers for queries
        List<int> rowNumbers;

        public static Dictionary<int, List<int>> AnswerList = new Dictionary<int, List<int>>();

        //Holds all the answers the quiz taker gives in the app/

        DataTable dataTable;

        Guid UserID;
        Guid QuizID;


        /// <summary>
        /// Gets the UserID, TableName and QuizID from the URL and decodes it from MD5 hash.
        /// Sets the session variables for the operation of the Quiz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AnswerList.Clear();
                Session["CurrentRow"] = null;

                var userid = Request.QueryString["UserID"];
                var quizid = Request.QueryString["QuizID"];
                var tablename = Request.QueryString["QuizTableName"];

                if(userid != null && quizid != null && tablename != null)
                {
                    UserID = new Guid(Server.UrlDecode(MD5EncryptionHelper.Decrypt(userid.ToString())));
                    QuizID = new Guid(Server.UrlDecode(MD5EncryptionHelper.Decrypt(quizid.ToString())));
                    tableName = Server.UrlDecode( MD5EncryptionHelper.Decrypt(tablename.ToString()));

                    Session["QuizTakerUserID"] = UserID;
                    Session["QuizTakerQuizID"] = QuizID;
                    Session["QuizTakertableName"] = tableName;
                }
                else
                {
                    Response.Redirect("testpage.aspx");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tableName = Session["QuizTakertableName"].ToString();

            LoadTable(tableName);

            var CurrentRow = Session["CurrentRow"];


            if (CurrentRow != null)
            {
                RowController(Convert.ToInt32(Session["CurrentRow"].ToString()));
            }
        }

        protected void LoadTable(string TableName)
        {

            string Connection = QuizDBContext.QuizConnectionString();
            string QueryString = "SELECT * FROM " + TableName;
            rowNumbers = new List<int>();
            dataTable = new DataTable();

            dataTable = QueryExecute(QueryString, Connection, TableName);

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    rowNumbers.Add(Convert.ToInt32(dr["QuizTableQuestionNumber"].ToString()));
                    Session["IsTableLoaded"] = "true";
                }
            }
            var parameterCurrentRow = Session["CurrentRow"];

            if (parameterCurrentRow != null)
            {
                currentRow = Convert.ToInt32(parameterCurrentRow.ToString());
            }
            else
            {
                currentRow = rowNumbers[0];
                Session["CurrentRow"] = currentRow;

            }
            RowController(currentRow);

        }

        //Sets the value of the text and checkboxes from the 
        protected void RowController(int currentRowNumber)
        {
            string QueryString = string.Format("QuizTableQuestionNumber={0}", currentRowNumber.ToString());

            try
            {
                //set global row number holder to the current one
                Session["CurrentRow"] = currentRowNumber;
                currentRow = currentRowNumber;
                DataRow result = dataTable.AsEnumerable().Where(dr => dr.Field<int>("QuizTableQuestionNumber") == currentRowNumber).SingleOrDefault();
                txtQuestion.InnerText = result.Field<string>("QuizTableQuizQuestion");
                txtanswer1.Value = result.Field<string>("QuizTableQuizAnswer1");
                txtanswer2.Value = result.Field<string>("QuizTableQuizAnswer2");
                txtanswer3.Value = result.Field<string>("QuizTableQuizAnswer3");
                txtanswer4.Value = result.Field<string>("QuizTableQuizAnswer4");
                txtanswer5.Value = result.Field<string>("QuizTableQuizAnswer5");
                txtanswer6.Value = result.Field<string>("QuizTableQuizAnswer6");
            }
            catch (Exception Error)
            {
                string errorString = Error.ToString();
            }

            Session["update"] = "true";
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int index = rowNumbers.IndexOf(currentRow);
            if (index + 1 < rowNumbers.Count)
            {
                SetDictionaryAnswerFromCheckbox(currentRow);
                RowController(rowNumbers[index + 1]);
                SetCheckboxFromDictionaryValues(currentRow);
            }

        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            int index = rowNumbers.IndexOf(currentRow);
            if (index - 1 >= 0)
            {
                SetDictionaryAnswerFromCheckbox(currentRow);
                RowController(rowNumbers[index - 1]);
                SetCheckboxFromDictionaryValues(currentRow);
            }
        }

        protected void SetDictionaryAnswerFromCheckbox(int currentRow)
        {
            List<int> answerArray = new List<int>();
            if (checkbox1.Checked == true)
            {
                if (txtanswer1.Value != "")
                {
                    answerArray.Add(1);
                }
            }

            if (checkbox2.Checked == true)
            {
                if (txtanswer2.Value != "")
                {
                    answerArray.Add(2);
                }
            }
            if (checkbox3.Checked == true)
            {
                if (txtanswer3.Value != "")
                {
                    answerArray.Add(3);
                }
            }
            if (checkbox4.Checked == true)
            {
                if (txtanswer4.Value != "")
                {
                    answerArray.Add(4);
                }
            }
            if (checkbox5.Checked == true)
            {
                if (txtanswer5.Value != "")
                {
                    answerArray.Add(5);
                }
            }
            if (checkbox6.Checked == true)
            {
                if (txtanswer6.Value != "")
                {
                    answerArray.Add(6);
                }
            }
            try
            {
                if (answerArray.Count > 0)
                {
                    AnswerList[currentRow] = answerArray;
                }
                else
                {
                    AnswerList[currentRow] = null;
                }

            }
            catch (Exception e)
            {
                string errorString = e.ToString();
            }

            //SetMessage((AnswerList[currentRow].ToArray()).ToString(), "alert alert-danger");
        }

        private void ResetCheckBoxes()
        {
            checkbox1.Checked = false;
            checkbox2.Checked = false;
            checkbox3.Checked = false;
            checkbox4.Checked = false;
            checkbox5.Checked = false;
            checkbox6.Checked = false;
        }

        protected void SetCheckboxFromDictionaryValues(int currentRow)
        {
            if (AnswerList.ContainsKey(currentRow))
            {
                List<int> currentAnswer = AnswerList[currentRow];

                if (currentAnswer != null)
                {
                    ResetCheckBoxes();

                    foreach (int value in currentAnswer)
                    {
                        switch (value)
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
                else
                {
                    ResetCheckBoxes();
                }
            }
            else
            {
                ResetCheckBoxes();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SetDictionaryAnswerFromCheckbox(currentRow);

            bool AllAnswered = true;
            string ErrorMessage = "";

            foreach (int value in rowNumbers)
            {
                if (AnswerList.ContainsKey(value))
                {
                    if (AnswerList[value] == null)
                    {
                        AllAnswered = false;
                        ErrorMessage = "Please answer all questions";
                    }
                }
                else
                {
                    AllAnswered = false;
                }
            }

            //if all the answers are answered, then the database is queried for each question's answer and compared with
            //the users answers stored in the local dictionary.
            if (AllAnswered)
            {
                int marks = 0;
                SetMessage("Congrats", "alert alert-success");
                foreach (int row in rowNumbers)
                {
                    List<int> UserAnswers = new List<int>();
                    List<int> DBAnswers = new List<int>();
                    using (SqlConnection SqlConnection1 = new SqlConnection(QuizDBContext.QuizConnectionString()))
                    {
                        try
                        {

                            DataTable dataTable = new DataTable();
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            string QueryString = string.Format("SELECT * FROM {0} WHERE QuizTableQuestionNumber = @qnumber", tableName);
                            SqlCommand Command = new SqlCommand(QueryString, SqlConnection1);
                            Command.Parameters.AddWithValue("@qnumber", row);
                            SqlConnection1.Open();
                            adapter.SelectCommand = Command;
                            adapter.Fill(dataTable);
                            SqlConnection1.Close();
                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow[] dr = dataTable.Select("QuizTableQuestionNumber = " + row);
                                DBAnswers = GetIntFromString(dr[0]["QuizTableAnswer"].ToString());

                                List<int> c;
                                if(AnswerList.TryGetValue(row, out c))
                                {
                                    UserAnswers = AnswerList[row];
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            string errorString = error.ToString();
                        }
                    }

                    if(UserAnswers.Count > 0)
                    {
                        if(CompareAnswers(UserAnswers, DBAnswers))
                        {
                            marks++;
                        }
                        
                    }
                }
                int MarksPercentage = (marks / rowNumbers.Count) * 100;

                SubmitDB(MarksPercentage);

            }
            else
            {
                SetMessage(ErrorMessage, "alert alert-danger");
            }
        }


        protected void SubmitDB(int marks)
        {
            UserID = new Guid(Session["QuizTakerUserID"].ToString());
            QuizID = new Guid(Session["QuizTakerQuizID"].ToString());

            using (SqlConnection SqlConnection1 = new SqlConnection(QuizDBContext.QuizConnectionString()))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    string QueryString = "INSERT INTO QuizScores";
                    QueryString += " (QuizID, QuizName, QuizTakenOn, QuizTakenBy, QuizScore) ";
                    QueryString += "VALUES (@QuizID, @QuizName, @QuizTakenOn, @QuizTakenBy, @QuizScore)";
                    SqlCommand InsertCommand = new SqlCommand(QueryString,  SqlConnection1);
                    InsertCommand.Parameters.AddWithValue("QuizID", QuizID);
                    InsertCommand.Parameters.AddWithValue("QuizName", tableName);
                    InsertCommand.Parameters.AddWithValue("QuizTakenOn", DateTime.Now);
                    InsertCommand.Parameters.AddWithValue("QuizTakenBy", UserID);
                    InsertCommand.Parameters.AddWithValue("QuizScore", marks);

                    SqlConnection1.Open();
                    adapter.InsertCommand = InsertCommand;
                    InsertCommand.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    string error = e.ToString();
                }
            }
        }

        /// <summary>
        /// converts the string "1,3,5" into a list of numbers List<int> 1 3 5</int> and returns the list
        /// </summary>
        /// <param name="answerString"></param>
        /// <returns></returns>
        protected List<int> GetIntFromString(string answerString)
        {
            List<int> ReturnList = new List<int>();
            string[] seperators = { "," };
            string[] tempAnswerArray = answerString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            foreach(string s in tempAnswerArray)
            {
                ReturnList.Add(Convert.ToInt32(s));
            }
            return ReturnList;
        }


        public DataTable QueryExecute(string QueryString, string Connection, string tableName)
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
                catch (Exception Error)
                {
                    string errorString = Error.ToString();
                    retVal = null;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Compares two lists and returns if they are equal or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="UserAnswers"></param>
        /// <param name="DBAnswers"></param>
        /// <returns></returns>

        static bool CompareAnswers<T>(ICollection<T> UserAnswers, ICollection<T> DBAnswers)
        {
            if (UserAnswers.Count != DBAnswers.Count)
            {
                return false;
            }

            Dictionary<T, int> answers = new Dictionary<T, int>();

            foreach (T item in UserAnswers)
            {
                int c;
                if (answers.TryGetValue(item, out c))
                {
                    answers[item] = c + 1;
                }
                else
                {
                    answers.Add(item, 1);
                }
            }

            foreach (T item in DBAnswers)
            {
                int c;
                if (answers.TryGetValue(item, out c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        answers[item] = c - 1;
                    }
                }
                else
                {
                    return false;
                }
            }

            foreach (int v in answers.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }

            return true;
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