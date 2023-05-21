using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SalesOrderDealer
    {
        public Int64 pkID { get; set; }

        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }

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

        public string QuotationNo { get; set; }
        public string ApprovalStatus { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal EstimatedAmt { get; set; }

        public string TermsCondition { get; set; }

        public string ClientOrderNo { get; set; }
        public DateTime ClientOrderDate { get; set; }
        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public Int64 DealerID { get; set; }
        public string DealerName { get; set; }

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

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

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


        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public string InquiryNo { get; set; }
        public string BillNo { get; set; }
        public string RefNo { get; set; }        // This will display any of (Inq#, Qt # / SO #)
        public string RefType { get; set; }      // Inquiry, Quotation, SalesOrderDealer
    }
}
