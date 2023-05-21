using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OverTime
    {
        public Int64 pkID { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Int64 TotalMinutes { get; set; }
        public string ReasonForOT { get; set; }

        public string OrgCode { get; set; }
        public string OrgName { get; set; }

        public string LoginUserID { get; set; }

        public string ApprovalStatus { get; set; }
        
        public Int64 ApprovedEmployeeID { get; set; }
        public string ApprovedEmployeeName { get; set; }
        
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
