using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Payroll
    {
        public Int64 pkID { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public String EmailAddress { get; set; }
        public string BasicPer { get; set; }
        public String DesigCode { get; set; }
        public String Designation { get; set; }
        public String OrgCode { get; set; }
        public String OrgName { get; set; }
        public Int64 ReportTo { get; set; }
        public String ReportToEmployeeName { get; set; }
        public String Gender { get; set; }
        public Int64 ShiftCode { get; set; }
        public String ShiftName { get; set; }
        public Decimal MinHrsFullDay { get; set; }
        public Decimal MinHrsHalfDay { get; set; }

        public DateTime PayDate { get; set; }
        public Int64 WDays { get; set; }
        public Decimal PDays { get; set; }
        public Decimal HDays { get; set; }
        public Decimal ODays { get; set; }
        public Decimal LDays { get; set; }
        public Decimal FixedSalary { get; set; }

        public Decimal Basic { get; set; }
        public Decimal HRA { get; set; }
        public Decimal DA { get; set; }
        public Decimal Conveyance { get; set; }
        public Decimal Medical { get; set; }
        public Decimal Special { get; set; }
        public Decimal OverTime { get; set; }
        public Decimal Total_Income { get; set; }

        public Decimal PF { get; set; }
        public Decimal ESI { get; set; }
        public Decimal PT { get; set; }
        public Decimal TDS { get; set; }
        public Decimal Loan { get; set; }
        public Decimal LoanAmt { get; set; }
        public Decimal Upad { get; set; }
        public Decimal Total_Deduct { get; set; }

        public Decimal NetSalary { get; set; }

        public String LoginUserID { get; set; }

        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        
    }
}
