using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarsProject
{
    public class ErrorMessages
    {
        public static string ShowRequiredFieldMsg(string field)
        {
            return "Please fill " + field;
        }
        public static string ShowRequiredDropdownMsg(string field)
        {
            return "Please select " + field;
        }
        public static string ShowValidMsg(string field)
        {
            return "Please fill valid " + field + " data";
        }
    }
}