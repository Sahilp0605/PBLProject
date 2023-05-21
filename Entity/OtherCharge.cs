using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OtherCharge
    {
        public Int64 pkID { get; set; }
        public String ChargeName { get; set; }
        public decimal  GST_Per { get; set; }
        public String HSNCODE { get; set; }
        public int  TaxType { get; set; }
        public string TaxTypeName { get; set; }
        public bool BeforeGST { get; set; }
        public String LoginUserID { get; set; }
    }
}
