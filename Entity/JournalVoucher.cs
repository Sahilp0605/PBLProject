using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class JournalVoucher
    {
        public Int64 pkID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }

        public Int64 DBCustomerID { get; set; }
        public string DBCustomerName { get; set; }
        public Int64 CRCustomerID { get; set; }
        public string CRCustomerName { get; set; }
        public Decimal VoucherAmount { get; set; }
        public string Remarks { get; set; }

        public string DBC { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String FileName { get; set; }
        public string LoginUserID { get; set; }

    }

    public class JournalVoucherDetail
    {
        public Int64 pkID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string TransType { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Decimal VoucherAmount { get; set; }
        public string Remarks { get; set; }

        public string DBC { get; set; }
        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        

    }

    public class JournalVoucherReport
    {
        public DateTime VoucherDate { get; set; }
        public Int64 CRCustomerID { get; set; }
        public string CRCustomerName { get; set; }
        public Int64 DBCustomerID { get; set; }
        public string DBCustomerName { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNo { get; set; }
        public string TransType { get; set; }
        public Decimal DebitAmt { get; set; }
        public Decimal CreditAmt { get; set; }

    }
}
