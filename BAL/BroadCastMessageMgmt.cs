using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class BroadCastMessageMgmt
    {
        public static List<Entity.BroadCastMessage> GetBroadCastMessageList(String LoginUserID)
        {
            return (new DAL.BroadCastMessageSQL().GetBroadCastMessageList(LoginUserID));
        }

        public static List<Entity.BroadCastMessage> GetBroadCastMessage(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.BroadCastMessageSQL().GetBroadCastMessage(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.BroadCastMessage> GetBroadCastMessage(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.BroadCastMessageSQL().GetBroadCastMessage(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateBroadCastMessage(Entity.BroadCastMessage entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BroadCastMessageSQL().AddUpdateBroadCastMessage(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteBroadCastMessage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BroadCastMessageSQL().DeleteBroadCastMessage(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
