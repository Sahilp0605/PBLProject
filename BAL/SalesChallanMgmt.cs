using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace BAL
{
    public class SalesChallanMgmt
    {
        public static List<Entity.SalesTaxDetail> GetTaxDetailHSNWiseForSalesChallan(String pModule, String pInquiryNo)
        {
            return (new DAL.SalesChallanSQL().GetTaxDetailHSNWiseForSalesChallan(pModule, pInquiryNo));
        }
        public static List<Entity.SalesChallan> GetSalesChallanList(String LoginUserID)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanList(LoginUserID));
        }

        public static List<Entity.SalesChallan> GetSalesChallanList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanList(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.SalesChallan> GetSalesChallanList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesChallan> GetSalesChallanList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesChallan(Entity.SalesChallan entity, out int ReturnCode, out string ReturnMsg, out string ReturnChallanNo)
        {
            new DAL.SalesChallanSQL().AddUpdateSalesChallan(entity, out ReturnCode, out ReturnMsg, out ReturnChallanNo);
        }

        public static void DeleteSalesChallan(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesChallanSQL().DeleteSalesChallan(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesChallanDetailByChallanNo(string pChallanNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesChallanSQL().DeleteSalesChallanDetailByChallanNo(pChallanNo, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateSalesChallanDetail(Entity.SalesChallanDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesChallanSQL().AddUpdateSalesChallanDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetSalesChallanDetail(string pChallanNo)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanDetail(pChallanNo));
        }
        public static DataTable GetSalesChallanDetailWithDisc(string pChallanNo)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanDetailWithDisc(pChallanNo));
        }

        public static List<Entity.InquiryInfo> GetInquiryInfoListByHospital(Int64 pCustomerID)
        {
            return (new DAL.SalesChallanSQL().GetInquiryInfoListByHospital(pCustomerID));
        }

        public static List<Entity.InquiryInfo> GetInquiryDetailForSalesChallan(String pInquiryNo, Int64 pHospitalID)
        {
            return (new DAL.SalesChallanSQL().GetInquiryDetailForSalesChallan(pInquiryNo, pHospitalID));
        }

        // -----------------------------------------------------------------------------
        // Export Detail
        // -----------------------------------------------------------------------------
        public static List<Entity.SalesChallan> GetSalesChallanExportList(Int64 pkID, string ChallanNo, string LoginUserID)
        {
            return (new DAL.SalesChallanSQL().GetSalesChallanExportList(pkID, ChallanNo, LoginUserID));
        }
        public static void AddUpdateSalesChallanExport(Entity.SalesChallan entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesChallanSQL().AddUpdateSalesChallanExport(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteSalesChallanExport(string pChallanNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesChallanSQL().DeleteSalesChallanExport(pChallanNo, out ReturnCode, out ReturnMsg);
        }

    }
}
