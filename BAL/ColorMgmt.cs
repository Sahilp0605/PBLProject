using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ColorMgmt
    {
        public static List<Entity.Color> GetColorList(String LoginUserID)
        {
            return (new DAL.ColorSQL().GetColorList(LoginUserID));
        }

        public static List<Entity.Color> GetColor(Int64 pkID, string LoginUserID, int PageNo, int PageColor, out int TotalRecord)
        {
            return (new DAL.ColorSQL().GetColor(pkID, LoginUserID, PageNo, PageColor, out TotalRecord));
        }

        public static List<Entity.Color> GetColor(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageColor, out int TotalRecord)
        {
            return (new DAL.ColorSQL().GetColor(pkID, LoginUserID, SearchKey, PageNo, PageColor, out TotalRecord));
        }

        public static void AddUpdateColor(Entity.Color entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ColorSQL().AddUpdateColor(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteColor(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ColorSQL().DeleteColor(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
