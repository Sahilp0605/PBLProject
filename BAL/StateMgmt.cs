using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class StateMgmt
    {
        public static List<Entity.State> GetStateList()
        {
            return (new DAL.StateSQL().GetStateList());
        }

        public static List<Entity.State> GetStateList(String CountryCode)
        {
            return (new DAL.StateSQL().GetStateList(CountryCode));
        }

        public static List<Entity.State> GetState(Int64 StateCode, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.StateSQL().GetState(StateCode,LoginUserID , PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.State> GetState(Int64 StateCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.StateSQL().GetState(StateCode, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateState(Entity.State entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.StateSQL().AddUpdateState(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteState(Int64 StateCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.StateSQL().DeleteState(StateCode, out ReturnCode, out ReturnMsg);
        }
    }
}
