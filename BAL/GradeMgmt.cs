using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
   public class GradeMgmt
    {
        public static List<Entity.Grade> GetGradeList(String LoginUserID)
        {
            return (new DAL.GradeSQL().GetGradeList(LoginUserID));
        }

        public static List<Entity.Grade> GetGrade(Int64 pkID, string LoginUserID, int PageNo, int PageGrade, out int TotalRecord)
        {
            return (new DAL.GradeSQL().GetGrade(pkID, LoginUserID, PageNo, PageGrade, out TotalRecord));
        }

        public static List<Entity.Grade> GetGrade(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.GradeSQL().GetGrade(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateGrade(Entity.Grade entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.GradeSQL().AddUpdateGrade(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteGrade(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.GradeSQL().DeleteGrade(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}

