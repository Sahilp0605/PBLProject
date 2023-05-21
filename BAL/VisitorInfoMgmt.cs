using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class VisitorInfoMgmt
    {
        public static List<Entity.VisitorInfo> GetVisitorInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VisitorInfoSQL().GetVisitorInfoList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.VisitorInfo> GetVisitorInfoList(Int64 pkID, string LoginUserID, string SearchKey, string pStatus, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VisitorInfoSQL().GetVisitorInfoList(pkID, LoginUserID, SearchKey, pStatus, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateVisitorInfo(Entity.VisitorInfo entity, out int ReturnCode, out string ReturnMsg, out string newInqNo, out Int64 ReturnVisitorId)
        {
            new DAL.VisitorInfoSQL().AddUpdateVisitorInfo(entity, out ReturnCode, out ReturnMsg, out newInqNo, out ReturnVisitorId);
        }

        public static void DeleteVisitorInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VisitorInfoSQL().DeleteVisitorInfo(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
