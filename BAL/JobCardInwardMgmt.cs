using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
   public class JobCardInwardMgmt
    {
       public static List<Entity.JobCardInward> GetJobCardInwardList(String LoginUserID)
        {
            return (new DAL.JobCardInwardSQL().GetJobCardInwardList(LoginUserID));
        }

       public static List<Entity.JobCardInward> GetJobCardInwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
       {
            return (new DAL.JobCardInwardSQL().GetJobCardInwardList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
       }

       public static List<Entity.JobCardInward> GetJobCardInwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
       {
           return (new DAL.JobCardInwardSQL().GetJobCardInwardList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
       }

       public static List<Entity.JobCardInward> GetJobCardInwardList(string pLoginUserID, Int64 pMonth, Int64 pYear)
       {
           return (new DAL.JobCardInwardSQL().GetJobCardInwardList(pLoginUserID, pMonth, pYear));
       }

        //public static List<Entity.JobCardInward> GetJobCardInwardListByStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        //{
        //    return (new DAL.JobCardInwardSQL().GetJobCardInwardListByStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        //}

        public static void AddUpdateJobCardInward(Entity.JobCardInward entity, out int ReturnCode, out string ReturnMsg, out string ReturnInwardNo)
        {
            new DAL.JobCardInwardSQL().AddUpdateJobCardInward(entity, out ReturnCode, out ReturnMsg, out ReturnInwardNo);
        }

        public static void DeleteJobCardInward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardInwardSQL().DeleteJobCardInward(pkID, out ReturnCode, out ReturnMsg);
        }

        //public static void UpdateJobCardInwardApproval(Entity.Inward entity, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.JobCardInwardSQL().UpdateJobCardInwardApproval(entity, out ReturnCode, out ReturnMsg);
        //}

        // -----------------------------------------------------------------------------
        public static DataTable GetJobCardInwardDetail(string pInwardNo)
        {
            return (new DAL.JobCardInwardSQL().GetJobCardInwardDetail(pInwardNo));
        }

        public static List<Entity.JobCardInwardDetail> GetJobCardInwardDetailList()
        {
            return (new DAL.JobCardInwardSQL().GetJobCardInwardDetailList());
        }

        public static List<Entity.JobCardInwardDetail> GetJobCardInwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobCardInwardSQL().GetJobCardInwardDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateJobCardInwardDetail(Entity.JobCardInwardDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardInwardSQL().AddUpdateJobCardInwardDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJobCardInwardDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardInwardSQL().DeleteJobCardInwardDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJobCardInwardDetailByInwardNo(string pInwardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardInwardSQL().DeleteJobCardInwardDetailByInwardNo(pInwardNo, out ReturnCode, out ReturnMsg);
        }
    }
}
