using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class RolesMgmt
    {
        public static List<Entity.Roles> GetRoleList()
        {
            return (new DAL.RolesSQL().GetRoleList());
        }

        public static List<Entity.Roles> GetRole(string RoleID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.RolesSQL().GetRole(RoleID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Roles> GetRole(string RoleID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.RolesSQL().GetRole(RoleID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateRoleDetail(Entity.Roles entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().AddUpdateRoleDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteRoleDetail(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().DeleteRoleDetail(RoleID, out ReturnCode, out ReturnMsg);
        }

        //-------For Role Menu Rights---------//
        public static void AddUpdateRoleRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().AddUpdateRoleRights(RoleID, MenuId, pLoginUserId, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteRoleRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().DeleteRoleRights(RoleID, out ReturnCode, out ReturnMsg);
        }
        public static string GetRoleRights(string RoleID)
        {
            return (new DAL.RolesSQL().GetRoleRights(RoleID));
        }

        //-------For Role Report Rights---------//
        public static void AddUpdateRoleReportRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().AddUpdateRoleReportRights(RoleID, MenuId, pLoginUserId, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteRoleReportRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().DeleteRoleReportRights(RoleID, out ReturnCode, out ReturnMsg);
        }
        public static string GetRoleReportRights(string RoleID)
        {
            return (new DAL.RolesSQL().GetRoleReportRights(RoleID));
        }

        //-------For ICON Report Rights---------//
        public static void AddUpdateRoleIconRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().AddUpdateRoleIconRights(RoleID, MenuId, pLoginUserId, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteRoleIconRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().DeleteRoleIconRights(RoleID, out ReturnCode, out ReturnMsg);
        }
        public static string GetRoleIconRights(string RoleID)
        {
            return (new DAL.RolesSQL().GetRoleIconRights(RoleID));
        }
        //-------For Role General Master Rights---------//
        public static void AddUpdateGetRoleGeneralMasterRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().AddUpdateGetRoleGeneralMasterRights(RoleID, MenuId, pLoginUserId, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteRoleGeneralMasterRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.RolesSQL().DeleteRoleGeneralMasterRights(RoleID, out ReturnCode, out ReturnMsg);
        }
        public static string GetRoleGeneralMasterRights(string RoleID)
        {
            return (new DAL.RolesSQL().GetRoleGeneralMasterRights(RoleID));
        }
    }
}
