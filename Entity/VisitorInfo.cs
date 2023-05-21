using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class VisitorInfo
    {
        public Int64 pkID { get; set; }
        public string InquiryNo { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string VisitorName { get; set; }
        public string VisitorContact { get; set; }
        public string VisitorEmail { get; set; }
        public string PurposeOfVisit { get; set; }

        public string Department { get; set; }
        public string MeetingTo { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }

        public Int64 CompanyID { get; set; }

        public string CompanyName { get; set; }
        public string CompanyContact { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string IdentityType { get; set; }
        public string IdentityDocumentNo { get; set; }

        public string VisitorImage { get; set; }
        public string VisitorDocument { get; set; }

        public string LoginUserID { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
