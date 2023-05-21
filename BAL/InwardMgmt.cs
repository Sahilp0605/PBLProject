using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
   public class InwardMgmt
    {
       public static List<Entity.Inward> GetInwardList(String LoginUserID)
        {
            return (new DAL.InwardSQL().GetInwardList(LoginUserID));
        }

       public static List<Entity.Inward> GetInwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
       {
            return (new DAL.InwardSQL().GetInwardList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
       }

       public static List<Entity.Inward> GetInwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
       {
           return (new DAL.InwardSQL().GetInwardList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
       }

       public static List<Entity.Inward> GetInwardList(string pLoginUserID, Int64 pMonth, Int64 pYear)
       {
           return (new DAL.InwardSQL().GetInwardList(pLoginUserID, pMonth, pYear));
       }
        public static List<Entity.Inward> GetInwardListPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.InwardSQL().GetInwardList(pLoginUserID, 0, 0, FromDate, ToDate));
        }
        //public static List<Entity.Inward> GetInwardListByStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        //{
        //    return (new DAL.InwardSQL().GetInwardListByStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        //}
        public static List<Entity.Inward> GetInwardListByCustomer(Int64 CustomerID, Int64 ProductID, string LoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.InwardSQL().GetInwardListByCustomer(CustomerID, ProductID, LoginUserID, pMonth, pYear));
        }
        public static void AddUpdateInward(Entity.Inward entity, out int ReturnCode, out string ReturnMsg, out string ReturnInwardNo)
        {
            new DAL.InwardSQL().AddUpdateInward(entity, out ReturnCode, out ReturnMsg, out ReturnInwardNo);
        }

        public static void DeleteInward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InwardSQL().DeleteInward(pkID, out ReturnCode, out ReturnMsg);
        }

        //public static void UpdateInwardApproval(Entity.Inward entity, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.InwardSQL().UpdateInwardApproval(entity, out ReturnCode, out ReturnMsg);
        //}

        // -----------------------------------------------------------------------------
        public static DataTable GetInwardDetail(string pInwardNo)
        {
            return (new DAL.InwardSQL().GetInwardDetail(pInwardNo));
        }

        public static List<Entity.InwardDetail> GetInwardDetailList()
        {
            return (new DAL.InwardSQL().GetInwardDetailList());
        }

        public static List<Entity.InwardDetail> GetInwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InwardSQL().GetInwardDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateInwardDetail(Entity.InwardDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InwardSQL().AddUpdateInwardDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInwardDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InwardSQL().DeleteInwardDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInwardDetailByInwardNo(string pInwardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InwardSQL().DeleteInwardDetailByInwardNo(pInwardNo, out ReturnCode, out ReturnMsg);
        }
    }
}
