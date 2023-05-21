using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class UserLog
    {
        public Int64 pkID { get; set; }
        public string UserID { get; set; }
        public string ScreenFullName { get; set; }
        public string Description { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }

        public DateTime LoginDateTime { get; set; }
        public DateTime LogoutDateTime { get; set; }
        public string MacID { get; set; }
        public string INOUT { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }

        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }

        public Int64 Contacts { get; set; }
        public Int64 ToDO { get; set; }
        public Int64 Leave { get; set; }
        public Int64 login_logout { get; set; }

        public Int64 Inquiry { get; set; }
        public Int64 Quotation { get; set; }
        public Int64 Followup { get; set; }
        public Int64 SalesOrder { get; set; }
        public Int64 SalesInvoice { get; set; }
        public Int64 PurchaseInvoice { get; set; }
        public Int64 Inward { get; set; }
        public Int64 Outward { get; set; }
        public Int64 LatePunch { get; set; }
        public Int64 DailyActivity { get; set; }
    }
}
