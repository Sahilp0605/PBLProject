using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class PrintDocument
    {
        public Int64 pkID { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string InquiryNo { get; set; }
        public Int64 InquirypkID { get; set; }
        public string ProjectName { get; set; }
        public string QuotationSubject { get; set; }
        public string QuotationKindAttn { get; set; }
        public string QuotationHeader { get; set; }
        public string QuotationFooter { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public decimal QuotationAmount { get; set; }
        public string InquiryStatus { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedEmployeeName { get; set; }
        public string CreatedEmployeeMobileNo { get; set; }
        public string UpdatedEmployeeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetRate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Unit { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal Amount { get; set; }
        public decimal BasicAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal ROffAmt { get; set; }
        public String ChargeName1 { get; set; }
        public String ChargeName2 { get; set; }
        public String ChargeName3 { get; set; }
        public String ChargeName4 { get; set; }
        public String ChargeName5 { get; set; }
        public Int64 ChargeID1 { get; set; }
        public decimal ChargeAmt1 { get; set; }
        public decimal ChargeBasicAmt1 { get; set; }
        public decimal ChargeGSTAmt1 { get; set; }
        public Int64 ChargeID2 { get; set; }
        public decimal ChargeAmt2 { get; set; }
        public decimal ChargeBasicAmt2 { get; set; }
        public decimal ChargeGSTAmt2 { get; set; }
        public Int64 ChargeID3 { get; set; }
        public decimal ChargeAmt3 { get; set; }
        public decimal ChargeBasicAmt3 { get; set; }
        public decimal ChargeGSTAmt3 { get; set; }
        public Int64 ChargeID4 { get; set; }
        public decimal ChargeAmt4 { get; set; }
        public decimal ChargeBasicAmt4 { get; set; }
        public decimal ChargeGSTAmt4 { get; set; }
        public Int64 ChargeID5 { get; set; }
        public decimal ChargeAmt5 { get; set; }
        public decimal ChargeBasicAmt5 { get; set; }
        public decimal ChargeGSTAmt5 { get; set; }
        public decimal NetAmt { get; set; }
        // from salesbill.cs
        //public Int64 pkID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Int64 FixedLedgerID { get; set; }
        public string FixedLedgerName { get; set; }
        //public Int64 CustomerID { get; set; }
        //public string CustomerName { get; set; }
        public Int64 TerminationOfDeliery { get; set; }
        public string TerminationOfDelieryName { get; set; }
        public string TermsCondition { get; set; }
        //public string InquiryNo { get; set; }
        //public string QuotationNo { get; set; }
        public string OrderNo { get; set; }
        public string ComplaintNo { get; set; }
        public string RefNo { get; set; }        // This will display any of (Inq#, Qt # / SO #)
        public string RefType { get; set; }      // Inquiry, Quotation, SalesOrder
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        //public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public decimal EstimatedAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }
        public Nullable<DateTime> LRDate { get; set; }
        public string TransportRemark { get; set; }
        public string EmployeeName { get; set; }
        public decimal BillAmount { get; set; }
    }
}
