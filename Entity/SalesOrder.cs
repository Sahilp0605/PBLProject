using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SalesOrder
    {
        public Int64 RowNum { get; set; }
        public Int64 pkID { get; set; }
        
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string HeaderID { get; set; }
        public string FilePath { get; set; }
        public string DocRefNoList { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string CreditDays { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string MobileNo { get; set; }
        public string QuotationNo { get; set; }
        public string ActionTaken { get; set; }
        public string TaskDescription { get; set; }
        public string ClosingRemarks { get; set; }
        public Int64 BankID { get; set; }
        public string ApprovalStatus { get; set; }
        public string ProjectStage { get; set; }
        public string StatusRemarks { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal EstimatedAmt { get; set; }

        public string TermsCondition { get; set; }
        public string DeliveryTerms { get; set; }
        public string PaymentTerms { get; set; }

        public string ClientOrderNo { get; set; }
        public DateTime ClientOrderDate { get; set; }
        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public decimal OrderAmount { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }

        public string LoginUserID { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }
        public string ProductSpecification { get; set; }

        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetRate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }

        public DateTime DueDate { get; set; }
        public decimal PayAmount { get; set; }
        
        public string IssuedBy { get; set; }
        public string DealerName { get; set; }

        public string CreatedBy { get; set; }
        public Int64 CreatedID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FromEmployeeName { get; set; }
        //////////////////////////////////////////////////////////////
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
        public decimal AdvAmt { get; set; }
        public decimal AdvPer { get; set; }

        public string CurrencyShortName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public string InquiryNo { get; set; }
        public string BillNo { get; set; }
        public string RefNo { get; set; }        // This will display any of (Inq#, Qt # / SO #)
        public string RefType { get; set; }      // Inquiry, Quotation, SalesOrder

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

        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BranchName { get; set; }
        public string BankIFSCCode { get; set; }

        public string BankSWIFTCode { get; set; }

        public string EmailHeader { get; set; }
        public string EmailContent { get; set; }
        public string ProjectName { get; set; }

        public DateTime DeliveryDate { get; set; }
        public String CreatedEmployeeName { get; set; }
        public String ApprovedBy { get; set; }
        public String SerialKey { get; set; }
        public string PIno { get; set; }
        public DateTime PIdate { get; set; }
        public string WorkOrderNo { get; set; }
        public DateTime WorkOrderDate { get; set; }

        //Shipping Details
        public string SComapnyName { get; set; }
        public string SGSTNo { get; set; }
        public string SContactNo { get; set; }
        public string SContactPersonName { get; set; }
        public string SAddress { get; set; }
        public string SArea { get; set; }
        public string SCountryCode { get; set; }
        public Int64 SCityCode { get; set; }
        public Int64 SStateCode { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string SPinCode { get; set; }
    }

    public class SalesOrderProduction
    {
        public Int64 PkID { get; set; }
        public string OrderNo { get; set; }
        public string HKMoterMake { get; set; }
        public string HKMoterMakeSRNO { get; set; }
        public string CTMotorMake { get; set; }
        public string CTMotorMakeSRNO { get; set; }
        public string HTBreak { get; set; }
        public string HTBreakSRNO { get; set; }
        public string CTBreak { get; set; }
        public string CTBreakSRNO { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
