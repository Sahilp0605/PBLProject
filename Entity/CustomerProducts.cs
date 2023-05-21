using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CustomerProducts
    {
        public Int64 pkID { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String CustomerType { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public String ProductNameLong { get; set; }
        public String ProductGroupName { get; set; }
        public String BrandName { get; set; }
        public String UnitSize { get; set; }
        public Decimal Weight { get; set; }
        public Decimal ConversionRate { get; set; }
        public Decimal RatePerBag { get; set; }
        public String LoginUserID { get; set; }
    }
}
