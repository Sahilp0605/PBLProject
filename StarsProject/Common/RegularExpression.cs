using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarsProject
{
    public class RegularExpression
    {
        public static string alphanumericexpresion()
        {
            return @"^[a-zA-Z0-9]*$";
        }
        public static string AlphNumberWithSomeSpecialCharacterExpresion()
        {
            return @"^[a-zA-Z0-9\ \-\.]*$";
        }
        public static string AlphNumberWithSpecialCharacterExpresion()
        {
            return @"^((?:[A-Za-z0-9-\s'.,@:?!()$#/\\]+|&[^#])*&?)$";
        }
    }
}