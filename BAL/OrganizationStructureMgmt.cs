using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class OrganizationStructureMgmt
    {
        public static List<Entity.OrganizationStructure> GetEmployeeLocation(Int64 pEmployeeID, DateTime pStartDate, DateTime pEndDate)
        {
            return (new DAL.OrganizationStructureSQL().GetEmployeeLocation(pEmployeeID, pStartDate, pEndDate));
        }

        public static List<Entity.OrganizationStructure> GetOrganizationStructureList()
        {
            return (new DAL.OrganizationStructureSQL().GetOrganizationStructureList());
        }

        public static List<Entity.OrganizationStructure> GetOrganizationStructureList(string OrganizationStructureCode, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationStructureSQL().GetOrganizationStructure(OrganizationStructureCode, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OrganizationStructure> GetOrganizationStructureList(string OrganizationStructureCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationStructureSQL().GetOrganizationStructure(OrganizationStructureCode, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OrganizationStructure> GetOrganizationStructureDropDownList(string ListMode, string pLoginUserID)
        {
            return (new DAL.OrganizationStructureSQL().GetOrganizationStructureDropDownList(ListMode, pLoginUserID));
        }

        public static void AddUpdateOrganizationStructure(Entity.OrganizationStructure entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationStructureSQL().AddUpdateOrganizationStructure(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteOrganizationStructure(string OrganizationStructureCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrganizationStructureSQL().DeleteOrganizationStructure(OrganizationStructureCode, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.OrganizationBank> GetOrganizationBankList(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationStructureSQL().GetOrganizationBankList(pkID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OrganizationBank> GetOrganizationBankListByCompID(Int64 CompanyID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrganizationStructureSQL().GetOrganizationBankListByCompID(CompanyID, PageNo, PageSize, out TotalRecord));
        }
    }
}
