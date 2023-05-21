using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{

    public class DBNote
    {
        public Int64 pkID { get; set; }
        public String VoucherNo { get; set; }
        public Decimal VoucherAmount { get; set; }
        public DateTime VoucherDate { get; set; }
        public String InvoiceNo { get; set; }
        public Int64 CRCustomerID { get; set; }
        public Int64 DBCustomerID { get; set; }
        public string CRCustomerName { get; set; }
        public string DBCustomerName { get; set; }
        public string DBC { get; set; }
        public string LoginUserID { get; set; }
        public Decimal BasicAmt { get; set; }
        public Decimal DiscountAmt { get; set; }
        public Decimal SGSTAmt { get; set; }
        public Decimal CGSTAmt { get; set; }
        public Decimal IGSTAmt { get; set; }
        public Decimal ROffAmt { get; set; }
        public Int64 ChargeID1 { get; set; }
        public Decimal ChargeAmt1 { get; set; }
        public Int64 ChargeID2 { get; set; }
        public Decimal ChargeAmt2 { get; set; }
        public Int64 ChargeID3 { get; set; }
        public Decimal ChargeAmt3 { get; set; }
        public Int64 ChargeID4 { get; set; }
        public Decimal ChargeAmt4 { get; set; }
        public Int64 ChargeID5 { get; set; }
        public Decimal ChargeAmt5 { get; set; }
        public Decimal NetAmt { get; set; }
        public Decimal ChargeBasicAmt1 { get; set; }
        public Decimal ChargeBasicAmt2 { get; set; }
        public Decimal ChargeBasicAmt3 { get; set; }
        public Decimal ChargeBasicAmt4 { get; set; }
        public Decimal ChargeBasicAmt5 { get; set; }
        public Decimal ChargeGSTAmt1 { get; set; }
        public Decimal ChargeGSTAmt2 { get; set; }
        public Decimal ChargeGSTAmt3 { get; set; }
        public Decimal ChargeGSTAmt4 { get; set; }
        public Decimal ChargeGSTAmt5 { get; set; }

    }
    public class CRNote
    {
        public Int64 pkID { get; set; }
        public String VoucherNo { get; set; }
        public Decimal VoucherAmount { get; set; }
        public DateTime VoucherDate { get; set; }
        public Int64 CRCustomerID { get; set; }
        public Int64 DBCustomerID { get; set; }
        public string CRCustomerName { get; set; }
        public string DBCustomerName { get; set; }
        public string LoginUserID { get; set; }
        public Decimal BasicAmt { get; set; }
        public Decimal DiscountAmt { get; set; }
        public Decimal SGSTAmt { get; set; }
        public Decimal CGSTAmt { get; set; }
        public Decimal IGSTAmt { get; set; }
        public Decimal ROffAmt { get; set; }
        public Int64 ChargeID1 { get; set; }
        public Decimal ChargeAmt1 { get; set; }
        public Int64 ChargeID2 { get; set; }
        public Decimal ChargeAmt2 { get; set; }
        public Int64 ChargeID3 { get; set; }
        public Decimal ChargeAmt3 { get; set; }
        public Int64 ChargeID4 { get; set; }
        public Decimal ChargeAmt4 { get; set; }
        public Int64 ChargeID5 { get; set; }
        public Decimal ChargeAmt5 { get; set; }
        public Decimal NetAmt { get; set; }
        public Decimal ChargeBasicAmt1 { get; set; }
        public Decimal ChargeBasicAmt2 { get; set; }
        public Decimal ChargeBasicAmt3 { get; set; }
        public Decimal ChargeBasicAmt4 { get; set; }
        public Decimal ChargeBasicAmt5 { get; set; }
        public Decimal ChargeGSTAmt1 { get; set; }
        public Decimal ChargeGSTAmt2 { get; set; }
        public Decimal ChargeGSTAmt3 { get; set; }
        public Decimal ChargeGSTAmt4 { get; set; }
        public Decimal ChargeGSTAmt5 { get; set; }
    }
    public class DBNote_Detail
    {
        public Int64 pkID { get; set; }
        public String VoucherNo { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductSpecification { get; set; }
        public Int64 LocationID { get; set; }
        public int TaxType { get; set; }
        public String DBC { get; set; }
        public Decimal UnitQty { get; set; }
        public Decimal Qty { get; set; }
        public String Unit { get; set; }
        public Decimal Rate { get; set; }
        public Decimal DiscountPer { get; set; }
        public Decimal DiscountAmt { get; set; }
        public Decimal NetRate { get; set; }
        public Decimal Amount { get; set; }
        public Decimal SGSTPer { get; set; }
        public Decimal SGSTAmt { get; set; }
        public Decimal CGSTPer { get; set; }
        public Decimal CGSTAmt { get; set; }
        public Decimal IGSTPer { get; set; }
        public Decimal IGSTAmt { get; set; }
        public Decimal AddTaxPer { get; set; }
        public Decimal AddTaxAmt { get; set; }
        public Decimal NetAmt { get; set; }
        public Decimal HeaderDiscAmt { get; set; }
        public String ForOrderNo { get; set; }
        public String LoginUserID { get; set; }

    }
 }