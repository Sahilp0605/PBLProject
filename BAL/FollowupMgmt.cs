using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class FollowupMgmt
    {
        public static List<Entity.Followup> GetFollowupList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetFollowupList(pkID, LoginUserID, "", PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Followup> GetFollowupList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetFollowupList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }


        public static List<Entity.Followup> GetFollowupByUser(string LoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.FollowupSQL().GetFollowupByUser(LoginUserID, pMonth, pYear));
        }

        public static List<Entity.Followup> GetFollowupByUserPeriod(string LoginUserID, string pFromDate, string pToDate)
        {
            return (new DAL.FollowupSQL().GetFollowupByUser(LoginUserID, 0, 0, pFromDate, pToDate));
        }

        public static void AddUpdateFollowup(Entity.Followup entity, out int ReturnCode, out string ReturnMsg,out Int64 ReturnFollowupPKID)
        {
            new DAL.FollowupSQL().AddUpdateFollowup(entity, out ReturnCode, out ReturnMsg, out ReturnFollowupPKID);
        }

        public static void DeleteFollowup(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FollowupSQL().DeleteFollowup(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.Followup> GetDashboardFollowupList(String FollowupStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetDashboardFollowupList(FollowupStatus, pMonth, pYear, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Followup> GetDashboardFollowupTimeline(Int64 pCustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetDashboardFollowupTimeline(pCustomerID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Inquiry Followup - External
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.Followup> GetFollowupExtList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetFollowupExtList(pkID, LoginUserID, "", PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Followup> GetFollowupExtList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetFollowupExtList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Followup> GetDashboardFollowupExtTimeline(Int64 ExtpkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FollowupSQL().GetDashboardFollowupExtTimeline(ExtpkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateFollowupExt(Entity.Followup entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FollowupSQL().AddUpdateFollowupExt(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteFollowupExt(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FollowupSQL().DeleteFollowupExt(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
