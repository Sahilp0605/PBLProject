using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class QuotationMgmt
    {
        public static List<Entity.Quotation> GetQuotationList(string pLoginUserID)
        {
            return (new DAL.QuotationSQL().GetQuotationList(pLoginUserID));
        }
        
        public static List<Entity.Quotation> GetQuotationList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.QuotationSQL().GetQuotationList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Quotation> GetQuotationList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.QuotationSQL().GetQuotationList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Quotation> GetQuotationByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.QuotationSQL().GetQuotationByUser(pLoginUserID, pMonth, pYear));
        }
        public static List<Entity.Quotation> GetQuotationByUserPeriod(string pLoginUserID, string pFromDate, string pToDate)
        {
            return (new DAL.QuotationSQL().GetQuotationByUser(pLoginUserID, 0, 0, pFromDate, pToDate));
        }
        public static void AddUpdateQuotation(Entity.Quotation entity, out int ReturnCode, out string ReturnMsg, out string ReturnQuotationNo)
        {
            new DAL.QuotationSQL().AddUpdateQuotation(entity, out ReturnCode, out ReturnMsg, out ReturnQuotationNo);
        }

        public static void DeleteQuotation(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationSQL().DeleteQuotation(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.Quotation> GetQuotationListByCustomer(Int64 pCustomerID)
        {
            return (new DAL.QuotationSQL().GetQuotationListByCustomer(pCustomerID));
        }

        public static void AddUpdateQuotationRevision(long pkID, string LoginUserID, out int ReturnCode,out string ReturnMsg)
        {
            new DAL.QuotationSQL().AddUpdateQuotationRevision(pkID, LoginUserID, out ReturnCode, out ReturnMsg);
        }

        public static string SendQuotationEmail(string pTemplateID, string pLoginUserID, Int64 pkID, string pEmailAddress)
        {
            return (new DAL.QuotationSQL().SendQuotationEmail(pTemplateID, pLoginUserID, pkID, pEmailAddress));
        }

        //-----------------Quatation Log-------------------------------

        public static List<Entity.Quotation> GetQuatationLogList(String HeaderID)
        {
            return (new DAL.QuotationSQL().GetQuatationLogList(HeaderID));
        }
        public static void AddUpdateQuatationLog(Entity.Quotation entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationSQL().AddUpdateQuatationLog(entity, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateQuotationDocuments(Entity.Quotation objEntity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationSQL().AddUpdateQuotationDocuments(objEntity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteQuotationDocumentsByQuotationNo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationSQL().DeleteQuotationDocumentsByQuotationNo(pkID, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.Quotation> GetAssemblyBrand(Int64 FinishProductID)
        {
            return (new DAL.QuotationSQL().GetAssemblyBrand(FinishProductID));
        }
    }
}
