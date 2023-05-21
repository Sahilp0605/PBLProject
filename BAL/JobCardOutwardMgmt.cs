using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class JobCardOutwardMgmt
    {
        public static List<Entity.JobCardOutward> GetJobCardOutwardList(String LoginUserID)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardList(LoginUserID));
        }

        public static List<Entity.JobCardOutward> GetJobCardOutwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JobCardOutward> GetJobCardOutwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        //Created by Vikram Rajput
        public static List<Entity.JobCardOutward> GetJobCardOutwardList(Int64 CustomerID, Int64 ProductID, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardList(CustomerID, ProductID, pLoginUserID, pMonth, pYear));
        }

        public static void AddUpdateJobCardOutward(Entity.JobCardOutward entity, out int ReturnCode, out string ReturnMsg,out string ReturnOutwardNo)
        {
            new DAL.JobCardOutwardSQL().AddUpdateJobCardOutward(entity, out ReturnCode, out ReturnMsg, out ReturnOutwardNo);
        }

        public static void DeleteJobCardOutward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardOutwardSQL().DeleteJobCardOutward(pkID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetJobCardOutwardDetail(string pOutwardNo)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardDetail(pOutwardNo));
        }

        public static List<Entity.JobCardOutwardDetail> GetJobCardOutwardDetailList()
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardDetailList());
        }

        public static List<Entity.JobCardOutwardDetail> GetJobCardOutwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateJobCardOutwardDetail(Entity.JobCardOutwardDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardOutwardSQL().AddUpdateJobCardOutwardDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJobCardOutwardDetailByOutwardNo(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardOutwardSQL().DeleteJobCardOutwardDetailByOutwardNo(pOutwardNo, out ReturnCode, out ReturnMsg);
        }
        // -----------------------------------------------------
        public static List<Entity.JobCardOutwardDetailAssembly> GetJobCardOutwardDetailAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            return (new DAL.JobCardOutwardSQL().GetJobCardOutwardDetailAssemblyList(pOutwardNo, pProductID, pAssemblyID));
        }
        public static void AddUpdateJobCardOutwardDetailAssembly(Entity.JobCardOutwardDetailAssembly entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardOutwardSQL().AddUpdateJobCardOutwardDetailAssembly(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJobCardOutwardDetailAssemblyByOutwardNo(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobCardOutwardSQL().DeleteJobCardOutwardDetailAssemblyByOutwardNo(pOutwardNo, pProductID, pAssemblyID, out ReturnCode, out ReturnMsg);
        }
    }
}
