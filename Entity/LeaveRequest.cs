using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class LeaveRequest
    {
        public Int64 pkID { get; set; }

        public Int64 LeaveTypeID { get; set; }
        public string LeaveType { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Decimal LeaveDays { get; set; }
        public Int64 TotalMinutes { get; set; }
        public Int64 TotalLeaveDays { get; set; }
        public string ReasonForLeave { get; set; }

        public string OrgCode { get; set; }
        public string OrgName { get; set; }

        public string LoginUserID { get; set; }
        public string LeaveCode { get; set; }
        public string PaidUnpaid { get; set; }
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

    public class EmployeeLeaveBalance
    {
        public Int64 pkID { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public Int64 LeaveTypeID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveCode { get; set; }
        public string Category { get; set; }

        
        public string PaidUnpaid { get; set; }
        public string ApprovalStatus { get; set; }

        public Int64 ApprovedEmployeeID { get; set; }
        public string ApprovedEmployeeName { get; set; }

        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }

        public Decimal OpeningBal { get; set; }
        public Decimal Earned { get; set; }
        public Decimal Used { get; set; }
        public Decimal ClosingBal { get; set; }

        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
