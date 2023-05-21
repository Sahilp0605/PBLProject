using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class InquiryInfoMgmt
    {
        public static DataTable GetInquiryProductDetail(string pInquiryNo)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryProductDetail(pInquiryNo));
        }

        public static DataTable GetAssemblyProductForQuotation(Int64 ProductID)
        {
            return (new DAL.InquiryInfoSQL().GetAssemblyProductForQuotation(ProductID));
        }

        public static DataTable GetAssemblyProductForProduction(Int64 ProductID)
        {
            return (new DAL.InquiryInfoSQL().GetAssemblyProductForProduction(ProductID));
        }


        public static DataTable GetInquiryProductForQuotation(string pInquiryNo, string forQuotationNo)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryProductForQuotation(pInquiryNo, forQuotationNo));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoList(string pLoginUserID)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryInfoList("", pLoginUserID, 0, 0));
        }
        public static List<Entity.InquiryInfo> GetInquiryInfoByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryInfoList("", pLoginUserID, 0, 0, FromDate, ToDate));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryInfoList(pStatus, pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.InquiryInfo> GetInquiryStatusList(string pLoginUserID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryStatusList(pLoginUserID, pStatus, pMonth, pYear));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryInfoList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoList(Int64 pkID, string LoginUserID, string SearchKey, string pStatus, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryInfoList(pkID, LoginUserID, SearchKey, pStatus, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoListByCustomer(Int64 pCustomerID)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryInfoListByCustomer(pCustomerID));
        }

        public static void AddUpdateInquiryInfo(Entity.InquiryInfo entity, out int ReturnCode, out string ReturnMsg, out string newInqNo,out Int64 ReturnFollowupNo)
        {
            new DAL.InquiryInfoSQL().AddUpdateInquiryInfo(entity, out ReturnCode, out ReturnMsg, out newInqNo, out ReturnFollowupNo);
        }

        public static void DeleteInquiryInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoSQL().DeleteInquiryInfo(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.InquiryInfo> GetInquiryProductGroupList(string pInquiryNo)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryProductGroupList(pInquiryNo));
        }

        public static void AddUpdateInquiryProduct(Entity.InquiryInfo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoSQL().AddUpdateInquiryProduct(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInquiryProductByInquiryNo(string pInquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoSQL().DeleteInquiryProductByInquiryNo(pInquiryNo, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInquiryProduct(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoSQL().DeleteInquiryProduct(pkID, out ReturnCode, out ReturnMsg);
        }

        //=======================Inquiry Owner==================================//
        public static void AddUpdateInquiryOwner(Entity.InquiryInfo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoSQL().AddUpdateInquiryOwner(entity, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.InquiryInfo> GetInquiryOwnerListByInquiryNo(string InquiryNo)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryOwnerListByInquiryNo(InquiryNo));
        }
        public static void DeleteInquiryOwnerByInquiryNo(string pInquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoSQL().DeleteInquiryOwnerByInquiryNo(pInquiryNo, out ReturnCode, out ReturnMsg);
        }
        //=====================================================================//

        public static List<Entity.InquiryInfo> GetInquiryListByStatus(string pInquiryStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.InquiryInfoSQL().GetInquiryListByStatus(pInquiryStatus, pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.InquiryInfo> GetDashboardAllLeads(string pInquiryStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, Int64 pFromDays, Int64 pToDays)
        {
            return (new DAL.InquiryInfoSQL().GetDashboardAllLeads(pInquiryStatus, pLoginUserID, pMonth, pYear, pFromDays, pToDays));
        }

        public static List<Entity.CRMSummary> GetCrmAnalysisReport(string pType, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.InquiryInfoSQL().GetCrmAnalysisReport(pType, pMonth, pYear));
        }
    }
}
