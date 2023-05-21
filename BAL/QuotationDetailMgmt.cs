using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class QuotationDetailMgmt
    {
        public static DataTable GetQuotationProductForSalesOrder(string pQuotationNo, string forOrderNo)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationProductForSalesOrder(pQuotationNo, forOrderNo));
        }        

        public static DataTable GetQuotationDetail(string QuotationNo)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationDetail(QuotationNo));
        }

        public static DataTable GetQuotationDetailCT(string QuotationNo)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationDetailCT(QuotationNo));
        }

        public static DataTable GetQuotationSubsidySummmary(string pQuotationNo)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationSubsidySummmary(pQuotationNo));
        }

        public static List<Entity.QuotationDetail> GetQuotationDetailList()
        {
            return (new DAL.QuotationDetailSQL().GetQuotationDetailList());
        }

        public static List<Entity.QuotationDetail> GetQuotationDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.QuotationDetail> GetQuotationDetailListByQuotationNo(string pQuotationNo, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationDetailListByQuotationNo(pQuotationNo, PageNo, PageSize, out TotalRecord));
        }


        public static void AddUpdateQuotationDetail(Entity.QuotationDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().AddUpdateQuotationDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteQuotationDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteQuotationDetailByQuotationNo(string pQuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationDetailByQuotationNo(pQuotationNo, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteQuotationSpecByProduct(string pQuotationNo, Int64 pFinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationSpecByProduct(pQuotationNo, pFinishProductID, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateQuotationSubsidy(Entity.QuotationSubsidy entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().AddUpdateQuotationSubsidy(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteQuotationSubsidy(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationSubsidy(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteQuotationSubsidyByQuotationNo(string pQuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationSubsidyByQuotationNo(pQuotationNo, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.QuotationSubsidy> GetQuotationSubsidyListByQuotationNo(string pQuotationNo, out int TotalRecord)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationSubsidyListByQuotationNo(pQuotationNo, out TotalRecord));
        }

        public static List<Entity.ProductPartDetail> GetQuotationProductPartList(String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationProductPartList(QuotationNo, FinishProductID, LoginUserID));
        }

        public static void AddUpdateQuotationProductParts(Entity.ProductPartDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().AddUpdateQuotationProductParts(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteQuotationProductParts(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationProductParts(OrderNo, FinishProductID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteQuotationProductPartByQuotationNo(String QuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationProductPartByQuotationNo(QuotationNo, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteUnwantedQuotationProductParts(String OrderNo)
        {
            new DAL.QuotationDetailSQL().DeleteUnwantedQuotationProductParts(OrderNo);
        }

        public static void DeleteQuotationAssemblyByQuotationNo(string QuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().DeleteQuotationAssemblyByQuotationNo(QuotationNo, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateQuotationDetailAssembly(Entity.QuotationDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.QuotationDetailSQL().AddUpdateQuotationAssembly(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.QuotationDetail> GetQuotationAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            return (new DAL.QuotationDetailSQL().GetQuotationAssemblyList(pOutwardNo, pProductID, pAssemblyID));
        }
        public static void DeleteUnwantedQuotationAssembly(String OrderNo)
        {
            new DAL.QuotationDetailSQL().DeleteUnwantedQuotationAssembly(OrderNo);
        }

    }
}
