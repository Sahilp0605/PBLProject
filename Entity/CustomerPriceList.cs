using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CustomerPriceList
    {
        public Int64 pkID { get; set; }
        public Int64 CustomerID { get; set; }

        public Int64 ProductGroupID { get; set; }
        public Int64 ProductID { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Discount { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
