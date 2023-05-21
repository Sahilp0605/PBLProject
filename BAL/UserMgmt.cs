using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class UserMgmt
    {
        #region Authenticate User
        public static Entity.Authenticate AuthenticateUser(string UserID, string UserPwd)
        {
            return (new DAL.UserSQL().AuthenticateUser(UserID, UserPwd));
        }
        public static Entity.Authenticate AuthenticateUserDealer(string UserID, string UserPwd)
        {
            return (new DAL.UserSQL().AuthenticateUserDealer(UserID, UserPwd));
        }
        #endregion
        
        public static List<Entity.Users> GetLoginUserList(string UserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.UserSQL().GetLoginUserList(UserID, PageNo, PageSize, out TotalRecord));
        }

        //public static DataTable <Entity.Users> GetUserLoginList(string LoginUserID)
        //{
        //    return (new DAL.UserSQL().GetUserLoginList(LoginUserID));
        //}

        public static List<Entity.Users> GetLoginUserList(string UserID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.UserSQL().GetLoginUserList(UserID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Users> GetUserLogList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.UserSQL().GetUserLogList(pLoginUserID,pMonth,pYear));
        }
        public static List<Entity.Users> GetUserLogListPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.UserSQL().GetUserLogList(pLoginUserID, 0, 0, FromDate, ToDate));
        }

        public static void AddUpdateUserManagement(Entity.Users entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.UserSQL().AddUpdateUserManagement(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteLoginUser(string UserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.UserSQL().DeleteLoginUser(UserID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.UserLog> GetUserActivityList(string pLoginUserID, Int64 pDay, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.UserSQL().GetUserActivityList(pLoginUserID, pDay, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.UserLog> GetUserActivityListByUser(string pLoginUserID, Int64 pDay, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.UserSQL().GetUserActivityListByUser(pLoginUserID, pDay, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.UserLog> GetUserActivityListByUser(string pLoginUserID, DateTime FromDate, DateTime ToDate, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.UserSQL().GetUserActivityListByUser(pLoginUserID, FromDate, ToDate, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateUserManagementRegistration(Entity.Users entity, string serialkey,out int ReturnCode, out string ReturnMsg)
        {
            new DAL.UserSQL().AddUpdateUserManagementRegistration(entity, serialkey, out ReturnCode, out ReturnMsg);
        }

        public static void UpdateUserPassword(string pLoginUserID, string pPassword, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.UserSQL().UpdateUserPassword(pLoginUserID, pPassword, out ReturnCode, out ReturnMsg);
        }
    }
}
