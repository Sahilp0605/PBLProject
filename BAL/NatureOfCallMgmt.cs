using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class NatureOfCallMgmt
    {
        public static List<Entity.NatureCall> GetNatureCallList()
        {
            return (new DAL.NatureOfCallSQL().GetNatureCallList());
        }

        public static List<Entity.NatureCall> GetNatureCallList(Int64 pkID, string LoginUserID, int PageNo, int PageBatch, out int TotalRecord)
        {
            return (new DAL.NatureOfCallSQL().GetNatureCallList(pkID, LoginUserID, PageNo, PageBatch, out TotalRecord));
        }

        public static List<Entity.NatureCall> GetNatureCallList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageBatch, out int TotalRecord)
        {
            return (new DAL.NatureOfCallSQL().GetNatureCallList(pkID, LoginUserID, SearchKey, PageNo, PageBatch, out TotalRecord));
        }

        public static void AddUpdateNatureCall(Entity.NatureCall entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.NatureOfCallSQL().AddUpdateNatureCall(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteNatureCall(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.NatureOfCallSQL().DeleteNatureCall(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
