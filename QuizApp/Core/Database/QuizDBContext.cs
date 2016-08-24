using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace QuizApp.Core.Database
{
    public class QuizDBContext
    {


        public string QuizConnectionString = ConfigurationManager.ConnectionStrings["QuizConnectionString"].ConnectionString;


    }
}