using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Loan
    {
        public Int64 pkID { get; set; }
        public Int64 RowNum { get; set; }
        public Int64 EmployeeID { get; set; }
        public String LoanCategory { get; set; }
        public String LoanType { get; set; }
        public String EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Decimal LoanAmount { get; set; }
        public Int32 NoOfInstallments { get; set; }
        public Decimal InstallmentAmount { get; set; }
        public String Remarks { get; set; }
        public String ApprovalStatus { get; set; }
        public String ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String LoginUserID { get; set; }
    }
}
