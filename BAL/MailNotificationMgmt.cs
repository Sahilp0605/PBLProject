using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class MailNotificationMgmt
    {
        public static void sendEmailNotification(Entity.EmailStructure entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MailNotificationSQL().sendEmailNotification(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
