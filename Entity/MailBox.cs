using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class MailBox
    {
        public Int64  pkID	{ get; set; }
        public string MessageID	{ get; set; }
        public DateTime MailDate	{ get; set; }
        public DateTime MailDateSent	{ get; set; }
        public string MailFrom	{ get; set; }
        public string MailTo	{ get; set; }
        public string MailCc	{ get; set; }
        public string MailSubject	{ get; set; }
        public string MailBody	{ get; set; }
        public string LoginUserID { get; set; }

    }
}
