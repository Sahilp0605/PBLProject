using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BAL
{
    public class CheckListMgmt
    {
        public static List<Entity.CheckList> GetCheckHead()
        {
            return (new DAL.CheckListSQL().GetCheckHead());
        }
        public static DataTable GetCheckList(String CheckHead)
        {
            return (new DAL.CheckListSQL().GetCheckList(CheckHead));
        }
        public static List<Entity.CheckList> GetCheckList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CheckListSQL().GetCheckList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.CheckList> GetCheckList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CheckListSQL().GetCheckList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateCheckList(Entity.CheckList entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CheckListSQL().AddUpdateCheckList(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteCheckList(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CheckListSQL().DeleteCheckList(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
