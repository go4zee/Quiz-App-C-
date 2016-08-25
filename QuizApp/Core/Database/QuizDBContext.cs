using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace QuizApp.Core.Database
{
    public static class QuizDBContext
    {

        public static string QuizConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["QuizConnectionString"].ConnectionString;
        }



    }
}