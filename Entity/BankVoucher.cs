using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class BankVoucher
    {
        
        public string InvoiceNo { get; set; }
        public string ListMode { get; set; }
        public string LoginUserId { get; set; }
        public string CustomerName { get; set; }
        public string NoOfWheels { get; set; }
        public string ClaimNo { get; set; }
        public string RegNo { get; set; }
        public string Surveyor { get; set; }
        public string CollectedFrom { get; set; }
        public Decimal Debit { get; set; }
        public Decimal Credit { get; set; }
        public Decimal TDSAmount { get; set; }
        public Decimal PayableAmt { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

    }
}
