using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class PurchaseBillMgmt
    {
        public static List<Entity.PurchaseBill> GetPurchaseBillList(String LoginUserID)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillList(LoginUserID));
        }

        public static List<Entity.PurchaseBill> GetPurchaseBillList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillList(pLoginUserID, pMonth, pYear));
        }
        public static List<Entity.PurchaseBill> GetPurchaseBillListPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillList(pLoginUserID, 0, 0, FromDate, ToDate));
        }
        public static List<Entity.PurchaseBill> GetPurchaseBillList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.PurchaseBill> GetPurchaseBillList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdatePurchaseBill(Entity.PurchaseBill entity, out int ReturnCode, out string ReturnMsg, out string ReturnInvoiceNo)
        {
            new DAL.PurchaseBillSQL().AddUpdatePurchaseBill(entity, out ReturnCode, out ReturnMsg, out ReturnInvoiceNo);
        }

        public static void DeletePurchaseBill(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseBillSQL().DeletePurchaseBill(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeletePurchaseBillDetailByInvoiceNo(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseBillSQL().DeletePurchaseBillDetailByInvoiceNo(pInvoiceNo, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdatePurchaseBillDetail(Entity.PurchaseBillDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PurchaseBillSQL().AddUpdatePurchaseBillDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetPurchaseBillDetail(string pInvoiceNo)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillDetail(pInvoiceNo));
        }
        public static DataTable GetPurchaseBillDetailWithDisc(string pInvoiceNo)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillDetailWithDisc(pInvoiceNo));
        }
        public static DataTable GetPurchaseBillNoByCustomerID(Int64 CustomerID)
        {
            return (new DAL.PurchaseBillSQL().GetPurchaseBillNoByCustomerID(CustomerID));
        }
        public static DataTable GetPurchasePendingBillsByCustomerID(Int64 CustomerID)
        {
            return (new DAL.PurchaseBillSQL().GetPurchasePendingBillsByCustomerID(CustomerID));
        }
        public static Decimal GetPurchasePendingBillsAmount(String InvoiceNo)
        {
            return (new DAL.PurchaseBillSQL().GetPurchasePendingBillsAmount(InvoiceNo));
        }
    }
}
