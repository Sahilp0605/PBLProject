using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class OverTimeMgmt
    {

        public static List<Entity.OverTime> GetOverTimeList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.OverTimeSQL().GetOverTimeList(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.OverTime> GetOverTimeList(Int64 pkID, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OverTimeSQL().GetOverTimeList(pkID, pLoginUserID, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OverTime> GetOverTimeList(Int64 pkID, string pLoginUserID, string SearchKey, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OverTimeSQL().GetOverTimeList(pkID, pLoginUserID, SearchKey, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.OverTime> GetOverTimeListByStatus(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OverTimeSQL().GetOverTimeListByStatus(pStatus, pLoginUserID, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        //public static List<Entity.OverTime> GetOverTimeListByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        //{
        //    return (new DAL.OverTimeSQL().GetOverTimeListByUser(pLoginUserID, pMonth, pYear));
        //}

        public static List<Entity.OverTime> GetOverTimeListByEmployeeID(Int64 pEmpID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.OverTimeSQL().GetOverTimeListByEmployeeID(pEmpID, pMonth, pYear));
        }

        public static void AddUpdateOverTime(Entity.OverTime entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OverTimeSQL().AddUpdateOverTime(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteOverTime(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OverTimeSQL().DeleteOverTime(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void UpdateOverTimeApproval(Entity.OverTime entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OverTimeSQL().UpdateOverTimeApproval(entity, out ReturnCode, out ReturnMsg);
        }

        public static Int64 GetOverTimeHours(Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        {
            return DAL.OverTimeSQL.GetOverTimeHours(pEmployeeID, pMonth, pYear);
        }

        public static Decimal GetOverTimeAllow(Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        {
            return DAL.OverTimeSQL.GetOverTimeAllow(pEmployeeID, pMonth, pYear);
        }
    }
}
