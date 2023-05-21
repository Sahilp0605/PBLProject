using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SalesBillDetail
    {
        public Int64 pkID { get; set; }
        public string InvoiceNo { get; set; }
        public string DocRefNo { get; set; }
        public Int64 ProductID { get; set; }
        public Int16 TaxType { get; set; }
        public decimal Rate { get; set; }
        public string Unit { get; set; } 
        public decimal Qty { get; set; }
        public decimal UnitQty { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmt{ get; set; }
        public decimal NetRate { get; set; } 
        public decimal Amount { get; set; } 
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; } 
        public decimal CGSTPer { get; set; } 
        public decimal CGSTAmt { get; set; }
        public decimal IGSTPer { get; set; } 
        public decimal IGSTAmt { get; set; }
        public decimal AddTaxPer { get; set; }
        public decimal AddTaxAmt { get; set; } 
        public decimal NetAmt { get; set; }
        public decimal HeaderDiscAmt { get; set; }
        public string ForOrderNo { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime UpdatedDate { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }
        public String ProductSpecification { get; set; }
    }
    public class SalesBillDetail_Report
    {
        public Int64 pkID { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public decimal Rate { get; set; }
        public String Unit { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal HeaderDiscAmt { get; set; }
        public decimal NetRate { get; set; }
        //public decimal BasicAmt { get; set; }
        public decimal Amount { get; set; }
        public decimal AddTaxPer { get; set; }
        public decimal AddTaxAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal NetAmt { get; set; }
        public decimal ROffAmt { get; set; }
        public Int64 TaxType { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal HeaderNetAmount { get; set; }
        public decimal HeaderBasicAmount { get; set; }
        public decimal TotalDisountAmount { get; set; }
        public string CustomerName { get; set; }
        public string InquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string OrderNo { get; set; }
        public String ChargeName1 { get; set; }
        public String ChargeName2 { get; set; }
        public String ChargeName3 { get; set; }
        public String ChargeName4 { get; set; }
        public String ChargeName5 { get; set; }
        public decimal ChargeTotalAmt1 { get; set; }
        public decimal ChargeTotalAmt2 { get; set; }
        public decimal ChargeTotalAmt3 { get; set; }
        public decimal ChargeTotalAmt4 { get; set; }
        public decimal ChargeTotalAmt5 { get; set; }
        public string TerminationOfDelieryName { get; set; }
        public string RefNo { get; set; }
        public decimal Qty { get; set; }
        public string CreatedEmployeeName { get; set; }
        public string UpdatedEmployeeName { get; set; }

        public decimal TaxAmt { get; set; }

        //public string ModeOfTransport { get; set; }
        //public string TransporterName { get; set; }
        //public string VehicleNo { get; set; }
        //public string LRNo { get; set; }
        //public DateTime LRDate { get; set; }
        //public string TransportRemark { get; set; }

        //public string CurrencyName { get; set; }
        //public string CurrencySymbol { get; set; }
        //public decimal ExchangeRate { get; set; }

        //public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    public class SalesBillJobWork
    {
        public Int64 pkID { get; set; }
        public string InvoiceNo { get; set; }
        public Int64 FinishProductID { get; set; }
        public string JobProductName { get; set; }
        public string JobHSNCode { get; set; }
        public decimal Quantity { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
