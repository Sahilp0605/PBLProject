using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Molding
    {
        public Int64 pkID { get; set; }
        public String MoldingNo { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String OrderNo { get; set; }
        public String WorkType { get; set; }
        public DateTime MoldingDate { get; set; }
        public String LoginUserID { get; set; }

    }
    public class MoldingDetail
    {
        public Int64 pkID { get; set; }
        public String MoldingNo { get; set; }
        public DateTime MoldingDate { get; set; }
        public String LoginUserID { get; set; }
        public String WorkType { get; set; }
        public String WorkerName { get; set; }
        public Int64 ClientID { get; set; }
        public String CustomerName { get; set; }
        public String OrderNo { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public String ProductNameLong { get; set; }
        public Decimal Die { get; set; }
        public Decimal Cavity { get; set; }
        public String DieNo { get; set; }
        public String Material { get; set; }
        public String Hardness { get; set; }
        public Decimal Quantity { get; set; }

    }
}
