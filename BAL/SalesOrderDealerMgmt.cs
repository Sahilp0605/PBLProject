using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class SalesOrderDealerMgmt
    {

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerList(String LoginUserID)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerList(LoginUserID));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerList(pkID, LoginUserID, "", PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerList(pStatus, pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerListByCustomer(string pLoginUserID, Int64 pCustomerID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerListByCustomer(pLoginUserID, pCustomerID, pStatus, pMonth, pYear));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerListbyCustomerForSales(string pLoginUserID, Int64 pCustomerID, string pStatus)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerListbyCustomerForSales(pLoginUserID, pCustomerID, pStatus));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerListByStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerListByStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesOrderDealer> GetSalesOrderDealerListByBillStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerListByBillStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesOrderDealer(Entity.SalesOrderDealer entity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            new DAL.SalesOrderDealerSQL().AddUpdateDealerSalesOrder(entity, out ReturnCode, out ReturnMsg, out ReturnOrderNo);
        }

        public static void DeleteSalesOrderDealer(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderDealerSQL().DeleteSalesOrderDealer(pkID, out ReturnCode, out ReturnMsg);
        }

        //public static void UpdateSalesOrderApproval(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.SalesOrderSQL().UpdateSalesOrderApproval(entity, out ReturnCode, out ReturnMsg);
        //}

        // -----------------------------------------------------------------------------
        public static DataTable GetSalesOrderDealerDetail(string pOrderNo)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerDetail(pOrderNo));
        }

        public static DataTable GetSalesOrderDealerDetailForSale(string pOrderNo)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerDetailForSale(pOrderNo));
        }

        public static List<Entity.SalesOrderDealerDetail> GetSalesOrderDealerDetailList()
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerDetailList());
        }

        public static List<Entity.SalesOrderDealerDetail> GetSalesOrderDealerDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderDealerSQL().GetSalesOrderDealerDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesOrderDealerDetail(Entity.SalesOrderDealerDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderDealerSQL().AddUpdateSalesOrderDealerDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderDealerDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderDealerSQL().DeleteSalesOrderDealerDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderDealerDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderDealerSQL().DeleteSalesOrderDealerDetailByOrderNo(pOrderNo, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        public static DataTable GetPayScheduleList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            return (new DAL.SalesOrderDealerSQL().GetPayScheduleList(pkID, OrderNo, LoginUserID));
        }
        public static void AddUpdateSalesOrderDealerPaySchedule(Entity.SalesOrderDealer entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderDealerSQL().AddUpdateSalesOrderDealerPaySchedule(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesOrderDealerPaySchedule(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderDealerSQL().DeleteSalesOrderDealerPaySchedule(pOrderNo, out ReturnCode, out ReturnMsg);
        }
    }
}
