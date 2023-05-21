using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class JobCardMgmt
    {
        public static List<Entity.JobCard> GetJobCardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobCardSQL().GetJobCardList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateJobCard(Entity.JobCard entity, out int ReturnCode, out string ReturnMsg, out string ReturnJobCardNo)
        {
            new DAL.JobCardSQL().AddUpdateJobCard(entity, out ReturnCode, out ReturnMsg,out ReturnJobCardNo);
        }
        public static void DeleteJobCard(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardSQL().DeleteJobCard(pkID, out ReturnCode, out ReturnMsg);
        }

        //====================================================================

        public static List<Entity.JobCardDetail> GetJobCardDetailList(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobCardSQL().GetJobCardDetailList(pkID, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateJobCardDetail(Entity.JobCardDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardSQL().AddUpdateJobCardDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteJobCardDetailByJobCardNo(string JobCardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardSQL().DeleteJobCardDetailByJobCardNo(JobCardNo, out ReturnCode, out ReturnMsg);
        }
        public static DataTable GetJobCardDetail(string JobCardNo)
        {
            return (new DAL.JobCardSQL().GetJobCardDetail(JobCardNo));
        }
        public static List<Entity.JobCard> GetJobCardByCustomer(Int64 pCustomerID)
        {
            return (new DAL.JobCardSQL().GetJobCardByCustomer(pCustomerID));
        }
        public static DataTable GetJobCardProductForSalesBill(string pJobCardNo)
        {
            return (new DAL.JobCardSQL().GetJobCardProductForSalesBill(pJobCardNo));
        }
    }
}
