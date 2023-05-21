using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class DesignationMgmt
    {
        public static List<Entity.Designations> GetDesignationList()
        {
            return (new DAL.DesignationSQL().GetDesignationList());
        }

        public static List<Entity.Designations> GetDesignation(string DesignationCode, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.DesignationSQL().GetDesignation(DesignationCode, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Designations> GetDesignation(string DesignationCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.DesignationSQL().GetDesignation(DesignationCode, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateDesignation(Entity.Designations entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.DesignationSQL().AddUpdateDesignation(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteDesignation(string DesignationCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.DesignationSQL().DeleteDesignation(DesignationCode, out ReturnCode, out ReturnMsg);
        }
    }
}
