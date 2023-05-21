using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
 public  class SizeMgmt
    {
        public static List<Entity.Size> GetSizeList(String LoginUserID)
        {
            return (new DAL.SizeSQL().GetSizeList(LoginUserID));
        }

        public static List<Entity.Size> GetSize(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SizeSQL().GetSize(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Size> GetSize(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SizeSQL().GetSize(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSize(Entity.Size entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SizeSQL().AddUpdateSize(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSize(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SizeSQL().DeleteSize(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
