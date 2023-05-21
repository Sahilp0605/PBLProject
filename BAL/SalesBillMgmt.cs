using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace BAL
{
    public class SalesBillMgmt
    {
        public static List<Entity.SalesTaxDetail> GetTaxDetailHSNWiseForSalesBillOC(String pModule, String pInquiryNo)
        {
            return (new DAL.SalesBillSQL().GetTaxDetailHSNWiseForSalesBillOC(pModule, pInquiryNo));
        }
        public static List<Entity.SalesTaxDetail> GetTaxDetailHSNWiseForSalesBill(String pModule, String pInquiryNo)
        {
            return (new DAL.SalesBillSQL().GetTaxDetailHSNWiseForSalesBill(pModule, pInquiryNo));
        }
        public static List<Entity.SalesBill> GetSalesBillList(String LoginUserID)
        {
            return (new DAL.SalesBillSQL().GetSalesBillList(LoginUserID));
        }
        public static List<Entity.SalesBill> GetSalesBillListPeriod(String LoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.SalesBillSQL().GetSalesBillList(LoginUserID, 0, 0, FromDate, ToDate));
        }
        public static List<Entity.SalesBill> GetSalesBillList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.SalesBillSQL().GetSalesBillList(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.SalesBill> GetSalesBillList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesBillSQL().GetSalesBillList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesBill> GetSalesBillList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesBillSQL().GetSalesBillList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesBill(Entity.SalesBill entity, out int ReturnCode, out string ReturnMsg, out string ReturnInvoiceNo)
        {
            new DAL.SalesBillSQL().AddUpdateSalesBill(entity, out ReturnCode, out ReturnMsg, out ReturnInvoiceNo);
        }

        public static void DeleteSalesBill(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().DeleteSalesBill(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesBillDetailByInvoiceNo(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().DeleteSalesBillDetailByInvoiceNo(pInvoiceNo, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateSalesBillDetail(Entity.SalesBillDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().AddUpdateSalesBillDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetSalesBillDetail(string pInvoiceNo)
        {
            return (new DAL.SalesBillSQL().GetSalesBillDetail(pInvoiceNo));
        }
        public static DataTable GetSalesBillDetailWithDisc(string pInvoiceNo)
        {
            return (new DAL.SalesBillSQL().GetSalesBillDetailWithDisc(pInvoiceNo));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoListByHospital(Int64 pCustomerID)
        {
            return (new DAL.SalesBillSQL().GetInquiryInfoListByHospital(pCustomerID));
        }

        public static List<Entity.InquiryInfo> GetInquiryDetailForSalesBill(String pInquiryNo, Int64 pHospitalID)
        {
            return (new DAL.SalesBillSQL().GetInquiryDetailForSalesBill(pInquiryNo, pHospitalID));
        }

        // -----------------------------------------------------------------------------
        // Export Detail
        // -----------------------------------------------------------------------------
        public static List<Entity.SalesBill> GetSalesBillExportList(Int64 pkID, string InvoiceNo, string LoginUserID)
        {
            return (new DAL.SalesBillSQL().GetSalesBillExportList(pkID, InvoiceNo, LoginUserID));
        }
        public static void AddUpdateSalesBillExport(Entity.SalesBill entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().AddUpdateSalesBillExport(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesBillExport(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().DeleteSalesBillExport(pInvoiceNo, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.SalesBill> GetInvoiceListByCustomer(Int64 CustomerID)
        {
            return (new DAL.SalesBillSQL().GetInvoiceListByCustomer(CustomerID));
        }

        public static DataTable GetSalesBillNoByCustomerID(Int64 CustomerID)
        {
            return (new DAL.SalesBillSQL().GetSalesBillNoByCustomerID(CustomerID));
        }

        
        // -----------------------------------------------------------------------------
        // Job Work Detail
        // -----------------------------------------------------------------------------
        public static List<Entity.SalesBillJobWork> GetSalesBillJobWorkList(Int64 pkID, string InvoiceNo, Int64 FinishProductID,  string LoginUserID)
        {
            return (new DAL.SalesBillSQL().GetSalesBillJobWorkList(pkID, InvoiceNo, FinishProductID, LoginUserID));
        }
        public static void AddUpdateSalesBillJobWork(Entity.SalesBillJobWork entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().AddUpdateSalesBillJobWork(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesBillJobWorkByInvoiceNo(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesBillSQL().DeleteSalesBillJobWorkByInvoiceNo(pInvoiceNo, out ReturnCode, out ReturnMsg);
        }
        // -----------------------------------------------------------------------------
        public static DataTable GetSalesPendingBillsByCustomerID(Int64 CustomerID)
        {
            return (new DAL.SalesBillSQL().GetSalesPendingBillsByCustomerID(CustomerID));
        }
        public static Decimal GetSalesPendingBillsAmount(String InvoiceNo)
        {
            return (new DAL.SalesBillSQL().GetSalesPendingBillsAmount(InvoiceNo));
        }
    }
}
