using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class OrgTypeMgmt
    {
        public static List<Entity.OrgTypes> GetOrgTypeList()
        {
            return (new DAL.OrgTypeSQL().GetOrgTypeList());
        }

        public static List<Entity.OrgTypes> GetOrgType(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrgTypeSQL().GetOrgType(pkID, PageNo, PageSize, out TotalRecord));
        }
        
        public static List<Entity.OrgTypes> GetOrgType(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OrgTypeSQL().GetOrgType(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateOrgType(Entity.OrgTypes entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrgTypeSQL().AddUpdateOrgType(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteOrgType(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OrgTypeSQL().DeleteOrgType(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
