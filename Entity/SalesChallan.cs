using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SalesChallan
    {
        public Int64 pkID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }

        public Int64 FixedLedgerID { get; set; }
        public string FixedLedgerName { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string GSTNO { get; set; }
        public Int64 BankID { get; set; }
        
        //----------------------------- BAnk details--------------
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIFSC { get; set; }
        public string BankSwift { get; set; }
        //-----------------------------------------------------
        public Int64 TerminationOfDeliery { get; set; }
        public string TerminationOfDelieryName { get; set; }
        public Int64 TerminationOfDelieryCity { get; set; }

        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        
        public string TerminationOfDelieryCityName { get; set; }

        public string TermsCondition { get; set; }

        public string InquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string OrderNo { get; set; }
        public string ComplaintNo { get; set; }
        

        public string RefNo { get; set; }        // This will display any of (Inq#, Qt # / SO #)
        public string RefType { get; set; }      // Inquiry, Quotation, SalesOrder

        public string SupplierRef { get; set; }
        public DateTime SupplierRefDate { get; set; }

        public string OtherRef { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public decimal EstimatedAmt { get; set; }

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
        public decimal TaxAmt { get; set; }

        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }

        public string DispatchDocNo { get; set; }
        public Nullable<DateTime> LRDate { get; set; }
        public string DeliveryNote { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string EwayBillNo { get; set; }
        public string ModeOfPayment { get; set; }
        public string TransportRemark { get; set; }
        public string DeliverTo { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }

        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string EmployeeName { get; set; }
        public Decimal BillAmount { get; set; }
        public Int64 CompanyID { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }

        //------------------------------------------SalesBill ExportDetails------------------------------

        public string PreCarrBy { get; set; }
        public string PreCarrRecPlace { get; set; }
        public string FlightNo { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDispatch { get; set; }
        public string PortOfDestination { get; set; }
        public string MarksNo { get; set; }
        public string Packages { get; set; }
        public string NetWeight { get; set; }
        public string GrossWeight { get; set; }
        public string PackageType { get; set; }
        public string FreeOnBoard { get; set; }
    }

    public class SalesChallanReport
    {
        public Int64 pkID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }

        public Int64 FixedLedgerID { get; set; }
        public string FixedLedgerName { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }

        public Int64 BankID { get; set; }

        //----------------------------- BAnk details--------------
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIFSC { get; set; }
        public string BankSwift { get; set; }
        //-----------------------------------------------------
        public Int64 TerminationOfDeliery { get; set; }
        public string TerminationOfDelieryName { get; set; }
        public Int64 TerminationOfDelieryCity { get; set; }
        public string TerminationOfDelieryCityName { get; set; }

        public string TermsCondition { get; set; }

        public string InquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string OrderNo { get; set; }
        public string ComplaintNo { get; set; }


        public string RefNo { get; set; }        // This will display any of (Inq#, Qt # / SO #)
        public string RefType { get; set; }      // Inquiry, Quotation, SalesOrder

        public string SupplierRef { get; set; }
        public DateTime SupplierRefDate { get; set; }

        public string OtherRef { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public decimal EstimatedAmt { get; set; }

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
        public decimal TaxAmt { get; set; }

        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }

        public string DispatchDocNo { get; set; }
        public DateTime LRDate { get; set; }
        public string DeliveryNote { get; set; }
        public string EwayBillNo { get; set; }
        public string ModeOfPayment { get; set; }
        public string TransportRemark { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }

        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string EmployeeName { get; set; }
        public Decimal BillAmount { get; set; }
        public Int64 CompanyID { get; set; }


        
    }
    public class SalesChallanTaxDetail
    {
        public String HSNCode { get; set; }
        public Decimal TaxableValue { get; set; }
        public Decimal SGSTPer { get; set; }
        public Decimal SGSTAmt { get; set; }
        public Decimal CGSTPer { get; set; }
        public Decimal CGSTAmt { get; set; }
        public Decimal IGSTPer { get; set; }
        public Decimal IGSTAmt { get; set; }
        public Decimal TotalTax { get; set; }
    }
    public class SalesChallanDetail
    {
        public Int64 pkID { get; set; }
        public string ChallanNo { get; set; }
        public Int64 ProductID { get; set; }
        public Int16 TaxType { get; set; }
        public decimal Rate { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitQty { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmt { get; set; }
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
    public class SalesChallanDetail_Report
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
}
