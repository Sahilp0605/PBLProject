using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Authenticate
    {
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string ScreenFullName { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public Int64 OrgTypeCode { get; set; }
        public string OrgType { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public string EmployeeImage { get; set; }
        public string ContactNo { get; set; }
        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string Landline1 { get; set; }
        public string Landline2 { get; set; }
        public string SerialKey { get; set; }
        public Int64 StateCode { get; set; }
        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }
        public DateTime NotificationTimestamp { get; set; }
        public DateTime MailTimestamp { get; set; }
        public string SMS_Uri { get; set; }
        public string SMS_AuthKey { get; set; }
        public string SMS_SenderID { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public Boolean CustomerAccess { get; set; }
        public Boolean ProductAccess { get; set; }
        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }
        public string DMSSystem { get; set; }
        public string LoginAs { get; set; }
        public string LoginUserID { get; set; }
    }
}
