using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TrialBalanceReport
    {
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal Opening { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Closing { get; set; }

    }
}
