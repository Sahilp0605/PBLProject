using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ShiftMasterMgmt
    {
        public static List<Entity.ShiftMaster> GetShiftMaster(String LoginUserID)
        {
            return (new DAL.ShiftMasterSQL().GetShiftMaster(LoginUserID));
        }

        public static List<Entity.ShiftMaster> GetShiftMaster(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ShiftMasterSQL().GetShiftMaster(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ShiftMaster> GetShiftMaster(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ShiftMasterSQL().GetShiftMaster(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateShiftMaster(Entity.ShiftMaster entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ShiftMasterSQL().AddUpdateShiftMaster(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteShiftMaster(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ShiftMasterSQL().DeleteShiftMaster(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
