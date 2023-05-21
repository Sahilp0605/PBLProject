using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OrgGroup
    {
        public Int64 pkID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string OrgDepartmentCode { get; set; }
        public string OrgDepartmentName { get; set; }
        public string OrgShortName { get; set; }
        public Boolean Email_Flag { get; set; }
        public Int64 Email_TemplateID { get; set; }
        public string Email_Template { get; set; }
        public Boolean SMS_Flag { get; set; }
        public Int64 SMS_TemplateID { get; set; }
        public string SMS_Template { get; set; }
        public Boolean FAX_Flag { get; set; }
        public Int64 FAX_TemplateID { get; set; }
        public string FAX_Template { get; set; }

        public string LoginUserID { get; set; }
    }
}
