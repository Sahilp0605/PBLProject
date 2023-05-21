using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class PurchaseOrderMgmt
    {
        public static List<Entity.PurchaseOrder> GetPurchaseOrderList(String LoginUserID)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderList(LoginUserID));
        }

        public static List<Entity.PurchaseOrder> GetPurchaseOrderList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderList(pkID, LoginUserID, "", PageNo, PageSize, out TotalRecord));
        }
        
        public static List<Entity.PurchaseOrder> GetPurchaseOrderList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.PurchaseOrder> GetPurchaseOrderList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderList(pStatus, pLoginUserID, pMonth, pYear));
        }
        public static List<Entity.PurchaseOrder> GetPurchaseOrderListByCustomer(string pLoginUserID, Int64 pCustomerID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderListByCustomer(pLoginUserID, pCustomerID, 0, pStatus, pMonth, pYear));
        }
        public static List<Entity.PurchaseOrder> GetPurchaseOrderListByCust(Int64 pCustomerID)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderListByCust(pCustomerID));
        }
        public static List<Entity.PurchaseOrder> GetPurchaseOrderListByCustomer(string pLoginUserID, Int64 pCustomerID, Int64 pProductID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderListByCustomer(pLoginUserID, pCustomerID, pProductID, pStatus, pMonth, pYear));
        }

        public static List<Entity.PurchaseOrder> GetPurchaseOrderListbyCustomerForSales(string pLoginUserID, Int64 pCustomerID, string pStatus)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderListbyCustomerForSales(pLoginUserID, pCustomerID, pStatus));
        }

        public static List<Entity.PurchaseOrder> GetPurchaseOrderListByStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderListByStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.PurchaseOrder> GetPurchaseOrderListByBillStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderListByBillStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdatePurchaseOrder(Entity.PurchaseOrder entity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            new DAL.PurchaseOrderSQL().AddUpdatePurchaseOrder(entity, out ReturnCode, out ReturnMsg, out ReturnOrderNo);
        }

        public static void DeletePurchaseOrder(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().DeletePurchaseOrder(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void UpdatePurchaseOrderApproval(Entity.PurchaseOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().UpdatePurchaseOrderApproval(entity, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        public static DataTable GetPurchaseOrderDetail(string pOrderNo)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderDetail(pOrderNo));
        }

        public static DataTable GetPurchaseOrderDetailForSale(string pOrderNo)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderDetailForSale(pOrderNo));
        }

        public static DataTable GetPurchaseOrderDetailForInward(string pOrderNo)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderDetailForInward(pOrderNo));
        }

        public static List<Entity.PurchaseOrderDetail> GetPurchaseOrderDetailList()
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderDetailList());
        }

        public static List<Entity.PurchaseOrderDetail> GetPurchaseOrderDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseOrderSQL().GetPurchaseOrderDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdatePurchaseOrderDetail(Entity.PurchaseOrderDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().AddUpdatePurchaseOrderDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeletePurchaseOrderDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().DeletePurchaseOrderDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeletePurchaseOrderDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().DeletePurchaseOrderDetailByOrderNo(pOrderNo, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        public static DataTable GetPayScheduleList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            return (new DAL.PurchaseOrderSQL().GetPayScheduleList(pkID, OrderNo, LoginUserID));
        }
        public static void AddUpdatePurchaseOrderPaySchedule(Entity.PurchaseOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().AddUpdatePurchaseOrderPaySchedule(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeletePurchaseOrderPaySchedule(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().DeletePurchaseOrderPaySchedule(pOrderNo, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        public static DataTable GetCheckList(string OrderNo)
        {
            return (new DAL.PurchaseOrderSQL().GetCheckList(OrderNo));
        }
        public static void AddUpdatePurchaseOrderCheckList(Entity.PurchaseOrderCheckList entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().AddUpdatePurchaseOrderCheckList(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeletePurchaseOrderCheckList(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseOrderSQL().DeletePurchaseOrderCheckList(pOrderNo, out ReturnCode, out ReturnMsg);
        }
    }
}
