using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Production
    {
        public Int64 pkID { get; set; }
        public Int64 ParentID { get; set; }
        public DateTime ProductionDate { get; set; }
        public Int64 FinishedProductID { get; set; }
        public String FinishedProductName { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String SoNo { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public Decimal Quantity { get; set; }
        public String Unit { get; set; }
        public String Remarks { get; set; }
        public String Ref { get; set; }
        public String LoginUserID { get; set; }
    }
}
