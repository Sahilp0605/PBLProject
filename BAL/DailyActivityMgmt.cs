using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class DailyActivityMgmt
    {
        public static List<Entity.DailyActivity> GetDailyActivityList(Int64 pkID, string ActivityDate, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.DailyActivitySQL().GetDailyActivityList(pkID, ActivityDate, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.DailyActivity> GetDailyActivityList(Int64 pkID, Int64 EmployeeID, string ActivityDate, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.DailyActivitySQL().GetDailyActivityList(pkID, EmployeeID, ActivityDate, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.DailyActivity> GetDailyActivityListByUser(string LoginUserID, string pActivityDate, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.DailyActivitySQL().GetDailyActivityListByUser(LoginUserID, pActivityDate, pMonth, pYear));
        }
        public static List<Entity.DailyActivity> GetDailyActivityListByUserPeriod(string LoginUserID, string pActivityDate, string FromDate, string ToDate)
        {
            return (new DAL.DailyActivitySQL().GetDailyActivityListByUser(LoginUserID, pActivityDate, 0, 0, FromDate, ToDate));
        }
        public static void AddUpdateDailyActivity(Entity.DailyActivity entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.DailyActivitySQL().AddUpdateDailyActivity(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteDailyActivity(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.DailyActivitySQL().DeleteDailyActivity(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.TaskCategory> GetTaskCategoryList(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.DailyActivitySQL().GetTaskCategoryList(pkID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.TaskCategory> GetTaskCategoryList(Int64 pkID, string pCategory, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.DailyActivitySQL().GetTaskCategoryList(pkID, pCategory, PageNo, PageSize, out TotalRecord));
        }
    }
}
