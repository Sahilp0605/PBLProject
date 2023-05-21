using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class SalesTargetMgmt
    {
        public static List<Entity.SalesTarget> GetSalesTargetList(String LoginUserID)
        {
            return (new DAL.SalesTargetSQL().GetSalesTargetList(LoginUserID));
        }

        public static List<Entity.SalesTarget> GetSalesTarget(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesTargetSQL().GetSalesTarget(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SalesTarget> GetSalesTarget(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesTargetSQL().GetSalesTarget(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSalesTarget(Entity.SalesTarget entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesTargetSQL().AddUpdateSalesTarget(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesTarget(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SalesTargetSQL().DeleteSalesTarget(pkID, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.SalesTarget> GetSalesTargetListByTargetType(String LoginUserID, Int64 day, Int64 month, Int64 year, string targettype, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SalesTargetSQL().GetSalesTargetListByTargetType(LoginUserID,day,month,year,targettype,PageNo, PageSize,out TotalRecord));
        }
    }
}
