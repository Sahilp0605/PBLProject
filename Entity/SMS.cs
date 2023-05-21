
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SMS
    {
        public Int64 pkID { get; set; }
        public Int64 CompanyID { get; set; }
        public string AuthKey { get; set; }
        public string PortalName { get; set; }
        public string SenderID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SMSType { get; set; }
        public string LoginUserID { get; set; }    
    }
}
