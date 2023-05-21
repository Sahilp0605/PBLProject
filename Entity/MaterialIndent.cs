using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class MaterialIndent
    {
        public Int64 pkID { get; set; }
        public string IndentNo { get; set; }
        public DateTime IndentDate { get; set; }
        public string Remarks { get; set; }
        public string ApprovalStatus { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedEmployeeName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedEmployeeName { get; set; }
    }

    public class MaterialIndent_detail
    {
        public Int64 pkID { get; set; }
        public string IndentNo { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public Decimal Qty { get; set; }
        public String Unit { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string Remarks { get; set; }
        public string LoginUserID { get; set; }
    }
}
