using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class OrganizationEmployeeMgmt
    {
        public static List<Entity.OrganizationEmployee> GetEmployeeExpnLedgerList(Int64 pEmployeeID, Int64 pMonth, Int64 pYear, string pLoginUserID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeExpnLedgerList(pEmployeeID, pMonth, pYear, pLoginUserID));
        }
        public static List<Entity.OrganizationEmployee> GetOrgEmployeeByRegion(Int64 pStateCode, Int64 pCityCode, string pLoginUserID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrgEmployeeByRegion(pStateCode, pCityCode, pLoginUserID));
        }

        public static List<Entity.OrganizationEmployee> GetOrganizationEmployeeList()
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrganizationEmployeeList());
        }

        public static List<Entity.OrganizationEmployee> GetOrgEmployeeByOrgName(string OrgName)
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrgEmployeeByOrgName(OrgName));
        }

        public static List<Entity.OrganizationEmployee> OrgEmployeeByMobileNumber(string EmployeeName)
        {
            return (new DAL.OrganizationEmployeeSQL().OrgEmployeeByMobileNumber(EmployeeName));
        }

        public static List<Entity.OrganizationEmployee> GetEmployeeList(string pEmployeeName)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeList(pEmployeeName));
        }

        public static List<Entity.OrganizationEmployee> GetOrganizationEmployeeList(long pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrganizationEmployeeList(pkID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OrganizationEmployee> GetOrganizationEmployeeList(string OrgCode, string pLoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrganizationEmployeeList(OrgCode, pLoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OrganizationEmployee> GetOrganizationEmployeeList(string OrgCode, string pLoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrganizationEmployeeList(OrgCode, pLoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OrganizationEmployee> GetEmployeeFollowerList(string pLoginUserID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeFollowerList(pLoginUserID));
        }

        public static List<Entity.OrganizationEmployee> GetEmployeeSupervisorList(string pLoginUserID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeSupervisorList(pLoginUserID));
        }

        public static List<Entity.OrganizationEmployee> GetEmployeeListByRole(string pLoginUserID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeListByRole(pLoginUserID));
        }

        public static List<Entity.OrganizationEmployee> GetEmployeeWorkPerfomance(string pLoginUserID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeWorkPerfomance(pLoginUserID));
        }

        public static void AddUpdateOrganizationEmployee(Entity.OrganizationEmployee entity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID)
        {
            new DAL.OrganizationEmployeeSQL().AddUpdateOrganizationEmployee(entity, out ReturnCode, out ReturnMsg, out ReturnpkID);
        }

        public static void DeleteOrganizationEmployee(long pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationEmployeeSQL().DeleteOrganizationEmployee(pkID, out ReturnCode, out ReturnMsg);
        }

        // >>>>>>>>>>> Employee Documents 
        public static List<Entity.Documents> GetEmployeeDocumentsList(Int64 pkID, Int64 EmployeeID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetEmployeeDocumentsList(pkID, EmployeeID));
        }
        public static void AddUpdateEmployeeDocuments(Int64 pEmployeeID, string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationEmployeeSQL().AddUpdateEmployeeDocuments(pEmployeeID, pFilename, pType, pLoginUserID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteEmployeeDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationEmployeeSQL().DeleteEmployeeDocuments(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteEmployeeDocumentsByEmployeeId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationEmployeeSQL().DeleteEmployeeDocumentsByEmployeeId(pkID, out ReturnCode, out ReturnMsg);
        }

        // >>>>>>>>>>>>> Employee Credentials Vault
        public static List<Entity.OrganizationEmployee> GetOrganizationEmployeeCredentials(Int64 pEmployeeID)
        {
            return (new DAL.OrganizationEmployeeSQL().GetOrganizationEmployeeCredentials(pEmployeeID));
        }

        public static void AddUpdateEmployeeCredentials(Entity.OrganizationEmployee entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationEmployeeSQL().AddUpdateEmployeeCredentials(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteEmployeeCredentials(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationEmployeeSQL().DeleteEmployeeCredentials(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
