using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class FinancialTrans
    {
        public Int64 pkID { get; set; }
        public string VoucherType { get; set; }
        public string RecPay { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public Int64 AccountID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 EmployeeID { get; set; }
        public Int64 TDSAccountID { get; set; }
        public string EmployeeName { get; set; }
        public string TransType { get; set; }
        public Int64 TransModeID { get; set; }
        public string TransID { get; set; }
        public DateTime TransDate { get; set; }
        public Decimal VoucherAmount { get; set; }
        public Decimal TDSAmount { get; set; }
        public string BankName { get; set; }
        public string Remark { get; set; }
        public Int64 TerminationOfDelivery { get; set; }
        public string RDURD { get; set; }
        public Decimal TaxPer { get; set; }
        public Decimal BasicAmt { get; set; }
        public Decimal GSTAmt { get; set; }
        public Decimal NetAmt { get; set; }
        public string CustomerName { get; set; }
        public string AccountName { get; set; }
        public string TDSAccountName { get; set; }
        public string TransModeName { get; set; }


        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }

        public string DBC { get; set; }     // BNKR/BNKP/CASR/CASP

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }

        public string InvoiceNoDetail { get; set; }
    }

    public class FinancialTransDetail
    {
        public Int64 pkID { get; set; }
        public Int64 ParentID { get; set; }
        public string VoucherType { get; set; }
        public string RecPay { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Int64 AccountID { get; set; }
        public string AccountName { get; set; }
        public string InvoiceNo { get; set; }
        public Decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }
    }

    public class CashBook
    {
        public DateTime VoucherDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal Receivable { get; set; }
        public decimal Payable { get; set; }
    }

    public class BankBook
    {
        public DateTime VoucherDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal Receivable { get; set; }
        public decimal Payable { get; set; }
    }
}
