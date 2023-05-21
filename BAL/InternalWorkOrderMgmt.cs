using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class InternalWorkOrderMgmt
    {
        public static List<Entity.InternalWorkOrder> GetInternalWorkOrderList(String LoginUserID)
        {
            return (new DAL.InternalWorkOrderSQL().GetInternalWorkOrderList(LoginUserID));
        }

        public static List<Entity.InternalWorkOrder> GetInternalWorkOrderList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InternalWorkOrderSQL().GetInternalWorkOrderList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.InternalWorkOrder> GetInternalWorkOrderList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InternalWorkOrderSQL().GetInternalWorkOrderList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateInternalWorkOrder(Entity.InternalWorkOrder entity, out int ReturnCode, out string ReturnMsg, out string ReturnWorkOrderNo)
        {
            new DAL.InternalWorkOrderSQL().AddUpdateInternalWorkOrder(entity, out ReturnCode, out ReturnMsg, out ReturnWorkOrderNo);
        }

        public static void DeleteInternalWorkOrder(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InternalWorkOrderSQL().DeleteInternalWorkOrder(pkID, out ReturnCode, out ReturnMsg);
        }
        //--------------------------------------------Internal Work Order Detail ----------------------------------------
        public static List<Entity.InternalWorkOrderDetail> GetIternalWorkOrderDetailList()
        {
            return (new DAL.InternalWorkOrderSQL().GetIternalWorkOrderDetailList());
        }

        public static List<Entity.InternalWorkOrderDetail> GetIternalWorkOrderDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InternalWorkOrderSQL().GetIternalWorkOrderDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.InternalWorkOrderDetail> GetIternalWorkOrderDetailListByWorkOrderNo(string WorkOrderNo, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InternalWorkOrderSQL().GetIternalWorkOrderDetailListByWorkOrderNo(WorkOrderNo, PageNo, PageSize, out TotalRecord));
        }


        public static void AddUpdateInternalWorkOrderDetail(Entity.InternalWorkOrderDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InternalWorkOrderSQL().AddUpdateInternalWorkOrderDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInternalWorkOrderDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InternalWorkOrderSQL().DeleteInternalWorkOrderDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInternalWorkOrderDetailByWorkOrderNo(string WorkOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InternalWorkOrderSQL().DeleteInternalWorkOrderDetailByWorkOrderNo(WorkOrderNo, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.InternalWorkOrderDetail> GetInternalWorkOrderProdSpecList(string WorkOrderNo,Int64 ProductID,String LoginUserID)
        {
            return (new DAL.InternalWorkOrderSQL().GetInternalWorkOrderProdSpecList(WorkOrderNo, ProductID, LoginUserID));
        }
        public static List<Entity.InternalWorkOrder> GetQuotationInquiry(String OrderNo, Int64 CustomerID)
        {
            return (new DAL.InternalWorkOrderSQL().GetQuotationInquiry(OrderNo, CustomerID));
        }

    }
}
