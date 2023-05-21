using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class VisitAcupanel
    {
        public Int64 pkID { get; set; }
        public Int64 ComplaintNo { get; set; }
        public Int64 ParentID { get; set; }
        public string NewRep { get; set; }
        public string ProductName { get; set; }
        public string SrNo { get; set; }
        public string ReplaceProduct { get; set; }
        public string NewSrNo { get; set; }
        public string Remarks { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
