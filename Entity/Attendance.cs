using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Attendance
    {
        public Int64 pkID { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public DateTime PresenceDate { get; set; }
        public String TimeIn { get; set; }
        public String TimeOut { get; set; }
        public String Notes { get; set; }
        public Decimal WorkingHrs { get; set; }
        public Decimal WorkingMins { get; set; }
        public Decimal WorkingTotalHrs { get; set; }
        public Decimal ShiftTotalHrs { get; set; }
        public Decimal OTHrs { get; set; }
        public Decimal GraceMins { get; set; }
        public String LunchFrom { get; set; }
        public String LunchTo { get; set; }
        public Decimal LunchMins { get; set; }
        public Decimal WorkingHrsFlag { get; set; }
        public String DayStatus { get; set; }
        public Int64 CardNo { get; set; }

        public Int64 OrgCode { get; set; }
        public String OrgName { get; set; }

        public Int64 CompanyID { get; set; }
        public String CompanyName { get; set; }

        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String CreatedEmployee { get; set; }

        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedEmployee { get; set; }

        public string LoginUserID { get; set; }
    }
    public class Attendance_Report
    {
        public Int64 pkID { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public DateTime PresenceDate { get; set; }
        public String TimeIn { get; set; }
        public String TimeOut { get; set; }
        public String WorkingHrs { get; set; }
        public String PresenceStatus { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }

    }
    public class MissedPunch
    {
        public Int64 pkID { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public DateTime PresenceDate { get; set; }
        public String TimeIn { get; set; }
        public String TimeOut { get; set; }
        public String Notes { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String ApprovalStatus { get; set; }
        public String ApprovedBy { get; set; }
        public String ApprovedOn { get; set; }

        public String CreatedEmployee { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedEmployee { get; set; }
        public string LoginUserID { get; set; }
    }

}
