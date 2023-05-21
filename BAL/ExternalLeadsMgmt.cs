using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ExternalLeadsMgmt
    {
        public static List<Entity.ExternalLeads> GetExternalLeadList(Int64 pkId, string acid, string source, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExternalLeadsSQL().GetExternalLeadList(pkId, acid, source, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ExternalLeads> GetExternalLeadListByStatus(Int64 pkId, string acid, string cat, string source, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExternalLeadsSQL().GetExternalLeadListByStatus(pkId, acid, cat, source, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ExternalLeads> GetExternalLeadListByStatus(Int64 pkId, string acid, string cat, string source, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExternalLeadsSQL().GetExternalLeadListByStatus(pkId, acid, cat, source, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ExternalLeads> GetExternalLeadView(string status, string source, Int64 month, Int64 year, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExternalLeadsSQL().GetExternalLeadView(status, source, month, year, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ExternalLeads> GetExternalLeadList_RPT(Int64 pkId, int PageNo, int PageSize, out int TotalRecord, DateTime Todate, DateTime Fromdate)
        {
            return (new DAL.ExternalLeadsSQL().GetExternalLeadList_RPT(pkId, PageNo, PageSize, out TotalRecord, Todate, Fromdate));
        }
        public static void AddUpdateExternalLeads(Entity.ExternalLeads entity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnInquiryPkID, out Int64 ReturnFollowupPkID)
        {
            new DAL.ExternalLeadsSQL().AddUpdateExternalLeads(entity, out ReturnCode, out ReturnMsg, out  ReturnInquiryPkID, out ReturnFollowupPkID);
        }

        public static void AddUpdateExternalLeadsUPDOWN(Entity.ExternalLeads entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExternalLeadsSQL().AddUpdateExternalLeadsUPDOWN(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.City> GetCityCodeByName(string CityName, string StateCode, string LoginUserID)
        {
            return (new DAL.ExternalLeadsSQL().GetCityCodeByName(CityName, StateCode, LoginUserID));
        }
        public static void DeleteExternalLeads(String pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExternalLeadsSQL().DeleteExternalLeads(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateExternalLeadsRegion(Entity.ExternalLeadsRegion objEntity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExternalLeadsSQL().AddUpdateExternalLeadsRegion(objEntity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteExternalLedasRegion(Int64 EmployeeID, string CountryCode, Int64 StateCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExternalLeadsSQL().DeleteExternalLedasRegion(EmployeeID, CountryCode, StateCode, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.ExternalLeadsRegion> GetExternalLeadsRegionList(Int64 pkId, Int64 EmployeeID, Int64 CountryCode, Int64 StateCode, Int64 CityCode, string LoginUserID, out int TotalCount)
        {
            return (new DAL.ExternalLeadsSQL().GetExternalLeadsRegionList(pkId, EmployeeID, CountryCode, StateCode, CityCode, LoginUserID, out TotalCount));
        }
    }
}
