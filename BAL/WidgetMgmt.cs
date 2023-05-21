using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class WidgetMgmt
    {
        public static void AddUpdateWidgetRole(string RoleID, string WidgetID, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WidgetSQL().AddUpdateWidgetRole(RoleID, WidgetID, pLoginUserId, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteWidgetRole(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WidgetSQL().DeleteWidgetRole(RoleID, out ReturnCode, out ReturnMsg);
        }
        public static string GetWidgetRole(string RoleID)
        {
            return (new DAL.WidgetSQL().GetWidgetRole(RoleID));
        }
        public static List<Entity.Widget> GetWidgetList()
        {
            return (new DAL.WidgetSQL().GetWidgetList());
        }
    }
}
