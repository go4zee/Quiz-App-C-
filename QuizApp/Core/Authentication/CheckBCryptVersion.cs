using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApp.Core.Authentication
{
    public class CheckBCryptVersion
    {
        private bool isVersionValid;
        public CheckBCryptVersion()
        {
            isVersionValid = false;
        }

        public bool CheckHash(String Hash)
        {
            Hash = Hash.Substring(0, 3);
            
            if(Hash == "$2a")
            {
                isVersionValid = true;
            }
            else
            {
                isVersionValid = false;
            }
            return isVersionValid;
        }
    }
}
