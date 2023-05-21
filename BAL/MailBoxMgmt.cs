using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class MailBoxMgmt
    {
        public static List<Entity.MailBox> GetMailBoxList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.MailBoxSQL().GetMailBoxList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMailBoxEntry(Entity.MailBox entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MailBoxSQL().AddUpdateMailBoxEntry(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMailBoxEntry(string MessageID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MailBoxSQL().DeleteMailBoxEntry(MessageID, out ReturnCode, out ReturnMsg);
        }

        public static string GetLastMailTimestamp(string pLoginUserID)
        {
            return (new DAL.MailBoxSQL().GetLastMailTimestamp(pLoginUserID));
        }

        public static string setLastMailTimestamp(string pLoginUserID)
        {
            return (new DAL.MailBoxSQL().setLastMailTimestamp(pLoginUserID));
        }
    }
}
