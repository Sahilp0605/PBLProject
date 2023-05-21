using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class SalesOrderMgmt
    {
        public static List<Entity.SalesBill> GetOutstandingBills(String pCategory, string pStatus, string ByDateType, DateTime AsOnDate, string LM1, string LM2, string LM3, string LM4, string LM5, string LM6)
        {
            return (new DAL.SalesOrderSQL().GetOutstandingBills(pCategory, pStatus, ByDateType, AsOnDate, LM1, LM2, LM3, LM4, LM5, LM6));
        }
        public static List<Entity.SalesOrder> GetSalesOrderList(String LoginUserID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderList(LoginUserID));
        }

        public static List<Entity.SalesOrder> GetSalesOrderListForProduction(Int64 CustomerID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListForProduction(CustomerID));
        }

        public static List<Entity.SalesOrder> GetSalesOrderList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderList(pkID, LoginUserID,"", "", PageNo, PageSize, out TotalRecord));
        }
        
        public static List<Entity.SalesOrder> GetSalesOrderList(Int64 pkID,string SerialKey, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderList(pkID, SerialKey, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesOrder> GetSalesOrderList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderList(pStatus, pLoginUserID, pMonth, pYear));
        }
        public static List<Entity.SalesOrder> GetSalesOrderListPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderList("", pLoginUserID, 0, 0, FromDate, ToDate));
        }

        public static List<Entity.SalesOrder> GetSalesOrderListBYProjectStatus(string pLoginUserID,string ProjectStage, string pStatus)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListBYProjectStatus(pLoginUserID,ProjectStage,pStatus));
        }

        public static List<Entity.SalesOrder> GetSalesOrderListByCustomer(string pLoginUserID, Int64 pCustomerID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByCustomer(pLoginUserID, pCustomerID, pStatus, pMonth, pYear));
        }
        public static List<Entity.SalesOrder> GetSalesOrderListByCustomer(string pLoginUserID, Int64 pCustomerID, string pStatus, Int64 pMonth, Int64 pYear, bool ForSalesBill)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByCustomer(pLoginUserID, pCustomerID, pStatus, pMonth, pYear, ForSalesBill));
        }
        public static List<Entity.SalesOrder> GetSalesOrderListByCustomer(Int64 pCustomerID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByCustomer(pCustomerID));
        }
        public static List<Entity.SalesOrder> GetSalesOrderListByCustomerForSaleBill(string pLoginUserID, Int64 pCustomerID, string pStatus, Int64 pMonth, Int64 pYear, String InvoiceNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByCustomerForSaleBill(pLoginUserID, pCustomerID, pStatus, pMonth, pYear, InvoiceNo));
        }
        public static List<Entity.SalesOrder> GetSalesOrderListByCustomerProduct(Int64 pCustomerID, Int64 pProductID, string pStatus, string pLoginUserID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByCustomerProduct(pCustomerID, pProductID, pStatus, pLoginUserID));
        }

        public static List<Entity.SalesOrder> GetSalesOrderListbyCustomerForSales(string pLoginUserID, Int64 pCustomerID, string pStatus)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListbyCustomerForSales(pLoginUserID, pCustomerID, pStatus));
        }

        public static List<Entity.SalesOrder> GetSalesOrderListByStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesOrder> GetSalesOrderListByBillStatus(String pApprovalStatus, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderListByBillStatus(pApprovalStatus, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesOrder(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrder(entity, out ReturnCode, out ReturnMsg, out ReturnOrderNo);
        }

        public static void DeleteSalesOrder(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrder(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void UpdateSalesOrderApproval(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().UpdateSalesOrderApproval(entity, out ReturnCode, out ReturnMsg);
        }

        public static void UpdateSalesOrderDealerApproval(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().UpdateSalesOrderDealerApproval(entity, out ReturnCode, out ReturnMsg);
        }

        /* ---------------------------------------------------------------------------------------------------- */
        /* Sales Order LOG LIST     */
        /* ---------------------------------------------------------------------------------------------------- */
        public static List<Entity.SalesOrder> GetSalesOrderLogList(String HeaderID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderLogList(HeaderID));
        }
        public static void AddUpdateSalesOrderLog(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderLog(entity, out ReturnCode, out ReturnMsg);
        }
        // -----------------------------------------------------------------------------
        public static DataTable GetSalesOrderDetail(string pOrderNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDetail(pOrderNo));
        }
        public static DataTable GetSalesOrderDetailForProduction(string pOrderNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDetailForProduction(pOrderNo));
        }
        public static DataTable GetSalesOrderAssemblyForProduction(string pOrderNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderAssemblyForProduction(pOrderNo));
        }

        public static DataTable GetSalesOrderDetailForOut(string pOrderNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDetailForOut(pOrderNo));
        }

        public static DataTable GetSalesOrderDetailForSale(string pOrderNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDetailForSale(pOrderNo));
        }
        public static DataTable GetSalesOrderAssembly(string pOrderNo)

        {
            return (new DAL.SalesOrderSQL().GetSalesOrderAssembly(pOrderNo));
        }

        public static List<Entity.SalesOrderDetail> GetSalesOrderDetailList()
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDetailList());
        }

        public static List<Entity.SalesOrderDetail> GetSalesOrderDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesOrderDetail(Entity.SalesOrderDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrderDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrderDetailByOrderNo(pOrderNo, out ReturnCode, out ReturnMsg);
        }


        // -----------------------------------------------------------------------------
        public static DataTable GetPayScheduleList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            return (new DAL.SalesOrderSQL().GetPayScheduleList(pkID, OrderNo, LoginUserID));
        }
        public static void AddUpdateSalesOrderPaySchedule(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderPaySchedule(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesOrderPaySchedule(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrderPaySchedule(pOrderNo, out ReturnCode, out ReturnMsg);
        }
        
        
        // -----------------------------------------------------------------------------
        // Export Detail
        // -----------------------------------------------------------------------------
        public static List<Entity.SalesOrder> GetSalesOrderExportList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderExportList(pkID, OrderNo, LoginUserID));
        }
        public static void AddUpdateSalesOrderExport(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderExport(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesOrderExport(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrderExport(pOrderNo, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        // Shipping  Detail
        // -----------------------------------------------------------------------------
        public static List<Entity.SalesOrder> GetSalesOrder_ShippingDetailsList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrder_ShippingDetailsList(pkID, OrderNo, LoginUserID));
        }
        public static void AddUpdateSalesOrder_ShippingDetails(Entity.SalesOrder entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrder_ShippingDetails(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesOrder_ShippingDetails(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrder_ShippingDetails(pOrderNo, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        // Sales Order Documents
        // -----------------------------------------------------------------------------
        public static List<Entity.SalesorderDocuments> GetSalesOrderDocumentsList(Int64 pkID, Int64 pLogID, String pOrderNo)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderDocumentsList(pkID, pLogID, pOrderNo));
        }
        public static void AddUpdateSalesOrderDocuments(Entity.SalesorderDocuments objEntity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderDocuments(objEntity, out ReturnCode, out ReturnMsg);
        }

        //public static void DeleteEmployeeDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.OrganizationEmployeeSQL().DeleteEmployeeDocuments(pkID, out ReturnCode, out ReturnMsg);
        //}

        //public static void DeleteEmployeeDocumentsByEmployeeId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.OrganizationEmployeeSQL().DeleteEmployeeDocumentsByEmployeeId(pkID, out ReturnCode, out ReturnMsg);
        //}

        public static List<Entity.SalesOrderProduction> GetOrderProductionDetailList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            return (new DAL.SalesOrderSQL().GetOrderProductionDetailList(pkID, OrderNo, LoginUserID));
        }

        public static void AddUpdateSalesOrderProductionDetails(Entity.SalesOrderProduction entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderProductionDetails(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderProductionDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrderProductionDetailByOrderNo(pOrderNo, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesOrderAssemblyByOrderNo(string QuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().DeleteSalesOrderAssemblyByOrderNo(QuotationNo, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateSalesOrderAssembly(Entity.QuotationDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesOrderSQL().AddUpdateSalesOrderAssembly(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.QuotationDetail> GetSalesOrderAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            return (new DAL.SalesOrderSQL().GetSalesOrderAssemblyList(pOutwardNo, pProductID, pAssemblyID));
        }
        public static void DeleteUnwantedSalesOrderAssembly(String OrderNo)
        {
            new DAL.SalesOrderSQL().DeleteUnwantedSalesOrderAssembly(OrderNo);
        }


    }
}
