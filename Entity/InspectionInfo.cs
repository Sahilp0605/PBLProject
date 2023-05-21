using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Inspection
    {
        public Int64 pkID { get; set; }
        public DateTime InspectionDate { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String OrderNo { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public string InspectionType { get; set; }
        public String LoginUserID { get; set; }
    }

    public class InspectionDetail
    {
        public Int64 pkID { get; set; }
        public Int64 RefID { get; set; }
        public String CheckDesc { get; set; }
        public String CheckFlag { get; set; }
        public string CheckRemark { get; set; }
        public String LoginUserID { get; set; }
    }
}
