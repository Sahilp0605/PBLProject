using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class SiteSurveyReportMgmt
    {
        public static List<Entity.SiteSurveyReport> GetSiteSurveyReport(String LoginUserID)
        {
            return (new DAL.SiteSurveyReportSQL().GetSiteSurveyReport(LoginUserID));
        }

        public static List<Entity.SiteSurveyReport> GetSiteSurveyReport(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SiteSurveyReportSQL().GetSiteSurveyReport(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SiteSurveyReport> GetSiteSurveyReport(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SiteSurveyReportSQL().GetSiteSurveyReport(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateGetSiteSurveyReport(Entity.SiteSurveyReport entity, out int ReturnCode, out string ReturnMsg, out String ReturnSiteSurvayNo)
        {
            new DAL.SiteSurveyReportSQL().AddUpdateSiteSurveyReport(entity, out ReturnCode, out ReturnMsg, out ReturnSiteSurvayNo);
        }

        public static void DeleteGetSiteSurveyReport(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSiteSurveyReport(pkID, out ReturnCode, out ReturnMsg);
        }
        //----------------------------------Site Survey Report Roof Details-----------------

        public static void AddUpdateSSRRoofingDetails(Entity.SSRRoofDetails entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().AddUpdateSSRRoofingDetails(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSRRoofDetails(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSRRoofDetails(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSRRoofDetailsBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSRRoofDetailsBySurveyID(SurveyID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetSSRRoofDetails(String SurveyID)
        {
            return (new DAL.SiteSurveyReportSQL().GetSSRRoofDetails(SurveyID));
        }

        //----------------------------------Site Survey Report Equipment Location Details-----------------

        public static void AddUpdateSSREquipmentLocation(Entity.SSREquipmentLocation entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().AddUpdateSSREquipmentLocation(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSREquipmentLocation(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSREquipmentLocation(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSREquipmentLocationBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSREquipmentLocationBySurveyID(SurveyID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetSSREquipmentLocation(String SurveyID)
        {
            return (new DAL.SiteSurveyReportSQL().GetSSREquipmentLocation(SurveyID));

        }

        //----------------------------------Site Survey Report System Availablity-----------------

        public static void AddUpdateSSRSysAvailablity(Entity.SSRSysAvailablity entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().AddUpdateSSRSysAvailablity(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSRSysAvailablity(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSRSysAvailablity(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSRSysAvailablityBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSRSysAvailablityBySurveyID(SurveyID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetSSRSysAvailablity(String SurveyID)
        {
            return (new DAL.SiteSurveyReportSQL().GetSSRSysAvailablity(SurveyID));
        }

        //----------------------------------Site Survey Report Required Engineering Details-----------------

        public static void AddUpdateSSRRequiredDetails(Entity.SSRRequiredDetails entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().AddUpdateSSRRequiredDetails(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSRRequiredDetails(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSRRequiredDetails(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSSRRequiredDetailsBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurveyReportSQL().DeleteSSRRequiredDetailsBySurveyID(SurveyID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetSSRRequiredDetails(String SurveyID)
        {
            return (new DAL.SiteSurveyReportSQL().GetSSRRequiredDetails(SurveyID));
        }
    }
}
