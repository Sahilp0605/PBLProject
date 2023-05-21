using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class AccountLedger
    {
        public Int64 pkID { get; set; }
        public String LedgerCode { get; set; }
        public String LedgerName { get; set; }

        public Decimal OpenBal { get; set; }
        public Decimal DebitBal { get; set; }
        public Decimal CreditBal { get; set; }
        public Decimal CloseBal { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
