using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BAL
{
    public class ShadeMgmt
    {
        public static List<Entity.Shade> GetShadeList(String LoginUserID)
        {
            return (new DAL.ShadeSQL().GetShadeList(LoginUserID));
        }
        public static List<Entity.Shade> GetShade(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ShadeSQL().GetShade(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.Shade> GetShade(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ShadeSQL().GetShade(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateShade(Entity.Shade entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ShadeSQL().AddUpdateShade(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteShade(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ShadeSQL().DeleteShade(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}