using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class PunchMgmt
    {
        public static List<Entity.Punch> GetPunchList(String LoginUserID)
        {
            return (new DAL.PunchSQL().GetPunchList(LoginUserID));
        }

        public static List<Entity.Punch> GetPunch(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PunchSQL().GetPunch(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Punch> GetPunch(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PunchSQL().GetPunch(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdatePunch(Entity.Punch entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PunchSQL().AddUpdatePunch(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeletePunch(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PunchSQL().DeletePunch(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
