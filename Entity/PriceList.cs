using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class PriceList
    {
        public Int64 pkID { get; set; }
        public String PriceListName { get; set; }
        public string LoginUserID { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class PriceListDetail
    {
        public Int64 pkID { get; set; }
        public String PriceList { get; set; }
        public Int64 ParentID { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Discount { get; set; }
        public string LoginUserID { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
